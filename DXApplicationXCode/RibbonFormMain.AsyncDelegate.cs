using System;
using System.Runtime.Remoting.Messaging;
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
        //public delegate void BeginEventHandler(object sender, object message);
        //public delegate void ProgressChangedEventHandler(object sender, object message);
        //public delegate void CancelEventHandler(object sender, object message);
        //public delegate void CompletedEventHandler(object sender, object message);
        public delegate void DoSomethingInAsyncTaskDemoEventHandler(object sender, object message);
        public delegate void ShowMessageInAsyncBackcallDemoEventHandler(object sender, object message);

        //public event BeginEventHandler BeginInAsyncDemoEventHandler;
        //public event ProgressChangedEventHandler ProgressChangedInAsyncDemoEventHandler;
        //public event CancelEventHandler CancelInAsyncDemoEventHandler;
        //public event CompletedEventHandler CompletedInAsyncDemoEventHandler;
        public event DoSomethingInAsyncTaskDemoEventHandler DoSomethingInAsyncTaskDemo;
        public event ShowMessageInAsyncBackcallDemoEventHandler ShowMessageInAsyncBackcallDemo;

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

        /// <summary>
        /// 申明一个实体类作为异步调用的传入参数
        /// </summary>
        class AsyncObjectStateClass
        {
            public string message;
            public AsyncObjectStateClass()
            {
                message = "AsyncObjectStateClass message";
            }
            public AsyncObjectStateClass(object mmm)
            {
                message = mmm.ToString();
            }
        }
        /// <summary>
        /// 申明一个实体类作为任务函数的参数
        /// </summary>
        public class AsyncMethodParameterObject : Object
        {
            public object parameterObject;
            public AsyncMethodParameterObject(Object o)
            {
                parameterObject = o;
            }
        }
        /// <summary>
        /// 申明一个实体类作为异步调用的返回结果
        /// </summary>
        public class AsyncMethodReturnObject : Object
        {
            public object returnObject;
            public AsyncMethodReturnObject(Object o)
            {
                returnObject = o;
            }
        }
        
        //AsyncCallback asyncCallback;                    //实例化回调
        //AsyncObjectStateClass asyncObjectState;         //实例化传入参数
        //AsyncMethodReturnObject asyncMethodReturnObject;//实例化返回结果

        /// <summary>
        /// 申明供异步委托调用的任务执行函数
        /// </summary>
        /// <returns></returns>
        private AsyncMethodReturnObject asyncMethodTask(AsyncMethodParameterObject sleepAsyncMethodParameterObject)
        {
            int sleepMS = (int)sleepAsyncMethodParameterObject.parameterObject;

            ParallelLoopResult result = Parallel.For(0, 1, i =>
            {
                Thread.Sleep(sleepMS);
                if (DoSomethingInAsyncTaskDemo != null)
                {
                    DoSomethingInAsyncTaskDemo(this, sleepMS);
                }
                else
                {

                }
            });
            return new AsyncMethodReturnObject("AsyncMethodReturnObject Return String");
        }
        /// <summary>
        /// 申明异步委托，返回类型与任务执行函数一致
        /// </summary>
        public delegate AsyncMethodReturnObject asyncDelegate(AsyncMethodParameterObject sleepObject);
        asyncDelegate asyncDelegateTask;
        private void backAsyncDelegate(IAsyncResult iAsyncResult)
        {
            if (true)
            {
                ////while (!iar.IsCompleted)
                ////{
                ////    //这里可以执行其他代码
                ////}

                AsyncObjectStateClass asyncObjectState = (AsyncObjectStateClass)(iAsyncResult.AsyncState);//通过AsyncState获取传入的object

                AsyncResult result = (AsyncResult)iAsyncResult;

                asyncDelegate caller = (asyncDelegate)result.AsyncDelegate;                               //任务执行函数

                AsyncMethodReturnObject asyncMethodReturnObject = asyncDelegateTask.EndInvoke(iAsyncResult); //通过EndInvoke获取异步结果

                if (ShowMessageInAsyncBackcallDemo != null)
                {
                    ShowMessageInAsyncBackcallDemo(this, asyncObjectState.message);
                    ShowMessageInAsyncBackcallDemo(this, asyncMethodReturnObject.returnObject.ToString());
                }
                else
                {

                }
            }
            else
            {
                iAsyncResult.AsyncWaitHandle.WaitOne();//等待方法调用的完成
            }          
        }
        public async Task taskStartAsyncDelegate(object sleepMS)
        {
            if (true)
            {
                await Task.Run(() =>
                {
                    AsyncMethodParameterObject asyncMethodParameterObject = new AsyncMethodParameterObject(sleepMS);
                    asyncDelegateTask = new asyncDelegate(asyncMethodTask);
                    AsyncCallback asyncCallback = new AsyncCallback(backAsyncDelegate);
                    AsyncObjectStateClass asyncObjectState = new AsyncObjectStateClass(sleepMS);//实例化类，该对象可以传入回调函数中
                    IAsyncResult iar = (AsyncResult)asyncDelegateTask.BeginInvoke(asyncMethodParameterObject, asyncCallback, asyncObjectState);//异步执行Method，界面不会假死，后执行回调函数，弹出提示框
                });
            }
            else
            {
                await Task.Run(() =>
                {
                //回调函数callback方法可以改写成lambda形式
                //AsyncMethodParameterObject asyncMethodParameterObject = new AsyncMethodParameterObject(sleepMS);
                new asyncDelegate(asyncMethodTask).BeginInvoke(new AsyncMethodParameterObject(sleepMS), ar =>
                    {   //回调函数写成了lambda形式
                        // Retrieve the delegate.
                    AsyncResult result = (AsyncResult)ar;
                        asyncDelegate caller = (asyncDelegate)result.AsyncDelegate;

                        // Retrieve the format string that was passed as state information.
                        AsyncObjectStateClass asyncObjectState = (AsyncObjectStateClass)ar.AsyncState;//通过AsyncState获取传入的object

                    // Call EndInvoke to retrieve the results.
                    AsyncMethodReturnObject asyncMethodReturnObject = caller.EndInvoke(ar);

                    //MessageBox.Show(formatString.ToString() + "\n\r" + returnValue);
                }, new AsyncObjectStateClass("Return String"));
                });
            }
        }
    }
   
}