//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2016 , Zonsun Healthcare Co.Ltd. 
//-----------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using DevExpress.LookAndFeel;
using DevExpress.XtraBars;
using DevExpress.XtraTabbedMdi;
using Microsoft.Win32;

namespace DotNet.WinForm
{
    using Controls;
    using DevExpress.XtraBars.Ribbon;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading;

    public partial class BaseRibbonForm :  RibbonForm, IBaseFormExtend
    {   
         private BaseFormExtend _BaseFormExtend = null;
        protected new bool DesignMode
        {
            get
            {
                bool returnFlag = false;
#if DEBUG
                if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                {
                    returnFlag = true;
                }
                else if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper().Equals("DEVENV"))
                {
                    returnFlag = true;
                }
#endif
                return returnFlag;
            }
        }

        #region 构造和初始化
        public BaseRibbonForm()
        {
            if (!this.DesignMode)
            {
                // 必须放在初始化组件之前
                this.GetIcon();
            }

            InitializeComponent();
        }
        public virtual void Form_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                // 设置鼠标繁忙状态，并保留原先的状态
                System.Windows.Forms.Cursor holdCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                this.FormLoaded = false;
                try
                {
                    // 必须放在初始化组件之前
                    this.GetIcon();
                    // 获得页面的权限
                    this.GetPermissions();
                    // 加载窗体
                    this.FormOnLoad();
                    // 设置按钮状态
                    this.SetControlState();
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
                    this.ProcessException(catchException);
                }
                finally
                {
                    this.FormLoaded = true;
                    // 设置鼠标默认状态，原来的光标状态
                    this.Cursor = holdCursor;
                }

