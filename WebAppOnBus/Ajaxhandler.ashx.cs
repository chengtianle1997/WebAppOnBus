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

namespace WebAppOnBus
{
    /// <summary>
    /// Summary description for Ajaxhandler
    /// </summary>
    /// 
  


    public class Ajaxhandler : IHttpHandler
    {
        //public static string IPAddr = "192.168.31.100";
        public static string IPAddr = _Default.GetIpAddr();
        public static int StartPort = 8080;
        static Socket[] serverSocket = new Socket[10];
        //图片大小常量设定
        public const int picHeight = 205;
        public const int picWidth = 259;

        //包大小常量设定
        public const int Packagebag = picWidth * picHeight * 4 + 4 + 64;

        //图像封装
        public struct SockPack
        {
            //帧号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public char[] SerialNum;
            public int Framecnt;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = picHeight * picWidth)]
            //图像像素信息
            public int[] Picture;
        }

        //图片路径存储
        public static string[] m_ImageUrl = new string [10];
        public static string[] ImageUrl
        {
            get
            {
                return m_ImageUrl;
            }
            set
            {
                string[] m_ImageUrl = (string[])value.Clone();
            }
        }
        //帧数存储
        public static string[] m_Frame = new string [10];
        public static string[] Frame
        {
            get
            {
                return m_Frame;
            }
            set
            {
                string[] m_Frame = (string[])value.Clone();
            }
        }

        //ServerInit函数ip和port
        public struct ServerInitParam
        {
            public string ip;
            public int port;
        }

        //ReceiveMessage函数传参结构体
        public struct ReciveMessageParam
        {
            public int CameraNum;
            public Socket clientSocket;
        }

        //DrawImage函数传参结构体
        public struct DrawImageParam
        {
           // [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
           // public char[] SerialNum;
            public int CameraNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = Packagebag)]
            //图片像素信息
            public byte[] imagesource;
        }

        public static bool ListenClientExit = false;

        //处理页面请求
        public void ProcessRequest(HttpContext context)
        {
            
            string action = context.Request.Params["action"].ToLower();
            switch (action)
            {
                case "startserver":
                    StartServer(context);
                    break;
                case "getimage0":
                    GetImage0(context);
                    break;
                case "getimage1":
                    GetImage1(context);
                    break;
                case "getimage2":
                    GetImage2(context);
                    break;
                case "getimage3":
                    GetImage3(context);
                    break;
                case "getimage4":
                    GetImage4(context);
                    break;
                case "getimage5":
                    GetImage5(context);
                    break;
                case "getimage6":
                    GetImage6(context);
                    break;
                case "getimage7":
                    GetImage7(context);
                    break;
                case "getimage8":
                    GetImage8(context);
                    break;
                case "getframe0":
                    GetFrame0(context);
                    break;
                case "getframe1":
                    GetFrame1(context);
                    break;
                case "getframe2":
                    GetFrame2(context);
                    break;
                case "getframe3":
                    GetFrame3(context);
                    break;
                case "getframe4":
                    GetFrame4(context);
                    break;
                case "getframe5":
                    GetFrame5(context);
                    break;
                case "getframe6":
                    GetFrame6(context);
                    break;
                case "getframe7":
                    GetFrame7(context);
                    break;
                case "getframe8":
                    GetFrame8(context);
                    break;


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
            //Thread ServerInitThread9 = new Thread(ServerInit);
            ServerInitThread0.Start(0);
            ServerInitThread1.Start(1);
            ServerInitThread2.Start(2);
            ServerInitThread3.Start(3);
            ServerInitThread4.Start(4);
            ServerInitThread5.Start(5);
            ServerInitThread6.Start(6);
            ServerInitThread7.Start(7);
            ServerInitThread8.Start(8);
            //ServerInitThread8.Start(9);

        }
        //设置图像和信息
        public void GetImage0(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(ImageUrl[0]);
            context.Response.End();
        }
        public void GetImage1(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(ImageUrl[1]);
            context.Response.End();
        }
        public void GetImage2(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(ImageUrl[2]);
            context.Response.End();
        }
        public void GetImage3(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(ImageUrl[3]);
            context.Response.End();
        }
        public void GetImage4(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(ImageUrl[4]);
            context.Response.End();
        }
        public void GetImage5(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(ImageUrl[5]);
            context.Response.End();
        }
        public void GetImage6(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(ImageUrl[6]);
            context.Response.End();
        }
        public void GetImage7(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(ImageUrl[7]);
            context.Response.End();
        }
        public void GetImage8(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(ImageUrl[8]);
            context.Response.End();
        }
        public void GetFrame0(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(Frame[0]);
            context.Response.End();
        }
        public void GetFrame1(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(Frame[1]);
            context.Response.End();
        }
        public void GetFrame2(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(Frame[2]);
            context.Response.End();
        }
        public void GetFrame3(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(Frame[3]);
            context.Response.End();
        }
        public void GetFrame4(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(Frame[4]);
            context.Response.End();
        }
        public void GetFrame5(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(Frame[5]);
            context.Response.End();
        }
        public void GetFrame6(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(Frame[6]);
            context.Response.End();
        }
        public void GetFrame7(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(Frame[7]);
            context.Response.End();
        }
        public void GetFrame8(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.Write(Frame[8]);
            context.Response.End();
        }



        //服务器初始化(输入端口的偏移 而非相机号)
        public void ServerInit(object ipparam)
        {
            int CameraNum = (int)ipparam;
           // ServerInitParam Sip = (ServerInitParam)ipparam;

           // int CameraNum = Sip.port;

            IPAddress ip = IPAddress.Parse(IPAddr);

            serverSocket[CameraNum] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                serverSocket[CameraNum].Bind(new IPEndPoint(ip, StartPort+CameraNum));  //绑定IP地址：端口

                serverSocket[CameraNum].Listen(10);    //设定最多20个排队连接请求
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

                    Thread.Sleep(5);
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

                        Thread DrawImageThread = new Thread(DrawPixelImage);

                        DrawImageParam dparam = new DrawImageParam();

                        dparam.CameraNum = CameraNum;

                        dparam.imagesource = (byte[])result.Clone();

                        DrawImageThread.Start(dparam);
                        // SockPack SockPicture = new SockPack();
                        // SockPicture = (SockPack)BytesToStruct(result, SockPicture.GetType());


                        // Console.WriteLine("{0}帧图像,第一个像素值为{1},最后一个像素值为{2}", SockPicture.Framecnt.ToString(), SockPicture.Picture[0].ToString(), SockPicture.Picture[259 * 205 - 1].ToString());

                    }


                    Thread.Sleep(5);

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

        //绘制图片
        private static void DrawPixelImage(object obj)
        {
            try
            {
                DrawImageParam param = (DrawImageParam)obj;
                //int CameraNum = param.CameraNum;
                byte[] imgcp = (byte[])param.imagesource;
                //byte[] imgcp = (byte[])imgsrc;
                SockPack SockPicture = new SockPack();
                SockPicture = (SockPack)BytesToStruct(imgcp, SockPicture.GetType());
                char[] SerialNum = SockPicture.SerialNum;
                int CameraNum = SerialNum2CameraNum(SerialNum);
                Bitmap bmp = new Bitmap(picWidth, picHeight);
                for (int i = 0; i < picHeight; i++)
                {
                    for (int j = 0; j < picWidth; j++)
                    {
                        if (SockPicture.Picture[i * picWidth + j] <= 255 && SockPicture.Picture[i * picWidth + j] >= 0)
                        {
                            bmp.SetPixel(j, i, Color.FromArgb(SockPicture.Picture[i * picWidth + j], SockPicture.Picture[i * picWidth + j], SockPicture.Picture[i * picWidth + j]));
                        }
                        else
                        {
                            bmp.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                        }
                        //bmp.SetPixel(j, i, Color.FromArgb(SockPicture.Picture[i*picWidth+j], SockPicture.Picture[i * picWidth + j], SockPicture.Picture[i * picWidth + j]));
                    }
                }
                //byte[] filepath = System.IO.File.ReadAllBytes("f0.jpg");

                //FileStream fs = new FileStream("f0.jpg",FileMode.OpenOrCreate,FileAccess.Write,FileShare.ReadWrite);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                string base64 = Convert.ToBase64String(ms.ToArray());
                //if (@default.Label4.InvokeRequired)
                //{

                //}


                ImageUrl[CameraNum] = "data:image/png;base64," + base64;
                Frame[CameraNum] = SockPicture.Framecnt.ToString();

                bmp.Dispose();
                ms.Dispose();
                //SockPicture.Picture
            }
            catch (System.IndexOutOfRangeException)
            {
                return;
            }

            return;
            


        }

        //序列号和相机号转换
        public static int SerialNum2CameraNum(object serialNum)
        {
            char[] SerialNumc = (char[])serialNum;
            int SerialLength = 0;
            for(int i = 0; i < 64; i++)
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
            for(int i = 0; i < 9; i++)
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