using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System.Data;
using DotNet.Model;
using DotNet.Business;
using DotNet.Utilities;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DotNet.WinForm
{
    public class BaseColpTermClass
    {
        static private ReaderWriterLockSlim termTableReaderWriterLockSlim = new ReaderWriterLockSlim();//允许多个线程同时获取读锁，但同一时间只允许一个线程获得写锁，因此也称作共享-独占锁
        static DataTable _allTermDataTable = null;

        public  string strTermSplit = ",";
        public  string userTermString = "";
        Dictionary<int, string> userStringTermNameDictionary = new Dictionary<int, string>();

        public static DataTable AllTermDataTable
        {
            get
            {
                if (_allTermDataTable == null)
                {
                    termTableReaderWriterLockSlim.EnterWriteLock();
                    //huoquzhengzhangbiao
                    int recordCount = 10000;
                    string whereClause = Colp_Term_DetailEntity.TableName + "." + Colp_Term_DetailEntity.FieldEnabled + " <>0 ";
                    whereClause += " AND " + Colp_Term_DetailEntity.TableName + "." + Colp_Check_ListEntity.FieldDeletion_State_Code + "=0";
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    _allTermDataTable = Colp_Term_DetailUtilities.GetDataTableByPage(BaseSystemInfo.UserInfo, "*", out recordCount, 1, 10000, whereClause, parameters,Colp_Term_DetailEntity.FieldSort_Code);
                    termTableReaderWriterLockSlim.ExitWriteLock();
                }
                return _allTermDataTable;
            }
        }

        public static void ClearTermDataTable()
        {
            _allTermDataTable = null;
        }

        public static DataTable GetColpChildTermDataTable(string strTermChildCode)
        {
            DataTable returnDataTable = null;
            //先找到主窗体获取用户信息
            //if (this.ParentForm is RibbonFormMain)
            //{
            //RibbonFormMain RibbonFormMain = this.ParentForm as RibbonFormMain;

            if (Utilities.BaseSystemInfo.UserIsLogOn)
            {
                int recordCount = 0;
                string orderby = "  Order by";

                //string whereClause = Colp_Term_DetailEntity.TableName + "." + Colp_Term_DetailEntity.FieldParent_Code + "='" + strTermChildCode.ToString().Trim() + "' " + orderby + "  " + Colp_Term_DetailEntity.TableName + "." + Colp_Term_DetailEntity.FieldCode + "";
                string whereClause = Colp_Term_DetailEntity.TableName + "." + Colp_Term_DetailEntity.FieldParent_Code + "='" + strTermChildCode.ToString().Trim() + "'";

                Dictionary<string, object> parameters = new Dictionary<string, object>();

                //根据ID判断记录是否存在
                //returnDataTable = ParameterUtilities.GetDataTable(RibbonFormMain.UserInfo, "Colp", "Colp_Patient_List");
                //returnDataTable = Colp_Patient_ListUtilities.GetDataTable(RibbonFormMain.UserInfo, "Colp", "Colp_Patient_List");
                //returnDataTable = Colp_Term_DetailUtilities.GetDataTable(RibbonFormMain.UserInfo, "Colp", parameters);
                returnDataTable = Colp_Term_DetailUtilities.GetDataTableByPage(BaseSystemInfo.UserInfo, "*", out recordCount, 1, 1000, whereClause, parameters, Colp_Term_DetailEntity.FieldCode);
            }
            //}
            return returnDataTable;
        }
        //使用递归获取TreeList中所有勾选的结点值

        public void LoadTermTree(TreeList TermTree, string strRootNodeName, string strParentCode, string strTermlist)
        {
            string strTermName = null;
            strTermlist = strTermlist.Replace(strTermSplit + strTermSplit, strTermSplit);
            strTermlist = strTermlist.Replace(strTermSplit + strTermSplit, strTermSplit);
            strTermlist = strTermlist.Replace(strTermSplit + strTermSplit, strTermSplit);
            TermTree.ClearNodes();
            userStringTermNameDictionary.Clear();

            TermTree.OptionsView.RootCheckBoxStyle = NodeCheckBoxStyle.Check;
            var query = from pDataRow in AllTermDataTable.AsEnumerable()
                        where (!Convert.IsDBNull(pDataRow[Colp_Term_DetailEntity.FieldParent_Code]) &&
                                Convert.ToString(pDataRow[Colp_Term_DetailEntity.FieldParent_Code]).Equals(strParentCode))
                        select pDataRow;
            if (query.Any())
            {
                DataTable dt = query.CopyToDataTable<DataRow>();
                if (dt.Rows.Count > 0)
                {
                    TreeListNode nodeRoot = null;
                    //   TreeListNode node = TermTree.AppendNode("id", null);
                    //    node.SetValue(0, strRootNodeName);
                    //    node.ChildrenCheckBoxStyle = NodeCheckBoxStyle.Check;

                    dt.DefaultView.Sort = "Sort_Code asc";
                    DataTable dtsort;
                    dtsort = dt.DefaultView.ToTable();
                    for (int i = 0; i < dtsort.Rows.Count; i++)
                    {
                        TreeListNode nodeTemp = TermTree.AppendNode(dtsort.Rows[i]["Code"], nodeRoot);
                        nodeTemp.SetValue(TermTree.Columns[0], dtsort.Rows[i]["Name"]);
                        nodeTemp.Tag = dtsort.Rows[i]["Name"];
                        nodeTemp.ChildrenCheckBoxStyle = NodeCheckBoxStyle.Check;

                        //取文本值返回树对应术语名称“√”
                        List<string> strTermNamelist = Regex.Split(strTermlist, strTermSplit, RegexOptions.IgnoreCase).ToList();
                        Dictionary<int, string> strTermNameDictionary = new Dictionary<int, string>();
                        if (strTermNamelist.Count > 0)
                        {
                            if (!String.IsNullOrEmpty(strTermNamelist[0]))
                            {
                                if (userStringTermNameDictionary.IsNullOrEmpty())
                                {
                                    for (int j = 0; j < strTermNamelist.Count; j++)
                                    {
                                        strTermName = strTermNamelist[j].Trim();
                                        userStringTermNameDictionary.Add(j, strTermName);
                                    }
                                }
                                try
                                {
                                    for (int j = 0; j < strTermNamelist.Count; j++)
                                    {
                                        strTermName = strTermNamelist[j].Trim();
                                        strTermNameDictionary.Add(j, strTermName);
                                        if (nodeTemp.Tag.ToString().Trim() == strTermName)
                                        {
                                            nodeTemp.Checked = true;
                                            var keys = userStringTermNameDictionary.Where(q => q.Value == strTermName).Select(q => q.Key).ToList();  //get all keys
                                            foreach (int key in keys)
                                            {
                                                userStringTermNameDictionary[key] = "";
                                            }
                                            strTermNameDictionary[j] = "";
                                            strTermlist = strTermlist.Replace(strTermName + strTermSplit, "");
                                        }
                                    }
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                        strTermlist = String.Empty;
                        foreach (var v in strTermNameDictionary)
                        {
                            if (!String.IsNullOrEmpty(v.Value))
                                strTermlist += v.Value + strTermSplit;
                        }
                        GetChildNode(TermTree, nodeTemp, dtsort.Rows[i]["Code"].ToString(), strTermlist);
                    }
                }
                //TermTree.ExpandAll();
                //TermTree.Tag = strTermlist;
            }
            strTermlist = String.Empty;
            foreach (var v in userStringTermNameDictionary)
            {
                if (!String.IsNullOrEmpty(v.Value))
                    strTermlist += v.Value + strTermSplit;
            }
            userTermString = strTermlist;
        }
        public void GetChildNode(TreeList TermTree, TreeListNode nodeTemp, string parentCode, string strTermlist)
        {
            string strTermName = null;
            var query = from pDataRow in AllTermDataTable.AsEnumerable()
                        where (!Convert.IsDBNull(pDataRow[Colp_Term_DetailEntity.FieldParent_Code]) &&
                                Convert.ToString(pDataRow[Colp_Term_DetailEntity.FieldParent_Code]).Equals(parentCode))
                        select pDataRow;
            if (query.Any())
            {
                DataTable dt2 = query.CopyToDataTable<DataRow>();
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        TreeListNode nodelist = nodeTemp.Nodes.Add(dt2.Rows[i]["Code"]);
                        nodelist.SetValue(TermTree.Columns[0], dt2.Rows[i]["Name"]);
                        nodelist.Tag = dt2.Rows[i]["Name"];
                        GetChildNode(TermTree, nodelist, dt2.Rows[i]["Code"].ToString(), strTermlist);
                        //if (nodelist.HasChildren)
                        //{
                        //    nodelist.ChildrenCheckBoxStyle = NodeCheckBoxStyle.Check;
                        //}
                        List<string> strTermNamelist = Regex.Split(strTermlist, strTermSplit, RegexOptions.IgnoreCase).ToList();
                        Dictionary<int, string> strTermNameDictionary = new Dictionary<int, string>();
                        if (strTermNamelist.Count > 0)
                        {
                            if (!String.IsNullOrEmpty(strTermNamelist[0]))
                            {
                                try
                                {
                                    for (int j = 0; j < strTermNamelist.Count; j++)
                                    {
                                        strTermName = strTermNamelist[j].Trim();
                                        strTermNameDictionary.Add(j, strTermName);
                                        if (nodelist.Tag.ToString().Trim() == strTermName)
                                        {
                                            nodelist.Checked = true;
                                            var keys = userStringTermNameDictionary.Where(q => q.Value == strTermName).Select(q => q.Key).ToList();  //get all keys
                                            foreach (int key in keys)
                                            {
                                                userStringTermNameDictionary[key] = "";
                                            }
                                            strTermNameDictionary[j] = "";
                                            strTermlist = strTermlist.Replace(strTermName + strTermSplit, "");
                                        }
                                    }
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                        strTermlist = String.Empty;
                        foreach (var v in strTermNameDictionary)
                        {
                            if (!String.IsNullOrEmpty(v.Value))
                                strTermlist += v.Value + strTermSplit;
                        }
                    }
                    if (nodeTemp.HasChildren)
                    {
                        if (nodeTemp.ParentNode != null)
                        {
                            nodeTemp.ParentNode.ChildrenCheckBoxStyle = NodeCheckBoxStyle.None;
                            nodeTemp.ChildrenCheckBoxStyle = NodeCheckBoxStyle.Check;
                            ////HideCheckBox(TermTree, nodeTemp);
                        }
                        else
                        {
                            TermTree.OptionsView.RootCheckBoxStyle = NodeCheckBoxStyle.Check;
                        }
                    }
                    else
                    {
                        if (nodeTemp.ParentNode != null)
                        {
                            nodeTemp.ParentNode.ChildrenCheckBoxStyle = NodeCheckBoxStyle.None;
                        }
                        else
                        {
                            TermTree.OptionsView.RootCheckBoxStyle = NodeCheckBoxStyle.Check;
                        }
                    }
                }
            }
        }
        public void GetSecondChildNode(TreeList TermTree, TreeListNode node, string parent)
        {
            DataTable dt2 = GetColpChildTermDataTable(parent);
            //DataTable dt = "从数据库中查询where id=" + parent;
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    TreeListNode nodelist = node.Nodes.Add(dt2.Rows[i]["Code"]);
                    nodelist.SetValue(TermTree.Columns[0], dt2.Rows[i]["Name"]);
                    nodelist.Tag = dt2.Rows[i];
                }
                if (node.HasChildren)
                {
                    node.ChildrenCheckBoxStyle = NodeCheckBoxStyle.Check;
                }
            }            
        }
        public IList GetCheckNodes(TreeList tree)
        {
            //用于存储勾选的结点
            IList list = new ArrayList();
            //首长循环获取已勾选的根结点
            foreach (TreeListNode n in tree.Nodes)
            {                 
                if (n.Checked) list.Add(new MyNodeItem(n, "")); //Checked表示勾选
                //如果当前结点有子结点集合，再枚举所有子结点
                if (n.Nodes.Count > 0)
                    this.DoGetCheckNodes(list, n);
            }
            return list;
        }
        /// <summary>
        /// 剃归，扫描所有结点。
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentNode"></param>
        private void DoGetCheckNodes(IList list, TreeListNode parentNode)
        {
            //枚举当前结点的所有子结点
            foreach (TreeListNode n in parentNode.Nodes)
            {
                if (n.Checked) list.Add(new MyNodeItem(n, ""));
                //如果当前结点有子结点集合，再枚举所有子结点
                if (n.Nodes.Count > 0)
                   this.DoGetCheckNodes(list, n);
            }
        }
        public class MyNodeItem
        {
            TreeListNode _n;
            string _tag;
            public MyNodeItem(TreeListNode n, string tag)
            {
                _n = n;
                _tag = tag;
            }
            public override string ToString()
            {
                //return _n.GetValue(0).ToString() + "(" + _tag + ")";
                return _n.GetValue(0).ToString();
            }
        }

        //public  TreeListNode GetParentNode(this TreeListNode node, Predicate<TreeListNode> conditionHanlder)
        //{
        //    TreeListNode _parentNode = node.ParentNode;//获取上一级父节点
        //    TreeListNode _conditonNode = null;
        //    if (_parentNode != null)
        //    {
        //        if (conditionHanlder(_parentNode))//判断上一级父节点是否符合要求
        //        {
        //            _conditonNode = _parentNode;
        //        }
        //        if (_conditonNode == null)//若没有找到符合要求的节点，递归继续
        //            _conditonNode = GetParentNode(_parentNode, conditionHanlder);
        //    }
        //    return _conditonNode;
        //}
        //private void tvCheck_DrawNode(object sender, DrawTreeNodeEventArgs e)
        //{

        //    //if (e.Node.Text == "aaaaaa")   //需要隐藏checkbox的节点名称
        //    //{
        //    //    HideCheckBox(this.tvCheck, e.Node);
        //    //}

        //    //e.DrawDefault = true;
        //}

        //private const int TVIF_STATE = 0x8;
        //private const int TVIS_STATEIMAGEMASK = 0xF000;
        //private const int TV_FIRST = 0x1100;
        //private const int TVM_SETITEM = TV_FIRST + 63;
        ////private void HideCheckBox(TreeList tvw, TreeListNode node)
        ////{
        ////    TvItem tvi = new TvItem();
        ////    tvi.hItem = node.Handle;
        ////    tvi.mask = TVIF_STATE;
        ////    tvi.stateMask = TVIS_STATEIMAGEMASK;
        ////    tvi.state = 0;
        ////    SendMessage(tvw.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);
        ////}

        //[StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        //private struct TvItem
        //{
        //    public int mask;
        //    public IntPtr hItem;
        //    public int state;
        //    public int stateMask;
        //    [MarshalAs(UnmanagedType.LPTStr)]
        //    public string lpszText;
        //    public int cchTextMax;
        //    public int iImage;
        //    public int iSelectedImage; public int cChildren; public IntPtr lParam;
        //}

        ////[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref TvItem lParam);

    }


}
