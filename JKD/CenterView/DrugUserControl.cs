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
using DevExpress.Data.Filtering;
using DevExpress.Utils.Drawing;
using JKD.Service;
using JKD.Dialog;
namespace JKD.CenterView
{
    public partial class DrugUserControl : DevExpress.XtraEditors.XtraUserControl
    {
        public DrugUserControl()
        {
            InitializeComponent();

            List<string> sss = checkedComboBoxEdit1.Properties.Items.GetCheckedValues().ConvertAll(item => Convert.ToString(item));
            List<Drug> drugs = new DrugManager().GetAll().Where(item => sss.Contains(item.cate)).ToList(); ;
            this.pagerNavigator1.GC = this.gridControl1;
            this.pagerNavigator1.SetData(drugs);
            this.gridView1.RowClick += (s, e) =>
            {
                if (e.Clicks == 2)
                {
                   Drug drug = gridView1.GetRow(e.RowHandle) as Drug;
                    DialogFactory.CreateDrugEditControl(drug);
                }
            };
            
            #region 单击查询按钮
            btnQuery.Click += (s, e) =>
            {
                Drug drug = new Drug()
                {
                    name=buttonEdit1.Text,
                    address=textEdit2.Text,
                    form=textEdit3.Text
                };
                List<string> checkedValue = checkedComboBoxEdit1.Properties.Items.GetCheckedValues().ConvertAll(item => Convert.ToString(item));
                List<Drug> li = new DrugManager().GetList(drug).Where(item => checkedValue.Contains(item.cate)).ToList() ;
               // gridControl1.DataSource = li;
                this.pagerNavigator1.SetData(li);

            };
            #endregion
            #region 单击查询全部按钮
            btnQueryAll.Click += (s, e) =>
            {
                checkedComboBoxEdit1.CheckAll();
                List<Drug> li = new DrugManager().GetAll();
               // gridControl1.DataSource = li;
                this.pagerNavigator1.SetData(li);
            };
            #endregion
            #region 单击重置按钮
            btnReset.Click += (s, e) =>
            {
                buttonEdit1.Text = textEdit2.Text = textEdit3.Text = string.Empty;
            };
            #endregion

        }

        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            Drug drug = e.Row as Drug;
            int count = new DrugManager().Update(drug);
            if(count> 0)
            {
                MessageBox.Show("更新成功");
            }
        }

        private void groupControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FileDialog fd = new OpenFileDialog();
            fd.Filter = "excel files(*.xls) | *.xls";
            if (DialogResult.OK == fd.ShowDialog())
            {
               int count = new DataLoad().ImportDrug(fd.FileName);
                System.Windows.Forms.MessageBox.Show("更新数据：" + count.ToString() + "条","更新信息",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DialogFactory.CreateSearchControl(this.buttonEdit1);

        }
        
    }
}
