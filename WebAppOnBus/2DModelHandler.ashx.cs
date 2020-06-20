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
        public static int StartPort = 8090;
        static Socket[] serverSocket = new Socket[10];

        //功能开启
        public static bool CloudOn = true;
        public static bool SmoothOn = false;

        

        public static bool FitOn = false;
        public static bool IdenOn = false;
        

        //包大小
        public const int DataRows = 204;
        public const int Packagebag = 64+4+sizeof(Single)*2*DataRows;

        //数据行数
        public const int DataRowsRAW = 2048;

        //图片大小常量设定
        public const int picHeight = 359;
        public const int picWidth = 359;
        //数据表大小常量设定
        public const int formHeight = 150;
        //图像整体大小设定
        public const int picHeightAll = picHeight + formHeight;
        
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

        //最小二乘圆弧拟合采样率（每n点采1点）
        public int SampFreq = 25;
        //采样范围
        public int StartNum = DataRows * 1;
        public int StopNum = DataRows * 7;
        //显示范围
        public int StartDispNum = DataRows / 2;
        public int StopDispNum = 15*DataRows / 2;

        //点阵颜色
        public Color PointColor = Color.Blue;
        //拟合结果颜色
        public Color FitResColor = Color.Green;
        //标注字体颜色
        public Color FontColor = Color.White;
        //标注箭头颜色
        public Color ArrowColor = Color.Red;
        //画布背景颜色
        public Color BackColor = Color.Gray;


        //退出生成模型
        public bool Generate2DExit = false;
        public bool Initialized = true;
        public bool AnalyzeDataExit = false;
        public bool LoadDataExit = false;


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
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = DataRows)]
            //public float[] x;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = DataRows)]
            //public float[] y;
        }

        //数据点缓存
        public struct PointMemory
        {
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = DataRows*8)]
            //public Single[] x;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = DataRows*8)]
            //public Single[] y;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = DataRows * 8)]
            public int[] px;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = DataRows * 8)]
            public int[] py;
        }

        //拟合圆
        public struct Circle
        {
            //X：圆心横坐标  Y:圆心纵坐标
            //R：圆半径
            public double X;
            public double Y;
            public double R;

        }

        //拟合点
        public struct Point
        {
            public int x;
            public int y;
        }

        //拟合结果展示
        //public struct ResultDisp
        //{

        //    public int R1;
        //    public int R2;
        //    public int R3;
        //    public int R4;
        //    public int R5;
        //    public int R6;
        //    public int R7;
        //    //public int R8;

        //}

        public int[] m_ResultDisp = new int[8];
        public int[] ResultDisp
        {
            get
            {
                return m_ResultDisp;
            }
            set
            {
                int[] m_ResultDisp = (int[])value.Clone();
            }
        }

        

        //拟合标尺
        public double R1a = 90 * 3.1415926535 / 180 + (45 * 3.1415926535 / 180);//左下
        public double R2a = 90 * 3.1415926535 / 180 + (90 * 3.1415926535 / 180);//左
        public double R3a = 90 * 3.1415926535 / 180 + (135 * 3.1415926535 / 180);//左上
        public double R4a = 90 * 3.1415926535 / 180 + (180 * 3.1415926535 / 180);//上
        public double R5a = 90 * 3.1415926535 / 180 + (225 * 3.1415926535 / 180);//右上
        public double R6a = 90 * 3.1415926535 / 180 + (270 * 3.1415926535 / 180);//右
        public double R7a = 90 * 3.1415926535 / 180 + (315 * 3.1415926535 / 180);//右下
        //public double R8a = 90 * 3.1415926535 / 180 + (360 * 3.1415926535 / 180);

        //相似基准判据
        public double Rea = 0.005;

 
        //相机状态指示 0为不在位 1为在位
        public static int[] m_CameraOn = new int[9];
        public static int [] CameraOn
        {
            get
            {
                return m_CameraOn;
            }
            set
            {
                int[] m_CameraOn = (int[])value.Clone();
            }
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
        public static bool[] m_Sock0Cond = new bool [9];
        public static bool[] Sock0Cond
        {
            get
            {
                return m_Sock0Cond;
            }
            set
            {
                bool[] m_Sock0Cond = (bool[])value.Clone();
            }
        }
        public static bool m_Sock0Ready = false;
        public static bool Sock0Ready
        {
            get
            {
                return m_Sock0Ready;
            }
            set
            {
                bool m_Sock0Ready = value;
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
        public static bool[] m_Sock1Cond = new bool[9];
        public static bool[] Sock1Cond
        {
            get
            {
                return m_Sock1Cond;
            }
            set
            {
                bool[] m_Sock1Cond =(bool[]) value.Clone();
            }
        }
        public static bool m_Sock1Ready = false;
        public static bool Sock1Ready
        {
            get
            {
                return m_Sock1Ready;
            }
            set
            {
                bool m_Sock1Ready = value;
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

        //标注数据存储
        //public static ResultDisp m_ResDisp;
        //public static ResultDisp ResDisp
        //{
        //    get
        //    {
        //        return m_ResDisp;
        //    }
        //    set
        //    {
        //        m_ResDisp = value;
        //    }
        //}





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
                case "cloudon":
                    CloudOn = true;
                    break;
                case "cloudoff":
                    CloudOn = false;
                    break;
                case "filteron":
                    SmoothOn = true;
                    break;
                case "filteroff":
                    SmoothOn = false;
                    break;
                case "fiton":
                    FitOn = true;
                    break;
                case "fitoff":
                    FitOn = false;
                    break;
                case "idenon":
                    IdenOn = true;
                    break;
                case "idenoff":
                    IdenOn = false;
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
                //相机状态信息初始化 缓存状态信息初始化
                //for (int i = 0; i < 9; i++)
                //{
                //    CameraOn[i] = 0;
                //    Sock1Cond[i] = false;
                //    Sock0Cond[i] = false;
                //}
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
            Thread Generate2DImageThread1 = new Thread(LoadData);
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

        //载入数据点
        public void LoadData()
        {
            while (!LoadDataExit)
            {
                //数据点计数
                int PointCounter = 0;
                //读取缓存0
                if (OnUsedMemory == 0)
                {
                    //多台相机数据读取
                    for(int k = 1; k <= 8; k++)
                    {
                        double BasicDegreeWorld = _Default.CameraDegree[k] * 3.14159265358 / 180;
                        //换算为图像坐标系角度
                        double BasicDegree = 90 * 3.14159265358 / 180 + BasicDegreeWorld;
                        //遍历该相机数据
                        for(int j = 0;j<DataRows;j++)
                        {
                            PointCounter = (k) * DataRows - j -1;
                            try
                            {
                                int px = 0;
                                int py = 0;
                                //读取并存储实际距离（mm）到px py
                                if (SockPack0[k].s[j] == 0 && SockPack0[k].ay[j] == 0)
                                {
                                    px = pMemory[(FPos + FMemory - 1) % FMemory].px[PointCounter];
                                    py = pMemory[(FPos + FMemory - 1) % FMemory].py[PointCounter];
                                }
                                else
                                {
                                    px = (System.Convert.ToInt32(SockPack0[k].s[j] * Math.Cos(BasicDegree + SockPack0[k].ay[j])));
                                    py = (System.Convert.ToInt32(SockPack0[k].s[j] * Math.Sin(BasicDegree + SockPack0[k].ay[j])));
                                    if (FitOn)
                                    {
                                        IsIdent(PointCounter, BasicDegree + SockPack0[k].ay[j]);
                                    }
                                                                
                                }


                                //计算x,y坐标
                                //float x = SockPack[k].s[j] * System.Convert.ToSingle(Math.Cos(BasicDegree - SockPack[k].ay[j]))/ ScaleParam + System.Convert.ToSingle(picWidth / 2);
                                //float y = SockPack[k].s[j] * System.Convert.ToSingle(Math.Sin(BasicDegree - SockPack[k].ay[j]))/ ScaleParam + System.Convert.ToSingle(picHeight / 2);
                                //int px = System.Convert.ToInt16(x);
                                //int py = System.Convert.ToInt16(y);

                                //int  px = 180;
                                //int py = 180;

                                ////处理数据
                                //if (px > 0 && px < picWidth && py > 0 && py < picHeight)
                                //{
                                //    //取前次计算数据
                                //    int lpx = pMemory[(FPos + FMemory - 1) % FMemory].px[PointCounter];
                                //    int lpy = pMemory[(FPos + FMemory - 1) % FMemory].py[PointCounter];
                                //    //与前帧进行比较并将不同处标出
                                //    if (Math.Abs(lpx * lpx + lpy * lpy - px * px - py * py) > DetRange)
                                //    {
                                //        bmp.SetPixel(px, py, Color.Red);
                                //        ColorCounter[PointCounter] = DelayedFrame;
                                //    }
                                //    else if (ColorCounter[PointCounter] > 0)
                                //    {
                                //        bmp.SetPixel(px, py, Color.Red);
                                //        ColorCounter[PointCounter] = ColorCounter[PointCounter] - 1;
                                //    }
                                //    else
                                //    {
                                //        bmp.SetPixel(px, py, Color.Blue);
                                //    }

                                //}
                                //OnUsedMemory = 1;


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
                    }
                    //所有相机数据读取完毕 开始分析
                    if (FitOn)
                    {
                        Circle pCircle = AnalyzeData(FPos);
                        Disp2DImageFit(pCircle, FPos);
                    }
                    else
                    {
                        Disp2DImage(FPos);
                    }
                    

                    //分析结束 展示结果
                    //Disp2DImage(pCircle, FPos);

                    //OnUsedMemory = 1;
                }

                else if (OnUsedMemory == 1)
                {
                    //多台相机2D点渲染
                    for (int k = 1; k <= 8; k++)
                    {
                        //if (!System.Convert.ToBoolean(CameraOn[k]))
                        //{
                        //    continue;
                        //}
                        double BasicDegreeWorld = _Default.CameraDegree[k] * 3.14159265358 / 180;
                        //换算为图像坐标系角度
                        double BasicDegree = 90 * 3.14159265358 / 180 + BasicDegreeWorld;
                        ///MutexSock.WaitOne();
                        ///MutexSock.ReleaseMutex();
                        for (int j = 0; j < DataRows; j++)
                        {
                            PointCounter = (k) * DataRows - j - 1;
                            try
                            {

                                int px = 0;
                                int py = 0;
                                if (SockPack1[k].s[j] == 0 && SockPack1[k].ay[j] == 0)
                                {
                                    px = pMemory[(FPos + FMemory - 1) % FMemory].px[PointCounter];
                                    py = pMemory[(FPos + FMemory - 1) % FMemory].py[PointCounter];
                                }
                                else
                                {
                                    px = (System.Convert.ToInt32(SockPack1[k].s[j] * Math.Cos(BasicDegree + SockPack1[k].ay[j]))) ;
                                    py = (System.Convert.ToInt32(SockPack1[k].s[j] * Math.Sin(BasicDegree + SockPack1[k].ay[j]))) ;
                                    if (FitOn)
                                    {
                                        IsIdent(PointCounter, BasicDegree + SockPack1[k].ay[j]);
                                    }
                                }


                                //计算x,y坐标
                                //float x = SockPack[k].s[j] * System.Convert.ToSingle(Math.Cos(BasicDegree - SockPack[k].ay[j]))/ ScaleParam + System.Convert.ToSingle(picWidth / 2);
                                //float y = SockPack[k].s[j] * System.Convert.ToSingle(Math.Sin(BasicDegree - SockPack[k].ay[j]))/ ScaleParam + System.Convert.ToSingle(picHeight / 2);
                                //int px = System.Convert.ToInt16(x);
                                //int py = System.Convert.ToInt16(y);
                                //int px = (System.Convert.ToInt32(SockPack1[k].s[j] * Math.Cos(BasicDegree - SockPack1[k].ay[j]))) / ScaleParam + picWidth / 2;
                                //int py = (System.Convert.ToInt32(SockPack1[k].s[j] * Math.Sin(BasicDegree - SockPack1[k].ay[j]))) / ScaleParam + picHeight / 2;
                                //int  px = 180;
                                //int py = 180;

                                //画布中渲染点
                                //if (px > 0 && px < picWidth && py > 0 && py < picHeight)
                                //{
                                //    //取前次计算数据
                                //    int lpx = pMemory[(FPos + FMemory - 1) % FMemory].px[PointCounter];
                                //    int lpy = pMemory[(FPos + FMemory - 1) % FMemory].py[PointCounter];
                                //    //与前帧进行比较并将不同处标出
                                //    if (Math.Abs(lpx * lpx + lpy * lpy - px * px - py * py) > DetRange)
                                //    {
                                //        bmp.SetPixel(px, py, Color.Red);
                                //        ColorCounter[PointCounter] = DelayedFrame;
                                //    }
                                //    else if (ColorCounter[PointCounter] > 0)
                                //    {
                                //        bmp.SetPixel(px, py, Color.Red);
                                //        ColorCounter[PointCounter] = ColorCounter[PointCounter] - 1;
                                //    }
                                //    else
                                //    {
                                //        bmp.SetPixel(px, py, Color.Blue);
                                //    }

                                //}
                                //OnUsedMemory = 1;


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

                        //所有相机数据读取完毕 开始分析
                        if (FitOn)
                        {
                            Circle pCircle = AnalyzeData(FPos);
                            Disp2DImageFit(pCircle, FPos);
                        }
                        else
                        {
                            Disp2DImage(FPos);

                        }

                    }
                    OnUsedMemory = 0;
                }


                FPos = (FPos + 1) % FMemory;
            }

        }

        //数据分析
        public Circle AnalyzeData(int FPos)
        {
            //采样
            List<Point> PointList = new List<Point>();
            int CountNum = 0;
            for(int i = StartNum;i<StopNum;i++)
            {
                if(i%SampFreq == 0)
                {
                    Point point = new Point();
                    point.x = pMemory[FPos].px[i];
                    point.y = pMemory[FPos].py[i];
                    PointList.Add(point);
                }
            }
            //拟合
            Circle pCircle = new Circle();
            if(PointList.Count <3)
            {
                pCircle.R = 2750;
                pCircle.X = 0;
                pCircle.Y = 0;
                return pCircle;
            }
            double X1 = 0;
            double Y1 = 0;
            double X2 = 0;
            double Y2 = 0;
            double X3 = 0;
            double Y3 = 0;
            double X1Y1 = 0;
            double X1Y2 = 0;
            double X2Y1 = 0;
            for (int i = 0;i<PointList.Count;i++)
            {
                X1 = X1 + PointList[i].x;
                Y1 = Y1 + PointList[i].y;
                X2 = X2 + PointList[i].x * PointList[i].x;
                Y2 = Y2 + PointList[i].y * PointList[i].y;
                X3 = X3 + PointList[i].x * PointList[i].x * PointList[i].x;
                Y3 = Y3 + PointList[i].y * PointList[i].y * PointList[i].y;
                X1Y1 = X1Y1 + PointList[i].x * PointList[i].y;
                X1Y2 = X1Y2 + PointList[i].x * PointList[i].y * PointList[i].y;
                X2Y1 = X2Y1 + PointList[i].x * PointList[i].x * PointList[i].y;
            }
            double C, D, E, G, H, N;
            double a, b, c;
            N = PointList.Count;
            C = N * X2 - X1 * X1;
            D = N * X1Y1 - X1 * Y1;
            E = N * X3 + N * X1Y2 - (X2 + Y2) * X1;
            G = N * Y2 - Y1 * Y1;
            H = N * X2Y1 + N * Y3 - (X2 + Y2) * Y1;
            a = (H * D - E * G) / (C * G - D * D);
            b = (H * C - E * D) / (D * D - G * C);
            c = -(a * X1 + b * Y1 + X2 + Y2) / N;
            pCircle.X = a / (-2);
            pCircle.Y = b / (-2);
            pCircle.R = Math.Sqrt(a * a + b * b - 4 * c) / 2;
            return pCircle;
        }

        //展示结果(拟合标注）
        public void Disp2DImageFit(Circle pCircle,int FPos)
        {
            //图像初始化
            Bitmap bmp = new Bitmap(picWidth, picHeightAll);
            Graphics g = Graphics.FromImage(bmp);
            //设定背景色
            g.Clear(BackColor);
            //设定点云颜色
            Pen pData = new Pen(PointColor);
            //设定字体及颜色
            SolidBrush bRes = new SolidBrush(FontColor);
            Font font = new Font("Times New Roman", 12);
            //g.DrawString("R0", font, bRes, new PointF(10, 10));//标注
            //设定拟合结果颜色
            Pen pData1 = new Pen(FitResColor);
            //设定标注颜色
            Pen Arrow = new Pen(ArrowColor);
            Arrow.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            Arrow.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            //g.DrawLine(Arrow, 30, 30, 100, 100);

            if (CloudOn)
            {
                //绘制点云
                for (int i = 0; i < DataRows * 8; i++)
                {
                    // 缩放至合适比例并移至画面中央
                    int px = pMemory[FPos].px[i] / ScaleParam + picWidth / 2;
                    int py = pMemory[FPos].py[i] / ScaleParam + picHeight / 2;
                    if (px > 0 && px < picWidth && py > 0 && py < picHeight)
                    {
                        g.DrawEllipse(pData, px, py, 1, 1);
                    }


                }
            }
            

            //绘制拟合后的结果
            if(pCircle.R<3500 && pCircle.R>2000)
            {
                try
                {
                    for (int i = StartDispNum; i < StopDispNum; i++)
                    {
                        int px = System.Convert.ToInt32((pCircle.X + pCircle.R * Math.Cos(((90 + 20) * 3.1415926535) / 180 + (i * 2 * 3.1415926535 / (DataRows * 9)))) / ScaleParam + picWidth / 2);
                        int py = System.Convert.ToInt32((pCircle.Y + pCircle.R * Math.Sin(((90 + 20) * 3.1415926535) / 180 + (i * 2 * 3.1415926535 / (DataRows * 9)))) / ScaleParam + picHeight / 2);
                        g.DrawEllipse(pData1, px, py, 1, 1);
                    }
                    //计算圆心的图像位置
                    int rx = System.Convert.ToInt32(pCircle.X / ScaleParam) + picWidth / 2;
                    int ry = System.Convert.ToInt32(pCircle.Y / ScaleParam) + picHeight / 2;

                    int[] Rs = new int[8];

                    if (IdenOn)
                    {
                        //绘制准线
                        for (int i = 1; i <= 7; i++)
                        {
                            if (ResultDisp[i] != 0)
                            //if(i==1)
                            {
                                int prx = System.Convert.ToInt32((pCircle.X + pCircle.R * Math.Cos(((90 + 20) * 3.1415926535) / 180 + (ResultDisp[i] * 2 * 3.1415926535 / (DataRows * 9)))) / ScaleParam + picWidth / 2);
                                int pry = System.Convert.ToInt32((pCircle.Y + pCircle.R * Math.Sin(((90 + 20) * 3.1415926535) / 180 + (ResultDisp[i] * 2 * 3.1415926535 / (DataRows * 9)))) / ScaleParam + picHeight / 2);
                                int pfx = System.Convert.ToInt32((pCircle.X + pCircle.R * (1.2) * Math.Cos(((90 + 20) * 3.1415926535) / 180 + (ResultDisp[i] * 2 * 3.1415926535 / (DataRows * 9)))) / ScaleParam + picWidth / 2);
                                int pfy = System.Convert.ToInt32((pCircle.Y + pCircle.R * (1.2) * Math.Sin(((90 + 20) * 3.1415926535) / 180 + (ResultDisp[i] * 2 * 3.1415926535 / (DataRows * 9)))) / ScaleParam + picHeight / 2);
                                //if (pfx > 0 && pfy < picWidth && pfy > 0 && pfy < picHeight)
                                //{
                                g.DrawLine(Arrow, rx, ry, prx, pry);
                                g.DrawString("R" + (i).ToString(), font, bRes, new PointF(pfx, pfy));
                                Rs[i] = System.Convert.ToInt32(Math.Sqrt((pMemory[FPos].px[ResultDisp[i]] - pCircle.X) * (pMemory[FPos].px[ResultDisp[i]] - pCircle.X) + (pMemory[FPos].py[ResultDisp[i]] - pCircle.Y) * (pMemory[FPos].py[ResultDisp[i]] - pCircle.Y)));
                                // }


                            }

                        }
                        //显示数据表               
                        g.DrawString("R1 = " + (Rs[1]).ToString(), font, bRes, new PointF(10, picHeight + 85));
                        g.DrawString("R2 = " + (Rs[2]).ToString(), font, bRes, new PointF(100, picHeight + 85));
                        g.DrawString("R3 = " + (Rs[3]).ToString(), font, bRes, new PointF(185, picHeight + 85));
                        g.DrawString("R4 = " + (Rs[4]).ToString(), font, bRes, new PointF(280, picHeight + 85));
                        g.DrawString("R5 = " + (Rs[5]).ToString(), font, bRes, new PointF(10, picHeight + 115));
                        g.DrawString("R6 = " + (Rs[6]).ToString(), font, bRes, new PointF(100, picHeight + 115));
                        g.DrawString("R7 = " + (Rs[7]).ToString(), font, bRes, new PointF(185, picHeight + 115));
                        g.DrawString("单位：mm", font, bRes, new PointF(270, picHeight + 115));
                    }




                    //g.DrawString("R1 = " + (ResultDisp[1]).ToString(), font, bRes, new PointF(10, picHeight + 85));
                    //g.DrawString("R2 = " + (ResultDisp[2]).ToString(), font, bRes, new PointF(100, picHeight + 85));
                    //g.DrawString("R3 = " + (ResultDisp[3]).ToString(), font, bRes, new PointF(185, picHeight + 85));
                    //g.DrawString("R4 = " + (ResultDisp[4]).ToString(), font, bRes, new PointF(280, picHeight + 85));
                    //g.DrawString("R5 = " + (ResultDisp[5]).ToString(), font, bRes, new PointF(10, picHeight + 115));
                    //g.DrawString("R6 = " + (ResultDisp[6]).ToString(), font, bRes, new PointF(100, picHeight + 115));
                    //g.DrawString("R7 = " + (ResultDisp[7]).ToString(), font, bRes, new PointF(185, picHeight + 115));

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
            


            //for (int i = 1;i<2600;i++)
            //{
            //    g.DrawEllipse(pData, 10, i, 1, 1); //画点
            //}

            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            string baseaddr = Convert.ToBase64String(ms.ToArray());
            //输出建模结果地址
            ModelImageUrl = "data:image/png;base64," + baseaddr;
            pData.Dispose();
            bRes.Dispose();
            font.Dispose();
            pData1.Dispose();
            Arrow.Dispose();
            g.Dispose();
            bmp.Dispose();


        }

        //展示结果（无拟合标注）
        public void Disp2DImage(int FPos)
        {
            //图像初始化
            Bitmap bmp = new Bitmap(picWidth, picHeightAll);
            Graphics g = Graphics.FromImage(bmp);
            
            //设定背景色
            g.Clear(BackColor);
            //设定点云颜色            
            Pen pData = new Pen(PointColor);

            //绘制点云
            for (int i = 0; i < DataRows * 8; i++)
            {
                //缩放至合适比例并移至画面中央
                int px = pMemory[FPos].px[i] / ScaleParam + picWidth / 2;
                int py = pMemory[FPos].py[i] / ScaleParam + picHeight / 2;
                g.DrawEllipse(pData, px, py, 1, 1);

            }

            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            string baseaddr = Convert.ToBase64String(ms.ToArray());
            //输出建模结果地址
            ModelImageUrl = "data:image/png;base64," + baseaddr;
            pData.Dispose();
            g.Dispose();
            bmp.Dispose();


        }

        //识别标注点
        public int IsIdent(int num, double a)
        {
            if(Math.Abs(a-R1a)<Rea)
            {
                ResultDisp[1] = num;
                return 1;
            }
            else if (Math.Abs(a - R2a) < Rea)
            {
                ResultDisp[2] = num;
                return 2;
            }
            else if (Math.Abs(a - R3a) < Rea)
            {
                ResultDisp[3] = num;
                return 3;
            }
            else if (Math.Abs(a - R4a) < Rea)
            {
                ResultDisp[4] = num;
                return 4;
            }
            else if (Math.Abs(a - R5a) < Rea)
            {
                ResultDisp[5] = num;
                return 5;
            }
            else if (Math.Abs(a - R6a) < Rea)
            {
                ResultDisp[6] = num;
                return 6;
            }
            else if (Math.Abs(a - R7a) < Rea)
            {
                ResultDisp[7] = num;
                return 7;
            }
            
            return 0;
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
                        //if(!System.Convert.ToBoolean(CameraOn[k]))
                        //{
                         //   continue;
                        //}
                        double BasicDegreeWorld = _Default.CameraDegree[k] * 3.14159265358 / 180;
                        //换算为图像坐标系角度
                        double BasicDegree = 90 * 3.14159265358 / 180 + BasicDegreeWorld;
                        ///MutexSock.WaitOne();

                        ///MutexSock.ReleaseMutex();
                        for (int j = 0; j < DataRows; j++)
                        {
                            PointCounter = (k - 1) * DataRows + j;
                            try
                            {
                                int px = 0;
                                int py = 0;
                                if (SockPack0[k].s[j]==0&& SockPack0[k].ay[j]==0)
                                {
                                    px = pMemory[(FPos + FMemory - 1) % FMemory].px[PointCounter];
                                    py = pMemory[(FPos + FMemory - 1) % FMemory].py[PointCounter];
                                }
                                else
                                {
                                    px = (System.Convert.ToInt32(SockPack0[k].s[j] * Math.Cos(BasicDegree + SockPack0[k].ay[j]))) / ScaleParam + picWidth / 2;
                                    py = (System.Convert.ToInt32(SockPack0[k].s[j] * Math.Sin(BasicDegree + SockPack0[k].ay[j]))) / ScaleParam + picHeight / 2;
                                }


                                //计算x,y坐标
                                //float x = SockPack[k].s[j] * System.Convert.ToSingle(Math.Cos(BasicDegree - SockPack[k].ay[j]))/ ScaleParam + System.Convert.ToSingle(picWidth / 2);
                                //float y = SockPack[k].s[j] * System.Convert.ToSingle(Math.Sin(BasicDegree - SockPack[k].ay[j]))/ ScaleParam + System.Convert.ToSingle(picHeight / 2);
                                //int px = System.Convert.ToInt16(x);
                                //int py = System.Convert.ToInt16(y);
                                
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
                                //OnUsedMemory = 1;


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
                        //if (!System.Convert.ToBoolean(CameraOn[k]))
                        //{
                        //    continue;
                        //}
                        double BasicDegreeWorld = _Default.CameraDegree[k] * 3.14159265358 / 180;
                        //换算为图像坐标系角度
                        double BasicDegree = 90 * 3.14159265358 / 180 + BasicDegreeWorld;
                        ///MutexSock.WaitOne();
                        ///MutexSock.ReleaseMutex();
                        for (int j = 0; j < DataRows; j++)
                        {
                            PointCounter = (k - 1) * DataRows + j;
                            try
                            {

                                int px = 0;
                                int py = 0;
                                if (SockPack0[k].s[j] == 0 && SockPack0[k].ay[j] == 0)
                                {
                                    px = pMemory[(FPos + FMemory - 1) % FMemory].px[PointCounter];
                                    py = pMemory[(FPos + FMemory - 1) % FMemory].py[PointCounter];
                                }
                                else
                                {
                                    px = (System.Convert.ToInt32(SockPack0[k].s[j] * Math.Cos(BasicDegree + SockPack0[k].ay[j]))) / ScaleParam + picWidth / 2;
                                    py = (System.Convert.ToInt32(SockPack0[k].s[j] * Math.Sin(BasicDegree + SockPack0[k].ay[j]))) / ScaleParam + picHeight / 2;
                                }


                                //计算x,y坐标
                                //float x = SockPack[k].s[j] * System.Convert.ToSingle(Math.Cos(BasicDegree - SockPack[k].ay[j]))/ ScaleParam + System.Convert.ToSingle(picWidth / 2);
                                //float y = SockPack[k].s[j] * System.Convert.ToSingle(Math.Sin(BasicDegree - SockPack[k].ay[j]))/ ScaleParam + System.Convert.ToSingle(picHeight / 2);
                                //int px = System.Convert.ToInt16(x);
                                //int py = System.Convert.ToInt16(y);
                                //int px = (System.Convert.ToInt32(SockPack1[k].s[j] * Math.Cos(BasicDegree - SockPack1[k].ay[j]))) / ScaleParam + picWidth / 2;
                                //int py = (System.Convert.ToInt32(SockPack1[k].s[j] * Math.Sin(BasicDegree - SockPack1[k].ay[j]))) / ScaleParam + picHeight / 2;
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
                //ms.Dispose();
                //Thread.Sleep(10);

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
                Thread.Sleep(5);

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
                Thread.Sleep(5);
            }
        }

        public static void HandleData(object obj)
        {
            try
            {
                byte[] Datasrc = (byte[])obj;
                SockPackage SockPackc = new SockPackage();
                SockPackc = (SockPackage)BytesToStruct(Datasrc, SockPackc.GetType());

                char[] SerialNum = SockPackc.SerialNum;
                int CameraNum = SerialNum2CameraNum(SerialNum);
                //if(!System.Convert.ToBoolean(CameraOn[CameraNum]))
                //{
                // CameraOn[CameraNum] = 1;
                //}
                if (CameraNum > 0 && CameraNum < 9)
                {

                    //if (OnUsedMemory == 0)
                    //{
                    //SockPack1[CameraNum] = SockPackc;
                    //载入并解析数据
                    //SockPack1[CameraNum].SerialNum = SockPackc.SerialNum;
                    //SockPack1[CameraNum].Framecnt = SockPackc.Framecnt;
                    //for(int i = 0;i< DataRows;i++)
                    //{
                    //    SockPack1[CameraNum].ay[i] = System.Convert.ToSingle(_Default.CameraP1[CameraNum]*Math.Atan((_Default.CameraVo[CameraNum]- SockPackc.ay[i])/_Default.CameraFy[CameraNum])+_Default.CameraP2[CameraNum]);
                    //    SockPack1[CameraNum].s[i] = System.Convert.ToSingle((_Default.CameraBmm[CameraNum] * Math.Tan(_Default.CameraPhi[CameraNum]+Math.Atan((_Default.CameraUo[CameraNum]-SockPackc.s[i])/_Default.CameraFx[CameraNum]))/Math.Cos(SockPack1[CameraNum].ay[i]))+_Default.CameraM[CameraNum]); 
                    //}
                    //}
                    //else if (OnUsedMemory == 1)
                    //{
                    //    //SockPack0[CameraNum] = SockPackc;
                    SockPack0[CameraNum].SerialNum = SockPackc.SerialNum;
                    SockPack0[CameraNum].Framecnt = SockPackc.Framecnt;
                    for (int i = 0; i < DataRows; i++)
                    {
                        if(SockPackc.s[i]==0|| SockPackc.ay[i]==0)
                        {
                            SockPack0[CameraNum].ay[i] = 0;
                            SockPack0[CameraNum].s[i] = 0;
                        }
                        else
                        {
                            //SockPack0[CameraNum].ay[i] = 0;
                            //SockPack0[CameraNum].s[i] = 0;
                            //cd loads the camera degree
                            float cd = Convert.ToSingle(Math.Atan((_Default.CameraVo[CameraNum] - SockPackc.ay[i]) / _Default.CameraFy[CameraNum]));                            
                            float x = SockPackc.s[i];
                            //ay load the device degree
                            SockPack0[CameraNum].ay[i] = System.Convert.ToSingle(_Default.CameraK00[CameraNum] + _Default.CameraK10[CameraNum]*cd + _Default.CameraK01[CameraNum]*x + _Default.CameraK11[CameraNum]*cd*x + _Default.CameraK02[CameraNum]*x*x);
                            float Yy = cd;
                            float alpha = System.Convert.ToSingle(_Default.CameraDegree[CameraNum] * 3.14159265358 / 180 + SockPack0[CameraNum].ay[i]);                          
                            float Fx = Convert.ToSingle(_Default.CameraP00[CameraNum]+_Default.CameraP10[CameraNum]*x+_Default.CameraP01[CameraNum]*Yy+_Default.CameraP20[CameraNum]*x*x+_Default.CameraP11[CameraNum]*x*Yy+_Default.CameraP02[CameraNum]*Yy*Yy);
                            //float Fx = (_Default.CameraP1[CameraNum] * SockPackc.s[i] * SockPackc.s[i] * SockPackc.s[i] + _Default.CameraP2[CameraNum] * SockPackc.s[i] * SockPackc.s[i] + _Default.CameraP3[CameraNum] * SockPackc.s[i] + _Default.CameraP4[CameraNum]);
                            float scor = System.Convert.ToSingle((_Default.CameraBmm[CameraNum] * Math.Tan(_Default.CameraPhi[CameraNum] + Math.Atan((_Default.CameraUo[CameraNum] - SockPackc.s[i]) / _Default.CameraFx[CameraNum])) + _Default.CameraM[CameraNum]) / Fx);
                            float num = (scor * _Default.CameraBmm[CameraNum] + (scor - _Default.CameraM[CameraNum]) * _Default.BP1);
                            float den = Convert.ToSingle(_Default.CameraBmm[CameraNum] + (_Default.CameraM[CameraNum] - scor) * (_Default.BP2 * Math.Cos(alpha) + _Default.BP3 * Math.Sin(alpha)));
                            SockPack0[CameraNum].s[i] =  num/ den;
                            //SockPack[CameraNum] = SockPackc;
                            int m = 11;
                        }

                    }
                }
                
            }
            catch (System.NullReferenceException)
            {

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