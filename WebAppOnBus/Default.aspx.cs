using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace WebAppOnBus
{
    public partial class _Default : Page
    {
        public static string Ipaddr;

        public static string Appdir;

        public static string Apparg;

        public static string Comp0IP;

        public static string Comp1IP;

        public static string Comp2IP;

        public static float BP1;

        public static float BP2;

        public static float BP3;

        //功能选项设置
        public static bool CloudOn = true;
        public static bool SmoothOn = false;
        public static bool FitOn = false;
        public static bool IdenOn = false;

        //0到8存储 0为vo 1-8为1-8号相机
        public static double[] m_CameraDegree = new double[9];
        public static string[] m_CameraSerial = new string[9];

        

        public static double[] CameraDegree
        {
            get
            {
                return m_CameraDegree;
            }
            set
            {
                double[] m_CameraDegree = (double[])value.Clone();
            }
        }

        public static string[] CameraSerial
        {
            get
            {
                return m_CameraSerial;
            }
            set
            {
                string[] m_CameraSerial = (string[])value.Clone();
            }
        }

        public static float[] CameraBmm = new float[9];   
        public static float[] CameraPhi = new float[9];
        public static float[] CameraUo = new float[9];
        public static float[] CameraVo = new float[9];
        public static float[] CameraFx = new float[9];
        public static float[] CameraFy = new float[9];
        public static float[] CameraM = new float[9];
        public static float[] CameraP00 = new float[9];
        public static float[] CameraP10 = new float[9];
        public static float[] CameraP01 = new float[9];
        public static float[] CameraP20 = new float[9];
        public static float[] CameraP11 = new float[9];
        public static float[] CameraP02 = new float[9];
        public static float[] CameraK00 = new float[9];
        public static float[] CameraK10 = new float[9];
        public static float[] CameraK01 = new float[9];
        public static float[] CameraK11 = new float[9];
        public static float[] CameraK02 = new float[9];


        //public string ConfigFile = "E:\\WebAppOnBus\\WebAppOnBus\\Config.ini";
        public string ConfigFile = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ToString() + "Config.ini";


        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

        public string ReadString(string Section, string Ident, string Default)
        {
            Byte[] Buffer = new byte[10240];
            int buflen = GetPrivateProfileString(Section, Ident, Default, Buffer, Buffer.GetUpperBound(0), ConfigFile);
            string s = Encoding.GetEncoding(0).GetString(Buffer);
            s = s.Substring(0, buflen);
            return s.Trim();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < 9; i++)
                {
                    string CameraN = "Camera" + i;
                    CameraSerial[i] = ReadString("CameraSerial", CameraN, null);
                    CameraDegree[i] = System.Convert.ToDouble(ReadString("CameraDegree", CameraN, null));
                }
                Ipaddr = GetIpAddr();
                Appdir = ReadString("ParamDetail", "AppDir", null);
                Comp0IP = ReadString("ParamDetail", "COMP0IP", null);
                Comp1IP = ReadString("ParamDetail", "COMP1IP", null);
                Comp2IP = ReadString("ParamDetail", "COMP2IP", null);
                BP1 = System.Convert.ToSingle(ReadString("ParamDetail", "BP1", null));
                BP2 = System.Convert.ToSingle(ReadString("ParamDetail", "BP2", null));
                BP3 = System.Convert.ToSingle(ReadString("ParamDetail", "BP3", null));
                for (int i = 1;i<=8;i++)
                {
                    string CameraNBmm = "Camera" + i + "Bmm";
                    string CameraNPhi = "Camera" + i + "Phi";
                    string CameraNUo = "Camera" + i + "Uo";
                    string CameraNVo = "Camera" + i + "Vo";
                    string CameraNFx = "Camera" + i + "Fx";
                    string CameraNFy = "Camera" + i + "Fy";
                    string CameraNM = "Camera" + i + "M";
                    string CameraNP00 = "Camera" + i + "P00";
                    string CameraNP10 = "Camera" + i + "P10";
                    string CameraNP01 = "Camera" + i + "P01";
                    string CameraNP20 = "Camera" + i + "P20";
                    string CameraNP11 = "Camera" + i + "P11";
                    string CameraNP02 = "Camera" + i + "P02";
                    string CameraNK00 = "Camera" + i + "K00";
                    string CameraNK10= "Camera" + i + "K10";
                    string CameraNK01 = "Camera" + i + "K01";
                    string CameraNK11 = "Camera" + i + "K11";
                    string CameraNK02 = "Camera" + i + "K02";
                    
                    CameraBmm[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNBmm, null));
                    CameraPhi[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNPhi, null));
                    CameraUo[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNUo, null));
                    CameraVo[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNVo, null));
                    CameraFx[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNFx, null));
                    CameraFy[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNFy, null));                
                    CameraM[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNM, null));
                    CameraP00[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNP00, null));
                    CameraP10[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNP10, null));
                    CameraP01[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNP01, null));
                    CameraP20[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNP20, null));
                    CameraP11[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNP11, null));
                    CameraP02[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNP02, null));
                    CameraK00[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNK00, null));
                    CameraK10[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNK10, null));
                    CameraK01[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNK01, null));
                    CameraK11[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNK11, null));
                    CameraK02[i] = System.Convert.ToSingle(ReadString("CameraParam", CameraNK02, null));
                    
                }
                
            }
            catch(FormatException)
            {
                return;
            }

            Apparg = "--ssar " + Ipaddr;
            //Process.Start(Appdir, Apparg); 
            
        }

        //本机ip地址获取
        public static string GetIpAddr()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry localhost = Dns.GetHostByName(hostName);
            IPAddress localaddr = localhost.AddressList[0];
            string ip = localaddr.ToString();
            return localaddr.ToString();
        }




        ////2D 3D菜单切换
        //protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        //{
        //    switch (Menu1.SelectedValue.ToString())
        //    {
        //        //切换到隧道2D图像界面
        //        case "1":
        //            {
        //                MultiView1.ActiveViewIndex = 0;
        //                break;
        //            }
        //        //切换到隧道3D建模界面
        //        case "2":
        //            {
        //                MultiView1.ActiveViewIndex = 1;
        //                break;
        //            }
        //        default:
        //            break;

        //    }
        //}

        //设置点云显隐
        public void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            string test = checkbox1.Value;
            switch (checkbox1.Value)
            {
                
                case "false":
                    {
                        CloudOn = false;
                        break;
                    }
                case "true":
                    {
                        CloudOn = true;
                        break;
                    }
                default:
                    break;

            }
        }

        



    }
}
