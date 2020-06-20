using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using static WebAppOnBus.Ajaxhandler;

namespace WebAppOnBus
{
    /// <summary>
    /// Summary description for _2DModelHandler
    /// </summary>
    public class _2DModelHandler : IHttpHandler
    {
        //public static string IPAddr = "192.168.31.100";
        public static string IPAddr = _Default.GetIpAddr();
        public static int StartPort = 8080;
        static Socket[] serverSocket = new Socket[10];

        //包大小
        public const int DataRows = 204;
        public const int Packagebag = 64+4+sizeof(Single)*2*DataRows;

        //图片大小常量设定
        public const int picHeight = 359;
        public const int picWidth = 359;

        //缩放系数设定
        public const int ScaleParam = 20;
        
        //建模帧缓存数量
        public const int FMemory = 10;
        //当前帧位置
        public int FPos = 0;
        //帧延迟帧数
        public int DelayedFrame = 5;

        //标注异常范围
        public const int DetRange = 10;

        //退出生成模型
        public bool Generate2DExit = false;
        public bool Initialized = true;

        //线程锁
        private static Mutex MutexSock = new Mutex(true);

        //占用缓存的编号
        private static int OnUsedMemory = 0;

        //画边框的测试
        //public int Borderchange = 0;       

        //数据封装
        public struct SockPackage
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public char[] SerialNum;
            public int Framecnt;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = DataRows)]
            public Single[] s;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = DataRows)]
            public Single[] ay;
        }

        //数据点缓存
        public struct PointMemory
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = DataRows*8)]
            public Single[] x;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = DataRows*8)]
            public Single[] y;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = DataRows * 8)]
            public int[] px;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = DataRows * 8)]
            public int[] py;
        }
        //相机计算数据双缓存
        //8台相机计算数据缓存
        public static SockPackage[] m_SockPack0 = new SockPackage[9];
        public static SockPackage[] SockPack0
        {
            get
            {
                return m_SockPack0;
            }
            set
            {
                SockPackage[] m_SockPack0 = (SockPackage[])value.Clone();
            }
        }

        public static SockPackage[] m_SockPack1 = new SockPackage[9];
        public static SockPackage[] SockPack1
        {
            get
            {
                return m_SockPack1;
            }
            set
            {
                SockPackage[] m_SockPack1 = (SockPackage[])value.Clone();
            }
        }


        //建模结果缓存
        public static PointMemory[] m_pMemory = new PointMemory[FMemory];
        public static PointMemory[] pMemory
        {
            get
            {
                return m_pMemory;
            }
            set
            {
                PointMemory[] m_pMemory = (PointMemory[])value.Clone();
            }
        }
        //建模结果颜色暂留
        public static int[] m_ColorCounter = new int[DataRows * 8];
        public static int[] ColorCounter
        {
            get
            {
                return m_ColorCounter;
            }
            set
            {
                int[] m_ColorCounter = (int[])value.Clone();
            }
        }

        //图片存储地址
        public static string m_ModelImageUrl;
        public static string ModelImageUrl
        {
            get
            {
                return m_ModelImageUrl;
            }
            set
            {
                m_ModelImageUrl = value;
            }
        }

        



        public void ProcessRequest(HttpContext context)
        {

            string action = context.Request.Params["action"].ToLower();
            switch(action)
            {
                case "startserver":
                    StartServer(context);
                    break;
                case "get2dimage":
                    Draw2DImage(context);
                    break;

            }

            if(Initialized)
            {
                for(int i = 0;i<FMemory;i++)
                {
                    pMemory[i].px = new int[DataRows * 8];
                    pMemory[i].py = new int[DataRows * 8];
                }
                
                for(int i = 0;i<9;i++)
                {
                    SockPack0[i].s = new float[DataRows];
                    SockPack0[i].ay = new float[DataRows];
                    SockPack1[i].s = new float[DataRows];
                    SockPack1[i].ay = new float[DataRows];
                }
                Initialized = false;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        //开启服务器指令（同时开启九台）
        public void StartServer(HttpContext context)
        {
            Thread ServerInitThread0 = new Thread(ServerInit);
            Thread ServerInitThread1 = new Thread(ServerInit);
            Thread ServerInitThread2 = new Thread(ServerInit);
            Thread ServerInitThread3 = new Thread(ServerInit);
            Thread ServerInitThread4 = new Thread(ServerInit);
            Thread ServerInitThread5 = new Thread(ServerInit);
            Thread ServerInitThread6 = new Thread(ServerInit);
            Thread ServerInitThread7 = new Thread(ServerInit);
            Thread ServerInitThread8 = new Thread(ServerInit);
            ServerInitThread0.Start(0);
            ServerInitThread1.Start(1);
            ServerInitThread2.Start(2);
            ServerInitThread3.Start(3);
            ServerInitThread4.Start(4);
            ServerInitThread5.Start(5);
            ServerInitThread6.Start(6);
            ServerInitThread7.Start(7);
            ServerInitThread8.Start(8);
            Thread Generate2DImageThread1 = new Thread(Generate2DImage);
            //Thread Generate2DImageThread2 = new Thread(Generate2DImage);
            Generate2DImageThread1.Start();
            //Generate2DImageThread2.Start();
        }

        public void Draw2DImage(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(ModelImageUrl);
            context.Response.End();
        }

        public void Generate2DImage()
        {
            while (!Generate2DExit)
            {

                //图像初始化
                Bitmap bmp = new Bitmap(picWidth, picHeight);
                //数据点计数
                int PointCounter = 0;
                //申请空间
                //pMemory[FPos].px = new int[DataRows * 8];
                //pMemory[FPos].py = new int[DataRows * 8];


                ////画边框
                //if (Borderchange == 0)
                //{
                for (int q = 1; q < picWidth; q++)
                {
                    bmp.SetPixel(q, 1, Color.Green);
                    bmp.SetPixel(q, 358, Color.Green);
                }
                for (int q = 1; q < picHeight; q++)
                {
                    bmp.SetPixel(1, q, Color.Green);
                    bmp.SetPixel(358, q, Color.Green);
                }
                //Borderchange = 1;
                //}
                //else
                //{
                //    for (int q = 111; q < picWidth - 110; q++)
                //    {
                //        bmp.SetPixel(q, 111, Color.Green);
                //        bmp.SetPixel(q, 248, Color.Green);
                //    }
                //    for (int q = 111; q < picHeight - 110; q++)
                //    {
                //        bmp.SetPixel(111, q, Color.Green);
                //        bmp.SetPixel(248, q, Color.Green);
                //    }
                //    Borderchange = 0;
                //}

                //使用缓存0进行渲染
                if (OnUsedMemory == 0)
                {
                    //多台相机2D点渲染
                    for (int k = 1; k <= 8; k++)
                    {
                        double BasicDegreeWorld = _Default.CameraDegree[k] * 3.14159265358 / 180;
                        //换算为图像坐标系角度
                        double BasicDegree = 90 * 3.14159265358 / 180 - BasicDegreeWorld;
                        ///MutexSock.WaitOne();

                        ///MutexSock.ReleaseMutex();
                        for (int j = 0; j < DataRows; j++)
                        {
                            PointCounter = (k - 1) * DataRows + j;
                            try
                            {




                                //计算x,y坐标
                                //float x = SockPack[k].s[j] * System.Convert.ToSingle(Math.Cos(BasicDegree - SockPack[k].ay[j]))/ ScaleParam + System.Convert.ToSingle(picWidth / 2);
                                //float y = SockPack[k].s[j] * System.Convert.ToSingle(Math.Sin(BasicDegree - SockPack[k].ay[j]))/ ScaleParam + System.Convert.ToSingle(picHeight / 2);
                                //int px = System.Convert.ToInt16(x);
                                //int py = System.Convert.ToInt16(y);
                                int px = (System.Convert.ToInt32(SockPack0[k].s[j] * Math.Cos(BasicDegree - SockPack0[k].ay[j]))) / ScaleParam + picWidth / 2;
                                int py = (System.Convert.ToInt32(SockPack0[k].s[j] * Math.Sin(BasicDegree - SockPack0[k].ay[j]))) / ScaleParam + picHeight / 2;
                                //int  px = 180;
                                //int py = 180;

                                //画布中渲染点
                                if (px > 0 && px < picWidth && py > 0 && py < picHeight)
                                {
                                    //取前次计算数据
                                    int lpx = pMemory[(FPos + FMemory - 1) % FMemory].px[PointCounter];
                                    int lpy = pMemory[(FPos + FMemory - 1) % FMemory].py[PointCounter];
                                    //与前帧进行比较并将不同处标出
                                    if (Math.Abs(lpx * lpx + lpy * lpy - px * px - py * py) > DetRange)
                                    {
                                        bmp.SetPixel(px, py, Color.Red);
                                        ColorCounter[PointCounter] = DelayedFrame;
                                    }
                                    else if (ColorCounter[PointCounter] > 0)
                                    {
                                        bmp.SetPixel(px, py, Color.Red);
                                        ColorCounter[PointCounter] = ColorCounter[PointCounter] - 1;
                                    }
                                    else
                                    {
                                        bmp.SetPixel(px, py, Color.Blue);
                                    }

                                }
                                OnUsedMemory = 1;


                                //存储点数据

                                //PointMemory pMemoryc = new PointMemory();
                                //pMemoryc.px[PointCounter] = px;
                                //pMemoryc.py[PointCounter] = py;
                                pMemory[FPos].px[PointCounter] = px;
                                pMemory[FPos].py[PointCounter] = py;
                            }
                            catch (System.NullReferenceException)
                            {
                                //    //int px = 0;
                                //    //int py = picHeight / 2;
                                //    //for (px = 0; px < picWidth; px++)
                                //    //{ bmp.SetPixel(px, py, Color.Purple); }


                                //continue;
                            }
                            catch (System.OverflowException)
                            {
                                //    //int px = picWidth / 2;
                                //    //int py = picHeight / 2;
                                //    //bmp.SetPixel(px, py, Color.Black);
                                //continue;
                            }
                        }

                        OnUsedMemory = 1;

                    }
                }
                else if(OnUsedMemory == 1)
                {
                    //多台相机2D点渲染
                    for (int k = 1; k <= 8; k++)
                    {
                        double BasicDegreeWorld = _Default.CameraDegree[k] * 3.14159265358 / 180;
                        //换算为图像坐标系角度
                        double BasicDegree = 90 * 3.14159265358 / 180 - BasicDegreeWorld;
                        ///MutexSock.WaitOne();

                        ///MutexSock.ReleaseMutex();
                        for (int j = 0; j < DataRows; j++)
                        {
                            PointCounter = (k - 1) * DataRows + j;
                            try
                            {




                                //计算x,y坐标
                                //float x = SockPack[k].s[j] * System.Convert.ToSingle(Math.Cos(BasicDegree - SockPack[k].ay[j]))/ ScaleParam + System.Convert.ToSingle(picWidth / 2);
                                //float y = SockPack[k].s[j] * System.Convert.ToSingle(Math.Sin(BasicDegree - SockPack[k].ay[j]))/ ScaleParam + System.Convert.ToSingle(picHeight / 2);
                                //int px = System.Convert.ToInt16(x);
                                //int py = System.Convert.ToInt16(y);
                                int px = (System.Convert.ToInt32(SockPack1[k].s[j] * Math.Cos(BasicDegree - SockPack1[k].ay[j]))) / ScaleParam + picWidth / 2;
                                int py = (System.Convert.ToInt32(SockPack1[k].s[j] * Math.Sin(BasicDegree - SockPack1[k].ay[j]))) / ScaleParam + picHeight / 2;
                                //int  px = 180;
                                //int py = 180;

                                //画布中渲染点
                                if (px > 0 && px < picWidth && py > 0 && py < picHeight)
                                {
                                    //取前次计算数据
                                    int lpx = pMemory[(FPos + FMemory - 1) % FMemory].px[PointCounter];
                                    int lpy = pMemory[(FPos + FMemory - 1) % FMemory].py[PointCounter];
                                    //与前帧进行比较并将不同处标出
                                    if (Math.Abs(lpx * lpx + lpy * lpy - px * px - py * py) > DetRange)
                                    {
                                        bmp.SetPixel(px, py, Color.Red);
                                        ColorCounter[PointCounter] = DelayedFrame;
                                    }
                                    else if (ColorCounter[PointCounter] > 0)
                                    {
                                        bmp.SetPixel(px, py, Color.Red);
                                        ColorCounter[PointCounter] = ColorCounter[PointCounter] - 1;
                                    }
                                    else
                                    {
                                        bmp.SetPixel(px, py, Color.Blue);
                                    }

                                }
                                OnUsedMemory = 1;


                                //存储点数据

                                //PointMemory pMemoryc = new PointMemory();
                                //pMemoryc.px[PointCounter] = px;
                                //pMemoryc.py[PointCounter] = py;
                                pMemory[FPos].px[PointCounter] = px;
                                pMemory[FPos].py[PointCounter] = py;
                            }
                            catch (System.NullReferenceException)
                            {
                                //    //int px = 0;
                                //    //int py = picHeight / 2;
                                //    //for (px = 0; px < picWidth; px++)
                                //    //{ bmp.SetPixel(px, py, Color.Purple); }


                                //continue;
                            }
                            catch (System.OverflowException)
                            {
                                //    //int px = picWidth / 2;
                                //    //int py = picHeight / 2;
                                //    //bmp.SetPixel(px, py, Color.Black);
                                //continue;
                            }
                        }

                        OnUsedMemory = 0;

                    }
                }


                FPos = (FPos + 1) % FMemory;


            


                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                string baseaddr = Convert.ToBase64String(ms.ToArray());
                //输出建模结果地址
                ModelImageUrl = "data:image/png;base64," + baseaddr;
                bmp.Dispose();
                ms.Dispose();


            }
            
        }

        //服务器初始化(输入端口的偏移 而非相机号)
        public void ServerInit(object PortEx)
        {
            int CameraNum = (int)PortEx;


            IPAddress ip = IPAddress.Parse(IPAddr);

            serverSocket[CameraNum] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                serverSocket[CameraNum].Bind(new IPEndPoint(ip, StartPort + CameraNum));  //绑定IP地址：端口

                serverSocket[CameraNum].Listen(10);    //设定最多10个排队连接请求
                                                       //if (System.Convert.ToBoolean(serverSocket.Available))
                                                       ///{

                Console.WriteLine("启动监听{0}成功", serverSocket[CameraNum].LocalEndPoint.ToString());

                //}


                //通过Clientsoket发送数据

                //Thread myThread = new Thread(ListenClientConnect);

                ListenClientConnect(CameraNum);

            }

            catch (System.Net.Sockets.SocketException)
            {
                return;
            }





        }

        //字符数组转结构体
        public static object BytesToStruct(byte[] bytes, Type type)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(type);
            //Byte数组小于结构体大小
            if (size > bytes.Length)
            {
                return null;
            }
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将byte数组拷贝到申请好的内存
            Marshal.Copy(bytes, 0, structPtr, size);
            //将内存空间格式化
            object obj = Marshal.PtrToStructure(structPtr, type);
            Marshal.FreeHGlobal(structPtr);
            return obj;

        }



        /// <summary>

        /// 监听客户端连接

        /// </summary>
        //接受并监听客户端
        private static void ListenClientConnect(object CameraNo)

        {
            int CameraNum = (int)CameraNo;

            while (true)

            {
                try
                {
                    Socket clientSocket = serverSocket[CameraNum].Accept();

                    //clientSocket.Send(Encoding.ASCII.GetBytes("Server Say Hello"));
                    //if (System.Convert.ToBoolean(clientSocket.Available))
                    //{
                    Thread receiveThread = new Thread(ReceiveMessage);

                    ReciveMessageParam param = new ReciveMessageParam();

                    param.CameraNum = CameraNum;

                    param.clientSocket = clientSocket;

                    receiveThread.Start(param);
                    //}
                }
                catch (System.ArgumentNullException)
                {
                    return;
                }
                catch (System.InvalidOperationException)
                {
                    return;
                }

            }

        }



        /// <summary>

        /// 接收消息

        /// </summary>

        /// <param name="clientSocket"></param>

        private static void ReceiveMessage(object obj)

        {
            ReciveMessageParam param = (ReciveMessageParam)obj;

            int CameraNum = param.CameraNum;

            Socket myClientSocket = param.clientSocket;

            int recievenum = 1;

            int packagepick = 0;

            //通过clientSocket接收数据
            List<byte> resultadd = new List<byte>();

            byte[] result = new byte[Packagebag];

            while (true)

            {
                try

                {
                    // while (System.Convert.ToBoolean(myClientSocket.Available))
                    //{
                    // int receiveNumber = myClientSocket.Receive(result);
                    //Console.WriteLine("111")
                    //}
                    //while (recievenum != 0)
                    //{

                    byte[] resultonce = new byte[300000];

                    recievenum = myClientSocket.Receive(resultonce);

                    //判断读出的为新字节
                    if (recievenum != 0)
                    {
                        resultadd.AddRange(resultonce);
                        packagepick = packagepick + recievenum;
                    }


                    //for (int i = packagepick; i < packagepick + recievenum; i++)
                    //{
                    //    resultadd[i] = resultonce[i-packagepick];
                    //}


                    //总字节数超过一包
                    if (packagepick >= Packagebag)
                    {
                        //得出所在包数据
                        result = resultadd.ToArray().Skip(0).Take(Packagebag).ToArray();
                        //其后数据迁移
                        byte[] Exchange = resultadd.ToArray().Skip(Packagebag).Take(packagepick - Packagebag).ToArray();

                        packagepick = packagepick - Packagebag;

                        resultadd.Clear();
                        resultadd.AddRange(Exchange);

                        Thread HandleDataThread = new Thread(HandleData);
                        HandleDataThread.Start(result);
                        //Thread DrawImageThread = new Thread(DrawPixelImage); 

                        //DrawImageThread.Start(dparam);
                        // SockPack SockPicture = new SockPack();
                        // SockPicture = (SockPack)BytesToStruct(result, SockPicture.GetType());


                        // Console.WriteLine("{0}帧图像,第一个像素值为{1},最后一个像素值为{2}", SockPicture.Framecnt.ToString(), SockPicture.Picture[0].ToString(), SockPicture.Picture[259 * 205 - 1].ToString());

                    }




                    //Console.WriteLine("接收客户端{0}消息{1}", myClientSocket.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(result, 0, receiveNumber));




                    //Console.WriteLine("接收客户端{0}消息{1}", myClientSocket.RemoteEndPoint.ToString(), result);
                }

                catch (Exception ex)

                {

                    Console.WriteLine(ex.Message);

                    myClientSocket.Shutdown(SocketShutdown.Both);

                    myClientSocket.Close();

                    break;

                }
            }
        }

        public static void HandleData(object obj)
        {
            byte[] Datasrc = (byte[]) obj;
            SockPackage SockPackc = new SockPackage();
            SockPackc = (SockPackage)BytesToStruct(Datasrc, SockPackc.GetType());

            char[] SerialNum = SockPackc.SerialNum;
            int CameraNum = SerialNum2CameraNum(SerialNum);
            if(CameraNum>0&&CameraNum<9)
            {
                if(OnUsedMemory == 0)
                {
                    SockPack1[CameraNum] = SockPackc;
                }
                else if(OnUsedMemory ==1)
                {
                    SockPack0[CameraNum] = SockPackc;
                }
                //SockPack[CameraNum] = SockPackc;
                
            }
            return;
        }

        


        //序列号和相机号转换
        public static int SerialNum2CameraNum(object serialNum)
        {
            char[] SerialNumc = (char[])serialNum;
            int SerialLength = 0;
            for (int i = 0; i < 64; i++)
            {
                if (SerialNumc[i].ToString() != "\0")
                {
                    ;
                }
                else
                {
                    SerialLength = i;
                    break;
                }
            }

            string SerialNums = new string(SerialNumc);
            string SerialNum = SerialNums.Substring(0, SerialLength);
            int CameraNum = 999;
            for (int i = 0; i < 9; i++)
            {
                if (SerialNum == _Default.CameraSerial[i])
                {
                    CameraNum = i;
                }
            }
            return CameraNum;
        }

    }
}