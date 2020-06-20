using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;

namespace WebAppOnBus
{
    /// <summary>
    /// Summary description for ActionHandler
    /// </summary>
    public class ActionHandler : IHttpHandler
    {
        //工作状态指示
        public static bool SwitchOn = false;
        public static DateTime StartTime;

        //网络参数
        public static string IPAddr = _Default.GetIpAddr();
        public static int StartPort = 8380;
        public static Socket[] clientSocket = new Socket[3];
        //static string Comp1IP = _Default.Comp1IP;
        //static string Comp2IP = _Default.Comp2IP;
        public static int ConnectPort = 8180;

        //Timer Param
        public static bool TimerExit = false;
        //Time Interval of checking
        public static int Intime = 180000;

        public struct ControlPack
        {
            public int FrameHead;
            public int func;
            public int choice;
            public int data1;
            public int data2;
            public float data3;
            public float data4;
            public int FrameTail;
        }

        public struct SendParam
        {
            public int CompNum;
            public ControlPack CtrlPack;
            
        }

        public struct ClientInitParam
        {
            public int Compnum;
            public string Compip;
        }

        public static ControlPack CtrlPack = new ControlPack();
        //获取包大小
        public static int Packagebag = Marshal.SizeOf(CtrlPack);

        public struct ReciveMessageParam
        {
            public int CameraNum;
            public Socket clientSocket;
        }

        //public const int Packagebag = Marshal.SizeOf(CtrlPack);
        //public int BagSize = 0;
        
        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string action = context.Request.Params["action"].ToLower(); 
            switch(action)
            {
                case "startaction":
                    StartServer(context);
                    break;
                case "startcamera":
                    StartCamera(context);
                    break;
                case "setcond":
                    SetCond(context);
                    break;
            }
        }
        public void SetCond(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            //开启状态展示工作时间
            if (SwitchOn)
            {
                TimeSpan timed = DateTime.Now - StartTime;
                String text = "采集开始  运行时间：   " + timed.ToString();
                context.Response.Write(text);
            }
            //未开启状态展示当前世界时间
            else
            {
                // String text = "当前时间：" + DateTime.Now.Year + "/"+ DateTime.Now.Month + "/" + DateTime.Now.Day + "   " + DateTime.Now.Hour +":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond;
                String text = "采集停止  当前时间:    " + DateTime.Now.ToString("yyyy/MM/dd  HH:mm:ss:fff");
                context.Response.Write(text);

            }
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int date = DateTime.Now.Day;
            
            context.Response.End();
        }

        public void StartCamera(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            if (SwitchOn)
            {
                TimerExit = true;
                ControlPack Startpack = new ControlPack();
                Startpack.FrameHead = 55;
                Startpack.FrameTail = 56;
                Startpack.func = 2;
                Startpack.choice = 0;
                SendParam Sendparam1, Sendparam2, Sendparam3;
                Sendparam1.CompNum = 0;
                Sendparam1.CtrlPack = Startpack;
                Sendparam2.CompNum = 1;
                Sendparam2.CtrlPack = Startpack;
                Sendparam3.CompNum = 2;
                Sendparam3.CtrlPack = Startpack;
                try
                {
                    SendMessage(Sendparam1);
                    
                }
                catch (System.Net.Sockets.SocketException)
                {
                    //context.Response.Write(true);
                    //context.Response.End();
                    //return;
                }
                try
                {
                    
                    SendMessage(Sendparam2);
                    
                }
                catch (System.Net.Sockets.SocketException)
                {
                    //context.Response.Write(true);
                    //context.Response.End();
                    //return;
                }
                try
                {
                    SendMessage(Sendparam3);
                }
                catch (System.Net.Sockets.SocketException)
                {
                    //context.Response.Write(true);
                    //context.Response.End();
                    //return;
                }
                TimerExit = true;
                SwitchOn = false;
                context.Response.Write(true);
            }
            else if (!SwitchOn)
            {
                
                //Startup
                StartTime = DateTime.Now;
                int MSO = StartTime.Hour * 60 * 60 * 1000 + StartTime.Minute * 60 * 1000 + StartTime.Second * 1000 + StartTime.Millisecond;
                ControlPack Startpack = new ControlPack();
                Startpack.FrameHead = 55;
                Startpack.FrameTail = 56;
                Startpack.func = 1;
                Startpack.choice = 1;
                Startpack.data1 = MSO;
                SendParam Sendparam1, Sendparam2,Sendparam3;
                Sendparam1.CompNum = 0;
                Sendparam1.CtrlPack = Startpack;
                Sendparam2.CompNum = 1;
                Sendparam2.CtrlPack = Startpack;
                Sendparam3.CompNum = 2;
                Sendparam3.CtrlPack = Startpack;
                try
                {
                    SendMessage(Sendparam1);                
                }
                catch(System.Net.Sockets.SocketException)
                {
                    //context.Response.Write(true);
                    //context.Response.End();
                    //return;
                }
                try
                {
                    
                    SendMessage(Sendparam2);
                    
                }
                catch (System.Net.Sockets.SocketException)
                {
                    //context.Response.Write(true);
                    //context.Response.End();
                    //return;
                }
                try
                {
                    
                    SendMessage(Sendparam3);
                }
                catch (System.Net.Sockets.SocketException)
                {
                    //context.Response.Write(true);
                    //context.Response.End();
                    //return;
                }
                SwitchOn = true;
                context.Response.Write(false);
                TimerExit = false;
                Thread timemanager = new Thread(TimeManager);
                timemanager.Start();
            }
            context.Response.End();
            
        }