                if (this.MdiParent == null)
                {
                    //LoadingUtil.StopLoading();
                }
                this.Visible = true;
            }
        }
        #endregion

        #region 窗体按键事件处理
        public virtual void Form_KeyDown(object sender, KeyEventArgs e)
        {
            // 按键事件
            if (e.KeyCode == Keys.F5)
            {
                // F5刷新，重新加载窗体
                this.FormOnLoad();
            }
            else
            {
                // 按了回车按钮处理光标焦点
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                {
                    if ((this.ActiveControl is DevExpress.XtraEditors.TextEdit)
                        || (this.ActiveControl is DevExpress.XtraEditors.TextBoxMaskBox)
                        || (this.ActiveControl is DevExpress.XtraEditors.ImageComboBoxEdit)
                        || (this.ActiveControl is DevExpress.XtraEditors.CheckEdit))
                    {
                        SendKeys.Send("{TAB}");
                    }

                    //if ((this.ActiveControl is TextBox) || (this.ActiveControl is ComboBox) || (this.ActiveControl is CheckBox))
                    //{
                    //    if ((this.ActiveControl is TextBox) && ((TextBox)this.ActiveControl).Multiline)
                    //    {
                    //        return;
                    //    }
                    //    SendKeys.Send("{TAB}");
                    //}
                }
            }
            // 打印界面的快捷方式
            if (e.KeyCode == Keys.F10)
            {
                Image iA = new Bitmap(this.Width, this.Height);
                Graphics g = Graphics.FromImage(iA);
                g.CopyFromScreen(new Point(this.Location.X, this.Location.Y), new Point(0, 0), new Size(this.Width, this.Height));
                Clipboard.SetImage(iA);
                ////FrmPrint frmPrint = new FrmPrint(BaseUserControl.ImageClone(iA));
                ////frmPrint.ShowDialog();
                ////frmPrint.Dispose();
            }
        }
        #endregion

        #region 关闭窗体
        /// <summary>
        /// 关闭窗体
        /// </summary>
        private void FormOnClosed()
        {
            if (!this.DesignMode)
            {
                // 是否记录访问日志，已经登录了系统了，才记录日志
                {
                    // 保存列宽
                    // BaseInterfaceLogic.SaveDataGridViewColumnWidth(this);
                    // 调用服务事件
                }
            }
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!this.DesignMode)
            {
                // 设置鼠标繁忙状态，并保留原先的状态
                System.Windows.Forms.Cursor holdCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    this.FormOnClosed();
                }
                catch (Exception catchException)
                {
                }
                finally
                {
                    // 设置鼠标默认状态，原来的光标状态
                    this.Cursor = holdCursor;
                }
            }
        }
        #endregion

        #region 重载事件
        //窗体加载时
        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
            }
            base.OnLoad(e);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }
        #endregion

        #region IBaseFormExtend 接口实现

        #region public virtual void FormOnLoad() 加载窗体
        /// <summary>
        /// 加载窗体
        /// </summary>
        public virtual void FormOnLoad()
        {
        }
        #endregion

        /// <summary>
        /// 窗体已经加载完毕
        /// </summary>

        public virtual bool FormLoaded
        {
            get
            {
                return _BaseFormExtend.FormLoaded;
            }

            set
            {
                _BaseFormExtend.FormLoaded = value;
            }
        }

        public virtual bool FormShowned
        {
            get
            {
                return _BaseFormExtend.FormShowned;
            }

            set
            {
                _BaseFormExtend.FormShowned = value;
            }
        }

        public virtual bool Busyness
        {
            get
            {
                return _BaseFormExtend.Busyness;
            }

            set
            {
                _BaseFormExtend.Busyness = value;
            }
        }

        public virtual bool Changed
        {
            get
            {
                return _BaseFormExtend.Changed;
            }

            set
            {
                _BaseFormExtend.Changed = value;
            }
        }

        public virtual IDbHelper DbHelper
        {
            get
            {
                return _BaseFormExtend.DbHelper;
            }
        }

        public virtual string EntityId
        {
            get
            {
                return _BaseFormExtend.EntityId;
            }

            set
            {
                _BaseFormExtend.EntityId = value;
            }
        }

        public virtual bool ExitApplication
        {
            get
            {
                return _BaseFormExtend.ExitApplication;
            }

            set
            {
                _BaseFormExtend.ExitApplication = value;
            }
        }
        public virtual bool LoadUserParameters
        {
            get
            {
                return _BaseFormExtend.LoadUserParameters;
            }

            set
            {
                _BaseFormExtend.LoadUserParameters = value;
            }
        }

        public virtual string LogId
        {
            get
            {
                return _BaseFormExtend.LogId;
            }

            set
            {
                _BaseFormExtend.LogId = value;
            }
        }

        public virtual bool PermissionAccess
        {
            get
            {
                return _BaseFormExtend.PermissionAccess;
            }

            set
            {
                _BaseFormExtend.PermissionAccess = value;
            }
        }

        public virtual bool PermissionAdd
        {
            get
            {
                return _BaseFormExtend.PermissionAdd;
            }

            set
            {
                _BaseFormExtend.PermissionAdd = value;
            }
        }

        public virtual bool PermissionAllCompany
        {
            get
            {
                return _BaseFormExtend.PermissionAllCompany;
            }

            set
            {
                _BaseFormExtend.PermissionAllCompany = value;
            }
        }

        public virtual bool PermissionCompany
        {
            get
            {
                return _BaseFormExtend.PermissionCompany;
            }

            set
            {
                _BaseFormExtend.PermissionCompany = value;
            }
        }

        public virtual bool PermissionEdit
        {
            get
            {
                return _BaseFormExtend.PermissionEdit;
            }

            set
            {
                _BaseFormExtend.PermissionEdit = value;
            }
        }

        public virtual bool PermissionExport
        {
            get
            {
                return _BaseFormExtend.PermissionExport;
            }

            set
            {
                _BaseFormExtend.PermissionExport = value;
            }
        }

        public virtual bool PermissionImport
        {
            get
            {
                return _BaseFormExtend.PermissionImport;
            }

            set
            {
                _BaseFormExtend.PermissionImport = value;
            }
        }

        public virtual bool PermissionSearch
        {
            get
            {
                return _BaseFormExtend.PermissionSearch;
            }

            set
            {
                _BaseFormExtend.PermissionSearch = value;
            }
        }

        public virtual bool PermissionSubCompany
        {
            get
            {
                return _BaseFormExtend.PermissionSubCompany;
            }

            set
            {
                _BaseFormExtend.PermissionSubCompany = value;
            }
        }

        public virtual bool RecordFormLog
        {
            get
            {
                return _BaseFormExtend.RecordFormLog;
            }

            set
            {
                _BaseFormExtend.RecordFormLog = value;
            }
        }

        public virtual bool ShowDialogOnly
        {
            get
            {
                return _BaseFormExtend.ShowDialogOnly;
            }

            set
            {
                _BaseFormExtend.ShowDialogOnly = value;
            }
        }

        public virtual IDbHelper UserCenterDbHelper
        {
            get
            {
                return _BaseFormExtend.UserCenterDbHelper;
            }
        }

        public virtual BaseUserInfo UserInfo
        {
            get
            {
                return _BaseFormExtend.UserInfo;
            }

            set
            {
                _BaseFormExtend.UserInfo = value;
            }
        }

        public virtual IDbHelper WorkFlowDbHelper
        {
            get
            {
                return _BaseFormExtend.WorkFlowDbHelper;
            }
        }

        public virtual bool PermissionDelete
        {
            get
            {
                return _BaseFormExtend.PermissionDelete;
            }

            set
            {
                _BaseFormExtend.PermissionDelete = value;
            }
        }

        public virtual int BatchDelete()
        {
            return _BaseFormExtend.BatchDelete();
        }

        public virtual System.Windows.Forms.Cursor BeginWait()
        {
            return _BaseFormExtend.BeginWait();
        }

        public virtual bool CheckInput()
        {
            return _BaseFormExtend.CheckInput();
        }

        public virtual bool CheckInputDelete()
        {
            return _BaseFormExtend.CheckInputDelete();
        }

        public virtual void Comment(string formName = null, string entityId = null)
        {
            _BaseFormExtend.Comment(formName, entityId);
        }

        public virtual void EndWait(System.Windows.Forms.Cursor holdCursor)
        {
            _BaseFormExtend.EndWait(holdCursor);
        }

        public virtual string GetClientVersion(string assemblyString = null)
        {
            return _BaseFormExtend.GetClientVersion(assemblyString);
        }

        public virtual IBaseMainForm GetMainForm()
        {
            return _BaseFormExtend.GetMainForm();
        }

        public virtual void GetIcon()
        {
            _BaseFormExtend.GetIcon();
        }

        public virtual void GetList()
        {
            _BaseFormExtend.GetList();
        }

        public virtual DataTable GetOrganizeDTByPermission(string permissionCode = "Resource.ManagePermission", bool isInnerOrganize = true)
        {
            return _BaseFormExtend.GetOrganizeDTByPermission(permissionCode, isInnerOrganize);
        }

        public virtual void GetPermissions()
        {
            _BaseFormExtend.GetPermissions();
        }

        public virtual bool IsAuthorized(string permissionCode, string permissionItemName = null)
        {
            return _BaseFormExtend.IsAuthorized(permissionCode, permissionItemName);
        }

        public virtual void LoadUserParameter(Form form)
        {
            _BaseFormExtend.LoadUserParameter(form);
        }

        public virtual void Localization(Form form)
        {
            _BaseFormExtend.Localization(form);
        }

        public virtual bool ModuleIsVisible(string formName)
        {
            return _BaseFormExtend.ModuleIsVisible(formName);
        }

        public virtual void PlaySound()
        {
            _BaseFormExtend.PlaySound();
        }

        public virtual void ProcessException(Exception ex, string receiveId = null)
        {
            _BaseFormExtend.ProcessException(ex, receiveId);
        }

        public virtual bool SaveEntity()
        {
            return _BaseFormExtend.SaveEntity();
        }

        public virtual void SetControlState()
        {
            _BaseFormExtend.SetControlState();
        }

        public virtual void SetControlState(bool enabled)
        {
            _BaseFormExtend.SetControlState(enabled);
        }

        public virtual void SetHelp()
        {
            _BaseFormExtend.SetHelp();
        }

        public virtual void ShowEntity()
        {
            _BaseFormExtend.ShowEntity();
        }
        #endregion
    }
}