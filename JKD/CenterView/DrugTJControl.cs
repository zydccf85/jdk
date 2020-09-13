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
    }
}
