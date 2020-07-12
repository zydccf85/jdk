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
using JKD.Models;
using System.Collections.Generic;
using DevExpress.XtraGrid.Columns;
using System.Linq;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using JKD.DB;
namespace JKD
{
    public partial class TableControl : DevExpress.XtraEditors.XtraUserControl
    {
        public TableControl(Cfhead dr)
        {
            InitializeComponent();
            List<Cfhead> lst;
            if (string.IsNullOrEmpty(dr.did))
            {
                lst = new CfheadManager().GetListByPatientAndAddress(dr.patient, dr.address);
            }
            else
            {
                lst = new CfheadManager().GetListByDid(dr.did);

            }
            this.gridControl1.DataSource = lst;
            Cfhead first = lst.First();
            txtPatient.Text = first.patient;
            txtAge.Text = first.age;
           
            txtPhone.Text = first.phone.Replace(first.phone.Substring(4,4),"****");
            txtId.Text =string.IsNullOrEmpty(first.did)? "无": first.did.Replace(first.did.Substring(first.did.Length-4,4),"****");
            txtAddress.Text = first.address;
            int tianShu = lst.Select(item => item.pid).Distinct().Count();
            int ciShu = lst.Count();
            double? price = lst.Select(item => item.totalprice).Sum();
            lblXJ.Text = string.Format("处方数量:{0:N0}张，就诊天数:{1:N0}天，总金额:{2:N2}元",ciShu,tianShu,price);
            txtAddress.Size = new Size(400, 20);
            lblXJ.Size = new Size(400, 20);
            this.tableLayoutPanel1.SetColumnSpan(txtAddress, 2);
            this.tableLayoutPanel1.SetColumnSpan(lblXJ, 3);
            this.gridView1.OptionsView.ColumnAutoWidth = true;
            this.gridView1.FocusedRowChanged += ShowDetail;
            int index = lst.FindIndex(item => item.opertime == dr.opertime);
            System.Diagnostics.Debug.WriteLine(index.ToString());
            if (index == 0)
            {
                LoadDetail(0);
            }
            else
            {
                gridView1.MoveBy(index);
            }
        }

        private void ShowDetail(object sender, FocusedRowChangedEventArgs e)
        {
            LoadDetail(e.FocusedRowHandle);
            
        }
        private void LoadDetail(int i)
        {
            string opertime = (this.gridView1.GetRow(i) as Cfhead).opertime;
            if (!string.IsNullOrEmpty(opertime))
            {
                this.gridControl2.DataSource = new DbContext().cfdetailDb.GetList(item => item.opertime == opertime)
                    .Where(item => item.enable == 1).OrderBy(iss=>iss.gid).ToList();
                //Dictionary<string, string> dic = Cfdetail.NameMap;
                //string[] isHidden = { "id", "enable", "opertime" };
                //foreach (GridColumn c in this.gridView2.Columns)
                //{
                //    c.Caption = dic[c.FieldName];
                //    if (isHidden.Contains(c.FieldName))
                //    {
                //        c.Visible = false;
                //    }
                //};
                //gridView2.Columns["gid"].Width = 30;
                //gridView2.Columns["drug"].Width = 150;
                //gridView2.Columns["spci"].Width = 150;
            }
        }
        private void TableControl_Load(object sender, EventArgs e)
        {
          
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
