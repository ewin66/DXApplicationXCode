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
using System.Linq.Expressions;
using System.ComponentModel;
using static DotNet.WinForm.BaseColpEnumClass;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using DotNet.Model;
using System.Net.NetworkInformation;

namespace DotNet.WinForm.Controls
{
    public partial class BaseUserControl : XtraUserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private const int INTERNET_CONNECTION_MODEM = 1;
        private const int INTERNET_CONNECTION_LAN = 2;

        private const int NET_TYPE_NO_CONNECT_NET = 0x41;                  //未连接到网络   
        private const int NET_STATE_VALID_CONNECT_NOCONNECT_NET = 0x40;    //可以联网，但当前不可用    0x40   
        private const int NET_STATE_MODEM_BUSY = 0x08;                    //调制解调器 繁忙           0x08   
        private const int NET_STATE_SYSTEM_OFFLINE_MODE = 0x20;           //系统脱机模式              0x20   

        [System.Runtime.InteropServices.DllImport("winInet.dll")]
        private static extern bool InternetGetConnectedState(ref int dwFlag, int dwReserved);

        ////public delegate void ShowPictureInVideoDelegateEventHandler(object sender, ShowPictureValueChangedEventArgs e);
        ////public delegate void ShowPictureInAnnotateDelegateEventHandler(object sender, ShowPictureValueChangedEventArgs e);
        public delegate void ShowPictureDelegateEventHandler(object sender, ShowPictureValueChangedEventArgs e);
        public delegate void ShowPreviewPictureDelegateEventHandler(object sender, ShowPreviewPictureValueChangedEventArgs e);
        public delegate void ShowFloatVideoDelegateEventHandler(object sender, ShowFloatVideoValueChangedEventArgs e);
        public delegate void SaveAnnotateDelegateEventHandler(object sender, SaveAnnotateValueChangedEventArgs e);
        public delegate void SavePictureInfoDelegateEventHandler(object sender, PictureInfoValueChangedEventArgs e);
        public delegate void OpenDirectoryDelegateEventHandler(object sender, OpenDirectoryValueChangedEventArgs e);
        public delegate void InsertPictureDelegateEventHandler(object sender, InsertPictureValueChangedEventArgs e);
        public delegate void DeletePictureDelegateEventHandler(object sender, DeletePictureValueChangedEventArgs e);
        public delegate void AddPictureToReportDelegateEventHandler(object sender, AddPictureValueChangedEventArgs e);
        public delegate void UploadAIPictureDelegateEventHandler(object sender, UploadAIPictureValueChangedEventArgs e);
        public delegate void AIResultResponseDelegateEventHandler(object sender, AIResultResponseValueChangedEventArgs e);

        public delegate void PlayVideoDelegateEventHandler(object sender, PlayVideoValueChangedEventArgs e);
        public delegate void SaveBiopsyDelegateEventHandler(object sender, SaveBiopsyValueChangedEventArgs e);

        public Color AIUploadResultMaskColor = Color.Green;
        public Color AIUploadResultConfidenceColor = Color.White;
        public Color AIUploadResultBiopsyColor = Color.White;
        public bool AIUploadResultBiopsyShowScribble = false;

        public String AIUploadResultBiopsyASCII = "X";
        public bool AIUploadResultBiopsyShowConfidence = true;
        public Font AIUploadResultBiopsyFont = null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime? RecordCreatedDateTime = DateTime.Now;

        #region 构造和初始化
        public BaseUserControl()
        {
            InitializeComponent();
        }

        public bool FinalizeInterfaces()
        {
            try
            {
                return true;
            }
            catch (Exception catchException)
            {
                catchException.ProcessException();
                return false;
            }
        }

