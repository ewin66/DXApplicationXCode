//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2016 , Zonsun Healthcare Co.Ltd. 
//--------------------------------------------------------------------

using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace DotNet.WinForm
{
    /// <summary>
    /// 设置按钮的可用状态
    /// </summary>
    /// <param name="setTop">置顶</param>
    /// <param name="setUp">上移</param>
    /// <param name="setDown">下移</param>
    /// <param name="setBottom">置底</param>
    /// <param name="add">添加</param>
    /// <param name="edit">编辑</param>
    /// <param name="batchDelete">批量删除</param>
    /// <param name="batchSave">批量保存</param>
    public delegate void SetControlStateEventHandler(bool setTop, bool setUp, bool setDown, bool setBottom, bool add, bool edit, bool batchDelete, bool batchSave);

    /// <summary>
    /// 窗体基类方法接口
    /// </summary>
    public interface IBaseFormExtend
    {
        #region public virtual void FormOnLoad() 加载窗体
        /// <summary>
        /// 加载窗体
        /// </summary>
        void FormOnLoad();
        #endregion

        void ProcessException(Exception ex, string receiveId);

        /// <summary>
        /// 实体主键
        /// </summary>
        string EntityId { get; set; }

        /// <summary>
        /// 只按对话框方式显示窗体
        /// </summary>
        bool ShowDialogOnly { get; set; }

        /// <summary>
        /// 是否记录窗体日志
        /// </summary>
        bool RecordFormLog { get; set; }

        /// <summary>
        /// 是否加在用户配置参数（表格）
        /// </summary>
        bool LoadUserParameters { get; set; }

        /// <summary>
        /// 访问权限
        /// </summary>
        bool PermissionAccess { get; set; }

        /// <summary>
        /// 新增权限
        /// </summary>
        bool PermissionAdd { get; set; }

        /// <summary>
        /// 公司内部的查询权限
        /// </summary>
        bool PermissionCompany { get; set; }

        /// <summary>
        /// 所有公司的权限
        /// </summary>
        bool PermissionAllCompany { get; set; }

        /// <summary>
        /// 所有下属公司的权限
        /// </summary>
        bool PermissionSubCompany { get; set; }

        /// <summary>
        /// 编辑权限
        /// </summary>
        bool PermissionEdit { get; set; }

        /// <summary>
        /// 删除权限
        /// </summary>
        bool PermissionDelete { get; set; }

        /// <summary>
        /// 导入权限
        /// </summary>
        bool PermissionImport { get; set; }

        /// <summary>
        /// 导出权限
        /// </summary>
        bool PermissionExport { get; set; }

        /// <summary>
        /// 查询权限
        /// </summary>
        bool PermissionSearch { get; set; }

        /// <summary>
        /// 获取客户端版本
        /// </summary>
        /// <param name="assemblyString">动态链接库名字</param>
        /// <returns>客户端版本</returns>
        string GetClientVersion(string assemblyString = null);

        void GetIcon();

        /// <summary>
        /// 获取系统的主窗口。
        /// </summary>
        /// <returns></returns>
        IBaseMainForm GetMainForm();

        #region virtual void GetPermissions() 获得页面的权限
        /// <summary>
        /// 获得页面的权限
        /// </summary>
        void GetPermissions();
        #endregion

        #region virtual void SetControlState() 设置控件状态
        /// <summary>
        /// 设置控件状态
        /// </summary>
        void SetControlState();
        #endregion

        #region virtual void SetControlState(bool enabled) 设置按钮状态
        /// <summary>
        /// 设置控件状态
        /// </summary>
        /// <param name="enabled">有效</param>
        void SetControlState(bool enabled);
        #endregion

        #region virtual void SetHelp() 设置帮助
        /// <summary>
        /// 设置帮助
        /// </summary>
        void SetHelp();
        #endregion

        #region virtual void ShowEntity() 显示内容
        /// <summary>
        /// 显示内容
        /// </summary>
        void ShowEntity();
        #endregion

        #region virtual void Search() 获得列表数据
        /// <summary>
        /// 获得列表数据
        /// </summary>
        void GetList();
        #endregion

        void PlaySound();

        #region virtual bool CheckInput() 检查输入的有效性
        /// <summary>
        /// 检查输入的有效性
        /// </summary>
        /// <returns>有效</returns>
        bool CheckInput();
        #endregion

        #region virtual bool SaveEntity() 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>保存成功</returns>
        bool SaveEntity();
        #endregion

        #region virtual int BatchDelete() 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns>影响行数</returns>
        int BatchDelete();
        #endregion

        #region virtual bool CheckInputDelete() 检查批量删除
        /// <summary>
        /// 检查批量删除
        /// </summary>
        /// <returns>允许删除</returns>
        bool CheckInputDelete();
        #endregion

        void Comment(string formName = null, string entityId = null);

        /// <summary>
        /// 是否退出应用程序
        /// </summary>
        bool ExitApplication { get; set; }

        /// <summary>
        /// 窗体已经加载完毕
        /// </summary>
        bool FormLoaded { get; set; }

        /// <summary>
        /// 是否忙碌状态
        /// </summary>
        bool Busyness { get; set; }

        /// <summary>
        /// 数据发生过变化
        /// </summary>
        bool Changed { get; set; }

        /// <summary>
        /// 当前日志主键
        /// </summary>
        string LogId { get; set; }

        /// <summary>
        /// 当前用户信息
        /// 这里表示是只读的
        /// </summary>

        #region void Localization(Form form) 多语言国际化加载
        /// <summary>
        /// 多语言国际化加载
        /// </summary>
        void Localization(Form form);
        #endregion

        #region void LoadUserParameter(Form form) 客户端页面配置加载
        /// <summary>
        /// 客户端页面配置加载
        /// </summary>
        void LoadUserParameter(Form form);
        #endregion

        #region bool IsAuthorized(string permissionCode, string permissionItemName = string.Empty) 是否有相应的权限
        /// <summary>
        /// 是否有相应的权限
        /// </summary>
        /// <param name="permissionCode">权限编号</param>
        /// <param name="permissionItemName">权限名称</param>
        /// <returns>有权限</returns>
        bool IsAuthorized(string permissionCode, string permissionItemName = null);
        #endregion

        /// <summary>
        /// 获取组织机构权限域数据
        /// </summary>
        DataTable GetOrganizeDTByPermission(string permissionCode = "Resource.ManagePermission", bool isInnerOrganize = true);

        #region bool ModuleIsVisible(string formName) 模块是否可见
        /// <summary>
        /// 模块是否可见
        /// 2015-11-20 Author || 逻辑进行改进
        /// </summary>
        /// <param name="formName">模块编号</param>
        /// <returns>有权限</returns>
        bool ModuleIsVisible(string formName);
        #endregion

        /// <summary>
        /// 业务数据库部分
        /// </summary>
        IDbHelper DbHelper { get; }

        /// <summary>
        /// 用户中心数据库部分
        /// </summary>
        IDbHelper UserCenterDbHelper { get; }

        /// <summary>
        /// 工作流数据库部分
        /// </summary>
        IDbHelper WorkFlowDbHelper { get; }

        Cursor BeginWait();

        void EndWait(Cursor holdCursor);
    }

    /// <summary>
    /// 窗体基类方法的实现类。
    /// </summary>
    public class BaseFormExtend
    {
        private Form _OwnerForm = null;

        internal BaseFormExtend(Form ownerForm)
        {
            _OwnerForm = ownerForm;
        }
        private string entityId = string.Empty;

        /// <summary>
        /// 实体主键
        /// </summary>
        public virtual string EntityId
        {
            get
            {
                return this.entityId;
            }
            set
            {
                this.entityId = value;
            }
        }

        /// <summary>
        /// 只按对话框方式显示窗体
        /// </summary>
        public bool ShowDialogOnly = false;

        /// <summary>
        /// 是否记录窗体日志
        /// </summary>
        public bool RecordFormLog = false;

        /// <summary>
        /// 是否加在用户配置参数（表格）
        /// </summary>
        public bool LoadUserParameters = true;

        /// <summary>
        /// 访问权限
        /// </summary>
        public bool PermissionAccess = false;

        /// <summary>
        /// 新增权限
        /// </summary>
        public bool PermissionAdd = false;

        /// <summary>
        /// 公司内部的查询权限
        /// </summary>
        public bool PermissionCompany = false;

        /// <summary>
        /// 所有公司的权限
        /// </summary>
        public bool PermissionAllCompany = false;

        /// <summary>
        /// 所有下属公司的权限
        /// </summary>
        public bool PermissionSubCompany = false;

        /// <summary>
        /// 编辑权限
        /// </summary>
        public bool PermissionEdit = false;

        /// <summary>
        /// 删除权限
        /// </summary>
        public bool PermissionDelete = false;

        /// <summary>
        /// 导入权限
        /// </summary>
        public bool PermissionImport = false;

        /// <summary>
        /// 导出权限
        /// </summary>
        public bool PermissionExport = false;

        /// <summary>
        /// 查询权限
        /// </summary>
        public bool PermissionSearch = false;

        protected GridViewCheckHeadUtil GridCheckUtil = null;

        /// <summary>
        /// 获取客户端版本
        /// </summary>
        /// <param name="assemblyString">动态链接库名字</param>
        /// <returns>客户端版本</returns>
        public string GetClientVersion(string assemblyString = null)
        {
            // 客户端版本号
            if (string.IsNullOrEmpty(assemblyString))
            {
                assemblyString = BaseSystemInfo.MainAssembly;
            }
            Assembly assembly = Assembly.Load(assemblyString);
            return assembly.GetName().Version.ToString();

            /*
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string clientVersion = fileVersionInfo.FileVersion;
            return clientVersion;
            */
        }

        /// <summary>
        /// 获取系统主窗口。
        /// </summary>
        /// <returns></returns>
        public virtual IBaseMainForm GetMainForm()
        {
            IBaseMainForm mainForm = null;
            if (_OwnerForm.MdiParent is IBaseMainForm)
                mainForm = _OwnerForm.MdiParent as IBaseMainForm;
            else if (_OwnerForm.Parent is IBaseMainForm)
                mainForm = _OwnerForm.Parent as IBaseMainForm;
            else if (_OwnerForm.Owner is IBaseMainForm)
                mainForm = _OwnerForm.Owner as IBaseMainForm;
            if (_OwnerForm is ISkinForm)
            {
                if (((ISkinForm)_OwnerForm).RealOwner is IBaseMainForm)
                    mainForm = ((ISkinForm)_OwnerForm).RealOwner as IBaseMainForm;
            }
            if (mainForm == null && _OwnerForm is IBaseMainForm)
                mainForm = _OwnerForm as IBaseMainForm;
            return mainForm;
        }

        public virtual void GetIcon()
        {
        }

        #region public virtual void GetPermissions() 获得页面的权限
        /// <summary>
        /// 获得页面的权限
        /// </summary>
        public virtual void GetPermissions()
        {
        }
        #endregion

        #region public virtual void SetControlState() 设置控件状态
        /// <summary>
        /// 设置控件状态
        /// </summary>
        public virtual void SetControlState()
        {
        }
        #endregion

        #region public virtual void SetControlState(bool enabled) 设置按钮状态
        /// <summary>
        /// 设置控件状态
        /// </summary>
        /// <param name="enabled">有效</param>
        public virtual void SetControlState(bool enabled)
        {
        }
        #endregion

        #region public virtual void SetHelp() 设置帮助
        /// <summary>
        /// 设置帮助
        /// </summary>
        public virtual void SetHelp()
        {
        }
        #endregion

        #region public virtual void ShowEntity() 显示内容
        /// <summary>
        /// 显示内容
        /// </summary>
        public virtual void ShowEntity()
        {
        }
        #endregion

        #region public virtual void Search() 获得列表数据
        /// <summary>
        /// 获得列表数据
        /// </summary>
        public virtual void GetList()
        {
        }
        #endregion

        public void PlaySound()
        {
        }

        #region public virtual bool CheckInput() 检查输入的有效性
        /// <summary>
        /// 检查输入的有效性
        /// </summary>
        /// <returns>有效</returns>
        public virtual bool CheckInput()
        {
            return true;
        }
        #endregion

        #region public virtual bool SaveEntity() 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>保存成功</returns>
        public virtual bool SaveEntity()
        {
            return true;
        }
        #endregion

        #region public virtual int BatchDelete() 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns>影响行数</returns>
        public virtual int BatchDelete()
        {
            return 0;
        }
        #endregion

        #region public virtual bool CheckInputDelete() 检查批量删除
        /// <summary>
        /// 检查批量删除
        /// </summary>
        /// <returns>允许删除</returns>
        public virtual bool CheckInputDelete()
        {
            return true;
        }
        #endregion

        public void Comment(string formName = null, string entityId = null)
        {
            if (string.IsNullOrEmpty(formName))
            {
                formName = _OwnerForm.Name;
            }
            if (string.IsNullOrEmpty(entityId))
            {
                entityId = this.EntityId;
            }

            // FrmCommnets frmCommnets = new FrmCommnets(formName, entityId);
            // frmCommnets.ShowDialog();
            //frmCommnets.Dispose();

            /*
            bool commnets = false;
            List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();
            parameters.Add(new KeyValuePair<string, object>(BaseCommentEntity.FieldCategoryCode, formName));
            parameters.Add(new KeyValuePair<string, object>(BaseCommentEntity.FieldObjectId, entityId));
            parameters.Add(new KeyValuePair<string, object>(BaseCommentEntity.FieldDeletionStateCode, 0));
            // commnets = DbLogic.Exists(this.UserCenterDbHelper, BaseCommentEntity.TableName, parameters);

            // 若有记录显示列表页面，若没记录直接显示添加页面
            if (commnets)
            {
                FrmCommnets frmCommnets = new FrmCommnets(formName, entityId);
                frmCommnets.ShowDialog();
                frmCommnets.Dispose();
            }
            else
            {
                FrmCommentAdd frmCommentAdd = new FrmCommentAdd(formName, entityId);
                frmCommentAdd.ShowDialog();
                frmCommentAdd.Dispose();
            }
            */
        }

        /// <summary>
        /// 是否退出应用程序
        /// </summary>
        public bool ExitApplication = false;

        /// <summary>
        /// 窗体已经加载完毕
        /// </summary>
        public bool FormLoaded = false;

        /// <summary>
        /// 窗体已经显示完毕
        /// </summary>
        public bool FormShowned = false;

        /// <summary>
        /// 是否忙碌状态
        /// </summary>
        public bool Busyness = false;

        /// <summary>
        /// 数据发生过变化
        /// </summary>
        public bool Changed = false;

        /// <summary>
        /// 当前日志主键
        /// </summary>
        public string LogId = string.Empty;

        /// <summary>
        /// 每个页面可以有独立的当前用户信息，多系统更换时为了更好的保持当前状态
        /// 20140720 Author 改进
        /// </summary>
        private BaseUserInfo userInfo = null;

        /// <summary>
        /// 当前用户信息
        /// 这里表示是只读的
        /// </summary>
        public BaseUserInfo UserInfo
        {
            get
            {
                // 加密混淆的，这个属性无法自动复制过去
                if (userInfo == null)
                {
                    userInfo = new BaseUserInfo();
                    BaseBusinessLogic.CopyObjectProperties(BaseSystemInfo.UserInfo, userInfo);
                }

                // 因为每次都可能在变化OpenId
                userInfo.Code = BaseSystemInfo.UserInfo.Code;
                userInfo.CompanyCode = BaseSystemInfo.UserInfo.CompanyCode;
                userInfo.CompanyId = BaseSystemInfo.UserInfo.CompanyId;
                userInfo.CompanyName = BaseSystemInfo.UserInfo.CompanyName;
                userInfo.DepartmentCode = BaseSystemInfo.UserInfo.DepartmentCode;
                userInfo.DepartmentId = BaseSystemInfo.UserInfo.DepartmentId;
                userInfo.DepartmentName = BaseSystemInfo.UserInfo.DepartmentName;
                userInfo.Id = BaseSystemInfo.UserInfo.Id;
                userInfo.IPAddress = BaseSystemInfo.UserInfo.IPAddress;
                userInfo.IsAdministrator = BaseSystemInfo.UserInfo.IsAdministrator;
                userInfo.MACAddress = BaseSystemInfo.UserInfo.MACAddress;
                userInfo.NickName = BaseSystemInfo.UserInfo.NickName;
                userInfo.OpenId = BaseSystemInfo.UserInfo.OpenId;
                userInfo.RealName = BaseSystemInfo.UserInfo.RealName;
                // userInfo.ServiceUserName = BaseSystemInfo.UserInfo.ServiceUserName;
                // userInfo.ServicePassword = BaseSystemInfo.UserInfo.ServicePassword;
                // userInfo.SystemCode = BaseSystemInfo.UserInfo.SystemCode;
                userInfo.UserName = BaseSystemInfo.UserInfo.UserName;
                userInfo.Signature = BaseSystemInfo.UserInfo.Signature;
                return userInfo;
            }
            set
            {
                userInfo = value;
            }
        }

        #region public void Localization(Form form) 多语言国际化加载
        /// <summary>
        /// 多语言国际化加载
        /// </summary>
        public void Localization(Form form)
        {
            // BaseInterfaceLogic.SetLanguageResource(form);
        }
        #endregion

        #region public void LoadUserParameter(Form form) 客户端页面配置加载
        /// <summary>
        /// 客户端页面配置加载
        /// </summary>
        public void LoadUserParameter(Form form)
        {
            // BaseInterfaceLogic.LoadDataGridViewColumnWidth(form);
        }
        #endregion

        #region public bool IsAuthorized(string permissionCode, string permissionName = null) 是否有相应的权限
        /// <summary>
        /// 是否有相应的权限
        /// </summary>
        /// <param name="permissionCode">权限编号</param>
        /// <param name="permissionName">权限名称</param>
        /// <returns>有权限</returns>
        public bool IsAuthorized(string permissionCode, string permissionName = null)
        {
            // 默认为了安全起见、设置为无权限比较好
            bool result = false;
            // 先判断用户是否超级管理员，若是超级管理员，就不用判断操作权限了（这个是有点儿C/S的特色）

            // 若不使用操作权限项定义，那就所有操作权限都是不用生效了
            bool getByCache = false;

            // 操作权限自动增加的功能实现
            /*
            if (BaseSystemInfo.UserInfo.IsAdministrator && !string.IsNullOrWhiteSpace(permissionCode))
            {
                BaseModuleManager moduleManager = new BaseModuleManager(BaseSystemInfo.UserInfo);
                BaseModuleEntity moduleEntity = moduleManager.GetObjectByCode(permissionCode);
                if (moduleEntity == null)
                {
                    moduleEntity = new BaseModuleEntity();
                    moduleEntity.Code = permissionCode;
                    moduleEntity.FullName = permissionName;
                    moduleManager.Add(moduleEntity);
                }
            }
            */

            // 若是调试状态，从服务器上获取权限
            // 加强安全验证防止未授权匿名调用
            if (UserInfo.IsAdministrator)
            {
                return true;
            }

            getByCache = true;
            if (getByCache)
            {
                // 这里也可以优化一下，没必要遍历所有的操作权限列表
                int count = ClientCache.Instance.UserPermissionList.Count(entity => !string.IsNullOrEmpty(entity.Code) && entity.Code.Equals(permissionCode, StringComparison.OrdinalIgnoreCase));
                //foreach (BaseModuleEntity bme in ClientCache.Instance.UserPermissionList)
                //{
                //    LogUtilities.WriteLine("系统编号：{0}，权限编号:：{1}，权限名称：{2}，窗体名称：{3}", bme.CategoryCode, bme.Code, bme.FullName, bme.FormName, bme.FullName);
                //}
                if (count < 1)
                {
                    count = ClientCache.Instance.UserPermissionList.Count(entity => !string.IsNullOrEmpty(entity.FormName) && entity.FormName.Equals(permissionCode, StringComparison.OrdinalIgnoreCase));
                }

                return count > 0;
            }

            // 虽然这样读取权限效率低一些，但是会及时性高一些，例如这个时候正好权限被调整了
            // 这里是在服务器上进行权限判断，远程进行权限判断（B/S的都用这个方法直接判断权限）
            if (!result)
            {
                result = PermissionUtilities.IsAuthorized(UserInfo, UserInfo.SystemCode, UserInfo.Id, permissionCode, permissionName, getByCache).Status;
            }

            return result;
        }
        #endregion

        /// <summary>
        /// 获取组织机构权限域数据
        /// </summary>
        public DataTable GetOrganizeDTByPermission(string permissionCode = "Resource.ManagePermission", bool isInnerOrganize = true)
        {
            // 获取部门数据，不启用权限域
            DataTable dt = new DataTable(BaseOrganizeEntity.TableName);
            if (isInnerOrganize
                || UserInfo.IsAdministrator
                || !BaseSystemInfo.UsePermissionScope
                || String.IsNullOrEmpty(permissionCode))
            {
                dt = ClientCache.Instance.GetInnerOrganizeDT(UserInfo);
                if (isInnerOrganize)
                {
                    // BaseBusinessLogic.SetFilter(dt, BaseOrganizeEntity.FieldIsInnerOrganize, "1");
                    // BaseInterfaceLogic.CheckTreeParentId(dt, BaseOrganizeEntity.FieldId, BaseOrganizeEntity.FieldParentId);
                }
                dt.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
            }
            else
            {
                DotNetService dotNetService = new DotNetService();
                dt = dotNetService.PermissionService.GetOrganizeDTByPermission(UserInfo, UserInfo.Id, permissionCode);
                if (dotNetService.PermissionService is ICommunicationObject)
                {
                    ((ICommunicationObject)dotNetService.PermissionService).Close();
                }
                if (isInnerOrganize)
                {
                    BaseBusinessLogic.SetFilter(dt, BaseOrganizeEntity.FieldIsInnerOrganize, "1");
                    BaseInterfaceLogic.CheckTreeParentId(dt, BaseOrganizeEntity.FieldId, BaseOrganizeEntity.FieldParentId);
                }
                if (dt.Columns.Contains(BaseOrganizeEntity.FieldSortCode))
                {
                    dt.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
                }
            }
            return dt;
        }

        #region public bool ModuleIsVisible(string formName) 模块是否可见
        /// <summary>
        /// 模块是否可见
        /// 2015-11-20 Author || 逻辑进行改进
        /// </summary>
        /// <param name="formName">模块编号</param>
        /// <returns>有权限</returns>
        public bool ModuleIsVisible(string ModelCode)
        {
            bool result = false;
            ClientCache.Instance.UserPermissionList = PermissionUtilities.GetPermissionList(UserInfo, UserInfo.SystemCode, UserInfo.Id);
            //var moduleCode = formName.Replace("Frm", "");
            foreach (var entity in ClientCache.Instance.UserPermissionList)
            {
                //if (((!string.IsNullOrEmpty(entity.Code) && entity.Code.Equals(moduleCode))
                //    || (!string.IsNullOrEmpty(entity.FormName) && entity.FormName.Equals(formName))))
                //{
                //    result = true;
                //    break;
                //}
                if ((!string.IsNullOrEmpty(entity.Code) && entity.Code.Equals(ModelCode) && entity.IsVisible==1))                    
                {
                    result = true;
                    break;
                }
            }

            // 模块是否可见;
            return result;
        }
        #endregion

        public void ProcessException(Exception ex, string receiveId)
        {
            if (ex != null)
                ex.ProcessException(receiveId);
        }

        /// <summary>
        /// 业务数据库部分
        /// </summary>
        private IDbHelper dbHelper = null;

        /// <summary>
        /// 业务数据库部分
        /// </summary>
        public IDbHelper DbHelper
        {
            get
            {
                if (dbHelper == null)
                {
                    // 当前数据库连接对象
                    dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.BusinessDbType, BaseSystemInfo.BusinessDbConnection);
                }
                return dbHelper;
            }
        }

        /// <summary>
        /// 工作流数据库部分
        /// </summary>
        private IDbHelper userCenterDbHelper = null;

        /// <summary>
        /// 用户中心数据库部分
        /// </summary>
        public IDbHelper UserCenterDbHelper
        {
            get
            {
                if (userCenterDbHelper == null)
                {
                    // 当前数据库连接对象
                    userCenterDbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
                }
                return userCenterDbHelper;
            }
        }

        /// <summary>
        /// 工作流数据库部分
        /// </summary>
        private IDbHelper workFlowDbHelper = null;

        /// <summary>
        /// 工作流数据库部分
        /// </summary>
        public IDbHelper WorkFlowDbHelper
        {
            get
            {
                if (workFlowDbHelper == null)
                {
                    // 当前数据库连接对象
                    workFlowDbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, BaseSystemInfo.WorkFlowDbConnection);
                }
                return workFlowDbHelper;
            }
        }

        public Cursor BeginWait()
        {
            // 设置鼠标繁忙状态，并保留原先的状态
            Cursor holdCursor = _OwnerForm.Cursor;
            _OwnerForm.Cursor = Cursors.WaitCursor;
            return holdCursor;
        }

        public void EndWait(Cursor holdCursor)
        {
            // 设置鼠标默认状态，原来的光标状态
            _OwnerForm.Cursor = holdCursor;
        }
    }
}
