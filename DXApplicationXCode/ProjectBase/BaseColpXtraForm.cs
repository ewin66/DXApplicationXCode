using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNet.WinForm.Controls;
using DevExpress.XtraSplashScreen;
using System.Diagnostics;
using System.Reflection;
using System.Net.NetworkInformation;

namespace DotNet.WinForm
{
    public partial class BaseColpXtraForm : BaseXtraForm,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IOverlaySplashScreenHandle handleOverlayForm;

        private const int INTERNET_CONNECTION_MODEM = 1;
        private const int INTERNET_CONNECTION_LAN = 2;

        [System.Runtime.InteropServices.DllImport("winInet.dll")]
        private static extern bool InternetGetConnectedState(ref int dwFlag, int dwReserved);

        public BaseColpXtraForm()
        {
            InitializeComponent();
        }

        #region Entity
        static DataTable _termDetailDataTable = null;
        public static DataTable TermDetailDataTable
        {
            get
            {
                if (_termDetailDataTable == null)
                {
                    if (BaseSystemInfo.UserIsLogOn)
                    {
                        _termDetailDataTable = Colp_Term_DetailUtilities.GetDataTable(BaseSystemInfo.UserInfo, "Colp");
                    }
                }
                return _termDetailDataTable;
            }
            set
            {
                _termDetailDataTable = value;
            }
        }

        
        //private DataTable UserList = new DataTable(BaseUserEntity.TableName);
        public string strPermissionCode = "Resource.ManagePermission";
        static DataTable _userDetailDataTable = null;
        public static DataTable UserDetailDataTable
        {
            get
            {
                //if (_userDetailDataTable == null)
                //{
                    if (BaseSystemInfo.UserIsLogOn)
                    {
                        if (!string.IsNullOrEmpty(BaseSystemInfo.UserInfo.CompanyId))
                        {
                            int recordCount = 0;
                            string stradmin = "超级管理员";
                            string whereClause = BaseUserEntity.TableName + "." + BaseUserEntity.FieldCompanyId + " = '" + BaseSystemInfo.UserInfo.CompanyId + "' ";
                            whereClause += " AND " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldEnabled + " = 1 AND  " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldRealName  + " != '" + stradmin + "'";
                            whereClause += " AND " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldDeletionStateCode + " = 0 AND " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldIsVisible + " = 1 ";
                        //_userDetailDataTable = UserUtilities.GetDataTableByPage(BaseSystemInfo.UserInfo, "*", out recordCount, 0, 1000, whereClause, null, BaseUserEntity.FieldSortCode);
                       
                        _userDetailDataTable = UserUtilities.GetDataTableByPage(BaseSystemInfo.UserInfo, "*", out recordCount, 1, 100, whereClause, null, BaseUserEntity.FieldSortCode,"", false, true, false);
                      
                    }
                    
                }
                //}
                return _userDetailDataTable;
            }
            set
            {
                _userDetailDataTable = value;
            }
        }
        static DataTable _configurationDataTable = null;
        public static DataTable ConfigurationDataTable
        {
            get
            {
                if (_configurationDataTable == null)
                {
                    if (BaseSystemInfo.UserIsLogOn)
                    {
                        _configurationDataTable = Colp_Configuration_ListUtilities.GetDataTable(BaseSystemInfo.UserInfo, "Colp");
                    }
                }
                return _configurationDataTable;
            }
            set
            {
                _configurationDataTable = value;
            }
        }

        #endregion

        #region Configation
        static String _biopsyCervixLabelColor = null;
        public static String BiopsyCervixLabelColor
        {
            get
            {
                if (_termDetailDataTable == null)
                {
                    if (BaseSystemInfo.UserIsLogOn)
                    {
                        _biopsyCervixLabelColor = Convert.ToString(GetConfiguration("BiopsyCervixLabelColor"));
                    }
                }
                return _biopsyCervixLabelColor;
            }
        }
        
        #endregion

