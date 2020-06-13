using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Messaging;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Data;
using System.Xml;
using DotNet.Utilities;
using DotNet.Business;
using System.Drawing;
using System.Diagnostics;
using System.ServiceModel;

namespace DotNet.WinForm.Controls
{
    public partial class BaseUserControlExtended : BaseUserControl, IBaseControlExtend
    {
        private BaseControlExtend _BaseControlExtend = null;

        #region 构造和初始化

        public BaseUserControlExtended()
        {
            //this.Visible = false;
            InitializeComponent();

            _BaseControlExtend = new BaseControlExtend(this);
        }

        public virtual void Control_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                if (this.Parent == null && this.Parent == null)
                {
                    LoadingUtil.ShowWaitLoading();
                }

                // 设置鼠标繁忙状态，并保留原先的状态
                System.Windows.Forms.Cursor holdCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                this.ControlLoaded = false;
                try
                {
                    // 是否记录访问日志
                    if (BaseSystemInfo.RecordLog)
                    {
                        // 已经登录了系统了，才记录日志
                        if (BaseSystemInfo.UserIsLogOn)
                        {
                            // 调用服务事件
                            if (this.RecordControlLog)
                            {
                                // 调用服务事件
                                // this.LogId = dotNetService.LogService.WriteLog(UserInfo, this.ItemName, this.Text, "FormLoad");
                                DotNetService dotNetService = new DotNetService();
                                dotNetService.LogService.WriteLog(UserInfo, this.Name, AppMessage.GetMessage(this.Name), "FormLoad", AppMessage.LoadWindow);
                                if (dotNetService.LogService is ICommunicationObject)
                                {
                                    ((ICommunicationObject)dotNetService.LogService).Close();
                                }
                            }
                        }
                    }

                    // 获得页面的权限
                    this.GetPermissions();
                    // 加载窗体
                    this.ControlOnLoad();
                    // 设置按钮状态
                    this.SetControlState();
                    if (BaseSystemInfo.MultiLanguage)
                    {
                        // 多语言国际化加载
                        /*
                        if (ResourceManagerWrapper.Instance.GetLanguages() != null)
                        {
                            this.Localization(this);
                        }
                        */
                    }
                    if (this.LoadUserParameters)
                    {
                        // 客户端页面配置加载
                        this.LoadUserParameter(this);
                    }
                    // 设置帮助
                    this.SetHelp();
                }
                catch (Exception catchException)
                {
                    catchException.ProcessException();
                }
                finally
                {
                    this.ControlLoaded = true;
                    // 设置鼠标默认状态，原来的光标状态
                    this.Cursor = holdCursor;
                }