        public void TimeManager()
        {
            while(!TimerExit)
            {
                Thread.Sleep(Intime);
                SyncTimer();
            }
        }

        public void SyncTimer()
        {
            StartTime = DateTime.Now;
            int MSO = StartTime.Hour * 60 * 60 * 1000 + StartTime.Minute * 60 * 1000 + StartTime.Second * 1000 + StartTime.Millisecond;
            ControlPack Startpack = new ControlPack();
            Startpack.FrameHead = 55;
            Startpack.FrameTail = 56;
            Startpack.func = 1;
            Startpack.choice = 2;
            Startpack.data1 = MSO;
            SendParam Sendparam1, Sendparam2, Sendparam3;
            Sendparam1.CompNum = 0;
            Sendparam1.CtrlPack = Startpack;
            Sendparam2.CompNum = 1;
            Sendparam2.CtrlPack = Startpack;
            Sendparam3.CompNum = 2;
            Sendparam3.CtrlPack = Startpack;
            try
            {
                SendMessage(Sendparam1);
            }
            catch (System.Net.Sockets.SocketException)
            {
                //context.Response.Write(true);
                //context.Response.End();
                //return;
            }
            try
            {

                SendMessage(Sendparam2);

            }
            catch (System.Net.Sockets.SocketException)
            {
                //context.Response.Write(true);
                //context.Response.End();
                //return;
            }
            try
            {

                SendMessage(Sendparam3);
            }
            catch (System.Net.Sockets.SocketException)
            {
                //context.Response.Write(true);
                //context.Response.End();
                //return;
            }
        }

        public void StartServer(HttpContext context)
        {
            
            
            Thread ServerInitThread0 = new Thread(ClientInit);
            Thread ServerInitThread1 = new Thread(ClientInit);
            Thread ServerInitThread2 = new Thread(ClientInit);
            ClientInitParam client0param, client1param, client2param;
            client0param.Compip = _Default.Comp0IP;
            client0param.Compnum = 2;
            client1param.Compip = _Default.Comp1IP;
            client1param.Compnum = 0;
            client2param.Compip = _Default.Comp2IP;
            client2param.Compnum = 1;
            ServerInitThread0.Start(client1param);
            ServerInitThread1.Start(client2param);
            ServerInitThread2.Start(client0param);
        }

        //客户端初始化
        public void ClientInit(object initparam)
        {
            ClientInitParam param = (ClientInitParam)initparam;
            string ipaddr = param.Compip;
            int compnum = param.Compnum;
            IPAddress ip = IPAddress.Parse(ipaddr);
            clientSocket[compnum] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket[compnum].Connect(new IPEndPoint(ip, ConnectPort));
                Thread Recievethread = new Thread(ReceiveMessage);
                Recievethread.Start(compnum);
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

        //struct转换为byte[]
        public static byte[] StructToBytes(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structObj, buffer, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        

        private static void SendMessage(object obj)
        {
            SendParam param = (SendParam)obj;
            ControlPack CtrlPack = param.CtrlPack;
            int Compnum = param.CompNum;
            //Socket myClientSocket = param.clientSocket;
            byte[] buffer = StructToBytes(CtrlPack);
            try
            {
                clientSocket[Compnum].Send(buffer);
            }
            catch(NullReferenceException)
            {
                return;
            }
            
        }



        /// <summary>

        /// 接收消息

        /// </summary>

        /// <param name="clientSocket"></param>

        private static void ReceiveMessage(object obj)

        {

            int CompNum = (int)obj;

            int recievenum = 1;

            int packagepick = 0;

            //Packagebag = Marshal.SizeOf(ControlPack);

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

                    recievenum = clientSocket[CompNum].Receive(resultonce);

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

                        //Thread DrawImageThread = new Thread(DrawPixelImage);

                        //DrawImageParam dparam = new DrawImageParam();

                        //dparam.CameraNum = CameraNum;

                        //dparam.imagesource = (byte[])result.Clone();

                        //DrawImageThread.Start(dparam);
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

                    clientSocket[CompNum].Shutdown(SocketShutdown.Both);

                    clientSocket[CompNum].Close();

                    break;

                }
            }

        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}