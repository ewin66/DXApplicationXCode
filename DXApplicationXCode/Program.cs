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
using System.Reflection;
using GACManagerApi;
using GACManagerApi.Fusion;
using System.Threading;

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

            UnregisterDll("NewLife.Core");
            UnregisterDll("Xcode");

            #region 从资源中加载DLL AssemblyResolve在对程序集的解析失败时发生。资源dll文件必须在资源视图中加入，要求在资源视图中能看到。
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                Assembly resourceManagerAssembly = null;
                string dllName = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");
                dllName = dllName.Replace(".", "_");
                if (dllName.EndsWith("_resources")) return null;

                //if (dllName.Equals("NewLife_Core"))
                //{
                //    RegisterDll("NewLife.Core", true);
                //    try
                //    {
                //        AssemblyDescription xxx = GetGacAssemblyPath("NewLife.Core");
                //        if (xxx != null)
                //        {
                //            ////resourceManagerAssembly = xxx.;
                //        }
                //    }
                //    catch (Exception)
                //    {

                //    }
                //}
                //else if (dllName.Equals("XCode"))
                //{
                //    RegisterDll(dllName, true);
                //}
                //else
                {
                    foreach (var eachAsembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        foreach (Module eachModule in eachAsembly.GetModules())
                        {
                            try
                            {
                                foreach (System.Type eachType in eachModule.FindTypes(null, null))
                                {
                                    //获取Resources类下的所有属性
                                    PropertyInfo[] peoperInfo = eachType.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
                                    foreach (PropertyInfo eachPropertyInfo in peoperInfo)
                                    {
                                        //筛选资源
                                        if (eachPropertyInfo.Name == "ResourceManager")
                                        {
                                            System.Resources.ResourceManager ResourceManager;
                                            //这里需要注意 eachPropertyInfo.DeclaringType.Namespace 是否包含了 Properties
                                            String resourcesDirectory = ".Resources";  //这个目录要与工程文件的目录结构的路径一致
                                            if (eachPropertyInfo.DeclaringType.Namespace.EndsWith("Properties"))
                                                ResourceManager = new System.Resources.ResourceManager(eachPropertyInfo.DeclaringType.Namespace + resourcesDirectory, System.Reflection.Assembly.GetExecutingAssembly());
                                            else
                                            {
                                                ResourceManager = new System.Resources.ResourceManager(eachPropertyInfo.DeclaringType.Namespace + ".Properties" + resourcesDirectory, System.Reflection.Assembly.GetExecutingAssembly());
                                            }
                                            try
                                            {
                                                byte[] bytes = (byte[])ResourceManager.GetObject(dllName);
                                                resourceManagerAssembly = System.Reflection.Assembly.Load(bytes);
                                                if (resourceManagerAssembly.FullName == args.Name)
                                                {
                                                    break;
                                                }
                                            }
                                            catch (Exception)
                                            {

                                            }
                                        }
                                    }
                                    if (resourceManagerAssembly != null)
                                        return resourceManagerAssembly;
                                }
                            }
                            catch (System.StackOverflowException catchStackOverflowException)
                            {

                            }
                        }
                    }
                }
                return resourceManagerAssembly;
            };
            #endregion

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

        private static void RegisterDll(String moduleName, Boolean log = false)
        {
            String embedResourcePath = "DXApplication";
            String embedResourceCategoryPath = "Path";
            String dllModuleName = moduleName;
            String targetPath = null;

            #region 释放文件
            try
            {
                String targetFileName = String.Empty;
                String tempPath = System.IO.Path.GetTempPath(); ////Path.Combine(new DirectoryInfo(Environment.SystemDirectory).Root.FullName.ToString(), "temp"); //win10无读取temp目录的权限////

                System.Reflection.Assembly assembly = Assembly.GetExecutingAssembly();
                foreach (String eachEmbedResourceName in assembly.GetManifestResourceNames())
                {
                    if (eachEmbedResourceName.Contains(embedResourcePath + "." + embedResourceCategoryPath) &&
                        eachEmbedResourceName.Contains(dllModuleName + "." + "dll"))
                    {
                        targetFileName = eachEmbedResourceName.Substring(eachEmbedResourceName.LastIndexOf($"{embedResourcePath}.{embedResourceCategoryPath}") + $"{embedResourcePath}.{embedResourceCategoryPath}.".Length);
                        System.IO.Stream embedResoutceStream = assembly.GetManifestResourceStream(eachEmbedResourceName);
                        if (eachEmbedResourceName.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase))
                        {
                            targetPath = Path.Combine(tempPath, targetFileName);
                        }
                        byte[] buffer = new byte[embedResoutceStream.Length];
                        embedResoutceStream.Read(buffer, 0, buffer.Length);     //将流的内容读到缓冲区 

                        FileStream fs = new FileStream(targetPath, FileMode.Create, FileAccess.Write);
                        fs.Write(buffer, 0, buffer.Length);
                        fs.Flush();
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion

            #region 选择文件
            if (String.IsNullOrEmpty(targetPath))
            {
                //  We'll need a path to the assembly to install.
                var dllFullPath = @"";
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Application.StartupPath;
                openFileDialog.Filter = "所有文件|*.*|Dll文件(*.Dll)|*.Dll";
                openFileDialog.FilterIndex = 7;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "打开Dll文件";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(openFileDialog.FileName))
                    {
                        targetPath = openFileDialog.FileName;
                    }
                }
            }
            #endregion

            #region 注册
            if (!String.IsNullOrEmpty(targetPath) && File.Exists(targetPath))
            {
                if (log)
                {
                    //  Install the assembly, without an install reference.
                    AssemblyCache.InstallAssembly(targetPath, null, AssemblyCommitFlags.Default);
                    string message = "The assembly was installed successfully!";
                }
            }
            #endregion

            #region 删除文件
            if (!String.IsNullOrEmpty(targetPath) && File.Exists(targetPath))
            {
                while (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                    Thread.Sleep(1000);
                }
            }
            #endregion
        }
        private static void UnregisterDll(String moduleName, Boolean log = false)
        {
            ////String embedResourcePath = "DXApplication";
            ////String embedResourceCategoryPath = "Path";
            String dllModuleName = moduleName;

            //  Create an assembly cache enumerator.
            var assemblyCacheEnum = new AssemblyCacheEnumerator(null);

            //  Enumerate the assemblies.
            var assemblyName = assemblyCacheEnum.GetNextAssembly();
            while (assemblyName != null)
            {
                //  Create the assembly description.
                var desc = new AssemblyDescription(assemblyName);

                if (desc.Name.Equals(dllModuleName))
                {
                    //  We'll need a display name of the assembly to uninstall.
                    ////var displayName = @"Apex, Version=1.4.0.0, Culture=neutral, PublicKeyToken=98d06957926c086d, processorArchitecture=MSIL";
                    var displayName = desc.DisplayName;
                    //  When we try to uninstall an assembly, an uninstall disposition will be
                    //  set to indicate the success of the operation.
                    var uninstallDisposition = IASSEMBLYCACHE_UNINSTALL_DISPOSITION.Unknown;

                    //  Install the assembly, without an install reference.
                    try
                    {
                        AssemblyCache.UninstallAssembly(displayName, null, out uninstallDisposition);
                        //  Depending on the result, show the appropriate message.
                        string message = string.Empty;
                        switch (uninstallDisposition)
                        {
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.Unknown:
                                message = "Failed to uninstall assembly.";
                                break;
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNINSTALLED:
                                message = "The assembly was uninstalled successfully!";
                                break;
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_STILL_IN_USE:
                                message = "Cannot uninstall this assembly - it is in use.";
                                break;
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_ALREADY_UNINSTALLED:
                                message = "Cannot uninstall this assembly - it has already been uninstalled.";
                                break;
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_DELETE_PENDING:
                                message = "Cannot uninstall this assembly - it has has a delete pending.";
                                break;
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_HAS_INSTALL_REFERENCES:
                                message = "Cannot uninstall this assembly - it was installed as part of another product.";
                                break;
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_REFERENCE_NOT_FOUND:
                                message = "Cannot uninstall this assembly - cannot find the assembly.";
                                break;
                            default:
                                break;
                        }
                        if (log)
                        {

                        }
                    }
                    catch (Exception exception)
                    {
                        //  We've failed to uninstall the assembly.
                        throw new InvalidOperationException("Failed to uninstall the assembly.", exception);
                    }
                    finally
                    {
                        ////////assemblyName = null;
                    }
                    //  Did we succeed?
                    if (uninstallDisposition == IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNINSTALLED)
                    {
                        //  Hooray!
                        if (false)
                        {
                            assemblyName = null;
                        }
                        else
                        {
                            //  Create an assembly cache enumerator.
                            assemblyCacheEnum = new AssemblyCacheEnumerator(null);
                            //  Enumerate the assemblies.
                            assemblyName = assemblyCacheEnum.GetNextAssembly();
                        }
                    }
                }
                else
                {
                    assemblyName = assemblyCacheEnum.GetNextAssembly();
                }
            }
        }

        private static AssemblyDescription GetGacAssemblyPath(String moduleName)
        {
            ////String embedResourcePath = "DXApplication";
            ////String embedResourceCategoryPath = "Path";
            String dllModuleName = moduleName;

            //  Create an assembly cache enumerator.
            var assemblyCacheEnum = new AssemblyCacheEnumerator(null);

            //  Enumerate the assemblies.
            var assemblyName = assemblyCacheEnum.GetNextAssembly();
            while (assemblyName != null)
            {
                //  Create the assembly description.
                var desc = new AssemblyDescription(assemblyName);

                if (desc.Name.Equals(dllModuleName))
                {
                    //  We'll need a display name of the assembly to uninstall.
                    ////var displayName = @"Apex, Version=1.4.0.0, Culture=neutral, PublicKeyToken=98d06957926c086d, processorArchitecture=MSIL";
                    var displayName = desc.DisplayName;
                    //  When we try to uninstall an assembly, an uninstall disposition will be
                    //  set to indicate the success of the operation.
                    var uninstallDisposition = IASSEMBLYCACHE_UNINSTALL_DISPOSITION.Unknown;

                    //  Install the assembly, without an install reference.
                    try
                    {
                        IAssemblyCache ac = AssemblyCache.GetIAssemblyCache(displayName, null);
                        if(ac != null) return desc;
                    }
                    catch (Exception exception)
                    {
                        //  We've failed to uninstall the assembly.
                        throw new InvalidOperationException("Failed to uninstall the assembly.", exception);
                    }
                    finally
                    {
                        ////////assemblyName = null;
                    }
                    //////////  Did we succeed?
                    ////////if (uninstallDisposition == IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNINSTALLED)
                    ////////{
                    ////////    //  Hooray!
                    ////////    if (false)
                    ////////    {
                    ////////        assemblyName = null;
                    ////////    }
                    ////////    else
                    ////////    {
                    ////////        //  Create an assembly cache enumerator.
                    ////////        assemblyCacheEnum = new AssemblyCacheEnumerator(null);
                    ////////        //  Enumerate the assemblies.
                    ////////        assemblyName = assemblyCacheEnum.GetNextAssembly();
                    ////////    }
                    ////////}
                }
                else
                {
                    assemblyName = assemblyCacheEnum.GetNextAssembly();
                }
            }
            return null;
        }    
    }
}
