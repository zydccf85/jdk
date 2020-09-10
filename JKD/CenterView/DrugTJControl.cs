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
                System.Diagnostics.Debug.WriteLine(deBegin.Text);
                DataTable dt = new DrugTJManager().Get(lueDrug.Text, deBegin.Text + " 00:00:00", deEnd.Text + " 23:59:59");
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

      
    }
}
