using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraGrid.Columns;
using System.Diagnostics;
using DevExpress.XtraEditors.Controls;
using JKD.ViewModels;

namespace JKD
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
       public ComboBoxEdit cbeRen = new ComboBoxEdit();
        public MainForm()
        {
            InitializeComponent();
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            var fluentAPI = mvvmContext1.OfType<MainFormViewModel>();
           


            refreshData("2000-01-01", "2050-01-01", "%%");
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "id","序号"}, {"accid","帐户编号" }, {"kxly","款项来源" }, {"bz","币种" },
                { "lrsj","录入时间"}, {"dysj","打印时间" }, {"jkrq","缴款日期" }, {"jkr","缴款人" }, {"je","金额" }

            };
          //  this.gridControl1.DataSource = dt.DefaultView;
            foreach(GridColumn gc in this.gridView1.Columns)
            {
                Debug.WriteLine(gc.FieldName);
                gc.Caption = dic[gc.FieldName];
                gc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gc.AppearanceHeader.Font = new Font("微软雅黑", 12);
                
                if(gc.FieldName == "je")
                {
                    gc.SummaryItem.FieldName = "je";
                    gc.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gc.SummaryItem.DisplayFormat = "小计金额：{0:C2}";
                }
                if(gc.FieldName == "lrsj")
                {
                    gc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    gc.DisplayFormat.FormatString = "yyyy-MM-dd hh:mm:ss";
                }
                if(gc.FieldName == "je")
                {
                    gc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gc.DisplayFormat.FormatString = "C2";
                }
                
                
            }
            
            ComboBoxEdit cbeDate = new ComboBoxEdit();
            cbeDate.Properties.Items.AddRange(new string[] { "本月", "上月", "本年", "上年" });
            cbeDate.Tag = "日期快捷选择：";
            cbeDate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cbeDate.SelectedIndex = 0;
           // ComboBoxEdit cbeRen = new ComboBoxEdit();
            cbeRen.Properties.Items.AddRange(new string[] { "所有","陈春峰","蔡晓星","胡燕","潘小琴"});
            cbeRen.Tag = "    缴款人：";
            cbeRen.SelectedIndex = 0;
            SimpleButton btnQuery = new SimpleButton();
            btnQuery.Text = "查询";
            btnQuery.Tag = "heeh";
            fluentAPI.SetBinding(cbeRen, y => y.EditValue, x => x.Jkr);
            #region 单击查询按钮
            btnQuery.Click += (x, y) =>
            {

               
                string name = cbeRen.EditValue.ToString()=="所有"?string.Empty: cbeRen.EditValue.ToString();
                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;
                int day = DateTime.Now.Day;
                //本月第一天
                DateTime thismonth = new DateTime(year, month, 1);
                string begin = string.Empty;
                string end = string.Empty;
                switch (cbeDate.EditValue.ToString())
                {
                    case "本月":
                        begin = new DateTime(year, month, 1).ToString("yyyy-MM-dd");
                        end = DateTime.Now.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                        break;
                    case "上月":
                        begin = new DateTime(year, month, 1).AddMonths(-1).ToString("yyyy-MM-dd");
                        end = new DateTime(year, month, 1).AddDays(-1).ToString("yyyy-MM-dd");
                        break;
                    case "本年":
                        begin = new DateTime(year,1, 1).ToString("yyyy-MM-dd");
                        end = new DateTime(year+1, 1, 1).AddDays(-1).ToString("yyyy-MM-dd");
                        break;
                    case "上年":
                        begin = new DateTime(year, 1, 1).AddYears(-1).ToString("yyyy-MM-dd");
                        end = new DateTime(year, 1, 1).AddDays(-1).ToString("yyyy-MM-dd");
                        break;
                }
                #endregion

             refreshData(begin, end, name=="所有"?string.Empty:name);

            };

            SimpleButton btnAdd = new SimpleButton();
            btnAdd.Text = "新增";
            Control[] cons = new Control[] { cbeDate,cbeRen,btnQuery,btnAdd};
            LayoutControl lc = new LayoutControl();
            lc.Dock = System.Windows.Forms.DockStyle.Fill;
            lc.Root.LayoutMode = LayoutMode.Flow;
            LayoutControlItem temps = null;
            this.groupControl3.Controls.Add(lc);
            int count = 0;
            foreach (Control temp in cons)
            {
                LayoutControlItem lci = new LayoutControlItem();
                lci.TextAlignMode = TextAlignModeItem.AutoSize;
                lci.Control = temp;
                lci.Text = Convert.ToString(temp.Tag);
                lci.Parent = lc.Root;
                if (count == 0)
                {
                    temps = lci;
                }
                lci.Move(temps, InsertType.Right);
                temps = lci;
                count++;
            }

        }

       public void refreshData(string begin,string end,string ren)
        {
            string sql = $"select * from account where lrsj between '{begin}' and '{end}' and jkr like '%{ren}%' order by id desc";
            this.gridControl1.DataSource = SqlHelper.ExecuteTable(sql).DefaultView;
        }
        private void gridControl1_Click(object sender, EventArgs e)
        {

        }
    }
}