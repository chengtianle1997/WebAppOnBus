using System;
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
            }
            catch(FormatException)
            {
                return;
            }
            
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




        //2D 3D菜单切换
        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (Menu1.SelectedValue.ToString())
            {
                //切换到隧道2D图像界面
                case "1":
                    {
                        MultiView1.ActiveViewIndex = 0;
                        break;
                    }
                //切换到隧道3D建模界面
                case "2":
                    {
                        MultiView1.ActiveViewIndex = 1;
                        break;
                    }
                default:
                    break;

            }
        }
    }
}
