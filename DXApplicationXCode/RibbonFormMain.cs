﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NewLife.Log;
using XCode.Membership;
using System.Diagnostics;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Threading;
using static DXApplicationXCode.AsyncDemoClass;

namespace DXApplicationXCode
{
    public partial class RibbonFormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public RibbonFormMain()
        {
            InitializeComponent();
        }

        private void RibbonFormMain_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            XTrace.UseConsole();
        }

        void navBarControl_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
            if (e.Group != null)
            {
                navigationFrame.SelectedPageIndex = navBarControl.Groups.IndexOf(e.Group);
            }
            else
            {

            }
        }
        void barButtonNavigation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int barItemIndex = barSubItemNavigation.ItemLinks.IndexOf(e.Link);
            navBarControl.ActiveGroup = navBarControl.Groups[barItemIndex];
        }

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (this.IsHandleCreated)
            {
                System.Diagnostics.Trace.WriteLine("navBarItem1_LinkClicked");
                System.Diagnostics.Debug.WriteLine("navBarItem1_LinkClicked");

                // 添加
                var newUser = new UserX
                {
                    Name = System.Guid.NewGuid().ToString(),
                    Enable = true,
                };
                newUser.Insert();
                // 自增字段user.ID已经取得值
                XTrace.WriteLine("用户ID：{0}", newUser.ID);
                if (bindingSourceMain.DataSource != null)
                {
                    bindingSourceMain.Add(newUser);
                    ////MessageBox.Show(bindingSourceMain.DataSource.GetType().ToString());
                    if ((bindingSourceMain.DataSource as BindingSource).DataSource is IList<UserX>)
                    {
                        IList<UserX> listUser = (bindingSourceMain.DataSource as BindingSource).DataSource as List<UserX>;
                    }
                }
            }
        }

        private void navBarItem2_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (this.IsHandleCreated)
            {
                System.Diagnostics.Trace.WriteLine("navBarItem2_LinkClicked");
                System.Diagnostics.Debug.WriteLine("navBarItem2_LinkClicked");
                IList<UserX> listUser = UserX.FindAll();
                if (listUser.Count > 0)
                {
                    if (bindingSourceMain.DataSource == null)
                    {
                        bindingSourceMain.DataSource = new BindingSource(listUser, null);
                    }
                    else
                    {
                        bindingSourceMain.Clear();
                        foreach (var eachvar in listUser)
                        {
                            bindingSourceMain.Add(eachvar);
                        }
                    }

                }
            }
        }

        private void bindingSourceMain_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            // Check if the data source has been updated, and that no error has occured.
            if (e.BindingCompleteContext == BindingCompleteContext.DataSourceUpdate
               && e.Exception == null)
            {
                // If not, end the current edit.
                e.Binding.BindingManagerBase.EndCurrentEdit();
            }
        }

        private void bindingSourceMain_DataSourceChanged(object sender, EventArgs e)
        {
            if (bindingSourceMain.DataSource != null)
            {
                this.gridControlMain.DataSource = bindingSourceMain;
                this.dataNavigatorMain.DataSource = bindingSourceMain;

                this.textEditName.DataBindings.Clear();
                this.textEditName.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSourceMain, UserX.__.Name, true));

                this.textEditPhone.DataBindings.Clear();
                this.textEditPhone.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSourceMain, UserX.__.Mobile, true));
            }
        }

        private void simpleButtonSave_Click(object sender, EventArgs e)
        {
            if (bindingSourceMain.Current != null)
            {
                if (bindingSourceMain.Current is UserX)
                {
                    bindingSourceMain.EndEdit();
                    bindingSourceMain.ResetBindings(false);

                    UserX currentUser = bindingSourceMain.Current as UserX;
                    currentUser.Update();
                }
            }
        }

        private void bindingSourceMain_ListChanged(object sender, ListChangedEventArgs e)
        {
            if(DXApplicationXCode.Properties.Settings.Default.ShowDebugInfo > 50) XTrace.WriteLine(String.Format(" Method: {0}", new StackTrace(new StackFrame(true)).GetFrame(0).GetMethod().Name));
        }

        private void bindingSourceMain_PositionChanged(object sender, EventArgs e)
        {
            if (DXApplicationXCode.Properties.Settings.Default.ShowDebugInfo > 50) XTrace.WriteLine(String.Format(" Method: {0}", new StackTrace(new StackFrame(true)).GetFrame(0).GetMethod().Name));
        }

        private void bindingSourceMain_DataMemberChanged(object sender, EventArgs e)
        {
            if (DXApplicationXCode.Properties.Settings.Default.ShowDebugInfo > 50) XTrace.WriteLine(String.Format(" Method: {0}", new StackTrace(new StackFrame(true)).GetFrame(0).GetMethod().Name));
        }

        private void bindingSourceMain_DataError(object sender, BindingManagerDataErrorEventArgs e)
        {
            if (DXApplicationXCode.Properties.Settings.Default.ShowDebugInfo > 50) XTrace.WriteLine(String.Format(" Method: {0}", new StackTrace(new StackFrame(true)).GetFrame(0).GetMethod().Name));
        }

        private void bindingSourceMain_CurrentChanged(object sender, EventArgs e)
        {
            if (DXApplicationXCode.Properties.Settings.Default.ShowDebugInfo > 50) XTrace.WriteLine(String.Format(" Method: {0}", new StackTrace(new StackFrame(true)).GetFrame(0).GetMethod().Name));
        }

        private void bindingSourceMain_CurrentItemChanged(object sender, EventArgs e)
        {
            if (DXApplicationXCode.Properties.Settings.Default.ShowDebugInfo > 50) XTrace.WriteLine(String.Format(" Method: {0}", new StackTrace(new StackFrame(true)).GetFrame(0).GetMethod().Name));
        }

        private void navBarItemDelete_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            int[] selectedRows = gridViewUser.GetSelectedRows();
            if (selectedRows.Length > 0)
            {
                List<UserX> listUser = new List<UserX>();
                foreach (var eachvar in selectedRows)
                {
                    listUser.Add(this.gridViewUser.GetRow(eachvar) as UserX);
                }
                for (int index = 0; index < listUser.Count; index++)
                {
                    UserX deleteuser = listUser[index];
                    deleteuser.Delete();
                }
            }
            else
            {
                ////Object currentDataRow = this.gridViewUser.GetDataRow(this.gridViewUser.FocusedRowHandle);
            }
        }

        private void navBarItemGridViewUser_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            this.gridControlMain.MainView = this.gridViewUser;
            this.gridControlMain.FocusedView = this.gridViewUser;
        }

        private void navBarItemLayoutViewUser_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            this.gridControlMain.MainView = this.layoutViewUser;
            this.gridControlMain.FocusedView = this.layoutViewUser;
        }

        private void navBarItemTileViewUser_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            this.gridControlMain.MainView = this.tileViewUser;
            this.gridControlMain.FocusedView = this.tileViewUser;
        }

        private void navBarItemCardViewUser_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            this.gridControlMain.MainView = this.cardViewUser;
            this.gridControlMain.FocusedView = this.cardViewUser;
        }

        private void gridViewUser_DoubleClick(object sender, EventArgs e)
        {
            Point pt = gridViewUser.GridControl.PointToClient(Control.MousePosition);
            GridHitInfo gridHitInfo = gridViewUser.CalcHitInfo(pt);
            if (gridHitInfo.InRow || gridHitInfo.InRowCell)
            {
                ////int selectRow = gridViewUser.GetSelectedRows()[0];
                int selectRow = gridViewUser.FocusedRowHandle;
                UserX currentUser = this.gridViewUser.GetRow(selectRow) as UserX;


                BaseXtraForm newXtraFormUser = new BaseXtraForm();
                newXtraFormUser.ShowDialog();
                newXtraFormUser.Dispose();
            }
        }

        private void navBarItemAsyncMethod_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (this.IsHandleCreated)
            {
                System.Diagnostics.Trace.WriteLine("navBarItemAsyncMethod_LinkClicked");
                System.Diagnostics.Debug.WriteLine("navBarItemAsyncMethod_LinkClicked"); AsyncDemoClass newAsyncDemoClass = new AsyncDemoClass();
                newAsyncDemoClass.DoSomethingInAsyncTaskDemo += new DoSomethingInAsyncTaskDemoEventHandler(DoSomethingInAsyncTaskDemo);
                newAsyncDemoClass.ShowMessageInAsyncCallbackDemo += new ShowMessageInAsyncCallbackDemoEventHandler(ShowMessageInAsyncCallbackDemo);
                //newAsyncDemoClass.ShowMessageInAsyncDemo += new ShowMessageInAsyncDemoEventHandler(ShowMessageInAsyncDemo);
                //newAsyncDemoClass.ShowMessageInAsyncDemo += new ShowMessageInAsyncDemoEventHandler(ShowMessageInAsyncDemo);
                //newAsyncDemoClass.ShowMessageInAsyncDemo += new ShowMessageInAsyncDemoEventHandler(ShowMessageInAsyncDemo);
                //newAsyncDemoClass.ShowMessageInAsyncDemo += new ShowMessageInAsyncDemoEventHandler(ShowMessageInAsyncDemo);
                var varTask = newAsyncDemoClass.taskStartAsyncDelegate(3000);
                MessageBox.Show("varTask");
            }
        }

        public void DoSomethingInAsyncTaskDemo(object sender, object message)
        {
            if (this.IsHandleCreated)
            {
                System.Diagnostics.Trace.WriteLine("DoSomethingInAsyncTaskDemo");
                System.Diagnostics.Debug.WriteLine("DoSomethingInAsyncTaskDemo"); AsyncDemoClass newAsyncDemoClass = new AsyncDemoClass();
                new Thread(new ParameterizedThreadStart(delegate (object threadObject)
                {
                    String messageString = threadObject as String;
                    IAsyncResult iar = this.BeginInvoke(new MethodInvoker(delegate
                    {
                        MessageBox.Show(messageString);
                    }));
                    this.EndInvoke(iar);
                })).Start(message);
            }
        }
        public void ShowMessageInAsyncCallbackDemo(object sender, object message)
        {
            if (this.IsHandleCreated)
            {
                System.Diagnostics.Trace.WriteLine("ShowMessageInAsyncCallbackDemo");
                System.Diagnostics.Debug.WriteLine("ShowMessageInAsyncCallbackDemo"); AsyncDemoClass newAsyncDemoClass = new AsyncDemoClass();

                new Thread(new ParameterizedThreadStart(delegate (object threadObject)
                {
                    String messageString = threadObject as String;
                    IAsyncResult iar = this.BeginInvoke(new MethodInvoker(delegate
                    {
                        MessageBox.Show(messageString);
                    }));
                    this.EndInvoke(iar);
                })).Start(message);
            }
        }

        private void navBarItemDrawImge_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (this.IsHandleCreated)
            {
                System.Diagnostics.Trace.WriteLine("navBarItemDrawImge_LinkClicked");
                System.Diagnostics.Debug.WriteLine("navBarItemDrawImge_LinkClicked"); AsyncDemoClass newAsyncDemoClass = new AsyncDemoClass();
            }
        }

        private void navBarControl_SelectedLinkChanged(object sender, DevExpress.XtraNavBar.ViewInfo.NavBarSelectedLinkChangedEventArgs e)
        {
            if (this.IsHandleCreated)
            {
                System.Diagnostics.Trace.WriteLine("navBarControl_SelectedLinkChanged");
                System.Diagnostics.Debug.WriteLine("navBarControl_SelectedLinkChanged"); AsyncDemoClass newAsyncDemoClass = new AsyncDemoClass();
            }
        }

        private void navBarControl_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (this.IsHandleCreated)
            {
                System.Diagnostics.Trace.WriteLine("navBarControl_LinkClicked");
                System.Diagnostics.Debug.WriteLine("navBarControl_LinkClicked"); AsyncDemoClass newAsyncDemoClass = new AsyncDemoClass();
            }
        }

        private void navBarControl_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("navBarControl_LinkPressed");
            System.Diagnostics.Debug.WriteLine("navBarControl_LinkPressed"); AsyncDemoClass newAsyncDemoClass = new AsyncDemoClass();
        }

        private void navBarItemAll_ItemChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("navBarItemAll_ItemChanged");
            System.Diagnostics.Debug.WriteLine("navBarItemAll_ItemChanged"); AsyncDemoClass newAsyncDemoClass = new AsyncDemoClass();
        }

        private void navBarItemAll_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("navBarItemAll_LinkPressed");
            System.Diagnostics.Debug.WriteLine("navBarItemAll_LinkPressed"); AsyncDemoClass newAsyncDemoClass = new AsyncDemoClass();
        }

        private void repositoryItemButtonEditName_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            String methodname = new StackTrace(true).GetFrame(0).GetMethod().Name.ToString();
            Debug.WriteLine(methodname);
            MessageBox.Show(methodname);
        }

        private void repositoryItemButtonEditName_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            String methodname = new StackTrace(true).GetFrame(0).GetMethod().Name.ToString();
            Debug.WriteLine(methodname);
            MessageBox.Show(methodname);
        }

        private void repositoryItemButtonEditName_Click(object sender, EventArgs e)
        {
            String methodname = new StackTrace(true).GetFrame(0).GetMethod().Name.ToString();
            Debug.WriteLine(methodname);
            MessageBox.Show(methodname);

        }
    }
}