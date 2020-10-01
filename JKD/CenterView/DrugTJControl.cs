using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using JKD.DB;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views.Grid;
using System.Reflection;
using JKD.utils;
using JKD.Models;

namespace JKD.CenterView
{
    public partial class DrugTJControl : DevExpress.XtraEditors.XtraUserControl
    {
        public DrugTJControl()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            sbQuery.Click += (s, e) =>
            {
                SimpleButton sb = s as SimpleButton;
                DataTable dt = new DrugTJManager().Get(lueDrug.EditValue==null?"":lueDrug.Text, deBegin.Text + " 00:00:00", deEnd.Text + " 23:59:59");
                gridControl1.DataSource = dt;
            };
            sbRefresh.Click += (s, e) =>
            {
                DataTable dt01 = ExcelHelper.ImportExceltoDt(@"C:\Users\Administrator\Desktop\1.xls");
                DataTable dt02 = ExcelHelper.ImportExceltoDt(@"C:\Users\Administrator\Desktop\2.xls");
                DateTime? maxTime01 = new FyByPatientManager().GetMaxTime();
                maxTime01 = maxTime01 == null ? DateTime.MinValue : maxTime01;
                DateTime? maxTime02 = new FyByDrugManager().GetMaxTime();

                maxTime02 = maxTime02 == null ? DateTime.Now.AddYears(-10) : maxTime02;
                List<fybypatient> li01 = DataTableToModel.ToListModel<fybypatient>(dt01).Where(item => item.fytime > maxTime01).ToList();
                List<fybydrug> li02 = DataTableToModel.ToListModel<fybydrug>(dt02).Where(item => item.fytime > maxTime02).ToList();
                int count01 = new FyByPatientManager().Insert(li01);
                int count02 = new FyByDrugManager().Insert(li02);
                MessageBox.Show($"成功更新表数据为{count01},{count02}");
            };
            xtraTabControl1.CustomHeaderButtonClick += (s, e) =>
            {
                if(e.Button.Caption == "导出数据")
                {
                    gridView1.ExportToXlsx(@"C:\Users\Administrator\Desktop\yytj.xlsx");
                }
            };
            lueDrug.Properties.DataSource = SqlHelper.ExecuteTable("select distinct name 名称,searchcode 搜索码 from drug");
            lueDrug.EditValue = "";
            lueDrug.Properties.DisplayMember = "名称";
            lueDrug.Properties.ValueMember = "名称";
            


        }
    
            private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DrugTJControl_Load(object sender, EventArgs e)
        {
           
        }

        private void lueDrug_Properties_EditValueChanged(object sender, EventArgs e)
        {
            
            //lueDrug.Properties.DataSource = SqlHelper.ExecuteTable($"select distinct name 名称,searchcode 搜索码 from drug where searchcode like '%{lueDrug.Text}%' ");
            //lueDrug.Properties.DisplayMember = "名称";
            //lueDrug.Properties.ValueMember = "名称";
            //lueDrug.ShowPopup();
            //lueDrug.Hide();
            //if (!lueDrug.IsPopupOpen)
            //{
            //    lueDrug.ShowPopup();
            //    lueDrug.Focus();
               
                
            //}
        }

        private void xtraTabPage1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
