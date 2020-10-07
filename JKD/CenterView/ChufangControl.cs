using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Diagnostics;
using JKD.DB;
using JKD.Models;
using JKD.ViewModels;
using DevExpress.XtraGrid.Columns;
using JKD.Reports;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid;
using DevExpress.XtraTab.Buttons;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTab.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using JKD.utils;
using DevExpress.XtraBars;

namespace JKD.CenterView
{
    public partial class ChufangControl : XtraUserControl
    {
       
        public FyFormViewModel FFVM;
        public Dictionary<string, string> dic = new Dictionary<string, string>()
        {
            {"gid","组号" }, { "drug","药品名称"}, { "spci","规格"}, { "unitprice","单价"}, { "quantity","数量"}, { "yongliang","用量"}
            , { "danwei","剂量单位"}, { "unit","单位"}, { "yongfa","用法"}, { "cishu","次数"}, { "opertime","操作时间"}
        };
        public ChufangControl()
        {
           InitializeComponent();
            FFVM = mvvmContext.GetViewModel<FyFormViewModel>();
            mvvmContext.SetBinding(this.deBegin, e => e.Text, "BeginTime");
            mvvmContext.SetBinding(this.deEnd, e => e.Text, "EndTime");
            mvvmContext.SetBinding(this.teDoctor, e => e.EditValue, "Doctor");
            mvvmContext.SetBinding(this.tePatient, e => e.EditValue, "Patient");
            mvvmContext.SetBinding(this.gridControl1, e => e.DataSource, "cfhead");
            mvvmContext.SetBinding(this.gridControl2, e => e.DataSource, "Dv");
            mvvmContext.SetBinding(this.gridControl3, e => e.DataSource, "CfDetailList");
            mvvmContext.SetBinding(this.gridControl4, e => e.DataSource, "HzByDoctor");
            mvvmContext.SetBinding(this.tsUnique, e => e.IsOn, "IsAll");
           // mvvmContext.SetBinding(this.xtraTabControl1.CustomHeaderButtons[0], e => e.Enabled, "IsEnable");
            mvvmContext.BindCommand<FyFormViewModel>(this.sbQuery, x => x.Query());
           // mvvmContext.SetBinding(this.cbeDate, e => e.Text, "SelectDate");
            mvvmContext.SetBinding(this.lblTotlprice, e => e.Text, "Huizong");

            mvvmContext.WithEvent<FyFormViewModel, CustomHeaderButtonEventArgs>(xtraTabControl1, "CustomHeaderButtonClick")
                .EventToCommand(x=>x.Refresh(),args=>args.Button.Caption=="同步数据");
            mvvmContext.WithEvent<FyFormViewModel, CustomHeaderButtonEventArgs>(xtraTabControl1, "CustomHeaderButtonClick")
                .EventToCommand(x => x.DeleteRow(), args => args.Button.Caption == "删除选中记录");
            mvvmContext.WithEvent<FyFormViewModel, CustomHeaderButtonEventArgs>(xtraTabControl1, "CustomHeaderButtonClick")
               .EventToCommand(x => x.Print(), args => args.Button.Caption == "打印处方封面");
            mvvmContext.WithEvent<FyFormViewModel, CustomHeaderButtonEventArgs>(xtraTabControl1, "CustomHeaderButtonClick")
               .EventToCommand(x=> x.Export(this.gridControl1),cs=>this.gridControl1, args => args.Button.Caption == "导出处方");
            mvvmContext.WithEvent<FyFormViewModel, CustomHeaderButtonEventArgs>(xtraTabControl1, "CustomHeaderButtonClick")
              .EventToCommand(x => x.Export(this.gridControl2), cs => this.gridControl2, args => args.Button.Caption == "导出汇总");
            mvvmContext.WithEvent<FyFormViewModel, RowClickEventArgs>(gridView1, "RowClick")
               .EventToCommand(x=>x.ShowHistory(),args=>args.Clicks==2);
            mvvmContext.WithEvent<FyFormViewModel, CustomHeaderButtonEventArgs>(xtraTabControl2, "CustomHeaderButtonClick")
              .EventToCommand(x=>x.ShowHistory(), args => args.Button.Caption == "查看历史处方");
            

            mvvmContext.WithEvent<FyFormViewModel, FocusedRowChangedEventArgs>(gridView1, "FocusedRowChanged")
               .EventToCommand(x =>x.ChangeRow(gridView1), args=>gridView1);
            groupControl3.CustomButtonClick += (s, e) =>
            {
                if (e.Button.Properties.Caption == "打印处方封面")
                {
                    FFVM.Print();
                }
            };

            Init();
          
            this.toggleSwitch1_Toggled(this.tsAll, new EventArgs());
           // layoutControlItem1.Enabled = layoutControlItem2.Enabled = false;
            groupControl2.CustomButtonChecked += (s, e) =>
            {
                bandedGridView1.ExpandAllGroups();
            };
            groupControl2.CustomButtonUnchecked += (s, e) =>
            {
                bandedGridView1.CollapseAllGroups();
            };
            groupControl2.CustomButtonClick += (s, e) =>
            {
                if (e.Button.Properties.Caption == "导出数据")
                {
                    string filename = MySaveFileDialog.Show();
                    if (!string.IsNullOrEmpty(filename))
                    {
                        bandedGridView1.ExportToXlsx(filename);
                    };
                }
            };
            popupMenu1.Manager.ItemClick += (s, e) =>
            {
                FFVM.SelectDate = e.Item.Caption;
            };
           
           
            dropDownButton1.DropDownControl=this.popupMenu1;
        }
        private void Init()
        {
           
            GridFormatRule gridFormatRule = new GridFormatRule();
           
            FormatConditionRuleExpression formatConditionRuleExpression = new FormatConditionRuleExpression();
            gridFormatRule.Column = this.gridView1.Columns["cftype"];
            // gridFormatRule.ApplyToRow = true;
            formatConditionRuleExpression.Appearance.BackColor = Color.LightGreen;
            formatConditionRuleExpression.Expression = "[cftype]='儿科'";
            gridFormatRule.Rule = formatConditionRuleExpression;
            gridView1.FormatRules.Add(gridFormatRule);
            this.gridView1.CustomDrawCell += (a, b) =>
            {
                if (b.Column.FieldName == "cftype")
                {
                    string val = b.CellValue.ToString();
                    if (val == "精二")
                    {
                        b.Appearance.BackColor = Color.DarkRed;
                        b.Appearance.ForeColor = Color.White;
                        b.Appearance.Font = new Font("宋体", 12, FontStyle.Bold);

                    }
                    else if (val == "儿科")
                    {
                        b.Appearance.BackColor = Color.Green;
                        b.Appearance.ForeColor = Color.White;
                        b.Appearance.Font = new Font("宋体", 12, FontStyle.Bold);
                    }
                }

            };


            this.gridView3.FormatRules.AddDataBar(this.gridView3.Columns["TotalPrice"]);

            this.gridView1.MasterRowGetRelationCount += (x, y) =>
            {
                y.RelationCount = 1;
            };
            this.gridView1.MasterRowGetChildList += (sender, e) =>
            {
                GridView gv = sender as GridView;
                gv.DetailTabHeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Top;

                string opertime = (gv.GetRow(e.RowHandle) as Cfhead).opertime;
                //string sqlstr = $"SELECT gid 组号,drug 药品名称,spci 规格,unitprice 单价,CONCAT('Sig：',yongliang,danwei,'  ',yongfa,'  ',cishu) 用法,Concat(round(quantity),unit) 数量  FROM cfdetail where opertime = '{opertime}'";
                //e.ChildList = SqlHelper.ExecuteTable(sqlstr).DefaultView;
                // DataTable dt = mvvmContext.GetViewModel<FyFormViewModel>().GetChild(opertime);
                e.ChildList = mvvmContext.GetViewModel<FyFormViewModel>().GetChild(opertime);
            };
            this.gridView1.MasterRowGetRelationName += (x, y) =>
            {
                y.RelationName = "处方明细";
            };
            this.gridView1.MasterRowGetLevelDefaultView += (x, y) =>
            {
                y.DefaultView = this.gridView2;
            };
            gridView1.IndicatorWidth = gridView3.IndicatorWidth = 25;
            this.gridView1.CustomDrawRowIndicator += (sender, e) =>
            {
                GridView view = (GridView)sender;
                //Check whether the indicator cell belongs to a data row
                if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                {
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
                    e.Info.Appearance.BackColor = Color.Azure;
                    e.Info.Appearance.ForeColor = Color.Silver;
                    e.Info.Appearance.Font = new Font("楷体", 12, FontStyle.Regular);

                }
            };
            this.gridView3.CustomDrawRowIndicator += (sender, e) =>
            {
                GridView view = (GridView)sender;
                //Check whether the indicator cell belongs to a data row
                if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                {
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
                    e.Info.Appearance.BackColor = Color.Silver;
                    e.Info.Appearance.ForeColor = Color.Silver;
                    e.Info.Appearance.Font = new Font("楷体", 12, FontStyle.Regular);

                }
            };
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            GridView view = (GridView)sender;
            //Check whether the indicator cell belongs to a data row
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
                e.Info.Appearance.ForeColor = Color.Blue;

            }

        }

      
       

        private void toggleSwitch1_Toggled(object sender, EventArgs e)
        {
            //ToggleSwitch ts = sender as ToggleSwitch;
            //if (ts.IsOn)
            //{
            //    dateEnd.Enabled = dateBegin.Enabled = labelControl1.Enabled = labelControl2.Enabled = true;
            //    comDoctor.Enabled = labelControl5.Enabled = false;
            //    FyFormViewModel ffvm = mvvmContext.GetViewModel<FyFormViewModel>();
            //    ffvm.SelectDate = string.Empty;
            //}
            //else
            //{
            //    dateEnd.Enabled = dateBegin.Enabled = labelControl1.Enabled = labelControl2.Enabled = false;
            //    comDoctor.Enabled = labelControl5.Enabled = true;
            //}
        }

       

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
          
        }

        private void tsAll_Properties_Toggled(object sender, EventArgs e)
        {
            ToggleSwitch ts = sender as ToggleSwitch;
            layoutControlItem1.Enabled = layoutControlItem2.Enabled = ts.IsOn;
            layoutControlItem4.Enabled = !ts.IsOn;
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }
    }
}