                if (this.Parent == null)
                {
                    //LoadingUtil.StopLoading();
                }
                this.Visible = true;
            }
        }

        #endregion

        #region 按键事件处理

        #endregion

        #region 文件打印和导出
        #region private bool FileExist(string fileName) 检查文件是否存在
        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>是否存在</returns>
        private bool FileExist(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                string targetFileName = System.IO.Path.GetFileName(fileName);
                if (DevExpress.XtraEditors.XtraMessageBox.Show(AppMessage.Format(AppMessage.MSG0236, targetFileName), AppMessage.MSG0000, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    System.IO.File.Delete(fileName);
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        private SaveFileDialog GetShowFileDialog(string directory, string fileName)
        {
            string directoryName = BaseSystemInfo.StartupPath + directory;
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            // 这里显示选择文件的对话框，可以取消导出可以确认导出，可以修改文件名。
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = fileName;
            fileName = BaseSystemInfo.StartupPath + directory + fileName;
            saveFileDialog.InitialDirectory = directoryName;
            //saveFileDialog.Filter = "导出数据文件(*.csv)|*.csv|所有文件|*.*";
            saveFileDialog.Filter = "Excel(*.xls)|*.xls|所有文件|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = "导出数据文件";
            return saveFileDialog;
        }

        private bool ValidateExport(SaveFileDialog saveFileDialog, out string fileName)
        {
            fileName = saveFileDialog.FileName;

            // 2012.04.02 Author 增加 判断文件是否被打开
            if (BaseExportExcel.CheckIsOpened(fileName))
            {
                LoadingUtil.ShowInformationMessage("Excel文件已经打开,请关闭后重试!", "提示信息", this);
                return false;
            }

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }
            return true;
        }

        #region private void ExportExcel(System.Data.DataTable dt, System.Collections.Generic.Dictionary<string, string> fieldList, string directory, string fileName) 导出Excel
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dataGridView">表格控件</param>
        /// <param name="result">数据表格</param>
        /// <param name="fieldList">数据表字段名-说明对应列表</param>
        /// <param name="directory">目录</param>
        /// <param name="fileName">文件名</param>
        public void ExportExcel(System.Data.DataTable dt, Dictionary<string, string> fieldList, string directory, string fileName)
        {
            MyExportExcel(directory, fileName, (fiName) =>
            {
                // BaseExportCSV.ExportCSV(dataGridView, dataView, fileName);
                // 2012.04.02 Author 增加新的导出Excel方法，非Com+方式，改用.Net控件
                BaseExportExcel.ExportXlsByNPOI(dt, fieldList, fiName);
            });
        }
        #endregion

        public void ExportExcel(DataGridView grd, string directory, string fileName)
        {
            ExportExcel(grd, (DataView)(grd.DataSource), directory, fileName);
        }

        #region private void ExportExcel(DataGridView grd, DataView dataView, string directory, string fileName) 导出Excel
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dataGridView">表格控件</param>
        /// <param name="dataView">数据表格</param>
        /// <param name="directory">目录</param>
        /// <param name="fileName">文件名</param>
        public void ExportExcel(DataGridView grd, DataView dataView, string directory, string fileName)
        {
            MyExportExcel(directory, fileName, (fiName) =>
            {
                // BaseExportCSV.ExportCSV(dataGridView, dataView, fileName);
                // 2012.04.02 Author 增加新的导出Excel方法，非Com+方式，改用.Net控件
                BaseExportExcel.ExportXlsByNPOI(grd, dataView, fiName);
            });
        }
        #endregion

        public void ExportExcel(DevExpress.XtraGrid.Views.Grid.GridView grv, string directory, string fileName)
        {
            ExportExcel(grv, (DataView)(grv.DataSource), directory, fileName);
        }

        #region private void ExportExcel(DevExpress.XtraGrid.Views.Grid.GridView grv, DataView dataView, string directory, string fileName) 导出Excel
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dataGridView">表格控件</param>
        /// <param name="dataView">数据表格</param>
        /// <param name="directory">目录</param>
        /// <param name="fileName">文件名</param>
        public void ExportExcel(DevExpress.XtraGrid.Views.Grid.GridView grv, DataView dataView, string directory, string fileName)
        {
            MyExportExcel(directory, fileName, (fiName) =>
            {
                // BaseExportCSV.ExportCSV(dataGridView, dataView, fileName);
                // 2012.04.02 Author 增加新的导出Excel方法，非Com+方式，改用.Net控件
                BaseExportExcel.ExportXlsByNPOI(grv, dataView, fiName);
            });
        }
        #endregion

        private delegate void ExportExcelFun(string fileName);

        private void MyExportExcel(string directory, string fileName, ExportExcelFun fun)
        {
            SaveFileDialog saveFileDialog = GetShowFileDialog(directory, fileName);
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                System.Windows.Forms.Cursor holdCursor = BeginWait();
                if (!ValidateExport(saveFileDialog, out fileName)) { EndWait(holdCursor); return; }
                fun(fileName);
                EndWait(holdCursor);
                Process.Start(fileName);
            }
        }

        public void Print(DevExpress.XtraGrid.Views.Grid.GridView dop)
        {
            dop.PrintDialog();
        }
        #endregion

        #region 关闭

        #endregion

        #region 表格(GridView)处理
        protected GridViewCheckHeadUtil GridCheckUtil = null;
        public void grv_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            // 显示表格序号
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// DataGridView加载
        /// </summary>
        public void DataGridViewOnLoad(DataGridView grd)
        {
            grd.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DataGridView_RowPostPaint);
        }

        public void DataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //序号右对齐
            //Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
            //    e.RowBounds.Location.Y,
            //    (sender as DataGridView).RowHeadersWidth - 4,
            //    e.RowBounds.Height);

            //TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            //    (sender as DataGridView).RowHeadersDefaultCellStyle.Font,
            //    rectangle,
            //    (sender as DataGridView).RowHeadersDefaultCellStyle.ForeColor,
            //    TextFormatFlags.VerticalCenter | TextFormatFlags.Right);

            // 序号居中
            // 定义一个画笔，颜色用行标题的前景色填充
            SolidBrush solidBrush = new SolidBrush((sender as DataGridView).RowHeadersDefaultCellStyle.ForeColor);
            //得到当前行的行号
            int rowIndex = e.RowIndex + 1;
            //DataGridView的RowHeadersWidth 为了算中间位置
            int rowHeadersWidth = (sender as DataGridView).RowHeadersWidth;
            //根据宽度与显示的字符数计算中间位置
            int rowHeadersX = (rowHeadersWidth - rowIndex.ToString().Length * 6) / 2;
            int rowHeadersY = e.RowBounds.Location.Y + 4;
            e.Graphics.DrawString((rowIndex).ToString(System.Globalization.CultureInfo.CurrentUICulture), (sender as DataGridView).DefaultCellStyle.Font, solidBrush, rowHeadersX, rowHeadersY);
        }
        #endregion

        #region 透明、异形实现

        #endregion

        #region 减少闪烁
        private void SetStyles()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer, true);
            //强制分配样式重新应用到控件上
            UpdateStyles();
            base.AutoScaleMode = AutoScaleMode.None;
        }
        #endregion

        #region 全部显示
        public void AllShow()
        {
            this.Show();
        }
        #endregion

        #region 全部隐藏
        public void AllHide()
        {
            this.Hide();
        }
        #endregion

        #region 变量属性
        /// <summary>
        /// 实际打开本窗体的父窗体，临时修改窗体的Owner时，需要将原来的Owner保存在此属性中
        /// </summary>
        public Form RealOwner
        {
            get; set;
        }
        #endregion

        #region 绑定背景
        #endregion
        
        #region IBaseControlExtend 接口实现

        #region public virtual void ControlOnLoad() 加载窗体
        /// <summary>
        /// 加载窗体
        /// </summary>
        public virtual void ControlOnLoad()
        {
        }
        #endregion
        public virtual bool Busyness
        {
            get
            {
                return _BaseControlExtend.Busyness;
            }

            set
            {
                _BaseControlExtend.Busyness = value;
            }
        }

        public virtual bool Changed
        {
            get
            {
                return _BaseControlExtend.Changed;
            }

            set
            {
                _BaseControlExtend.Changed = value;
            }
        }

        public virtual IDbHelper DbHelper
        {
            get
            {
                return _BaseControlExtend.DbHelper;
            }
        }

        public virtual string EntityId
        {
            get
            {
                return _BaseControlExtend.EntityId;
            }

            set
            {
                _BaseControlExtend.EntityId = value;
            }
        }

        public virtual bool ExitApplication
        {
            get
            {
                return _BaseControlExtend.ExitApplication;
            }

            set
            {
                _BaseControlExtend.ExitApplication = value;
            }
        }

        public virtual bool ControlLoaded
        {
            get
            {
                return _BaseControlExtend.ControlLoaded;
            }

            set
            {
                _BaseControlExtend.ControlLoaded = value;
            }
        }

        public virtual bool LoadUserParameters
        {
            get
            {
                return _BaseControlExtend.LoadUserParameters;
            }

            set
            {
                _BaseControlExtend.LoadUserParameters = value;
            }
        }

        public virtual string LogId
        {
            get
            {
                return _BaseControlExtend.LogId;
            }

            set
            {
                _BaseControlExtend.LogId = value;
            }
        }

        public virtual bool PermissionAccess
        {
            get
            {
                return _BaseControlExtend.PermissionAccess;
            }

            set
            {
                _BaseControlExtend.PermissionAccess = value;
            }
        }

        public virtual bool PermissionAdd
        {
            get
            {
                return _BaseControlExtend.PermissionAdd;
            }

            set
            {
                _BaseControlExtend.PermissionAdd = value;
            }
        }

        public virtual bool PermissionAllCompany
        {
            get
            {
                return _BaseControlExtend.PermissionAllCompany;
            }

            set
            {
                _BaseControlExtend.PermissionAllCompany = value;
            }
        }

        public virtual bool PermissionCompany
        {
            get
            {
                return _BaseControlExtend.PermissionCompany;
            }

            set
            {
                _BaseControlExtend.PermissionCompany = value;
            }
        }

        public virtual bool PermissionEdit
        {
            get
            {
                return _BaseControlExtend.PermissionEdit;
            }

            set
            {
                _BaseControlExtend.PermissionEdit = value;
            }
        }

        public virtual bool PermissionExport
        {
            get
            {
                return _BaseControlExtend.PermissionExport;
            }

            set
            {
                _BaseControlExtend.PermissionExport = value;
            }
        }

        public virtual bool PermissionImport
        {
            get
            {
                return _BaseControlExtend.PermissionImport;
            }

            set
            {
                _BaseControlExtend.PermissionImport = value;
            }
        }

        public virtual bool PermissionSearch
        {
            get
            {
                return _BaseControlExtend.PermissionSearch;
            }

            set
            {
                _BaseControlExtend.PermissionSearch = value;
            }
        }

        public virtual bool PermissionSubCompany
        {
            get
            {
                return _BaseControlExtend.PermissionSubCompany;
            }

            set
            {
                _BaseControlExtend.PermissionSubCompany = value;
            }
        }

        public virtual bool RecordControlLog
        {
            get
            {
                return _BaseControlExtend.RecordControlLog;
            }

            set
            {
                _BaseControlExtend.RecordControlLog = value;
            }
        }

        public virtual IDbHelper UserCenterDbHelper
        {
            get
            {
                return _BaseControlExtend.UserCenterDbHelper;
            }
        }

        public virtual BaseUserInfo UserInfo
        {
            get
            {
                return _BaseControlExtend.UserInfo;
            }

            set
            {
                _BaseControlExtend.UserInfo = value;
            }
        }

        public virtual IDbHelper WorkFlowDbHelper
        {
            get
            {
                return _BaseControlExtend.WorkFlowDbHelper;
            }
        }

        public virtual bool PermissionDelete
        {
            get
            {
                return _BaseControlExtend.PermissionDelete;
            }

            set
            {
                _BaseControlExtend.PermissionDelete = value;
            }
        }

        public virtual int BatchDelete()
        {
            return _BaseControlExtend.BatchDelete();
        }

        public virtual System.Windows.Forms.Cursor BeginWait()
        {
            return _BaseControlExtend.BeginWait();
        }

        public virtual bool CheckInput()
        {
            return _BaseControlExtend.CheckInput();
        }

        public virtual bool CheckInputDelete()
        {
            return _BaseControlExtend.CheckInputDelete();
        }

        public virtual void Comment(string formName = null, string entityId = null)
        {
            _BaseControlExtend.Comment(formName, entityId);
        }

        public virtual void EndWait(System.Windows.Forms.Cursor holdCursor)
        {
            _BaseControlExtend.EndWait(holdCursor);
        }

        public virtual string GetClientVersion(string assemblyString = null)
        {
            return _BaseControlExtend.GetClientVersion(assemblyString);
        }
        
        public virtual void GetList()
        {
            _BaseControlExtend.GetList();
        }

        public virtual DataTable GetOrganizeDTByPermission(string permissionCode = "Resource.ManagePermission", bool isInnerOrganize = true)
        {
            return _BaseControlExtend.GetOrganizeDTByPermission(permissionCode, isInnerOrganize);
        }

        public virtual void GetPermissions()
        {
            _BaseControlExtend.GetPermissions();
        }

        public virtual bool IsAuthorized(PermissionItem permissionItem)
        {
            return _BaseControlExtend.IsAuthorized(permissionItem);
        }

        public virtual bool IsAuthorized(string permissionCode, string permissionItemName = null)
        {
            return _BaseControlExtend.IsAuthorized(permissionCode, permissionItemName);
        }

        public virtual void LoadUserParameter(Control form)
        {
            _BaseControlExtend.LoadUserParameter(form);
        }

        public virtual void Localization(Form form)
        {
            _BaseControlExtend.Localization(form);
        }

        public virtual bool ModuleIsVisible(string formName)
        {
            return _BaseControlExtend.ModuleIsVisible(formName);
        }

        public virtual void PlaySound()
        {
            _BaseControlExtend.PlaySound();
        }

        public virtual void ProcessException(Exception ex, string receiveId = null)
        {
            _BaseControlExtend.ProcessException(ex, receiveId);
        }

        public virtual bool SaveEntity()
        {
            return _BaseControlExtend.SaveEntity();
        }

        public virtual void SetControlState()
        {
            _BaseControlExtend.SetControlState();
        }

        public virtual void SetControlState(bool enabled)
        {
            _BaseControlExtend.SetControlState(enabled);
        }

        public virtual void SetHelp()
        {
            _BaseControlExtend.SetHelp();
        }

        public virtual void ShowEntity()
        {
            _BaseControlExtend.ShowEntity();
        }
#endregion
    }
}
