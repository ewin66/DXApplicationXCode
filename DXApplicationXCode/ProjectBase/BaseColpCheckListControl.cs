using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotNet.Business;
using DotNet.Model;

namespace DotNet.WinForm.Controls
{
    public partial class BaseColpCheckListControl :  BaseUserControlExtended
    {
        public static DataTable ColpCheckListDataTable = new DataTable(Colp_Check_ListEntity.TableName);  // 表名

        /// <summary>
        /// 编辑公开的权限
        /// </summary>
        public static bool permissionEditPublic = false;

        /// <summary>
        /// 删除公开的权限
        /// </summary>
        public static bool permissionDeletePublic = false;

        /// <summary>
        /// 导出公开的权限
        /// </summary>
        public static bool permissionExportPublic = false;

        public static string currentEntityId = string.Empty;
        /// <summary>
        /// 当前选中的记录主键
        /// </summary>
        public static string CurrentEntityId
        {
            get
            {
                return currentEntityId;
            }
            set
            {
                currentEntityId = value;
            }
        }
        public BaseColpCheckListControl()
        {
            InitializeComponent();
        }
    }
}
