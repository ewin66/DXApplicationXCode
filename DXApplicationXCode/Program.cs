using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using DevExpress.XtraEditors;
using System.Drawing;

namespace DXApplicationXCode
{
    static class Program
    {
        [DllImport("kernel32.dll",
            EntryPoint = "GetStdHandle",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll",
            EntryPoint = "AllocConsole",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int FreeConsole();

        private const int STD_OUTPUT_HANDLE = -11;
        private const int MY_CODE_PAGE = 437;
        
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        extern static IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);
        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        extern static IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("msvcrt.dll")]
        public static extern int system(string cmd);
        public static void Open()
        {
            AllocConsole();
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }

        //clear all the texts in allocconsole
        public static void ClearAllocConsole()
        {
            system("CLS");
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            #region 是否开启控制台
            ////if ((args.Length > 0 && args.Contains("-c")))
            {
                AllocConsole();//开启控制台
                IntPtr windowHandle = FindWindow(null, Process.GetCurrentProcess().MainModule.FileName);//查找控制台窗口
                IntPtr closeMenu = GetSystemMenu(windowHandle, IntPtr.Zero);
                const uint SC_CLOSE = 0xF060;
                ////RemoveMenu(closeMenu, SC_CLOSE, 0x0);//移除关闭按钮
                Console.WriteLine("控制台已启动。");
            }
            #endregion

            #region 字体名称和大小 从配置文件读取配置信息(FontName,FontSize)

            #endregion

            #region 设置默认字体、日期格式

            #endregion

            Application.Run(new RibbonFormMain());
        }
    }
}
