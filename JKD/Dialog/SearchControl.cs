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
using JKD.Models;
using System.Diagnostics;
namespace JKD.Dialog
{
    public partial class SearchControl : DevExpress.XtraEditors.XtraUserControl
    {
        public DrugManager dm = new DrugManager();
        public List<Drug> drugs;
        public ButtonEdit BE;
        public SearchControl(ButtonEdit be)
        {
            BE = be;
            InitializeComponent();
            InitData();
            teDrug.TextChanged += (s, e) =>
            {
                TextEdit te = s as TextEdit;
                List<Drug> drugs = FreshData(te.Text);
                gridControl1.DataSource = drugs;
                bhiCount.Caption = drugs.Count.ToString();
            };
            teDrug.PreviewKeyDown += (s, e) =>
            {
                if(e.KeyCode == Keys.Down)
                {
                    gridView1.Focus();
                    gridView1.SelectRow(0);
                }
            };
            this.gridView1.FocusedRowObjectChanged += (s, e) =>
            {
                Drug dd = gridView1.GetRow(e.FocusedRowHandle) as Drug;
                BE.EditValue = dd.name;
                
            };
            
                      
        }
        public void InitData()
        {
            List<Drug> drugs = FreshData(teDrug.Text);
            gridControl1.DataSource = drugs;
            bhiCount.Caption = drugs.Count.ToString();
        }
        public List<Drug> FreshData(string str)
        {
            if (comboBoxEdit1.Text == "按搜索码")
            {
                return dm.GetListBySearchcode(str);
            }else if (comboBoxEdit1.Text == "按药品名称")
            {
                return dm.GetListByName(str);
            }
            return  null;
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit cbm = sender as ComboBoxEdit;
            layoutControlItem1.Text = cbm.Text+":";
        }
    }
}
