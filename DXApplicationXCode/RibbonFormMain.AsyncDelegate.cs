using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DXApplicationXCode
{
    public partial class RibbonFormMain
    {
        
    }
    
    public class AsyncDemoClass
    {      
        public delegate void BeginEventHandler(object sender, string message);
        public delegate void ProgressChangedEventHandler(object sender, string message);
        public delegate void CancelEventHandler(object sender, string message);
        public delegate void CompletedEventHandler(object sender, string message);
        public delegate void ShowMessageInAsyncDemoEventHandler(object sender, string message);

        public event BeginEventHandler BeginInAsyncDemoEventHandler;
        public event ProgressChangedEventHandler ProgressChangedInAsyncDemoEventHandler;
        public event CancelEventHandler CancelInAsyncDemoEventHandler;
        public event CompletedEventHandler CompletedInAsyncDemoEventHandler;
        public event ShowMessageInAsyncDemoEventHandler ShowMessageInAsyncDemo;

        #region 异步Demo代码
        ////////private void navBarItemAsyncMethod_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        ////////{
        ////////    AsyncDemoClass newAsyncDemoClass = new AsyncDemoClass();
        ////////    newAsyncDemoClass.ShowMessageInAsyncDemo += new ShowMessageInAsyncDemoEventHandler(ShowMessageInAsyncDemo);
        ////////    newAsyncDemoClass.ShowMessageInAsyncDemo += new ShowMessageInAsyncDemoEventHandler(ShowMessageInAsyncDemo);
        ////////    newAsyncDemoClass.ShowMessageInAsyncDemo += new ShowMessageInAsyncDemoEventHandler(ShowMessageInAsyncDemo);
        ////////    newAsyncDemoClass.ShowMessageInAsyncDemo += new ShowMessageInAsyncDemoEventHandler(ShowMessageInAsyncDemo);
        ////////    newAsyncDemoClass.ShowMessageInAsyncDemo += new ShowMessageInAsyncDemoEventHandler(ShowMessageInAsyncDemo);
        ////////    var varTask = newAsyncDemoClass.taskStartAsyncDelegate(3000);
        ////////    MessageBox.Show("varTask");
        ////////}

        ////////public void ShowMessageInAsyncDemo(object sender, String message)
        ////////{
        ////////    if (this.IsHandleCreated)
        ////////    {
        ////////        new Thread(new ParameterizedThreadStart(delegate (object threadObject)
        ////////        {
        ////////            String mm = threadObject as String;
        ////////            IAsyncResult iar = this.BeginInvoke(new MethodInvoker(delegate
        ////////            {
        ////////                MessageBox.Show(mm);
        ////////            }));
        ////////            this.EndInvoke(iar);
        ////////        })).Start(message);
        ////////    }
        ////////}
        #endregion
        public AsyncDemoClass()
        {

        }

        class AsyncObjectStateClass //申明一个实体类
        {
            public string message;
            public AsyncObjectStateClass()
            {
                message = "AsyncObjectStateClass message";
            }
        }

        public class AsyncMethodParameterObject : Object
        {
            public object parameterObject;
            public AsyncMethodParameterObject(Object o)
            {
                parameterObject = o;
            }
        }
        public class AsyncMethodReturnObject : Object
        {
            public object returnObject;
            public AsyncMethodReturnObject(Object o)
            {
                returnObject = o;
            }
        }
        AsyncCallback asyncCallback;
        AsyncObjectStateClass asyncObjectState;  //实例化

        /// <summary>
        /// 申明供异步委托调用的执行函数
        /// </summary>
        /// <returns></returns>
        private AsyncMethodReturnObject asyncMethod(AsyncMethodParameterObject sleepAsyncMethodParameterObject)
        {
            int sleepMS = (int)sleepAsyncMethodParameterObject.parameterObject;
            Thread.Sleep(sleepMS);
            return new AsyncMethodReturnObject("AsyncMethodReturnObject String");
        }
        /// <summary>
        /// 申明异步委托，返回类型与执行函数一致
        /// </summary>
        public delegate AsyncMethodReturnObject asyncDelegate(AsyncMethodParameterObject sleepObject);
        asyncDelegate beginAsyncDelegate;
        private void backAsyncDelegate(IAsyncResult iAsyncResult)
        {
            AsyncMethodReturnObject asyncMethodReturnObject = beginAsyncDelegate.EndInvoke(iAsyncResult);
            string iarString = asyncMethodReturnObject.returnObject as string;
            AsyncObjectStateClass asyncObjectState = (AsyncObjectStateClass)(iAsyncResult.AsyncState);//通过AsyncState获取传入的object

            if (ShowMessageInAsyncDemo != null)
            {
                ShowMessageInAsyncDemo(this, asyncObjectState.message);
            }
        }
        public async Task taskStartAsyncDelegate(int sleepMS)
        {
            await Task.Run(() =>
            {
                AsyncMethodParameterObject asyncMethodParameterObject = new AsyncMethodParameterObject(sleepMS);
                beginAsyncDelegate = new asyncDelegate(asyncMethod);
                asyncCallback = new AsyncCallback(backAsyncDelegate);
                asyncObjectState = new AsyncObjectStateClass();//实例化类，该对象可以传入回调函数中
                beginAsyncDelegate.BeginInvoke(asyncMethodParameterObject, backAsyncDelegate, asyncObjectState);//异步执行Method，界面不会假死，5秒后执行回调函数，弹出提示框
            });
        }
    }
   
}