        #region 控件
        #endregion
        public static void CurrentDomain_UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception unhandledException = (Exception)args.ExceptionObject;
            ////Console.WriteLine("UnhandledExceptionHandler caught : " + unhandledException.Message);
            unhandledException.ProcessException();
            //throw new NotImplementedException();
            //Application.Restart();
        }
        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs args)
        {
            Exception threadException = (Exception)args.Exception;
            threadException.ProcessException();
            //throw new NotImplementedException();
            ////Application.Restart();
        }

        public void RefreshForm()
        {
            if (BaseSystemInfo.UserIsLogOn)
            {

            }
        }
        
        public void Add()
        {
        }

        public void Delete()
        {

        }
        public void DeleteAll()
        {

        }
        public void DeleteCurrent()
        {
        }

        public void Save()
        {

        }
        public void SaveAll()
        {

        }
        public void Colp_Check_ListEntityClear(Colp_Check_ListEntity clearColp_Check_ListEntity)
        {
            #region  UI数据对应被清空

            clearColp_Check_ListEntity.Id = null;
            clearColp_Check_ListEntity.Parent_Id = null;
            clearColp_Check_ListEntity.Parent_Name = null;
            clearColp_Check_ListEntity.Parent_Code = null;
            clearColp_Check_ListEntity.Organization_Id = null;
            clearColp_Check_ListEntity.Organization_Name = null;
            clearColp_Check_ListEntity.Organization_Code = null;
            clearColp_Check_ListEntity.Department_Id = null;
            clearColp_Check_ListEntity.Department_Name = null;
            clearColp_Check_ListEntity.Department_Code = null;
            clearColp_Check_ListEntity.Equipment_Id = null;
            clearColp_Check_ListEntity.Equipment_Name = null;
            clearColp_Check_ListEntity.Equipment_Code = null;
            clearColp_Check_ListEntity.User_Id = null;
            clearColp_Check_ListEntity.User_Name = null;
            clearColp_Check_ListEntity.User_Code = null;
            clearColp_Check_ListEntity.Code = null;
            clearColp_Check_ListEntity.Date = null;
            clearColp_Check_ListEntity.Full_Name = null;
            clearColp_Check_ListEntity.Name = null;
            clearColp_Check_ListEntity.Age = null;
            clearColp_Check_ListEntity.Gender = null;
            clearColp_Check_ListEntity.Nationality = null;
            clearColp_Check_ListEntity.BirthDate = null;
            clearColp_Check_ListEntity.IdType = null;
            clearColp_Check_ListEntity.IdNumber = null;
            clearColp_Check_ListEntity.Marriage = null;
            clearColp_Check_ListEntity.Occupation = null;
            clearColp_Check_ListEntity.HomeAddress = null;
            clearColp_Check_ListEntity.RegisterDate = null;
            clearColp_Check_ListEntity.PatientDirectory = null;
            clearColp_Check_ListEntity.ContactNumber = null;
            clearColp_Check_ListEntity.Lastdaymenses = null;
            clearColp_Check_ListEntity.Source = null;
            clearColp_Check_ListEntity.HCG = null;
            clearColp_Check_ListEntity.Condommode = null;
            clearColp_Check_ListEntity.PregnancyNumber = null;
            clearColp_Check_ListEntity.ProduceNumber = null;
            clearColp_Check_ListEntity.AbortionNumber = null;
            clearColp_Check_ListEntity.BloodType = null;
            clearColp_Check_ListEntity.PartnerNumber = null;
            clearColp_Check_ListEntity.Smoke = null;
            clearColp_Check_ListEntity.Remark = null;
            clearColp_Check_ListEntity.IsRecheck = null;
            clearColp_Check_ListEntity.PregnancyState = null;
            clearColp_Check_ListEntity.PregnancyWeeks = null;
            clearColp_Check_ListEntity.MenopauseStatus = null;
            clearColp_Check_ListEntity.Ulcer = null;
            clearColp_Check_ListEntity.ExogenicLesion = null;
            clearColp_Check_ListEntity.IrregularBleeding = null;
            clearColp_Check_ListEntity.PhysicalExamination = null;
            clearColp_Check_ListEntity.GestationHighLesions = null;
            clearColp_Check_ListEntity.TreatmentHistory = null;
            clearColp_Check_ListEntity.MedicalHistory = null;
            clearColp_Check_ListEntity.TCTCheckResult = null;
            clearColp_Check_ListEntity.TCTCheckDate = null;
            clearColp_Check_ListEntity.TCTCheckResultSource = null;
            clearColp_Check_ListEntity.HPVCheckResult = null;
            clearColp_Check_ListEntity.HPVCheckDate = null;
            clearColp_Check_ListEntity.HPVCheckResultSource = null;
            clearColp_Check_ListEntity.HistopathologyDiagnoseResult = null;
            clearColp_Check_ListEntity.HistopathologyDiagnoseDate = null;
            clearColp_Check_ListEntity.HistopathologyDiagnoseResultSource = null;
            clearColp_Check_ListEntity.OtherCharacteristic = null;
            clearColp_Check_ListEntity.CheckDirectory = null;
            clearColp_Check_ListEntity.Category_Code = null;
            clearColp_Check_ListEntity.Category = null;
            clearColp_Check_ListEntity.State_Code = null;
            clearColp_Check_ListEntity.State = null;
            clearColp_Check_ListEntity.Sort_Code = null;
            clearColp_Check_ListEntity.Allow_Edit = null;
            clearColp_Check_ListEntity.Allow_Delete = null;
            clearColp_Check_ListEntity.Is_Visible = null;
            clearColp_Check_ListEntity.Is_Public = null;
            clearColp_Check_ListEntity.Enabled = null;
            clearColp_Check_ListEntity.Deletion_State_Code = null;
            clearColp_Check_ListEntity.Contents = null;
            clearColp_Check_ListEntity.Description = null;
            clearColp_Check_ListEntity.Tag = null;
            clearColp_Check_ListEntity.Encryption_Code = null;
            clearColp_Check_ListEntity.Software_Version = null;
            clearColp_Check_ListEntity.Language_Version = null;
            clearColp_Check_ListEntity.Term_Version = null;
            //clearColp_Check_ListEntity.Create_On = null;
            //clearColp_Check_ListEntity.Create_User_Id = null;
            //clearColp_Check_ListEntity.Create_By = null;
            //clearColp_Check_ListEntity.Modified_On = null;
            //clearColp_Check_ListEntity.Modified_User_Id = null;
            //clearColp_Check_ListEntity.Modified_By = null;
            clearColp_Check_ListEntity.Tag_BindingSource = null;
            clearColp_Check_ListEntity.Tag_DataRowView = null;
            clearColp_Check_ListEntity.Tag_DataTable = null;
            clearColp_Check_ListEntity.Tag_Object = null;
            #endregion

            clearColp_Check_ListEntity = null;
        }

        public void Colp_Appointment_DetailEntityClear(Colp_Appointment_DetailEntity clearColp_Appointment_DetailEntity)
        {
            #region  UI数据对应被清空

            clearColp_Appointment_DetailEntity.Id = null;
            clearColp_Appointment_DetailEntity.Parent_Id = null;
            clearColp_Appointment_DetailEntity.Parent_Name = null;
            clearColp_Appointment_DetailEntity.Parent_Code = null;
            clearColp_Appointment_DetailEntity.Organization_Id = null;
            clearColp_Appointment_DetailEntity.Organization_Name = null;
            clearColp_Appointment_DetailEntity.Organization_Code = null;
            clearColp_Appointment_DetailEntity.Department_Id = null;
            clearColp_Appointment_DetailEntity.Department_Name = null;
            clearColp_Appointment_DetailEntity.Department_Code = null;
            clearColp_Appointment_DetailEntity.Equipment_Id = null;
            clearColp_Appointment_DetailEntity.Equipment_Name = null;
            clearColp_Appointment_DetailEntity.Equipment_Code = null;
            clearColp_Appointment_DetailEntity.User_Id = null;
            clearColp_Appointment_DetailEntity.User_Name = null;
            clearColp_Appointment_DetailEntity.User_Code = null;
            clearColp_Appointment_DetailEntity.Code = null;
            clearColp_Appointment_DetailEntity.Name = null;
            clearColp_Appointment_DetailEntity.Contents = null;
            clearColp_Appointment_DetailEntity.AppointmentDate = null;
            clearColp_Appointment_DetailEntity.AppointmentTime = null;
            clearColp_Appointment_DetailEntity.Remark = null;
            clearColp_Appointment_DetailEntity.Category_Code = null;
            clearColp_Appointment_DetailEntity.Category = null;
            clearColp_Appointment_DetailEntity.State_Code = null;
            clearColp_Appointment_DetailEntity.State = null;
            clearColp_Appointment_DetailEntity.Sort_Code = null;
            clearColp_Appointment_DetailEntity.Allow_Edit = null;
            clearColp_Appointment_DetailEntity.Allow_Delete = null;
            clearColp_Appointment_DetailEntity.Is_Visible = null;
            clearColp_Appointment_DetailEntity.Is_Public = null;
            clearColp_Appointment_DetailEntity.Enabled = null;
            clearColp_Appointment_DetailEntity.Deletion_State_Code = null;
            clearColp_Appointment_DetailEntity.Contents = null;
            clearColp_Appointment_DetailEntity.Description = null;
            clearColp_Appointment_DetailEntity.Tag = null;
            clearColp_Appointment_DetailEntity.Encryption_Code = null;

            //clearColp_Appointment_DetailEntity.Create_On = null;
            //clearColp_Appointment_DetailEntity.Create_User_Id = null;
            //clearColp_Appointment_DetailEntity.Create_By = null;
            //clearColp_Appointment_DetailEntity.Modified_On = null;
            //clearColp_Appointment_DetailEntity.Modified_User_Id = null;
            //clearColp_Appointment_DetailEntity.Modified_By = null;
            clearColp_Appointment_DetailEntity.Tag_BindingSource = null;
            clearColp_Appointment_DetailEntity.Tag_DataRowView = null;
            clearColp_Appointment_DetailEntity.Tag_DataTable = null;
            clearColp_Appointment_DetailEntity.Tag_Object = null;
            #endregion

            clearColp_Appointment_DetailEntity = null;
        }

        public void Colp_Flup_DetailEntityClear(Colp_Flup_DetailEntity clearColp_Flup_DetailEntity)
        {
            #region  UI数据对应被清空

            clearColp_Flup_DetailEntity.Id = null;
            clearColp_Flup_DetailEntity.Parent_Id = null;
            clearColp_Flup_DetailEntity.Parent_Name = null;
            clearColp_Flup_DetailEntity.Parent_Code = null;
            clearColp_Flup_DetailEntity.Organization_Id = null;
            clearColp_Flup_DetailEntity.Organization_Name = null;
            clearColp_Flup_DetailEntity.Organization_Code = null;
            clearColp_Flup_DetailEntity.Department_Id = null;
            clearColp_Flup_DetailEntity.Department_Name = null;
            clearColp_Flup_DetailEntity.Department_Code = null;
            clearColp_Flup_DetailEntity.Equipment_Id = null;
            clearColp_Flup_DetailEntity.Equipment_Name = null;
            clearColp_Flup_DetailEntity.Equipment_Code = null;
            clearColp_Flup_DetailEntity.User_Id = null;
            clearColp_Flup_DetailEntity.User_Name = null;
            clearColp_Flup_DetailEntity.User_Code = null;
            clearColp_Flup_DetailEntity.Code = null;
            clearColp_Flup_DetailEntity.Date = null;
            clearColp_Flup_DetailEntity.Name = null;
            clearColp_Flup_DetailEntity.FlupItem = null;
            clearColp_Flup_DetailEntity.FlupTimeInterval = null;
            clearColp_Flup_DetailEntity.Remark = null;
            clearColp_Flup_DetailEntity.Category_Code = null;
            clearColp_Flup_DetailEntity.Category = null;
            clearColp_Flup_DetailEntity.State_Code = null;
            clearColp_Flup_DetailEntity.State = null;
            clearColp_Flup_DetailEntity.Sort_Code = null;
            clearColp_Flup_DetailEntity.Allow_Edit = null;
            clearColp_Flup_DetailEntity.Allow_Delete = null;
            clearColp_Flup_DetailEntity.Is_Visible = null;
            clearColp_Flup_DetailEntity.Is_Public = null;
            clearColp_Flup_DetailEntity.Enabled = null;
            clearColp_Flup_DetailEntity.Deletion_State_Code = null;
            clearColp_Flup_DetailEntity.Contents = null;
            clearColp_Flup_DetailEntity.Description = null;
            clearColp_Flup_DetailEntity.Tag = null;
            clearColp_Flup_DetailEntity.Encryption_Code = null;
           
            //clearColp_Flup_DetailEntity.Create_On = null;
            //clearColp_Flup_DetailEntity.Create_User_Id = null;
            //clearColp_Flup_DetailEntity.Create_By = null;
            //clearColp_Flup_DetailEntity.Modified_On = null;
            //clearColp_Flup_DetailEntity.Modified_User_Id = null;
            //clearColp_Flup_DetailEntity.Modified_By = null;
            clearColp_Flup_DetailEntity.Tag_BindingSource = null;
            clearColp_Flup_DetailEntity.Tag_DataRowView = null;
            clearColp_Flup_DetailEntity.Tag_DataTable = null;
            clearColp_Flup_DetailEntity.Tag_Object = null;
            #endregion

            clearColp_Flup_DetailEntity = null;
        }

        #region public bool AddCheckListEntity(Colp_Check_ListEntity addColpCheckListEntity)
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>保存成功</returns>
        public BaseResult AddCheckListEntity(Colp_Check_ListEntity addColpCheckListEntity)
        {
            BaseResult result = Colp_Check_ListUtilities.Add(this.UserInfo, "Colp", addColpCheckListEntity);

            if (result.StatusCode == Status.OKAdd.ToString())
            {
                if (BaseSystemInfo.ShowInformation)
                {
                    // 成功，进行提示
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Changed = true;
            }
            else
            {
                //DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 是否名称重复了，提高友善性
                if (result.StatusCode == Status.ErrorNameExist.ToString())
                {

                }
            }
            return result;
        }
        #endregion
        public BaseResult AddAppointmentListEntity(Colp_Appointment_DetailEntity addAppointmentListEntity)
        {
            BaseResult result = Colp_Appointment_DetailUtilities.Add(this.UserInfo, "Colp", addAppointmentListEntity);

            if (result.StatusCode == Status.OKAdd.ToString())
            {
                if (BaseSystemInfo.ShowInformation)
                {
                    // 成功，进行提示
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Changed = true;
            }
            else
            {
                //DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 是否名称重复了，提高友善性
                if (result.StatusCode == Status.ErrorNameExist.ToString())
                {

                }
            }
            return result;
        }
        public BaseResult AddFlupDetailEntity(Colp_Flup_DetailEntity addColpFlupDetailEntity)
        {
            BaseResult result = Colp_Flup_DetailUtilities.Add(this.UserInfo, "Colp", addColpFlupDetailEntity);

            if (result.StatusCode == Status.OKAdd.ToString())
            {
                if (BaseSystemInfo.ShowInformation)
                {
                    // 成功，进行提示
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Changed = true;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 是否名称重复了，提高友善性
                if (result.StatusCode == Status.ErrorNameExist.ToString())
                {

                }
            }
            return result;
        }

        #region public SaveCheckListEntity(Colp_Check_ListEntity saveColp_Check_ListEntity)
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>保存成功</returns>
        public BaseResult SaveCheckListEntity(Colp_Check_ListEntity saveColp_Check_ListEntity)
        {
            try
            {
                saveColp_Check_ListEntity.Modified_On = DateTime.Now;

                if (String.IsNullOrEmpty(saveColp_Check_ListEntity.CheckDirectory) ||
                   !Directory.Exists(saveColp_Check_ListEntity.CheckDirectory))
                {
                    if (saveColp_Check_ListEntity.RegisterDate == null)
                    {
                        return null;
                    }
                    DateTime RegisterDateDirectoryDateTime = Convert.ToDateTime(saveColp_Check_ListEntity.RegisterDate);
                    if (RegisterDateDirectoryDateTime != null)
                    {   //用DateTime.Now.ToString("yyyyMMddHHmmssfff") 替换 saveColp_Check_ListEntity.Code
                        String yyyyString = RegisterDateDirectoryDateTime.ToString("yyyy");
                        String MMString = RegisterDateDirectoryDateTime.ToString("MM");
                        String ddString = RegisterDateDirectoryDateTime.ToString("dd");
                        String DatayyyyMMddPathCodeString = Path.Combine(ColpSystemInfoClass.DataFullPath,
                                                                 yyyyString,
                                                                 MMString,
                                                                 ddString,
                                                                 DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                        if (!Directory.Exists(DatayyyyMMddPathCodeString) && DatayyyyMMddPathCodeString.Length > 32)
                        {
                            Directory.CreateDirectory(DatayyyyMMddPathCodeString);
                        }
                        saveColp_Check_ListEntity.CheckDirectory = DatayyyyMMddPathCodeString;
                    }
                }

                JsonResult<bool> entityExits = null;

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Colp_Check_ListEntity.FieldId, saveColp_Check_ListEntity.Id);
                entityExits = Colp_Check_ListUtilities.Exists(this.UserInfo, "Colp", parameters);
                if (entityExits.Status && entityExits.Data == false)
                {
                    //不存在则新建
                    BaseResult result = AddCheckListEntity(saveColp_Check_ListEntity);
                    if (result.StatusCode == Status.OKAdd.ToString())
                    {
                        if (BaseSystemInfo.ShowInformation)
                        {
                            if (this.ParentForm is RibbonFormMain)
                            {
                                RibbonFormMain ribbonFormMain = this.ParentForm as RibbonFormMain;
                                ribbonFormMain.ShowAlartInformation("SmartColp:",
                                        $"成功新建记录：{saveColp_Check_ListEntity.Name}",
                                        $"检查编号：{saveColp_Check_ListEntity.Code}",
                                        null, null, true);
                            }
                        }
                    }
                    else
                    {
                        if (result.StatusCode == Status.ErrorNameExist.ToString())
                        {
                        }
                    }
                    return result;
                }
                else
                {
                    Boolean update = IsDirtyColpCheckList(saveColp_Check_ListEntity);

                    ////Colp_Check_ListEntity existColp_Check_ListEntity = Colp_Check_ListUtilities.GetObject(this.UserInfo, "Colp", saveColp_Check_ListEntity.Id);
                    ////if (existColp_Check_ListEntity != null &&
                    ////    existColp_Check_ListEntity.Modified_On < DateTime.Now.Date
                    ////    )  //只开放当天的数据供修改,也可以避免频繁写数据问题
                    ////{
                    ////    ////////update = false;
                    ////}

                    if (update)
                    {
                        BaseResult updateBaseResult = Colp_Check_ListUtilities.Update(this.UserInfo, "Colp", saveColp_Check_ListEntity);

                        if (updateBaseResult != null)
                        {
                            saveColp_Check_ListEntity.Tag_DataRowView.Row.BeginEdit();
                            saveColp_Check_ListEntity.UpdateTo(saveColp_Check_ListEntity.Tag_DataRowView.Row);
                            saveColp_Check_ListEntity.Tag_DataRowView.Row.AcceptChanges();
                            saveColp_Check_ListEntity.Tag_DataRowView.Row.EndEdit();
                            ////////if (CurrentColp_Check_ListEntity.Tag_DataRowView.Row.RowState != DataRowState.Unchanged &&
                            ////////    (CurrentColp_Check_ListEntity.Tag_DataRowView.Row.RowState == DataRowState.Modified ||
                            ////////    CurrentColp_Check_ListEntity.Tag_DataRowView.Row.RowState == DataRowState.Added))
                            ////////{
                            ////////    //切换新行要把原行的数据保存到数据库
                            ////////}
                        }

                        if (updateBaseResult.Status == true || updateBaseResult.StatusCode == Status.OKUpdate.ToString())
                        {
                            if (BaseSystemInfo.ShowInformation)
                            {
                                if (this.ParentForm is RibbonFormMain)
                                {
                                    RibbonFormMain ribbonFormMain = this.ParentForm as RibbonFormMain;
                                    ribbonFormMain.ShowAlartInformation("SmartColp:",
                                            $"保存成功：{saveColp_Check_ListEntity.Name}",
                                            $"检查编号：{saveColp_Check_ListEntity.Code}",
                                            null, null, true);
                                }
                            }
                            this.Changed = true;
                            return updateBaseResult;
                        }
                        else
                        {
                            if (BaseSystemInfo.ShowInformation)
                            {
                                if (this.ParentForm is RibbonFormMain)
                                {
                                    RibbonFormMain ribbonFormMain = this.ParentForm as RibbonFormMain;
                                    ribbonFormMain.ShowAlartInformation("SmartColp:",
                                            $"保存失败：{saveColp_Check_ListEntity.Name}",
                                            $"检查编号：{saveColp_Check_ListEntity.Code}",
                                            null, null, true);
                                }
                            }
                            // 是否名称重复了，提高友善性
                            if (updateBaseResult.StatusCode == Status.ErrorNameExist.ToString())
                            {
                                DevExpress.XtraEditors.XtraMessageBox.Show("保存失败" + updateBaseResult.StatusMessage, "提示信息：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            return updateBaseResult;
                        }
                    }
                    return null;
                }
            }
            catch (OutOfMemoryException catchOutOfMemoryException)
            {
                if (DotNet.WinForm.Properties.Settings.Default.ShowcatchOutOfMemoryException) catchOutOfMemoryException.ProcessException();
                return null;
            }
            catch (Exception catchException)
            {
                if (DotNet.WinForm.Properties.Settings.Default.ShowcatchOutOfMemoryException) catchException.ProcessException();
                return null;
            }
        }
        #endregion

        #region 保存随访
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>保存成功</returns>
        public BaseResult SaveFlupDetailEntity(Colp_Flup_DetailEntity saveColp_Flup_DetailEntity)
        {
            //saveColp_Flup_DetailEntity.Modified_On = DateTime.Now;

            //if (String.IsNullOrEmpty(saveColp_Flup_DetailEntity.CheckDirectory) ||
            //   !Directory.Exists(saveColp_Flup_DetailEntity.CheckDirectory))
            //{
            //    if (saveColp_Flup_DetailEntity.RegisterDate == null)
            //    {
            //        return null;
            //    }
            //    DateTime RegisterDateDirectoryDateTime = Convert.ToDateTime(saveColp_Flup_DetailEntity.RegisterDate);
            //    if (RegisterDateDirectoryDateTime != null)
            //    {
            //        String yyyyString = RegisterDateDirectoryDateTime.ToString("yyyy");
            //        String MMString = RegisterDateDirectoryDateTime.ToString("MM");
            //        String ddString = RegisterDateDirectoryDateTime.ToString("dd");
            //        String DatayyyyMMddPathCodeString = Path.Combine(ColpSystemInfoClass.DataFullPath,
            //                                                 yyyyString,
            //                                                 MMString,
            //                                                 ddString,
            //                                                 saveColp_Flup_DetailEntity.Code);
            //        if (!Directory.Exists(DatayyyyMMddPathCodeString) && DatayyyyMMddPathCodeString.Length > 32)
            //        {
            //            Directory.CreateDirectory(DatayyyyMMddPathCodeString);
            //        }
            //        saveColp_Flup_DetailEntity.CheckDirectory = DatayyyyMMddPathCodeString;
            //    }
            //}

            JsonResult<bool> entityExits = null;

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            if (saveColp_Flup_DetailEntity != null)
            {
                parameters.Add(Colp_Flup_DetailEntity.FieldId, saveColp_Flup_DetailEntity.Id);
            }
            else
            {
                return entityExits;
            }
            entityExits = Colp_Flup_DetailUtilities.Exists(this.UserInfo, "Colp", parameters);
            if (entityExits.Status && entityExits.Data == false)
            {
                //不存在则新建
                return AddFlupDetailEntity(saveColp_Flup_DetailEntity);
            }
            else
            {
                BaseResult updateBaseResult = Colp_Flup_DetailUtilities.Update(this.UserInfo, "Colp", saveColp_Flup_DetailEntity);

                if (updateBaseResult.Status == true || updateBaseResult.StatusCode == Status.OKUpdate.ToString())
                {

                    if (BaseSystemInfo.ShowInformation)
                    {
                        // 成功，进行提示
                        ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ////DevExpress.XtraEditors.XtraMessageBox.Show("保存成功" + updateBaseResult.StatusMessage, "提示信息：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    this.Changed = true;
                    return updateBaseResult;
                }
                else
                {
                    ////DevExpress.XtraEditors.XtraMessageBox.Show("保存失败" + updateBaseResult.StatusMessage, "提示信息：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // 是否名称重复了，提高友善性
                    if (updateBaseResult.StatusCode == Status.ErrorNameExist.ToString())
                    {

                    }
                    return updateBaseResult;
                }
            }
        }
        #endregion

        #region public DeleteCheckListEntity(Colp_Check_ListEntity deleteColp_Check_ListEntity)
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns>成功</returns>
        public bool DeleteCheckListEntity(Colp_Check_ListEntity deleteColp_Check_ListEntity)
        {
            String code = deleteColp_Check_ListEntity.Code;
            String id = deleteColp_Check_ListEntity.Id;

            BaseResult result = Colp_Check_ListUtilities.SetDeleted(this.UserInfo, "Colp", new string[] { deleteColp_Check_ListEntity.Id });

            if (result.Status == true || result.StatusCode == Status.OKDelete.ToString())
            {
                if (deleteColp_Check_ListEntity.Tag_DataRowView != null)
                {
                    DataRowView tag_DataRowView = deleteColp_Check_ListEntity.Tag_DataRowView;
                    deleteColp_Check_ListEntity.Tag_DataRowView = null;   //现行清空
                    tag_DataRowView.Delete();   //从表格中删除掉对应行，会引起行index改变事件
                }
                deleteColp_Check_ListEntity.Tag_BindingSource = null;
                if (deleteColp_Check_ListEntity.Tag_DataRowView != null) deleteColp_Check_ListEntity.Tag_DataRowView = null;
                deleteColp_Check_ListEntity.Tag_DataTable = null;
                deleteColp_Check_ListEntity.Tag_Object = null;

                if (BaseSystemInfo.ShowInformation)
                {
                    // 成功，进行提示
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage + ",编号：" + code, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Changed = true;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 是否名称重复了，提高友善性
                if (result.StatusCode == Status.ErrorNameExist.ToString())
                {

                }
            }
            return result.Status;
        }
        #endregion


        //DeleteFlupDetailEntity
        public bool DeleteAppointmentDetailEntity(Colp_Appointment_DetailEntity deleteColp_Appointment_DetailEntity)
        {
            String code = deleteColp_Appointment_DetailEntity.Code;
            String id = deleteColp_Appointment_DetailEntity.Id;

            BaseResult result = Colp_Appointment_DetailUtilities.SetDeleted(this.UserInfo, "Colp", new string[] { deleteColp_Appointment_DetailEntity.Id });

            if (result.Status == true || result.StatusCode == Status.OKDelete.ToString())
            {
                if (deleteColp_Appointment_DetailEntity.Tag_DataRowView != null)
                {
                    DataRowView tag_DataRowView = deleteColp_Appointment_DetailEntity.Tag_DataRowView;
                    deleteColp_Appointment_DetailEntity.Tag_DataRowView = null;   //现行清空
                    tag_DataRowView.Delete();   //从表格中删除掉对应行，会引起行index改变事件
                }
                deleteColp_Appointment_DetailEntity.Tag_BindingSource = null;
                if (deleteColp_Appointment_DetailEntity.Tag_DataRowView != null) deleteColp_Appointment_DetailEntity.Tag_DataRowView = null;
                deleteColp_Appointment_DetailEntity.Tag_DataTable = null;
                deleteColp_Appointment_DetailEntity.Tag_Object = null;

                if (BaseSystemInfo.ShowInformation)
                {
                    // 成功，进行提示
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage + ",编号：" + code, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Changed = true;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 是否名称重复了，提高友善性
                if (result.StatusCode == Status.ErrorNameExist.ToString())
                {

                }
            }
            return result.Status;
        }
        public BaseResult SaveAppointmentListEntity(Colp_Appointment_DetailEntity saveColp_Appointment_DetailEntity)
        {
            try
            {
                saveColp_Appointment_DetailEntity.Modified_On = DateTime.Now;
                JsonResult<bool> entityExits = null;
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Colp_Appointment_DetailEntity.FieldId, saveColp_Appointment_DetailEntity.Id);
                entityExits = Colp_Appointment_DetailUtilities.Exists(this.UserInfo, "Colp", parameters);
                if (entityExits.Status && entityExits.Data == false)
                {
                    //不存在则新建
                    BaseResult result = AddAppointmentListEntity(saveColp_Appointment_DetailEntity);
                    if (result.StatusCode == Status.OKAdd.ToString())
                    {
                        if (BaseSystemInfo.ShowInformation)
                        {
                            if (this.ParentForm is RibbonFormMain)
                            {
                                RibbonFormMain ribbonFormMain = this.ParentForm as RibbonFormMain;
                                ribbonFormMain.ShowAlartInformation("SmartColp:",
                                        $"成功新建记录：{saveColp_Appointment_DetailEntity.Name}",
                                        $"检查编号：{saveColp_Appointment_DetailEntity.Code}",
                                        null, null, true);
                            }
                        }
                    }
                    else
                    {
                        if (result.StatusCode == Status.ErrorNameExist.ToString())
                        {
                        }
                    }
                    return result;
                }
                else
                {
                    //Colp_Appointment_DetailEntity existColp_Appointment_DetailEntity = Colp_Appointment_DetailUtilities.GetObject(this.UserInfo, "Colp", saveColp_Appointment_DetailEntity.Id);
                    //Boolean update = true;
                    //if (existColp_Appointment_DetailEntity != null &&
                    //    existColp_Appointment_DetailEntity.Modified_On < DateTime.Now.Date
                    //    )  //只开放当天的数据供修改,也可以避免频繁写数据问题
                    //{
                    //    update = false;
                    //}
                    Boolean update = IsDirtyColpAppointmentList(saveColp_Appointment_DetailEntity);

                    if (update)
                    {
                        BaseResult updateBaseResult = Colp_Appointment_DetailUtilities.Update(this.UserInfo, "Colp", saveColp_Appointment_DetailEntity);

                        if (updateBaseResult.Status == true || updateBaseResult.StatusCode == Status.OKUpdate.ToString())
                        {
                            if (BaseSystemInfo.ShowInformation)
                            {
                                if (this.ParentForm is RibbonFormMain)
                                {
                                    RibbonFormMain ribbonFormMain = this.ParentForm as RibbonFormMain;
                                    ribbonFormMain.ShowAlartInformation("SmartColp:",
                                            $"保存成功：{saveColp_Appointment_DetailEntity.Name}",
                                            $"检查编号：{saveColp_Appointment_DetailEntity.Code}",
                                            null, null, true);
                                }
                            }
                            this.Changed = true;
                            return updateBaseResult;
                        }
                        else
                        {
                            if (BaseSystemInfo.ShowInformation)
                            {
                                if (this.ParentForm is RibbonFormMain)
                                {
                                    RibbonFormMain ribbonFormMain = this.ParentForm as RibbonFormMain;
                                    ribbonFormMain.ShowAlartInformation("SmartColp:",
                                            $"保存失败：{saveColp_Appointment_DetailEntity.Name}",
                                            $"预约编号：{saveColp_Appointment_DetailEntity.Code}",
                                            null, null, true);
                                }
                            }
                            // 是否名称重复了，提高友善性
                            if (updateBaseResult.StatusCode == Status.ErrorNameExist.ToString())
                            {
                                DevExpress.XtraEditors.XtraMessageBox.Show("保存失败" + updateBaseResult.StatusMessage, "提示信息：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            return updateBaseResult;
                        }
                    }
                    return null;
                }
            }
            catch (OutOfMemoryException catchOutOfMemoryException)
            {
                if (DotNet.WinForm.Properties.Settings.Default.ShowcatchOutOfMemoryException) catchOutOfMemoryException.ProcessException();
                return null;
            }
            catch (Exception catchException)
            {
                if (DotNet.WinForm.Properties.Settings.Default.ShowcatchOutOfMemoryException) catchException.ProcessException();
                return null;
            }
        }
        public bool DeleteFlupDetailEntity(Colp_Flup_DetailEntity deleteColp_Flup_DetailEntity)
        {
            String code = deleteColp_Flup_DetailEntity.Code;
            String id = deleteColp_Flup_DetailEntity.Id;

            BaseResult result = Colp_Flup_DetailUtilities.SetDeleted(this.UserInfo, "Colp", new string[] { deleteColp_Flup_DetailEntity.Id });

            if (result.Status == true || result.StatusCode == Status.OKDelete.ToString())
            {
                if (deleteColp_Flup_DetailEntity.Tag_DataRowView != null)
                {
                    DataRowView tag_DataRowView = deleteColp_Flup_DetailEntity.Tag_DataRowView;
                    deleteColp_Flup_DetailEntity.Tag_DataRowView = null;   //现行清空
                    tag_DataRowView.Delete();   //从表格中删除掉对应行，会引起行index改变事件
                }
                deleteColp_Flup_DetailEntity.Tag_BindingSource = null;
                if (deleteColp_Flup_DetailEntity.Tag_DataRowView != null) deleteColp_Flup_DetailEntity.Tag_DataRowView = null;
                deleteColp_Flup_DetailEntity.Tag_DataTable = null;
                deleteColp_Flup_DetailEntity.Tag_Object = null;

                if (BaseSystemInfo.ShowInformation)
                {
                    // 成功，进行提示
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage + ",编号：" + code, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Changed = true;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 是否名称重复了，提高友善性
                if (result.StatusCode == Status.ErrorNameExist.ToString())
                {

                }
            }
            return result.Status;
        }
        public bool SaveDiagnoseDetailEntity(Colp_Diagnose_DetailEntity saveColp_Diagnose_DetailEntity)
        {
            if (saveColp_Diagnose_DetailEntity != null)
            {
                BaseResult result = Colp_Diagnose_DetailUtilities.Update(this.UserInfo, "Colp", saveColp_Diagnose_DetailEntity);

                if (result.Status == true || result.StatusCode == Status.OKUpdate.ToString())
                {
                    if (BaseSystemInfo.ShowInformation)
                    {
                        // 成功，进行提示
                        ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ////DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "Diagnose_Detai保存成功。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    this.Changed = true;
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "Diagnose_Detai保存失败。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // 是否名称重复了，提高友善性
                    if (result.StatusCode == Status.ErrorNameExist.ToString())
                    {

                    }
                }
                return result.Status;
            }
            else
            {
                return false;
            }
       }

        public bool SaveOperationDetailEntity(Colp_Operation_DetailEntity saveColp_Operation_DetailEntity)
        {
            if (saveColp_Operation_DetailEntity != null)
            {
                BaseResult result = Colp_Operation_DetailUtilities.Update(this.UserInfo, "Colp", saveColp_Operation_DetailEntity);

                if (result.Status == true || result.StatusCode == Status.OKUpdate.ToString())
                {
                    if (BaseSystemInfo.ShowInformation)
                    {
                        // 成功，进行提示
                        ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ////DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "Diagnose_Detai保存成功。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    this.Changed = true;
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "Operation_Detai保存失败。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // 是否名称重复了，提高友善性
                    if (result.StatusCode == Status.ErrorNameExist.ToString())
                    {

                    }
                }
                return result.Status;
            }
            else
            {
                return false;
            }
        }
        public bool SaveBiopsyDetailEntity(Colp_Biopsy_DetailEntity saveColp_Biopsy_DetailEntity)
        {
            if (saveColp_Biopsy_DetailEntity != null)
            {
                BaseResult result = Colp_Biopsy_DetailUtilities.Update(this.UserInfo, "Colp", saveColp_Biopsy_DetailEntity);

                if (result.Status == true || result.StatusCode == Status.OKUpdate.ToString())
                {
                    if (BaseSystemInfo.ShowInformation)
                    {
                        // 成功，进行提示
                        ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ////DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "Biopsy_Detai保存成功。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    this.Changed = true;
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "Biopsy_Detai保存失败。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // 是否名称重复了，提高友善性
                    if (result.StatusCode == Status.ErrorNameExist.ToString())
                    {

                    }
                }
                return result.Status;
            }
            else
            {
                return false;
            }
        }

        public bool SaveRCIDetailEntity(Colp_Evaluate_RCI_DetailEntity saveColp_Evaluate_RCI_DetailEntity)
        {
            if (saveColp_Evaluate_RCI_DetailEntity != null)
            {
                BaseResult result = Colp_Evaluate_RCI_DetailUtilities.Update(this.UserInfo, "Colp", saveColp_Evaluate_RCI_DetailEntity);

                if (result.Status == true || result.StatusCode == Status.OKUpdate.ToString())
                {
                    if (BaseSystemInfo.ShowInformation)
                    {
                        // 成功，进行提示
                        ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ////DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "Biopsy_Detai保存成功。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    this.Changed = true;
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "Biopsy_Detai保存失败。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // 是否名称重复了，提高友善性
                    if (result.StatusCode == Status.ErrorNameExist.ToString())
                    {

                    }
                }
                return result.Status;
            }
            else
            {
                return false;
            }
        }

        public bool SavePictureAnnotateDetailEntity(Colp_Picture_Annotate_DetailEntity saveColp_Picture_Annotate_DetailEntity)
        {
            BaseResult result = Colp_Picture_Annotate_DetailUtilities.Update(this.UserInfo, "Colp", saveColp_Picture_Annotate_DetailEntity);

            if (result.Status == true || result.StatusCode == Status.OKUpdate.ToString())
            {
                if (BaseSystemInfo.ShowInformation)
                {
                    // 成功，进行提示
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "Diagnose_Detai保存成功。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Changed = true;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "Diagnose_Detai保存失败。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 是否名称重复了，提高友善性
                if (result.StatusCode == Status.ErrorNameExist.ToString())
                {

                }
            }
            return result.Status;
        }

        public bool SaveReportListEntity(Colp_Report_ListEntity saveColp_Report_ListEntity)
        {
            BaseResult result = Colp_Report_ListUtilities.Update(this.UserInfo, "Colp", saveColp_Report_ListEntity);

            if (result.Status == true || result.StatusCode == Status.OKUpdate.ToString())
            {
                if (BaseSystemInfo.ShowInformation)
                {
                    // 成功，进行提示
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "Diagnose_Detai保存成功。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Changed = true;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "Report_List保存失败。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 是否名称重复了，提高友善性
                if (result.StatusCode == Status.ErrorNameExist.ToString())
                {

                }
            }
            return result.Status;
        }
        //添加已检查的图片 20181211
        public bool AddReportPictureEntity(Colp_Report_ListEntity addReportPictureEntity)
        {
            BaseResult result = Colp_Report_ListUtilities.Add(this.UserInfo, "Colp", addReportPictureEntity);

            if (result.StatusCode == Status.OKAdd.ToString())
            {
                if (BaseSystemInfo.ShowInformation)
                {
                    // 成功，进行提示
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "Diagnose_Detail添加成功。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Changed = true;
            }
            else
            {
                //DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "Diagnose_Detail添加失败。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 是否名称重复了，提高友善性
                if (result.StatusCode == Status.ErrorNameExist.ToString())
                {

                }
            }
            return result.Status;
        }

        public void Colp_Term_DetailEntityClear(Colp_Term_DetailEntity clearColp_Term_DetailEntity)
        {
            #region  UI数据对应被清空

            clearColp_Term_DetailEntity.Id = null;
            clearColp_Term_DetailEntity.Parent_Id = null;
            clearColp_Term_DetailEntity.Parent_Name = null;
            clearColp_Term_DetailEntity.Parent_Code = null;
            clearColp_Term_DetailEntity.Code = null;
            clearColp_Term_DetailEntity.Full_Name = null;
            clearColp_Term_DetailEntity.Name = null;
            clearColp_Term_DetailEntity.Category_Code = null;
            clearColp_Term_DetailEntity.Category = null;
            clearColp_Term_DetailEntity.State_Code = null;
            clearColp_Term_DetailEntity.State = null;
            clearColp_Term_DetailEntity.Sort_Code = null;
            clearColp_Term_DetailEntity.Allow_Edit = null;
            clearColp_Term_DetailEntity.Allow_Delete = null;
            clearColp_Term_DetailEntity.Is_Visible = null;
            clearColp_Term_DetailEntity.Is_Public = null;
            clearColp_Term_DetailEntity.Enabled = null;
            clearColp_Term_DetailEntity.Deletion_State_Code = null;
            clearColp_Term_DetailEntity.Contents = null;
            clearColp_Term_DetailEntity.Description = null;
            clearColp_Term_DetailEntity.Tag = null;
            clearColp_Term_DetailEntity.Software_Version = null;
            clearColp_Term_DetailEntity.Language_Version = null;
            clearColp_Term_DetailEntity.Term_Version = null;
            //clearColp_Term_DetailEntity.Create_On = null;
            //clearColp_Term_DetailEntity.Create_User_Id = null;
            //clearColp_Term_DetailEntity.Create_By = null;
            //clearColp_Term_DetailEntity.Modified_On = null;
            //clearColp_Term_DetailEntity.Modified_User_Id = null;
            //clearColp_Term_DetailEntity.Modified_By = null;
            clearColp_Term_DetailEntity.Tag_BindingSource = null;
            clearColp_Term_DetailEntity.Tag_DataRowView = null;
            clearColp_Term_DetailEntity.Tag_DataTable = null;
            clearColp_Term_DetailEntity.Tag_Object = null;
            #endregion

            clearColp_Term_DetailEntity = null;
        }  

        #region public bool AddTermDetailEntity(Colp_Term_DetailEntity addColpTermDetailEntity)
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>保存成功</returns>
        public BaseResult AddTermDetailEntity(Colp_Term_DetailEntity addColpTermDetailEntity)
        {
            BaseResult result = Colp_Term_DetailUtilities.Add(this.UserInfo, "Colp", addColpTermDetailEntity);

            if (result.StatusCode == Status.OKAdd.ToString())
            {
                if (BaseSystemInfo.ShowInformation)
                {
                    // 成功，进行提示
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Changed = true;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 是否名称重复了，提高友善性
                if (result.StatusCode == Status.ErrorNameExist.ToString())
                {

                }
            }
            return result;
        }
        #endregion

        #region public SaveTermDetailEntity(Colp_Term_DetailEntity saveColp_Term_DetailEntity)
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>保存成功</returns>
        public BaseResult SaveTermDetailEntity(Colp_Term_DetailEntity saveColp_Term_DetailEntity)
        {
            saveColp_Term_DetailEntity.Modified_On = DateTime.Now;

            JsonResult<bool> entityExits = null;

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(Colp_Term_DetailEntity.FieldId, saveColp_Term_DetailEntity.Id);
            entityExits = Colp_Term_DetailUtilities.Exists(this.UserInfo, "Colp", parameters);
            if (entityExits.Status && entityExits.Data == false)
            {
                //不存在则新建
                return AddTermDetailEntity(saveColp_Term_DetailEntity);
            }
            else
            {
                BaseResult updateBaseResult = Colp_Term_DetailUtilities.Update(this.UserInfo, "Colp", saveColp_Term_DetailEntity);

                if (updateBaseResult.Status == true || updateBaseResult.StatusCode == Status.OKUpdate.ToString())
                {

                    if (BaseSystemInfo.ShowInformation)
                    {
                        // 成功，进行提示
                        ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ////DevExpress.XtraEditors.XtraMessageBox.Show("保存成功" + updateBaseResult.StatusMessage, "提示信息：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    this.Changed = true;
                    return updateBaseResult;
                }
                else
                {
                    ////DevExpress.XtraEditors.XtraMessageBox.Show("保存失败" + updateBaseResult.StatusMessage, "提示信息：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // 是否名称重复了，提高友善性
                    if (updateBaseResult.StatusCode == Status.ErrorNameExist.ToString())
                    {

                    }
                    return updateBaseResult;
                }
            }
        }
        #endregion

        #region public DeleteTermDetailEntity(Colp_Term_DetailEntity deleteColp_Term_DetailEntity)
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns>成功</returns>
        public bool DeleteTermDetailEntity(Colp_Term_DetailEntity deleteColp_Term_DetailEntity)
        {
            String code = deleteColp_Term_DetailEntity.Code;
            String id = deleteColp_Term_DetailEntity.Id;

            BaseResult result = Colp_Term_DetailUtilities.SetDeleted(this.UserInfo, "Colp", new string[] { deleteColp_Term_DetailEntity.Id });

            if (result.Status == true || result.StatusCode == Status.OKDelete.ToString())
            {
                if (deleteColp_Term_DetailEntity.Tag_DataRowView != null)
                {
                    DataRowView tag_DataRowView = deleteColp_Term_DetailEntity.Tag_DataRowView;
                    deleteColp_Term_DetailEntity.Tag_DataRowView = null;   //现行清空
                    tag_DataRowView.Delete();   //从表格中删除掉对应行，会引起行index改变事件
                }
                deleteColp_Term_DetailEntity.Tag_BindingSource = null;
                if (deleteColp_Term_DetailEntity.Tag_DataRowView != null) deleteColp_Term_DetailEntity.Tag_DataRowView = null;
                deleteColp_Term_DetailEntity.Tag_DataTable = null;
                deleteColp_Term_DetailEntity.Tag_Object = null;

                if (BaseSystemInfo.ShowInformation)
                {
                    // 成功，进行提示
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage + ",编号：" + code, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Changed = true;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 是否名称重复了，提高友善性
                if (result.StatusCode == Status.ErrorNameExist.ToString())
                {

                }
            }
            return result.Status;
        }
        #endregion

        public void Colp_Configuration_ListEntityClear(Colp_Configuration_ListEntity clearColp_Configuration_ListEntity)
        {
            #region  UI数据对应被清空

            clearColp_Configuration_ListEntity.Id = null;
            clearColp_Configuration_ListEntity.Parent_Id = null;
            clearColp_Configuration_ListEntity.Parent_Name = null;
            clearColp_Configuration_ListEntity.Parent_Code = null;
            clearColp_Configuration_ListEntity.Code = null;
            clearColp_Configuration_ListEntity.Full_Name = null;
            clearColp_Configuration_ListEntity.Name = null;
            clearColp_Configuration_ListEntity.Category_Code = null;
            clearColp_Configuration_ListEntity.Category = null;
            clearColp_Configuration_ListEntity.State_Code = null;
            clearColp_Configuration_ListEntity.State = null;
            clearColp_Configuration_ListEntity.Sort_Code = null;
            clearColp_Configuration_ListEntity.Allow_Edit = null;
            clearColp_Configuration_ListEntity.Allow_Delete = null;
            clearColp_Configuration_ListEntity.Is_Visible = null;
            clearColp_Configuration_ListEntity.Is_Public = null;
            clearColp_Configuration_ListEntity.Enabled = null;
            clearColp_Configuration_ListEntity.Deletion_State_Code = null;
            clearColp_Configuration_ListEntity.Contents = null;
            clearColp_Configuration_ListEntity.Description = null;
            clearColp_Configuration_ListEntity.Tag = null;
            clearColp_Configuration_ListEntity.Software_Version = null;
            clearColp_Configuration_ListEntity.Language_Version = null;
            clearColp_Configuration_ListEntity.Term_Version = null;
            //clearColp_Configuration_ListEntity.Create_On = null;
            //clearColp_Configuration_ListEntity.Create_User_Id = null;
            //clearColp_Configuration_ListEntity.Create_By = null;
            //clearColp_Configuration_ListEntity.Modified_On = null;
            //clearColp_Configuration_ListEntity.Modified_User_Id = null;
            //clearColp_Configuration_ListEntity.Modified_By = null;
            clearColp_Configuration_ListEntity.Tag_BindingSource = null;
            clearColp_Configuration_ListEntity.Tag_DataRowView = null;
            clearColp_Configuration_ListEntity.Tag_DataTable = null;
            clearColp_Configuration_ListEntity.Tag_Object = null;
            #endregion

            clearColp_Configuration_ListEntity = null;
        }

        #region public bool AddConfigurationListEntity(Colp_Configuration_ListEntity addColpConfigurationListEntity)
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>保存成功</returns>
        public BaseResult AddConfigurationListEntity(Colp_Configuration_ListEntity addColpConfigurationListEntity)
        {
            BaseResult result = Colp_Configuration_ListUtilities.Add(this.UserInfo, "Colp", addColpConfigurationListEntity);

            if (result.StatusCode == Status.OKAdd.ToString())
            {
                if (BaseSystemInfo.ShowInformation)
                {
                    // 成功，进行提示
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Changed = true;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 是否名称重复了，提高友善性
                if (result.StatusCode == Status.ErrorNameExist.ToString())
                {

                }
            }
            return result;
        }
        #endregion

        #region public SaveConfigurationListEntity(Colp_Configuration_ListEntity saveColp_Configuration_ListEntity)
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>保存成功</returns>
        public BaseResult SaveConfigurationListEntity(Colp_Configuration_ListEntity saveColp_Configuration_ListEntity)
        {
            saveColp_Configuration_ListEntity.Modified_On = DateTime.Now;

            JsonResult<bool> entityExits = null;

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(Colp_Configuration_ListEntity.FieldId, saveColp_Configuration_ListEntity.Id);
            entityExits = Colp_Configuration_ListUtilities.Exists(this.UserInfo, "Colp", parameters);
            if (entityExits.Status && entityExits.Data == false)
            {
                //不存在则新建
                return AddConfigurationListEntity(saveColp_Configuration_ListEntity);
            }
            else
            {
                BaseResult updateBaseResult = Colp_Configuration_ListUtilities.Update(this.UserInfo, "Colp", saveColp_Configuration_ListEntity);

                if (updateBaseResult.Status == true || updateBaseResult.StatusCode == Status.OKUpdate.ToString())
                {

                    if (BaseSystemInfo.ShowInformation)
                    {
                        // 成功，进行提示
                        ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ////DevExpress.XtraEditors.XtraMessageBox.Show("保存成功" + updateBaseResult.StatusMessage, "提示信息：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    this.Changed = true;
                    return updateBaseResult;
                }
                else
                {
                    ////DevExpress.XtraEditors.XtraMessageBox.Show("保存失败" + updateBaseResult.StatusMessage, "提示信息：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // 是否名称重复了，提高友善性
                    if (updateBaseResult.StatusCode == Status.ErrorNameExist.ToString())
                    {

                    }
                    return updateBaseResult;
                }
            }
        }
        #endregion

        #region public DeleteConfigurationListEntity(Colp_Configuration_ListEntity deleteColp_Configuration_ListEntity)
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns>成功</returns>
        public bool DeleteConfigurationListEntity(Colp_Configuration_ListEntity deleteColp_Configuration_ListEntity)
        {
            String code = deleteColp_Configuration_ListEntity.Code;
            String id = deleteColp_Configuration_ListEntity.Id;

            BaseResult result = Colp_Configuration_ListUtilities.SetDeleted(this.UserInfo, "Colp", new string[] { deleteColp_Configuration_ListEntity.Id });

            if (result.Status == true || result.StatusCode == Status.OKDelete.ToString())
            {
                if (deleteColp_Configuration_ListEntity.Tag_DataRowView != null)
                {
                    DataRowView tag_DataRowView = deleteColp_Configuration_ListEntity.Tag_DataRowView;
                    deleteColp_Configuration_ListEntity.Tag_DataRowView = null;   //现行清空
                    tag_DataRowView.Delete();   //从表格中删除掉对应行，会引起行index改变事件
                }
                deleteColp_Configuration_ListEntity.Tag_BindingSource = null;
                if (deleteColp_Configuration_ListEntity.Tag_DataRowView != null) deleteColp_Configuration_ListEntity.Tag_DataRowView = null;
                deleteColp_Configuration_ListEntity.Tag_DataTable = null;
                deleteColp_Configuration_ListEntity.Tag_Object = null;

                if (BaseSystemInfo.ShowInformation)
                {
                    // 成功，进行提示
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage + ",编号：" + code, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Changed = true;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 是否名称重复了，提高友善性
                if (result.StatusCode == Status.ErrorNameExist.ToString())
                {

                }
            }
            return result.Status;
        }
        #endregion

        public DataTable GetColpEvaluateAIDataTable(String parentId)
        {
            DataTable returnDataTable = null;
            //先找到主窗体获取用户信息
            if (this.ParentForm is RibbonFormMain)
            {
                RibbonFormMain RibbonFormMain = this.ParentForm as RibbonFormMain;

                if (BaseSystemInfo.UserIsLogOn)
                {
                    //returnDataTable = ParameterUtilities.GetDataTable(RibbonFormMain.UserInfo,"Colp", "Colp_Patient_List");
                    //returnDataTable = Colp_Patient_ListUtilities.GetDataTable(RibbonFormMain.UserInfo, "Colp", "Colp_Patient_List");
                    returnDataTable = Colp_Evaluate_AIUtilities.GetDataTableByParent(RibbonFormMain.UserInfo, "Colp", parentId);
                }
            }
            return returnDataTable;
        }

        public void Colp_Evaluate_AIEntityClear(Colp_Evaluate_AIEntity clearColp_Evaluate_AIEntity)
        {
            #region  UI数据对应被清空

            clearColp_Evaluate_AIEntity.Id = null;
            clearColp_Evaluate_AIEntity.Parent_Id = null;
            clearColp_Evaluate_AIEntity.Parent_Name = null;
            clearColp_Evaluate_AIEntity.Parent_Code = null;
            clearColp_Evaluate_AIEntity.Code = null;
            clearColp_Evaluate_AIEntity.Full_Name = null;
            clearColp_Evaluate_AIEntity.Name = null;
            clearColp_Evaluate_AIEntity.Category_Code = null;
            clearColp_Evaluate_AIEntity.Category = null;
            clearColp_Evaluate_AIEntity.State_Code = null;
            clearColp_Evaluate_AIEntity.State = null;
            clearColp_Evaluate_AIEntity.Sort_Code = null;
            clearColp_Evaluate_AIEntity.Allow_Edit = null;
            clearColp_Evaluate_AIEntity.Allow_Delete = null;
            clearColp_Evaluate_AIEntity.Is_Visible = null;
            clearColp_Evaluate_AIEntity.Is_Public = null;
            clearColp_Evaluate_AIEntity.Enabled = null;
            clearColp_Evaluate_AIEntity.Deletion_State_Code = null;
            clearColp_Evaluate_AIEntity.Contents = null;
            clearColp_Evaluate_AIEntity.Description = null;
            clearColp_Evaluate_AIEntity.Tag = null;
            //clearColp_Evaluate_AIEntity.Create_On = null;
            //clearColp_Evaluate_AIEntity.Create_User_Id = null;
            //clearColp_Evaluate_AIEntity.Create_By = null;
            //clearColp_Evaluate_AIEntity.Modified_On = null;
            //clearColp_Evaluate_AIEntity.Modified_User_Id = null;
            //clearColp_Evaluate_AIEntity.Modified_By = null;
            clearColp_Evaluate_AIEntity.Tag_BindingSource = null;
            clearColp_Evaluate_AIEntity.Tag_DataRowView = null;
            clearColp_Evaluate_AIEntity.Tag_DataTable = null;
            clearColp_Evaluate_AIEntity.Tag_Object = null;
            #endregion

            clearColp_Evaluate_AIEntity = null;
        }

        #region public bool AddEvaluateAIEntity(Colp_Evaluate_AIEntity addColpEvaluateAIEntity)
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>保存成功</returns>
        public BaseResult AddEvaluateAIEntity(Colp_Evaluate_AIEntity addColpEvaluateAIEntity)
        {
            BaseResult result = Colp_Evaluate_AIUtilities.Add(this.UserInfo, "Colp", addColpEvaluateAIEntity);

            if (result.StatusCode == Status.OKAdd.ToString())
            {
                if (BaseSystemInfo.ShowInformation)
                {
                    // 成功，进行提示
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Changed = true;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 是否名称重复了，提高友善性
                if (result.StatusCode == Status.ErrorNameExist.ToString())
                {

                }
            }
            return result;
        }
        #endregion

        #region public SaveEvaluateAIEntity(Colp_Evaluate_AIEntity saveColp_Evaluate_AIEntity)
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>保存成功</returns>
        public BaseResult SaveEvaluateAIEntity(Colp_Evaluate_AIEntity saveColp_Evaluate_AIEntity)
        {
            saveColp_Evaluate_AIEntity.Modified_On = DateTime.Now;

            JsonResult<bool> entityExits = null;

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(Colp_Evaluate_AIEntity.FieldId, saveColp_Evaluate_AIEntity.Id);
            entityExits = Colp_Evaluate_AIUtilities.Exists(this.UserInfo, "Colp", parameters);
            if (entityExits.Status && entityExits.Data == false)
            {
                //不存在则新建
                return AddEvaluateAIEntity(saveColp_Evaluate_AIEntity);
            }
            else
            {
                BaseResult updateBaseResult = Colp_Evaluate_AIUtilities.Update(this.UserInfo, "Colp", saveColp_Evaluate_AIEntity);

                if (updateBaseResult.Status == true || updateBaseResult.StatusCode == Status.OKUpdate.ToString())
                {

                    if (BaseSystemInfo.ShowInformation)
                    {
                        // 成功，进行提示
                        ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ////DevExpress.XtraEditors.XtraMessageBox.Show("保存成功" + updateBaseResult.StatusMessage, "提示信息：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    this.Changed = true;
                    return updateBaseResult;
                }
                else
                {
                    ////DevExpress.XtraEditors.XtraMessageBox.Show("保存失败" + updateBaseResult.StatusMessage, "提示信息：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // 是否名称重复了，提高友善性
                    if (updateBaseResult.StatusCode == Status.ErrorNameExist.ToString())
                    {

                    }
                    return updateBaseResult;
                }
            }
        }
        #endregion

        #region public DeleteEvaluateAIEntity(Colp_Evaluate_AIEntity deleteColp_Evaluate_AIEntity)
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns>成功</returns>
        public bool DeleteEvaluateAIEntity(Colp_Evaluate_AIEntity deleteColp_Evaluate_AIEntity)
        {
            String code = deleteColp_Evaluate_AIEntity.Code;
            String id = deleteColp_Evaluate_AIEntity.Id;
            BaseResult result = Colp_Evaluate_AIUtilities.SetDeleted(this.UserInfo, "Colp", new string[] { deleteColp_Evaluate_AIEntity.Id });

            if (result.Status == true || result.StatusCode == Status.OKDelete.ToString())
            {
                if (deleteColp_Evaluate_AIEntity.Tag_DataRowView != null)
                {
                    DataRowView tag_DataRowView = deleteColp_Evaluate_AIEntity.Tag_DataRowView;
                    deleteColp_Evaluate_AIEntity.Tag_DataRowView = null;   //现行清空
                    tag_DataRowView.Delete();   //从表格中删除掉对应行，会引起行index改变事件
                }
                deleteColp_Evaluate_AIEntity.Tag_BindingSource = null;
                if (deleteColp_Evaluate_AIEntity.Tag_DataRowView != null) deleteColp_Evaluate_AIEntity.Tag_DataRowView = null;
                deleteColp_Evaluate_AIEntity.Tag_DataTable = null;
                deleteColp_Evaluate_AIEntity.Tag_Object = null;

                if (BaseSystemInfo.ShowInformation)
                {
                    // 成功，进行提示
                    ////DevExpress.XtraEditors.XtraMessageBox.Show(statusMessage, AppMessage.MSG0000, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage + ",编号：" + code, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Changed = true;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(result.StatusMessage, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 是否名称重复了，提高友善性
                if (result.StatusCode == Status.ErrorNameExist.ToString())
                {

                }
            }
            return result.Status;
        }
        #endregion
        public static List<String> Load_TermListByParentName(string tempTermParentName)
        {
            //例子 checkedBoxXxxxx_ButtonClick(checkedBoxXxxxx, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(checkedBoxXxxxx.Properties.Buttons[0]));
             
                List<String> listString = new List<string>();
                if (TermDetailDataTable != null)
                {
                    var qDataGridViewRows = from pDataRowView in TermDetailDataTable.AsEnumerable()
                                            where (pDataRowView[Colp_Term_DetailEntity.FieldParent_Name].ToString() == tempTermParentName)
                                            orderby Colp_Term_DetailEntity.FieldSort_Code ascending
                                            select pDataRowView;
                    if (qDataGridViewRows.Any())
                    {
                        foreach (var eachRow in qDataGridViewRows)
                        {
                            if (eachRow[Colp_Term_DetailEntity.FieldName] != null && 
                                !String.IsNullOrEmpty(Convert.ToString(eachRow[Colp_Term_DetailEntity.FieldName])))
                            {
                                ////String list = Convert.ToString(eachRow[Colp_Term_DetailEntity.FieldName]);
                                listString.Add(Convert.ToString(eachRow[Colp_Term_DetailEntity.FieldName]));
                                ////DevExpress.XtraEditors.XtraMessageBox.Show(list);
                            }
                        }
                    }
                }
                return listString;     
        }

        public static List<String> Load_TermListByParentCode(string tempTermParentCode)
        {
            //例子 checkedBoxXxxxx_ButtonClick(checkedBoxXxxxx, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(checkedBoxXxxxx.Properties.Buttons[0]));

            List<String> listString = new List<string>();
            if (TermDetailDataTable != null)
            {

                var qDataGridViewRows = from pDataRowView in TermDetailDataTable.AsEnumerable()
                                        where (pDataRowView[Colp_Term_DetailEntity.FieldParent_Code].ToString() == tempTermParentCode)
                                        orderby Colp_Term_DetailEntity.FieldSort_Code ascending
                                        select pDataRowView;
                if (qDataGridViewRows.Any())
                {
                    foreach (var eachRow in qDataGridViewRows)
                    {
                        if (eachRow[Colp_Term_DetailEntity.FieldName] != null &&
                            !String.IsNullOrEmpty(Convert.ToString(eachRow[Colp_Term_DetailEntity.FieldName])))
                        {
                            ////String list = Convert.ToString(eachRow[Colp_Term_DetailEntity.FieldName]);
                            listString.Add(Convert.ToString(eachRow[Colp_Term_DetailEntity.FieldName]));
                            ////DevExpress.XtraEditors.XtraMessageBox.Show(list);
                        }
                    }
                }
            }
            return listString;
        }

        public static List<String> Load_UserListRealName()
        {
            List<String> listString = new List<string>();
            if (UserDetailDataTable != null)
            {

                var qDataGridViewRows = from pDataRowView in UserDetailDataTable.AsEnumerable()
                                       // where (pDataRowView[Colp_Term_DetailEntity.FieldParent_Name].ToString() == tempTermParentName)
                                        orderby BaseUserEntity.FieldCode ascending
                                        select pDataRowView;
                if (qDataGridViewRows.Any())
                {
                    foreach (var eachRow in qDataGridViewRows)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(eachRow[BaseUserEntity.FieldRealName])))
                        {
                            ////String list = Convert.ToString(eachRow[Colp_Term_DetailEntity.FieldName]);
                            listString.Add(Convert.ToString(eachRow[BaseUserEntity.FieldRealName]));
                            ////DevExpress.XtraEditors.XtraMessageBox.Show(list);
                        }
                    }
                }
            }
            return listString;
        }
        /// <summary>
        /// //加载术语条目内容
        /// </summary>
        /// <param name="tempTermParentName"></param>
        /// <param name="tempCheckedComboBoxEdit"></param>
        /// <returns></returns>
        public static List<String> CheckComboBoxEditLoad_TermList(string tempTermParentName, DevExpress.XtraEditors.CheckedComboBoxEdit tempCheckedComboBoxEdit)
        {
            List<String> listString = BaseColpXtraForm.Load_TermListByParentName(tempTermParentName);
            
            tempCheckedComboBoxEdit.Properties.Items.Clear();
      
            //tempCheckedComboBoxEdit.Properties.ShowButtons = false;
            //tempCheckedComboBoxEdit.Properties.ShowPopupCloseButton = false;
            foreach (var eachvar in listString)
            {
                if (!String.IsNullOrEmpty(eachvar))
                {
                    tempCheckedComboBoxEdit.Properties.Items.Add(eachvar);

                    //List<String> ChildlistString = BaseColpXtraForm.Load_TermListByParentName(eachvar);

                    //foreach (var eacheachvar in ChildlistString)
                    //{
                    //    if (!String.IsNullOrEmpty(eacheachvar))
                    //    {
                    //        tempCheckedComboBoxEdit.Properties.Items.Add(eacheachvar);
                    //    }
                    //}

                }
            }
            return listString;
            
        }

        /// //加载术语条项目 checkedListBoxControl
        /// </summary>
        /// <param name="tempTermParentName"></param>
        /// <param name="tempCheckedComboBoxEdit"></param>
        /// <returns></returns>
        public static List<String> checkedListBoxControlLoad_TermList(string tempTermParentName, DevExpress.XtraEditors.CheckedListBoxControl tempCheckedListBoxControl)
        {
            List<String> listString = BaseColpXtraForm.Load_TermListByParentName(tempTermParentName);

            tempCheckedListBoxControl.Items.Clear();
            //tempCheckedComboBoxEdit.Properties.ShowButtons = false;
            //tempCheckedComboBoxEdit.Properties.ShowPopupCloseButton = false;
            foreach (var eachvar in listString)
            {
                if (!String.IsNullOrEmpty(eachvar))
                {
                    tempCheckedListBoxControl.Items.Add(eachvar);
                }
            }
            return listString;

        }

        public static List<String> checkedListBoxControlLoad_TermListByCode(string tempTermParentCode, DevExpress.XtraEditors.CheckedListBoxControl tempCheckedListBoxControl)
        {
            List<String> listString = BaseColpXtraForm.Load_TermListByParentCode(tempTermParentCode);

            tempCheckedListBoxControl.Items.Clear();
            //tempCheckedComboBoxEdit.Properties.ShowButtons = false;
            //tempCheckedComboBoxEdit.Properties.ShowPopupCloseButton = false;
            foreach (var eachvar in listString)
            {
                if (!String.IsNullOrEmpty(eachvar))
                {
                    tempCheckedListBoxControl.Items.Add(eachvar);
                }
            }
            return listString;

        }
        /// <summary>
        /// //加载医生列表（即在用户表内查）
        /// </summary>
        /// <param name="tempComboBoxEdit"></param>
        /// <returns></returns>
        public static List<String> comboBoxEditLoad_UserList(DevExpress.XtraEditors.ComboBoxEdit tempComboBoxEdit)
        {
            List<String> listString = BaseColpXtraForm.Load_UserListRealName();

            tempComboBoxEdit.Properties.Items.Clear();

            foreach (var eachvar in listString)
            {

                if (!String.IsNullOrEmpty(eachvar))
                {
                    tempComboBoxEdit.Properties.Items.Add(eachvar);
                }
            }
            return listString;
        }

        /// <summary>
        /// //加载术语条目内容
        /// </summary>
        /// <param name="tempTermParentName"></param>
        /// <param name="tempComboBoxEdit"></param>
        /// <returns></returns>
        public static List<String> comboBoxEditLoad_TermList(string tempTermParentName, DevExpress.XtraEditors.ComboBoxEdit tempComboBoxEdit)
        {
            List<String> listString = BaseColpXtraForm.Load_TermListByParentName(tempTermParentName);

            tempComboBoxEdit.Properties.Items.Clear();
            
            foreach (var eachvar in listString)
            {

                if (!String.IsNullOrEmpty(eachvar))
                {
                    tempComboBoxEdit.Properties.Items.Add(eachvar);
                }
            }
            return listString;
        }
        public static Object GetConfiguration(string configurationCode)
        {
            Object configurationObject = null;

            if (ConfigurationDataTable != null)
            {
                var qDataGridViewRows = from pDataRowView in ConfigurationDataTable.AsEnumerable()
                                        where (pDataRowView[Colp_Configuration_ListEntity.FieldCode].ToString() == configurationCode)
                                        //orderby 
                                        select pDataRowView;
                if (qDataGridViewRows.Any())
                {
                    foreach (var eachRow in qDataGridViewRows)
                    {
                        configurationObject = eachRow[Colp_Configuration_ListEntity.FieldContents];
                        break;
                    }
                }
            }
            return configurationObject;
        }

        private void BaseColpXtraForm_Load(object sender, EventArgs e)
        {
            #region 开始新线程检查授权信息
            #endregion
        }

        // Public Declare Function sndPlaySound Lib "winmm.dll" Alias "sndPlaySoundA" (ByVal lpszSoundName As String, ByVal uflags As Long) As Long

        /// <summary>
        /// 判断本地的连接状态
        /// </summary>
        /// <returns></returns>
        private static bool LocalConnectionStatus()
        {
            System.Int32 dwFlag = new Int32();
            if (!InternetGetConnectedState(ref dwFlag, 0))
            {
                //////Console.WriteLine("LocalConnectionStatus--未连网!");
                return false;
            }
            else
            {
                if ((dwFlag & INTERNET_CONNECTION_MODEM) != 0)
                {
                    //////Console.WriteLine("LocalConnectionStatus--采用调制解调器上网。");
                    return true;
                }
                else if ((dwFlag & INTERNET_CONNECTION_LAN) != 0)
                {
                    //////Console.WriteLine("LocalConnectionStatus--采用网卡上网。");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Ping命令检测网络是否畅通
        /// </summary>
        /// <param name="urls">URL数据</param>
        /// <param name="errorCount">ping时连接失败个数</param>
        /// <returns></returns>
        public static bool MyPing(string[] urls, out int errorCount)
        {
            bool isconn = true;
            Ping ping = new Ping();
            errorCount = 0;
            try
            {
                PingReply pr;
                for (int i = 0; i < urls.Length; i++)
                {
                    pr = ping.Send(urls[i]);
                    if (pr.Status != IPStatus.Success)
                    {
                        isconn = false;
                        errorCount++;
                    }
                }
            }
            catch
            {
                isconn = false;
                errorCount = urls.Length;
            }
            //if (errorCount > 0 && errorCount < 3)
            //  isconn = true;
            return isconn;
        }

        //获取各表编号流水号
        public string CurFlowCode(DataTable DataTablename, String Profix = "", String KeyField = "Code", int FlowNumbLen = 3, String FilterStr = "")
        {

            string FlowCode = null;
            string conds;
            string tmpStr;
            string DateStr;
            string maxCode;
            DateTime dtDay;
            dtDay = DateTime.Now.ToDateTime();
            int L;
            L = 1;
            for (int d = 1; d <= FlowNumbLen; d++)
            {
                L = L * 10;
            }
            DateStr = dtDay.ToString("yyyyMMdd");

            DataTable TodayListDataTable = DataTablename; // GetColpCheckListTodayListDataTable();

            if (TodayListDataTable == null ||
                TodayListDataTable.Rows.Count == 0)
            {
                FlowCode = DateStr + Convert.ToString((L + 1)).ToString().Substring(Convert.ToString(L + 1).Length - FlowNumbLen, FlowNumbLen);
            }
            else
            {
                //int maxKeyIndex = TodayListDataTable.AsEnumerable().Select(t => t.Field<int>(Convert.ToInt32("Code"))).Max();
                //FlowCode =Convert.ToString(maxKeyIndex + 1);
                FlowCode = DateStr + Convert.ToString(L + TodayListDataTable.Rows.Count + 1).ToString().Substring(Convert.ToString(L + TodayListDataTable.Rows.Count + 1).Length - FlowNumbLen, FlowNumbLen);
            }
            return FlowCode;
        }
        public static Boolean IsDirtyColpCheckList(Colp_Check_ListEntity paraColp_Check_ListEntity)
        {
            if (paraColp_Check_ListEntity.Tag_DataRowView == null) return false;

            DataTable dtB = paraColp_Check_ListEntity.Tag_DataRowView.Row.Table.Clone();
            dtB.ImportRow(paraColp_Check_ListEntity.Tag_DataRowView.Row);

            Colp_Check_ListEntity Colp_Check_ListEntityB = new Colp_Check_ListEntity();
            Colp_Check_ListEntityB.GetFrom(dtB.Rows[0]);

            Boolean dif = false;

            ////PropertyInfo[] listPropertyInfo = paraColp_Check_ListEntity.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo info in paraColp_Check_ListEntity.GetType().GetProperties())
            {
                if (info.Name != Colp_Check_ListEntity.FieldCreate_On &&
                    info.Name != Colp_Check_ListEntity.FieldCreate_By &&
                    info.Name != Colp_Check_ListEntity.FieldCreate_User_Id &&
                    info.Name != Colp_Check_ListEntity.FieldModified_On &&
                    info.Name != Colp_Check_ListEntity.FieldModified_By &&
                    info.Name != Colp_Check_ListEntity.FieldTag &&
                    !info.Name.StartsWith("Tag"))
                {
                    var varA = info.GetValue(paraColp_Check_ListEntity, null);
                    var varB = info.GetValue(Colp_Check_ListEntityB, null);

                    if (varA == null)
                    {
                        if (varB == null)
                        {

                        }
                        else
                        {
                            if (String.IsNullOrEmpty(varB.ToString().Trim()))
                            {
                            }
                            else
                            {
                                dif = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (varB == null)
                        {
                            if (String.IsNullOrEmpty(varA.ToString().Trim()))
                            {

                            }
                            else
                            {
                                dif = true;
                                break;
                            }
                        }
                        else
                        {
                            if (varA.ToString().Trim() != varB.ToString().Trim())
                            {
                                dif = true;
                                break;
                            }
                        }
                    }
                }
            }
            return dif;
        }
        public static Boolean IsDirtyColpAppointmentList(Colp_Appointment_DetailEntity paraColp_Appointment_ListEntity)
        {
            if (paraColp_Appointment_ListEntity.Tag_DataRowView == null) return false;

            DataTable dtB = paraColp_Appointment_ListEntity.Tag_DataRowView.Row.Table.Clone();
            dtB.ImportRow(paraColp_Appointment_ListEntity.Tag_DataRowView.Row);

            Colp_Appointment_DetailEntity Colp_Appointment_DetailEntityB = new Colp_Appointment_DetailEntity();
            Colp_Appointment_DetailEntityB.GetFrom(dtB.Rows[0]);

            Boolean dif = false;

            ////PropertyInfo[] listPropertyInfo = paraColp_Appointment_ListEntity.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo info in paraColp_Appointment_ListEntity.GetType().GetProperties())
            {
                if (info.Name != Colp_Appointment_DetailEntity.FieldCreate_On &&
                    info.Name != Colp_Appointment_DetailEntity.FieldCreate_By &&
                    info.Name != Colp_Appointment_DetailEntity.FieldCreate_User_Id &&
                    info.Name != Colp_Appointment_DetailEntity.FieldModified_On &&
                    info.Name != Colp_Appointment_DetailEntity.FieldModified_By &&
                    info.Name != Colp_Appointment_DetailEntity.FieldTag &&
                    !info.Name.StartsWith("Tag")
                    )
                {
                    var varA = info.GetValue(paraColp_Appointment_ListEntity, null);
                    var varB = info.GetValue(Colp_Appointment_DetailEntityB, null);

                    if (varA == null)
                    {
                        if (varB == null)
                        {

                        }
                        else
                        {
                            if (String.IsNullOrEmpty(varB.ToString().Trim()))
                            {
                            }
                            else
                            {
                                dif = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (varB == null)
                        {
                            if (String.IsNullOrEmpty(varA.ToString().Trim()))
                            {

                            }
                            else
                            {
                                dif = true;
                                break;
                            }
                        }
                        else
                        {
                            if (varA.ToString().Trim() != varB.ToString().Trim())
                            {
                                dif = true;
                                break;
                            }
                        }
                    }
                }
            }
            return dif;
        }
    }
}
