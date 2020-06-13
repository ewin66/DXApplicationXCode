using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DXApplicationXCode
{
    using DevExpress.XtraBars.Docking2010.Views;
    using DevExpress.XtraBars.Docking2010.Views.Widget;

    public partial class UCDocumentManager : UserControl
    {
        #region 私有变量
        /// <summary>
        /// 列集合
        /// </summary>
        List<StackGroup> _stackGroups;
        /// <summary>
        /// Document集合
        /// </summary>
        List<Document> _documents;

        #endregion

        public UCDocumentManager()
        {
            InitializeComponent();
        }

        #region
        /// <summary>
        /// 创建列
        /// </summary>
        /// <param name="count">列数量</param>
        /// <param name="pixels">列宽度像素点数量,缺省值200</param>
        public void CreateStackGroups(int count, double pixels = 200)
        {
            this.widgetView1.StackGroups.Clear();
            _stackGroups = new List<StackGroup>();
            for (int i = 0; i < count; i++)
            {
                StackGroup stackGroup = new StackGroup();
                stackGroup.Length = new Length(pixels);
                _stackGroups.Add(stackGroup);
            }
            this.widgetView1.StackGroups.AddRange(_stackGroups);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="documents"></param>
        public void CreateDocuments(IEnumerable<Document> documents)
        {
            if (documents == null) return;
            if (documents.Count() == 0)
            {
                this.widgetView1.Documents.Clear();
                return;
            }
            this._documents = documents.ToList<Document>();
            this.widgetView1.Documents.AddRange(this._documents);
            int columnCount = _stackGroups.Count;//列的数量
            double dRowCount = this._documents.Count * 1.0 / columnCount;//document需占最大行数，有小数点
            int averageRowCount = (int)dRowCount;//倒数第二层的行数
            int maxColumnCount = (int)((dRowCount - averageRowCount) * columnCount);//最后一行占前几列

            int index = 0;//document集合索引
            this.documentManager1.BeginUpdate();
            for (int j = 0; j < columnCount; j++)
            {
                for (int i = 0; i < averageRowCount; i++)
                {
                    _stackGroups[j].Items.Add(this._documents[index++]);
                }
                if (j <= maxColumnCount)
                {
                    _stackGroups[j].Items.Add(this._documents[index++]);
                }
            }
            this.documentManager1.EndUpdate();
        }
        /// <summary>
        /// 新增一个document
        /// </summary>
        /// <param name="document"></param>
        public void AddDocument(Document document)
        {
            this.widgetView1.Documents.Add(document);
            this._documents.Add(document);
            StackGroup stackGroupp = this.GetStackGroupPositionToAddDocument();
            stackGroupp.Items.Add(document);
        }
        /// <summary>
        /// 批量新增document，应保证之前document肯定不存在
        /// </summary>
        /// <param name="documents"></param>
        public void AddRangeDocument(IEnumerable<Document> documents)
        {
            this._documents.AddRange(documents);
            this.widgetView1.Documents.AddRange(documents);
            foreach (Document document in documents)
            {
                StackGroup stackGroupp = this.GetStackGroupPositionToAddDocument();
                stackGroupp.Items.Add(document);
            }
        }

        public void RemoveDocument(string caption)
        {
            Document document = this._documents.FirstOrDefault<Document>(x => x.Caption.Equals(caption));
            if (document != null)
            {
                this.widgetView1.Documents.Remove(document);
                this._stackGroups[document.ColumnIndex].Items.Remove(document);
                this._documents.Remove(document);
            }
        }

        public void RemoveRangeDocument(IEnumerable<string> captions)
        {
            foreach (string caption in captions)
            {
                this.RemoveDocument(caption);
            }
        }
        #endregion

        #region 内部函数
        int count = 0;
        /// <summary>
        /// 匹配每个document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void widgetView_QueryControl(object sender, QueryControlEventArgs e)
        {
            Control widget = new Control();
            widget.Text = companyFullNames[e.Document.Caption];
            e.Control = widget;
            (e.Document as Document).MaximizedControl = new Control();
            //if (++count >= this._documents.Count)
            //    Jesus.Utility.DevExpressHelper.DevExpressDeal.CloseWaitForm();
        }
        /// <summary>
        /// 获取从前往后最少行的列
        /// </summary>
        /// <returns></returns>
        StackGroup GetStackGroupPositionToAddDocument()
        {
            int columnIndex = 0;
            int count = this._stackGroups.Count;
            int i = 0;
            for (; i < count; i++)
            {
                if (this._stackGroups[columnIndex].Items.Count > this._stackGroups[i].Items.Count)
                    columnIndex = i;
            }
            return this._stackGroups[columnIndex];
        }
        #endregion

        #region 临时
        Dictionary<string, string> companyFullNames = new Dictionary<string, string>();
        void FillCompaniesInformation()
        {
            companyFullNames.Add("AAPL", "Apple Inc.");
            companyFullNames.Add("YHOO", "Yahoo! Inc.");
            companyFullNames.Add("CSCO", "Cisco Systems Inc.");
            companyFullNames.Add("ADBE", "Adobe Systems Inc.");
            companyFullNames.Add("BAC", "Bank of America Corporation");
            companyFullNames.Add("DELL", "Dell Inc.");
            companyFullNames.Add("NVDA", "NVIDIA Corporation");
            companyFullNames.Add("HPQ", "Hewlett-Packard Company");
        }

        public void CreateDocuments()
        {
            FillCompaniesInformation();
            List<Document> documents = new List<Document>();
            List<string> keys = companyFullNames.Keys.ToList<string>();
            for (int i = 0; i < 8; i++)
            {
                Document document = new Document();
                document.Caption = keys[i];
                document.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.False;
                document.Properties.AllowDock = DevExpress.Utils.DefaultBoolean.False;
                document.Properties.AllowFloat = DevExpress.Utils.DefaultBoolean.False;
                document.Properties.AllowActivate = DevExpress.Utils.DefaultBoolean.False;

                documents.Add(document);
            }
            this.CreateDocuments(documents);
        }

        public void AddRangeDocument(IEnumerable<string> captions)
        {
            List<Document> documents = new List<Document>();
            foreach (string caption in captions)
            {
                Document document = new Document();
                document.Caption = caption;
                document.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.False;
                document.Properties.AllowDock = DevExpress.Utils.DefaultBoolean.False;
                document.Properties.AllowFloat = DevExpress.Utils.DefaultBoolean.False;
                document.Properties.AllowActivate = DevExpress.Utils.DefaultBoolean.False;
                documents.Add(document);
            }
            this.AddRangeDocument(documents);
        }

        public void AddDocument(string caption)
        {
            Document document = new Document();
            document.Caption = caption;
            document.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.False;
            document.Properties.AllowDock = DevExpress.Utils.DefaultBoolean.False;
            document.Properties.AllowFloat = DevExpress.Utils.DefaultBoolean.False;
            document.Properties.AllowActivate = DevExpress.Utils.DefaultBoolean.False;
            this.AddDocument(document);
        }
        #endregion

        private void UCDocumentManager_Load(object sender, EventArgs e)
        {
            if (_documents == null)
            {
                _documents = new List<Document>();
            }
        }
    }
}