        #endregion
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
        public class ShowPictureValueChangedEventArgs : EventArgs
        {
            private string _message;
            public ShowPictureValueChangedEventArgs(string message)
            {
                this.Message = message;
            }
            public string Message
            {
                get { return _message; }
                set { _message = value; }
            }
        }
        public class PlayVideoValueChangedEventArgs : EventArgs
        {
            private string _message;
            public PlayVideoValueChangedEventArgs(string message)
            {
                this.Message = message;
            }
            public string Message
            {
                get { return _message; }
                set { _message = value; }
            }
        }
        public class ShowFloatVideoValueChangedEventArgs : EventArgs
        {
            private string _message;
            public ShowFloatVideoValueChangedEventArgs(string message)
            {
                this.Message = message;
            }
            public string Message
            {
                get { return _message; }
                set { _message = value; }
            }
        }
        public class ShowPreviewPictureValueChangedEventArgs : EventArgs
        {
            private string _message;
            public ShowPreviewPictureValueChangedEventArgs(string message)
            {
                this.Message = message;
            }
            public string Message
            {
                get { return _message; }
                set { _message = value; }
            }
        }
        //ShowPreviewPictureDelegateEventHandler
        public class SaveAnnotateValueChangedEventArgs : EventArgs
        {
            private string _message;
            public SaveAnnotateValueChangedEventArgs(string message)
            {
                this.Message = message;
            }
            public string Message
            {
                get { return _message; }
                set { _message = value; }
            }
        }
        public class AddPictureValueChangedEventArgs : EventArgs
        {
            private string _message;
            public AddPictureValueChangedEventArgs(string message)
            {
                this.Message = message;
            }
            public string Message
            {
                get { return _message; }
                set { _message = value; }
            }
        }
        public class OpenDirectoryValueChangedEventArgs : EventArgs
        {
            private string _message;
            public OpenDirectoryValueChangedEventArgs(string message)
            {
                this.Message = message;
            }
            public string Message
            {
                get { return _message; }
                set { _message = value; }
            }
        }
        public class InsertPictureValueChangedEventArgs : EventArgs
        {
            private string _message;
            public InsertPictureValueChangedEventArgs(string message)
            {
                this.Message = message;
            }
            public string Message
            {
                get { return _message; }
                set { _message = value; }
            }
        }
        public class DeletePictureValueChangedEventArgs : EventArgs
        {
            private string _message;
            public DeletePictureValueChangedEventArgs(string message)
            {
                this.Message = message;
            }
            public string Message
            {
                get { return _message; }
                set { _message = value; }
            }
        }
        public class UploadAIPictureValueChangedEventArgs : EventArgs
        {
            private string _message;
            public UploadAIPictureValueChangedEventArgs(string message)
            {
                this.Message = message;
            }
            public string Message
            {
                get { return _message; }
                set { _message = value; }
            }
        }
        public class AIResultResponseValueChangedEventArgs : EventArgs
        {
            private string _message;
            public AIResultResponseValueChangedEventArgs(string message)
            {
                this.Message = message;
            }
            public string Message
            {
                get { return _message; }
                set { _message = value; }
            }
        }
        public DataTable GetColpEvaluateAIEntityDataTable(String parentId)
        {
            DataTable returnDataTable = null;

            if (BaseSystemInfo.UserIsLogOn)
            {//两种方式获取表格
                if (true)
                {
                    returnDataTable = Colp_Evaluate_AIUtilities.GetDataTableByParent(BaseSystemInfo.UserInfo, "Colp", parentId);
                }
                else
                {
                    returnDataTable = null;
                }
            }
            return returnDataTable;
        }
        public DataTable GetColpPictureAnnotateEntityDataTable(String parentId)
        {
            DataTable returnDataTable = null;

            if (BaseSystemInfo.UserIsLogOn)
            {//两种方式获取表格
                if (true)
                {
                    returnDataTable = Colp_Picture_Annotate_DetailUtilities.GetDataTableByParent(BaseSystemInfo.UserInfo, "Colp", parentId);
                }
                else
                {

                }
            }
            return returnDataTable;
        }
        public Image GetAIUploadResponseImage(Colp_File_DetailEntity insertColp_File_DetailEntity)
        {
            if (insertColp_File_DetailEntity == null) return null;

            DataTable sourceColpEvaluateAIEntityDataTable = GetColpEvaluateAIEntityDataTable(insertColp_File_DetailEntity.Id);
            if (sourceColpEvaluateAIEntityDataTable != null && sourceColpEvaluateAIEntityDataTable.Rows.Count > 0)
            {
                var Colp_Evaluate_AIEntityList = new Colp_Evaluate_AIManager().GetList<Colp_Evaluate_AIEntity>(sourceColpEvaluateAIEntityDataTable);
                //给实体赋值
                foreach (Colp_Evaluate_AIEntity eachColp_Evaluate_AI in Colp_Evaluate_AIEntityList)
                {
                    Color AIColor = Color.White;
                    //绑定标注信息
                    try
                    {
                        Bitmap taiUploadResponseBitmap = null;
                        taiUploadResponseBitmap = new Bitmap(1440, 1080);

                        #region  _smartColpAIStudyUploadResponse.Data.Mask
                        //在这里使用 pictureEdit 的mask层功能完成AI结果的叠加显示
                        //参考代码见 this.pictureEditVideo.Properties.OptionsMask.CustomMask = (System.Drawing.Image)newBitmap;
                        try
                        {
                            using (Graphics g = Graphics.FromImage(taiUploadResponseBitmap))
                            {
                                Pen pen = new Pen(Color.Yellow, 3);
                                g.Clear(Color.Transparent);

                                //g.FillEllipse(new SolidBrush(Color.Red), 0 + 1, 0 + 1, taiUploadResponseBitmap.Width-1, taiUploadResponseBitmap.Height-1);  //外圆 

                                g.DrawLine(pen, new PointF(0, 0), new PointF((float)taiUploadResponseBitmap.Width - 1, 0));
                                g.DrawLine(pen, new PointF(taiUploadResponseBitmap.Width - 1, 0), new PointF((float)taiUploadResponseBitmap.Width - 1, taiUploadResponseBitmap.Height - 1));
                                g.DrawLine(pen, new PointF(taiUploadResponseBitmap.Width - 1, taiUploadResponseBitmap.Height - 1), new PointF((float)0, taiUploadResponseBitmap.Height - 1));
                                g.DrawLine(pen, new PointF(0, taiUploadResponseBitmap.Height - 1), new PointF((float)0, 0));

                                if (true)
                                {
                                    g.SmoothingMode = SmoothingMode.AntiAlias; //
                                    g.SmoothingMode = SmoothingMode.HighQuality;//绘图模式 默认为粗糙模式，将会出现锯齿！
                                    float width = taiUploadResponseBitmap.Width;//宽度
                                    float height = taiUploadResponseBitmap.Height;//高度
                                    int x1 = 0;//开始绘制起点X坐标
                                    int y1 = 0;//开始绘制起点Y坐标

                                    #region _smartColpAIStudyUploadResponse.Data.Mask
                                    if (true)
                                    {
                                        //_smartColpAIStudyUploadResponse.Data.Mask.Mask
                                        ////_smartColpAIStudyUploadResponse.Data.Mask.Mask = "";
                                        if (!String.IsNullOrEmpty(eachColp_Evaluate_AI.UploadAIMask) &&
                                            eachColp_Evaluate_AI.UploadAIMaskHeight > 480)
                                        {
                                            byte[] zippedData = Convert.FromBase64String(eachColp_Evaluate_AI.UploadAIMask);
                                            string decodeMaskString = Unzip(zippedData);

                                            //LogUtilities.WriteLine("UploadResponse.Data.Mask.Mask:");
                                            //LogUtilities.WriteLine(_smartColpAIStudyUploadResponse.Data.Mask.Mask);
                                            //LogUtilities.WriteLine("DecodeMaskString:");
                                            //LogUtilities.WriteLine(decodeMaskString);

                                            if (decodeMaskString != null)
                                            {
                                                int weithMask = decodeMaskString.Length / (int)eachColp_Evaluate_AI.UploadAIMaskHeight;
                                                for (int i = 0; i < decodeMaskString.Length; i++)
                                                {
                                                    Point iPoint = new Point(i % weithMask, i / weithMask);
                                                    Char iChar = decodeMaskString[i];
                                                    if (iChar == '0')
                                                    {

                                                    }
                                                    else if (iChar == '1')
                                                    {

                                                    }
                                                    else
                                                    {
                                                        if (iChar == '2')
                                                        {
                                                            pen = new Pen(AIUploadResultMaskColor, 1);
                                                        }
                                                        else if (iChar == '3')
                                                        {
                                                            pen = new Pen(AIUploadResultMaskColor, 1);
                                                        }
                                                        else if (iChar == '4')
                                                        {
                                                            pen = new Pen(AIUploadResultMaskColor, 1);
                                                        }
                                                        else if (iChar == '5')
                                                        {
                                                            pen = new Pen(AIUploadResultMaskColor, 1);
                                                        }
                                                        if (iPoint.X < taiUploadResponseBitmap.Width && iPoint.Y < taiUploadResponseBitmap.Height)
                                                        {
                                                            taiUploadResponseBitmap.SetPixel(iPoint.X, iPoint.Y, pen.Color);
                                                        }
                                                        ////g.DrawLine(pen, iPoint, iPoint);
                                                    }
                                                }

                                                #region  添加辅助识别线
                                                if (false)
                                                {
                                                    for (int i = 0; i < decodeMaskString.Length; i++)
                                                    {
                                                        Point iPoint = new Point(i % weithMask, i / weithMask);
                                                        Char iChar = decodeMaskString[i];

                                                        if (iPoint.X == 0 || iPoint.Y == 0 ||
                                                           //iPoint.X == _smartColpAIStudyUploadResponse.Data.Mask.Height - 1 || iPoint.Y == _smartColpAIStudyUploadResponse.Data.Mask.Height - 1 ||
                                                           iPoint.X == iPoint.Y)
                                                        {
                                                            iChar = '5';
                                                        }
                                                        else
                                                        {
                                                            iChar = '0';
                                                        }
                                                        if (iChar == '0')
                                                        {

                                                        }
                                                        else if (iChar == '1')
                                                        {

                                                        }
                                                        else
                                                        {
                                                            if (iChar == '2')
                                                            {
                                                                pen = new Pen(Color.Red, 1);
                                                            }
                                                            else if (iChar == '3')
                                                            {
                                                                pen = new Pen(Color.Yellow, 1);
                                                            }
                                                            else if (iChar == '4')
                                                            {
                                                                pen = new Pen(Color.White, 1);
                                                            }
                                                            else if (iChar == '5')
                                                            {
                                                                pen = new Pen(Color.Pink, 1);
                                                            }
                                                            if (iPoint.X < taiUploadResponseBitmap.Width && iPoint.Y < taiUploadResponseBitmap.Height)
                                                            {
                                                                taiUploadResponseBitmap.SetPixel(iPoint.X, iPoint.Y, pen.Color);
                                                            }
                                                            ////g.DrawLine(pen, iPoint, iPoint);
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion

                                            //_smartColpAIStudyUploadResponse.Data.Mask.Height

                                            //_smartColpAIStudyUploadResponse.Data.Mask.Weight
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                        catch (OutOfMemoryException catchOutOfMemoryException)
                        {
                            AIColor = Color.Red;
                            if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchOutOfMemoryException.ProcessException();
                        }
                        catch (Exception catchImageException)
                        {
                            AIColor = Color.Red;
                        }
                        #endregion

                        if (taiUploadResponseBitmap != null)
                        {
                            taiUploadResponseBitmap = Robert(taiUploadResponseBitmap, 8);
                        }

                        if (taiUploadResponseBitmap == null)
                        {
                            AIColor = Color.Red;
                            taiUploadResponseBitmap = new Bitmap(1440, 1080);
                        }
                        //叠加颜色表

                        #region _smartColpAIStudyUploadResponse.Data.AiMarks    作废20190515
                        if (taiUploadResponseBitmap == null)
                        {
                            AIColor = Color.Red;
                            taiUploadResponseBitmap = new Bitmap(1440, 1080);
                        }
                        if (taiUploadResponseBitmap != null)
                        {
                            //using (Graphics g = Graphics.FromImage(taiUploadResponseBitmap))
                            //{
                            //    Pen pen = new Pen(Color.Yellow, 5);
                            //    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash; //虚线

                            //    if (true)
                            //    {
                            //        g.SmoothingMode = SmoothingMode.AntiAlias; //
                            //        g.SmoothingMode = SmoothingMode.HighQuality;//绘图模式 默认为粗糙模式，将会出现锯齿！
                            //        int width = taiUploadResponseBitmap.Width;//宽度
                            //        int height = taiUploadResponseBitmap.Height;//高度
                            //        int x1 = 0;//开始绘制起点X坐标
                            //        int y1 = 0;//开始绘制起点Y坐标
                            //        {
                            //            try
                            //            {
                            //                XmlDocument xmlDocument = new XmlDocument();
                            //                xmlDocument.LoadXml(eachColp_Evaluate_AI.UploadAIMarks);

                            //                ////XmlNodeList AnnotateXmlNodeList = xmlDocument.GetElementsByTagName("Annotate");
                            //                {
                            //                    XmlNodeList itemXmlNodeList = xmlDocument.GetElementsByTagName("Item");
                            //                    foreach (XmlNode xNode in itemXmlNodeList)
                            //                    {
                            //                        string eachConfidenceString = xNode.Attributes["Confidence"].Value.ToString();
                            //                        string eachShapeDescString = xNode.Attributes["ShapeDesc"].Value.ToString();

                            //                        float eachConfidence = 0f;
                            //                        float.TryParse(eachConfidenceString, out eachConfidence);

                            //                        List<PointF> currentScribble = new List<PointF>();
                            //                        ////eachCervixImageResultAiMarks.Confidence = 60.0f;
                            //                        ////eachCervixImageResultAiMarks.ShapeDesc = "[[471.5,150.5],[550.5,150.5],[550.5, 225.5],[471.5,225.5],[471.5,150.5]]";

                            //                        eachShapeDescString.Replace("[[", "[").Replace("]]", "]");

                            //                        String[] shapeDescArray = eachShapeDescString.Split(@"],");
                            //                        for (int i = 0; i < shapeDescArray.Length; i++)
                            //                        {
                            //                            string floatPoint = shapeDescArray[i].Replace("[", "").Replace("]", "");
                            //                            string[] RamdomArray = new Regex(@",", RegexOptions.None).Split(floatPoint.Trim());
                            //                            float f0 = float.Parse(RamdomArray[0]);
                            //                            float f1 = float.Parse(RamdomArray[1]);
                            //                            currentScribble.Add(new PointF(f0, f1));
                            //                        }
                            //                        List<Point> redrawscribble = new List<Point>();
                            //                        Point centerPoint = new Point(0, 0);
                            //                        Point leftTopPoint = new Point(taiUploadResponseBitmap.Width, taiUploadResponseBitmap.Height);
                            //                        Point rightBottomPoint = new Point(0, 0);

                            //                        foreach (PointF eachPoint in currentScribble)
                            //                        {
                            //                            //按比例进行恢复
                            //                            Point realLocation = new Point((int)eachPoint.X,
                            //                                                           (int)eachPoint.Y);
                            //                            centerPoint.X = centerPoint.X + realLocation.X;
                            //                            centerPoint.Y = centerPoint.Y + realLocation.Y;

                            //                            if (eachPoint.X < leftTopPoint.X)
                            //                            {
                            //                                leftTopPoint.X = (int)eachPoint.X;
                            //                            }
                            //                            if (eachPoint.Y < leftTopPoint.Y)
                            //                            {
                            //                                leftTopPoint.Y = (int)eachPoint.Y;
                            //                            }

                            //                            if (eachPoint.X > rightBottomPoint.X)
                            //                            {
                            //                                rightBottomPoint.X = (int)eachPoint.X;
                            //                            }
                            //                            if (eachPoint.Y > rightBottomPoint.Y)
                            //                            {
                            //                                rightBottomPoint.Y = (int)eachPoint.Y;
                            //                            }

                            //                            redrawscribble.Add(realLocation);
                            //                        }
                            //                        g.DrawLines(pen, redrawscribble.ToArray());

                            //                        //叠加一个置信度标记
                            //                        Point leftBottomPoint = new Point(leftTopPoint.X, rightBottomPoint.Y - 40);

                            //                        centerPoint.X = centerPoint.X / redrawscribble.Count;
                            //                        centerPoint.Y = centerPoint.Y / redrawscribble.Count;

                            //                        string myString = ((int)(eachConfidence * 100)).ToString() + "%";
                            //                        //绘制数值
                            //                        g.DrawString(myString, new Font(new FontFamily("Times New Roman"), 36, FontStyle.Bold, GraphicsUnit.Pixel), new SolidBrush(Color.Green), leftBottomPoint);
                            //                    }
                            //                }
                            //            }
                            //            catch (Exception catchException)
                            //            {
                            //                AIColor = Color.Red;
                            //                this.ProcessException(catchException);
                            //            }
                            //        }
                            //    }
                            //}
                        }
                        #endregion

                        #region _smartColpAIStudyUploadResponse.Data.Biopsies
                        if (taiUploadResponseBitmap == null)
                        {
                            AIColor = Color.Red;
                            taiUploadResponseBitmap = new Bitmap(1440, 1080);
                        }
                        if (taiUploadResponseBitmap != null)
                        {
                            try
                            {
                                using (Graphics g = Graphics.FromImage(taiUploadResponseBitmap))
                                {
                                    Pen pen = new Pen(Color.Yellow, 5);
                                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash; //虚线

                                    if (true)
                                    {
                                        g.SmoothingMode = SmoothingMode.AntiAlias; //
                                        g.SmoothingMode = SmoothingMode.HighQuality;//绘图模式 默认为粗糙模式，将会出现锯齿！
                                        int width = taiUploadResponseBitmap.Width;//宽度
                                        int height = taiUploadResponseBitmap.Height;//高度
                                        int x1 = 0;//开始绘制起点X坐标
                                        int y1 = 0;//开始绘制起点Y坐标
                                        {
                                            try
                                            {
                                                XmlDocument xmlDocument = new XmlDocument();
                                                if (!String.IsNullOrEmpty(eachColp_Evaluate_AI.UploadAIMarks))  //判断是否为空 by yyg 20190903
                                                {
                                                    xmlDocument.LoadXml(eachColp_Evaluate_AI.UploadAIMarks);

                                                    ////XmlNodeList AnnotateXmlNodeList = xmlDocument.GetElementsByTagName("Annotate");
                                                    {
                                                        XmlNodeList itemXmlNodeList = xmlDocument.GetElementsByTagName("Item");
                                                        int xn = 0;
                                                        foreach (XmlNode xNode in itemXmlNodeList)
                                                        {
                                                            xn++;
                                                            string eachCxString = xNode.Attributes["Cx"].Value.ToString();
                                                            string eachCyString = xNode.Attributes["Cy"].Value.ToString();
                                                            string eachWString = xNode.Attributes["W"].Value.ToString();
                                                            string eachHString = xNode.Attributes["H"].Value.ToString();
                                                            string eachConfidenceString = xNode.Attributes["Confidence"].Value.ToString();

                                                            float eachCx = -1f;
                                                            float.TryParse(eachCxString, out eachCx);

                                                            float eachCy = -1f;
                                                            float.TryParse(eachCyString, out eachCy);

                                                            float eachW = -1f;
                                                            float.TryParse(eachWString, out eachW);

                                                            float eachH = -1f;
                                                            float.TryParse(eachHString, out eachH);

                                                            float eachConfidence = 0f;
                                                            float.TryParse(eachConfidenceString, out eachConfidence);

                                                            if (eachCx != -1 &&
                                                                  eachCy != -1 //&&
                                                                               //eachW != -1 &&
                                                                               //eachH != -1
                                                                  )
                                                            {
                                                                if (eachW == -1)
                                                                {
                                                                    eachW = 200;
                                                                }
                                                                if (eachH == -1)
                                                                {
                                                                    eachH = 200;
                                                                }

                                                                List<PointF> currentScribble = new List<PointF>();
                                                                ////eachCervixImageResultAiMarks.Confidence = 60.0f;

                                                                List<PointF> redrawscribble = new List<PointF>();
                                                                PointF centerPointF = new PointF(eachCx, eachCy);

                                                                PointF leftTopPointF = new PointF(centerPointF.X - eachW / 2, centerPointF.Y - eachH / 2);
                                                                PointF rightTopPointF = new PointF(centerPointF.X + eachW / 2, centerPointF.Y - eachH / 2);

                                                                PointF leftBottomPointF = new PointF(centerPointF.X - eachW / 2, centerPointF.Y + eachH / 2);
                                                                PointF rightBottomPointF = new PointF(centerPointF.X + eachW / 2, centerPointF.Y + eachH / 2);

                                                                //叠加一个活检区域
                                                                if (AIUploadResultBiopsyShowScribble)
                                                                {
                                                                    currentScribble.Add(leftTopPointF);
                                                                    currentScribble.Add(rightTopPointF);
                                                                    currentScribble.Add(rightBottomPointF);
                                                                    currentScribble.Add(leftBottomPointF);
                                                                    currentScribble.Add(leftTopPointF);

                                                                    foreach (PointF eachPointF in currentScribble)
                                                                    {
                                                                        //按比例进行恢复
                                                                        PointF realLocation = new PointF(eachPointF.X,
                                                                                                         eachPointF.Y);
                                                                        redrawscribble.Add(realLocation);
                                                                    }
                                                                    g.DrawLines(pen, redrawscribble.ToArray());
                                                                }
                                                                //叠加一个Xn标记
                                                                {
                                                                    string xnString = AIUploadResultBiopsyASCII + xn.ToString();
                                                                    //AIUploadResultBiopsyFont
                                                                    Font xnStringFont = new Font(AIUploadResultBiopsyFont.Name, Properties.Settings.Default.AIUploadResultBiopsyFontSize, GraphicsUnit.Pixel);
                                                                    var xnStringWidthHeight = TextRenderer.MeasureText(xnString, xnStringFont);

                                                                    PointF xnPointF = new PointF(centerPointF.X - xnStringWidthHeight.Width / 2, centerPointF.Y - xnStringWidthHeight.Height / 2);
                                                                    //绘制数值
                                                                    g.DrawString(xnString, xnStringFont, new SolidBrush(AIUploadResultBiopsyColor), xnPointF);

                                                                    //叠加一个置信度标记
                                                                    if (AIUploadResultBiopsyShowConfidence)
                                                                    {
                                                                        string confidenceString = @"(" + ((int)(eachConfidence * 100)).ToString() + @"%)";
                                                                        Font confidenceStringFont = new Font(xnStringFont.Name, (int)(xnStringFont.Size * 0.6), GraphicsUnit.Pixel);
                                                                        var confidenceStringWidthHeight = TextRenderer.MeasureText(confidenceString, confidenceStringFont);

                                                                        PointF confidencePointF = new PointF(centerPointF.X + xnStringWidthHeight.Width / 2, centerPointF.Y - confidenceStringWidthHeight.Height / 2);
                                                                        //绘制数值
                                                                        g.DrawString(confidenceString, confidenceStringFont, new SolidBrush(Color.Green), confidencePointF);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {

                                                }
                                            }
                                            catch (Exception catchException)
                                            {
                                                AIColor = Color.Red;
                                                if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchException.ProcessException();
                                            }
                                        }
                                    }
                                }
                            }
                            catch (OutOfMemoryException catchOutOfMemoryException)
                            {
                                AIColor = Color.Red;
                                if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchOutOfMemoryException.ProcessException();
                            }
                            catch (Exception catchException)
                            {
                                AIColor = Color.Red;
                                if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchException.ProcessException();
                            }
                            finally
                            {

                            }
                        }
                        #endregion

                        ////TransparentImage(taiUploadResponseBitmap, 0.5f);

                        #region //叠加一个AI标记
                        if (taiUploadResponseBitmap == null)
                        {
                            AIColor = Color.Red;
                            taiUploadResponseBitmap = new Bitmap(1440, 1080);
                        }
                        if (taiUploadResponseBitmap != null)
                        {
                            try
                            {
                                using (Graphics g = Graphics.FromImage(taiUploadResponseBitmap))
                                {
                                    int width = taiUploadResponseBitmap.Width / 10;
                                    int heitht = taiUploadResponseBitmap.Height / 10;

                                    int startPoingX = taiUploadResponseBitmap.Width / 20;
                                    int startPoingY = taiUploadResponseBitmap.Height - taiUploadResponseBitmap.Height / 20 - heitht;

                                    int startPoingXofString = startPoingX + 5;
                                    int startPoingYofString = startPoingY - 5;

                                    string myString = "AI";
                                    //绘制整点刻度
                                    g.DrawRectangle(new Pen(Color.White, 10), new Rectangle(startPoingX, startPoingY, width, heitht));
                                    //绘制数值
                                    g.DrawString(myString, new Font(new FontFamily("Times New Roman"), 100, FontStyle.Bold, GraphicsUnit.Pixel), new SolidBrush(AIColor), new PointF(startPoingXofString, startPoingYofString));
                                }
                            }
                            catch (OutOfMemoryException catchOutOfMemoryException)
                            {
                                AIColor = Color.Red;
                                if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchOutOfMemoryException.ProcessException();
                            }
                        }
                        #endregion

                        return taiUploadResponseBitmap;
                    }
                    catch (OutOfMemoryException catchOutOfMemoryException)
                    {
                        if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchOutOfMemoryException.ProcessException();
                    }
                    catch (Exception catchException)
                    {
                        if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchException.ProcessException();
                    }
                    break;
                }
            }
            return null;
        }

        public void LoadAnnotateInfo(String xmlString, Image xxxxxImage)
        {
            //<?xml version="1.0" encoding="UTF-8"?><Annotate Date="2019-03-20"><Item Name="" Location="{X=0,Y=0}" Color="-2434342" Description="L1" /><Item Name="" Location="{X=1200,Y=0}" Color="-2434342" Description="L2" /><Item Name="" Location="{X=1200,Y=900}" Color="-2434342" Description="L3" /><Item Name="" Location="{X=0,Y=900}" Color="-2434342" Description="L4" /><Scribble Points ="0,0;1200,0;1200,900;0,900;0,0;" /></Annotate>
            if (!String.IsNullOrEmpty(xmlString))
            {
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(xmlString);

                    ////XmlNodeList AnnotateXmlNodeList = xmlDocument.GetElementsByTagName("Annotate");
                    {
                        Size realImageSize = new Size(0, 0);

                        Size panelSize = new Size(xxxxxImage.Size.Width, xxxxxImage.Size.Height);
                        Size imageSize = panelSize;
                        if (xxxxxImage != null)
                            imageSize = new Size(xxxxxImage.Size.Width, xxxxxImage.Size.Height);

                        if (imageSize.Width * panelSize.Height >= imageSize.Height * panelSize.Width)
                        {
                            realImageSize.Width = panelSize.Width;
                            realImageSize.Height = realImageSize.Width * imageSize.Height / imageSize.Width;
                        }
                        else
                        {
                            realImageSize.Height = panelSize.Height;
                            realImageSize.Width = realImageSize.Height * imageSize.Width / imageSize.Height;
                        }

                        Point offPoint = new Point((panelSize.Width - realImageSize.Width) / 2, (panelSize.Height - realImageSize.Height) / 2);

                        XmlNodeList itemXmlNodeList = xmlDocument.GetElementsByTagName("Item");
                        foreach (XmlNode xNode in itemXmlNodeList)
                        {
                            Label newLabel = null;
                            string newString = xNode.Attributes["Location"].Value.ToString().Replace("{", "").Replace("}", "")
                                                              .Replace("X=", "").Replace("Y=", "").Replace(" ", "");
                            string[] myString = Regex.Split(newString, ",");
                            Point loadLocation = new Point(int.Parse(myString[0]), int.Parse(myString[1]));

                            Point labelLocation = new Point(loadLocation.X * realImageSize.Width / imageSize.Width + offPoint.X,
                                                            loadLocation.Y * realImageSize.Height / imageSize.Height + offPoint.Y);

                            if (newLabel == null)   //没找到，新建
                            {
                                newLabel = new Label();
                            }
                        }

                        List<Point> singleScribble = null;
                        List<Point[]> allScribbles = new List<Point[]>();

                        XmlNodeList scribbleXmlNodeList = xmlDocument.GetElementsByTagName("Scribble");
                        foreach (XmlNode eachXmlNode in scribbleXmlNodeList)
                        {
                            String scribblePoints = eachXmlNode.Attributes["Points"].Value;
                            String[] pointArray = Regex.Split(scribblePoints, ";");
                            if (pointArray.Length > 1)
                            {
                                singleScribble = new List<Point>();
                                foreach (String eachString in pointArray)
                                {
                                    String[] pArray = Regex.Split(eachString, ",");
                                    if (pArray.Length == 2)
                                    {
                                        int x, y;
                                        if (int.TryParse(pArray[0], out x) && int.TryParse(pArray[1], out y))
                                        {
                                            Point loadLocation = new Point(x, y);
                                            //按比例进行恢复
                                            Point realPoint = loadLocation;
                                            singleScribble.Add(realPoint);
                                        }
                                    }
                                }
                                allScribbles.Add(singleScribble.ToArray());
                                singleScribble = null;
                            }
                        }
                        if (allScribbles.Count > 0)
                        {
                            try
                            {
                                using (Graphics g = Graphics.FromImage(xxxxxImage))
                                {
                                    Pen pen = new Pen(Color.Yellow, 5);
                                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash; //虚线
                                    foreach (Point[] points in allScribbles)
                                    {
                                        g.DrawLines(pen, points);
                                    }
                                }
                            }
                            catch (OutOfMemoryException catchOutOfMemoryException)
                            {
                                if(Properties.Settings.Default.ShowcatchOutOfMemoryException) catchOutOfMemoryException.ProcessException();
                            }
                        }
                    }
                }
                catch (OutOfMemoryException catchOutOfMemoryException)
                {
                    if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchOutOfMemoryException.ProcessException();
                }
                catch (Exception catchException)
                {
                    if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchException.ProcessException();
                    DevExpress.XtraEditors.XtraMessageBox.Show("标注信息加载出错： " + catchException.Message);
                }
            }
        }

        public Image GetAnnotateImage(Colp_File_DetailEntity insertColp_File_DetailEntity,Size imageSize)
        {
            if (insertColp_File_DetailEntity == null) return null;

            DataTable sourceColpPictureAnnotateEntityDataTable = GetColpPictureAnnotateEntityDataTable(insertColp_File_DetailEntity.Id);
            if (sourceColpPictureAnnotateEntityDataTable != null && sourceColpPictureAnnotateEntityDataTable.Rows.Count > 0)
            {
                var Colp_Picture_Annotate_DetailEntityList = new Colp_Picture_Annotate_DetailManager().GetList<Colp_Picture_Annotate_DetailEntity>(sourceColpPictureAnnotateEntityDataTable);
                //给实体赋值
                foreach (Colp_Picture_Annotate_DetailEntity eachColp_Picture_Annotate_Detail in Colp_Picture_Annotate_DetailEntityList)
                {
                    //绑定标注信息
                    try
                    {
                        Bitmap taiUploadResponseBitmap = null;
                        taiUploadResponseBitmap = new Bitmap(1440,1080);

                        LoadAnnotateInfo(eachColp_Picture_Annotate_Detail.AnnotateXML, taiUploadResponseBitmap);

                        return taiUploadResponseBitmap;
                    }
                    catch (OutOfMemoryException catchOutOfMemoryException)
                    {
                        if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchOutOfMemoryException.ProcessException();
                    }
                    catch (Exception catchException)
                    {
                        if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchException.ProcessException();
                    }
                    break;
                }
            }
            return null;
        }

        #region 获取直接采图的全部文件
        public DataTable GetColpFileDetailDataTable(String parentId)
        {
            DataTable returnDataTable = null;
            //先找到主窗体获取用户信息
            //if (this.ParentForm is frmInspectViewA)
            //{
            //    //frmInspectViewA frmInspectViewA = this.ParentForm as frmInspectViewA;
            //    //if (frmInspectViewA.MainfrmInspectViewA != null)
            //    {

            //        if (BaseSystemInfo.UserIsLogOn)
            //        {
            //returnDataTable = ParameterUtilities.GetDataTable(frmInspectViewA.UserInfo, "Colp", "Colp_Patient_List");
            //returnDataTable = Colp_Patient_ListUtilities.GetDataTable(frmInspectViewA.UserInfo, "Colp", "Colp_Patient_List");
            returnDataTable = Colp_File_DetailUtilities.GetDataTableByParent(BaseSystemInfo.UserInfo, "Colp", parentId);
            //    }
            //}
            //}

            return returnDataTable;
        }
        #endregion

        public class PictureInfoValueChangedEventArgs : EventArgs
        {
            private string _message;
            public PictureInfoValueChangedEventArgs(string message)
            {
                this.Message = message;
            }
            public string Message
            {
                get { return _message; }
                set { _message = value; }
            }
        }
        public class SaveBiopsyValueChangedEventArgs : EventArgs
        {
            private string _message;
            public SaveBiopsyValueChangedEventArgs(string message)
            {
                this.Message = message;
            }
            public string Message
            {
                get { return _message; }
                set { _message = value; }
            }
        }

        public static String DictionarytoXml(Dictionary<String, String> paraDictionary)
        {
            XElement returnXElement = new XElement("root",
            paraDictionary.Select(kv => new XElement(kv.Key, kv.Value)));
            return returnXElement.ToString();
        }
        public static Dictionary<String, String> DictionaryparseXML(String paraXElementString)
        {
            XElement rootElement = XElement.Parse(paraXElementString);
            Dictionary<string, string> returnDictionary = new Dictionary<string, string>();
            foreach (var el in rootElement.Elements())
            {
                returnDictionary.Add(el.Name.LocalName, el.Value);
            }
            return returnDictionary;
        }

        /// <summary>
        /// 将xml文件转换为DataSet
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <returns></returns>
        public static DataSet ConvertXMLFileToDataSet(string xmlFile)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                XmlDocument xmld = new XmlDocument();
                xmld.Load(xmlFile);
                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmld.InnerXml);
                //从stream装载到XmlTextReader  
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                //xmlDS.ReadXml(xmlFile);
                return xmlDS;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        /// <summary>
        /// 将DataSet转换为xml文件
        /// </summary>
        /// <param name="xmlDS"></param>
        /// <param name="xmlFile"></param>
        public static void ConvertDataSetToXMLFile(DataSet xmlDS, string xmlFile)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;

            try
            {
                stream = new MemoryStream();
                //从stream装载到XmlTextReader  
                writer = new XmlTextWriter(stream, Encoding.Unicode);

                //用WriteXml方法写入文件.  
                xmlDS.WriteXml(writer);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);

                //返回Unicode编码的文本  
                UnicodeEncoding utf = new UnicodeEncoding();
                StreamWriter sw = new StreamWriter(xmlFile);
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf - 8\"?>");
                sw.WriteLine(utf.GetString(arr).Trim());
                sw.Close();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }
        public static String ConvertDataTableToXML(DataTable xmlDataTable)
        {
            //xmlDataTable写成Xml结构
            System.IO.TextWriter tw = new System.IO.StringWriter();
            xmlDataTable.WriteXml(tw);
            string xml = tw.ToString();
            return xml;
        }
        public static string GetPropertyName<T>(Expression<Func<T, object>> expr)
        {
            var rtn = "";
            if (expr.Body is UnaryExpression)
            {
                rtn = ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member.Name;
            }
            else if (expr.Body is MemberExpression)
            {
                rtn = ((MemberExpression)expr.Body).Member.Name;
            }
            else if (expr.Body is ParameterExpression)
            {
                rtn = ((ParameterExpression)expr.Body).Type.Name;
            }
            return rtn;

            //Response.Write(GetPropertyName<TestClass>(p => p.ID)); //输出的是 "ID" 两字母  
            //Response.Write(GetPropertyName<TestClass>(p => p.Name)); //输出的是 "Name" 四个字母  
            //Response.Write(GetPropertyName<TestClass>(p => p)); //输出的是 "TestClass" 九个字母 
        }

        /// <summary>
        /// 将".mp4" 替换成  ".jpg"
        /// </summary>
        /// <param name="mp4FullPath"></param>
        /// <returns></returns>
        public static string GetJpgFullPath(String mp4FullPath)
        {
            String jpgFullPath = mp4FullPath;
            ////////if (File.Exists(mp4FullPath) && Path.HasExtension(mp4FullPath))
            {
                String mp4fileExtension = Path.GetExtension(mp4FullPath);

                if (mp4fileExtension.EqualIgnoreCase(".mp4"))
                {
                    jpgFullPath = Path.Combine(Path.GetDirectoryName(mp4FullPath),
                                               Path.GetFileNameWithoutExtension(mp4FullPath) + ".jpg");
                }
            }
            return jpgFullPath;
        }
        /// <summary>
        ///  将".jpg"替换成  ".mp4" 
        /// </summary>
        /// <param name="jpgFullPath"></param>
        /// <returns></returns>
        public static string GetVideoFullPath(String jpgFullPath)
        {
            String mp4FullPath = jpgFullPath;
            ////////if (File.Exists(mp4FullPath) && Path.HasExtension(mp4FullPath))
            {
                String jpgfileExtension = Path.GetExtension(jpgFullPath);

                if (jpgfileExtension.EqualIgnoreCase(".jpg"))
                {
                    mp4FullPath = Path.Combine(Path.GetDirectoryName(jpgFullPath),
                                               Path.GetFileNameWithoutExtension(jpgFullPath) + ".mp4");
                }
            }
            return mp4FullPath;
        }

        /// <summary>
        /// 将数据库里面记录里面的图片目录替换成与当前运行目录并行的Data下的目录
        /// </summary>
        /// <param name="oldFullPath"></param>
        /// <returns></returns>
        public static string GetNewFullPath(String oldFullPath)
        {
            String newFullPath = oldFullPath;
            if (!File.Exists(newFullPath))
            {
                String[] checkDirectoryArray = newFullPath.SplitOnLast(@"\" + ColpSystemInfoClass.DataDirectory + @"\");
                if (checkDirectoryArray.Length == 2)
                {
                    String checkDirectoryBeforData = checkDirectoryArray[0];
                    String checkDirectoryAfterData = checkDirectoryArray[1];
                    if (!checkDirectoryBeforData.EqualIgnoreCase(ColpSystemInfoClass.ApplicationFullPath))
                    {
                        newFullPath = Path.Combine(ColpSystemInfoClass.DataFullPath, checkDirectoryAfterData);
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            return newFullPath;
        }

        public Image ResizeImage(Image initImage, float destRatio)
        {
            Size newSize = new Size((int)(initImage.Width * destRatio), (int)(initImage.Height * destRatio));
            return ResizeImage(initImage, newSize);
        }

        public Image ResizeImage(Image initImage, Size newSize)
        {
            if (initImage != null)
            {
                //原始图片的宽、高
                int initWidth = initImage.Width;
                int initHeight = initImage.Height;

                newSize.Height = initHeight * newSize.Width / initWidth;

                Image returnImage = new System.Drawing.Bitmap(newSize.Width, newSize.Height);
                try
                {
                    using (Graphics g = System.Drawing.Graphics.FromImage(returnImage))
                    {
                        try
                        {
                            //设置质量
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            //定位
                            Rectangle fromR = new Rectangle(0, 0, initImage.Width, initHeight);
                            Rectangle toR = new Rectangle(0, 0, returnImage.Width, returnImage.Height);
                            //画图
                            g.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                            //将截图对象赋给原图
                            ////initImage = BaseUserControl.ImageClone(pickedImage);
                            //释放截图资源
                            //initImage.Dispose();
                            //IntPtr vHandle = _lopen(localFileName, OF_READWRITE | OF_SHARE_DENY_NONE);
                            //while(vHandle == HFILE_ERROR) { Thread.Sleep(100); }
                            //CloseHandle(vHandle);
                            return returnImage;
                        }
                        catch (OutOfMemoryException catchOutOfMemoryException)
                        {
                            if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchOutOfMemoryException.ProcessException();
                        }
                        catch (Exception catchException)
                        {
                            catchException.ProcessException();
                            return returnImage;
                        }
                        finally
                        {
                            g.Dispose();

                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                    }
                }
                catch (OutOfMemoryException catchOutOfMemoryException)
                {
                    if(Properties.Settings.Default.ShowcatchOutOfMemoryException) catchOutOfMemoryException.ProcessException();
                }
                catch (Exception catchImageException)
                {
                    if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchImageException.ProcessException();
                }
                return null;
            }
            else
            {
                return null;
            }
        }
        public static Image CombinImage(Image sourceImg, Image destImg)
        {
            //从指定的System.Drawing.Image创建新的System.Drawing.Graphics     
            try
            {
                using (Graphics g = Graphics.FromImage(sourceImg))
                {
                    g.DrawImage(destImg, 0, 0, sourceImg.Width, sourceImg.Height);
                    return sourceImg;
                }
            }
            catch (OutOfMemoryException catchOutOfMemoryException)
            {
                if(Properties.Settings.Default.ShowcatchOutOfMemoryException) catchOutOfMemoryException.ProcessException();
            }
            finally
            {
            }
            return null;
        }

        //定义roberts算子函数
        public static Bitmap Robert(Bitmap sourceBitmap, int thickness)
        {
            int w = sourceBitmap.Width > 1920 ? 1440 : sourceBitmap.Width;
            int h = sourceBitmap.Height > 1080 ? 1080 : sourceBitmap.Height;
            try
            {
                Bitmap dstBitmap = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                System.Drawing.Imaging.BitmapData srcData = sourceBitmap.LockBits(new Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                System.Drawing.Imaging.BitmapData dstData = dstBitmap.LockBits(new Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                unsafe
                {
                    byte* pIn = (byte*)srcData.Scan0.ToPointer();
                    byte* pOut = (byte*)dstData.Scan0.ToPointer();
                    byte* p;
                    int stride = srcData.Stride;
                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            //边缘八个点像素不变
                            if (x == 0 || x == w - 1 || y == 0 || y == h - 1)
                            {
                                pOut[0] = pIn[0];
                                pOut[1] = pIn[1];
                                pOut[2] = pIn[2];
                            }
                            else
                            {
                                int r0, r5, r6, r7;
                                int g5, g6, g7, g0;
                                int b5, b6, b7, b0;
                                double vR, vG, vB;
                                //右
                                p = pIn + 3;
                                r5 = p[2];
                                g5 = p[1];
                                b5 = p[0];
                                //左下
                                p = pIn + stride - 3;
                                r6 = p[2];
                                g6 = p[1];
                                b6 = p[0];
                                //正下
                                p = pIn + stride;
                                r7 = p[2];
                                g7 = p[1];
                                b7 = p[0];
                                //中心点
                                p = pIn;
                                r0 = p[2];
                                g0 = p[1];
                                b0 = p[0];
                                vR = (double)(Math.Abs(r0 - r5) + Math.Abs(r5 - r7));
                                vG = (double)(Math.Abs(g0 - g5) + Math.Abs(g5 - g7));
                                vB = (double)(Math.Abs(b0 - b5) + Math.Abs(b5 - b7));
                                if (vR > 0)
                                {
                                    vR = Math.Min(255, vR);
                                }
                                else
                                {
                                    vR = Math.Max(0, vR);
                                }
                                if (vG > 0)
                                {
                                    vG = Math.Min(255, vG);
                                }
                                else
                                {
                                    vG = Math.Max(0, vG);
                                }
                                if (vB > 0)
                                {
                                    vB = Math.Min(255, vB);
                                }
                                else
                                {
                                    vB = Math.Max(0, vB);
                                }
                                pOut[0] = (byte)vB;
                                pOut[1] = (byte)vG;
                                pOut[2] = (byte)vR;
                            }
                            pIn += 3;
                            pOut += 3;
                        }
                        pIn += srcData.Stride - w * 3;
                        pOut += srcData.Stride - w * 3;
                    }
                }
                sourceBitmap.UnlockBits(srcData);
                dstBitmap.UnlockBits(dstData);
                dstBitmap.MakeTransparent(Color.Black);
                using (Bitmap cloneBitmap = (Bitmap)BaseUserControl.ImageClone(dstBitmap))
                {
                    try
                    {
                        using (Graphics g = Graphics.FromImage(dstBitmap))
                        {
                            //颜色替换
                            //ColorMap[] colorMap = new ColorMap[1];
                            //colorMap[0] = new ColorMap();
                            //colorMap[0].OldColor = Color.Blue;
                            //colorMap[0].NewColor = Color.White;
                            //ImageAttributes attr = new ImageAttributes();
                            //attr.SetRemapTable(colorMap);
                            //Rectangle rect = new Rectangle(0, 0, dstBitmap.Width, dstBitmap.Height);
                            //rect.Offset(0, 0);
                            //g.DrawImage(dstBitmap, rect, 0, 0, rect.Width, rect.Height, g.PageUnit, attr);

                            //加粗
                            if (thickness == 2)
                            {
                                g.DrawImage(dstBitmap, 1, 1, cloneBitmap.Width, cloneBitmap.Height);
                            }
                            if (thickness == 3)
                            {
                                g.DrawImage(dstBitmap, 0, 1, cloneBitmap.Width, cloneBitmap.Height);
                            }
                            if (thickness == 4)
                            {
                                g.DrawImage(dstBitmap, 0, 1, cloneBitmap.Width, cloneBitmap.Height);
                                g.DrawImage(dstBitmap, 1, 0, cloneBitmap.Width, cloneBitmap.Height);
                                g.DrawImage(dstBitmap, 1, 1, cloneBitmap.Width, cloneBitmap.Height);
                            }
                            else if (thickness >= 5)
                            {
                                g.DrawImage(dstBitmap, -1, -1, cloneBitmap.Width, cloneBitmap.Height);
                                g.DrawImage(dstBitmap, 1, 1, cloneBitmap.Width, cloneBitmap.Height);
                                g.DrawImage(dstBitmap, 0, -1, cloneBitmap.Width, cloneBitmap.Height);
                                g.DrawImage(dstBitmap, 1, -1, cloneBitmap.Width, cloneBitmap.Height);
                                g.DrawImage(dstBitmap, 1, 0, cloneBitmap.Width, cloneBitmap.Height);
                                g.DrawImage(dstBitmap, 0, 1, cloneBitmap.Width, cloneBitmap.Height);
                                g.DrawImage(dstBitmap, -1, 1, cloneBitmap.Width, cloneBitmap.Height);
                                g.DrawImage(dstBitmap, -1, 0, cloneBitmap.Width, cloneBitmap.Height);
                            }
                        }
                        cloneBitmap.Dispose();
                        sourceBitmap.Dispose();
                        return dstBitmap;
                    }
                    catch (OutOfMemoryException catchOutOfMemoryException)
                    {
                       if(Properties.Settings.Default.ShowcatchOutOfMemoryException) catchOutOfMemoryException.ProcessException();
                        return null;
                    }
                    catch (Exception catchException)
                    {
                        if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchException.ProcessException();
                        return null;
                    }
                    return null;
                }
            }
            catch (OutOfMemoryException catchOutOfMemoryException)
            {
                if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchOutOfMemoryException.ProcessException();
                return null;
            }
            catch (Exception catchException)
            {
                ////////catchException.ProcessException();
                return null;
            }
            finally
            {

            }
        }

        /// <summary>  
        /// 处理图片透明操作  
        /// </summary>  
        /// <param name="srcImage">原始图片</param>  
        /// <param name="opacity">透明度(0.0---1.0)</param>  
        /// <returns></returns>  
        public Image TransparentImage(Image srcImage, float opacity)
        {
            float[][] nArray ={ new float[] {1, 0, 0, 0, 0},
                        new float[] {0, 1, 0, 0, 0},
                        new float[] {0, 0, 1, 0, 0},
                        new float[] {0, 0, 0, opacity, 0},
                        new float[] {0, 0, 0, 0, 1}};
            ColorMatrix matrix = new ColorMatrix(nArray);
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            try
            {
                using (Bitmap cloneBitmap = new Bitmap(BaseUserControl.ImageClone(srcImage)))
                {
                    using (Graphics g = Graphics.FromImage(srcImage))
                    {
                        g.Clear(Color.Transparent);
                        g.DrawImage(cloneBitmap, new Rectangle(0, 0, cloneBitmap.Width, cloneBitmap.Height), 0, 0, cloneBitmap.Width, cloneBitmap.Height, GraphicsUnit.Pixel, attributes);

                        return srcImage;
                    }
                }
            }
            catch (OutOfMemoryException catchOutOfMemoryException)
            {
                if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchOutOfMemoryException.ProcessException();
            }            
            return null;
        }


        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }
                return mso.ToArray();
            }
        }

        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }
                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        public static string GetMemberName<T>(System.Linq.Expressions.Expression<System.Func<T>> memberExpression)
        {
            System.Linq.Expressions.MemberExpression expressionBody = (System.Linq.Expressions.MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }

        /// <summary>
        /// 判断本地的连接状态
        /// </summary>
        /// <returns></returns>
        public static bool LocalConnectionStatus()
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
            catch (Exception catchException)
            {
                ////////if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchException.ProcessException();
                isconn = false;
                errorCount = urls.Length;
            }
            //if (errorCount > 0 && errorCount < 3)
            //  isconn = true;
            return isconn;
        }

        public static Bitmap ImageClone(Image initImage)
        {
            if (initImage != null)
            {
                try
                {
                    System.Drawing.Bitmap imageClone = new System.Drawing.Bitmap(initImage.Width, initImage.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(imageClone))
                    {
                        //设置质量
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位
                        Rectangle fromR = new Rectangle(0, 0, initImage.Width, initImage.Height);
                        Rectangle toR = new Rectangle(0, 0, imageClone.Width, imageClone.Height);
                        //画图
                        g.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                        ////////g.DrawImage(initImage, 0, 0);
                        g.Flush();
                        g.Dispose();
                        return imageClone;
                    }
                }
                catch (OutOfMemoryException catchOutOfMemoryException)
                {
                    if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchOutOfMemoryException.ProcessException();
                }
                catch (Exception catchException)
                {
                    if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchException.ProcessException();
                }
            }
            return null;
        }

        public static string RunCmd(string path, string command)
        {
            Process pro = new Process();
            pro.StartInfo.FileName = "cmd.exe";
            command = command.Trim().TrimEnd('&') + "&exit";//说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态
            pro.StartInfo.CreateNoWindow = true;         // 不创建新窗口    
            pro.StartInfo.UseShellExecute = false;       //不启用shell启动进程  
            pro.StartInfo.Verb = "RunAs";
            pro.StartInfo.RedirectStandardInput = true;  // 重定向输入    
            pro.StartInfo.RedirectStandardOutput = true; // 重定向标准输出    
            pro.StartInfo.RedirectStandardError = true;
            pro.StartInfo.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
            pro.StartInfo.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
            // 重定向错误输出  
            pro.StartInfo.WorkingDirectory = path;
            pro.Start();//开启cmd
            pro.StandardInput.WriteLine(command);
            pro.StandardInput.AutoFlush = true;
            ////pro.StandardInput.WriteLine("exit"); //若是运行时间短可加入此命令

            string output = pro.StandardOutput.ReadToEnd();

            pro.WaitForExit();//若运行时间长,使用这个,等待程序执行完退出进程
            pro.Close();
            return output;
        }
    }
}
