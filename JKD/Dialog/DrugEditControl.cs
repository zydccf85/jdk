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
using JKD.utils;

namespace JKD.Dialog
{
    public partial class DrugEditControl : DevExpress.XtraEditors.XtraUserControl
    {
        public Drug drug;
        public DrugEditControl( Drug d)
        {
            drug = d;
            InitializeComponent();
            InitData();
        }
        public void InitData()
        {
            tdId.Text = drug.id.ToString();
            teCode.Text = drug.code;
            teName.Text = drug.name;
            teForm.Text = drug.form;
            teAddress.Text = drug.address;
            teSpci.Text = drug.spci;
            teUnit.Text = drug.unit;
            teUnitprice.Text = drug.unitprice.ToString();
            teSearchcode.Text = drug.searchcode;
            comboBoxEdit1.Text = drug.cate;
            DataTable dt = SqlHelper.ExecuteTable("select name from drugcate order by id ");
            foreach (DataRow dr in dt.Rows)
            {
                comboBoxEdit1.Properties.Items.Add(dr.Field<string>("name"));
            }
            DataTable dt02 = SqlHelper.ExecuteTable("select name from drugcata order by id");
            foreach (DataRow dr in dt02.Rows)
            {
                comboBoxEdit2.Properties.Items.Add(dr.Field<string>("name"));
            }
            comboBoxEdit2.Text = drug.cata;

        }
        //根据药品名称，自动生成搜索码
        private void btnAutoGenarator_Click(object sender, EventArgs e)
        {
            string pi = new PinyinHelper().GetFirstLetter(teName.Text);
            if (teSearchcode.Text == pi) return;
           DialogResult dr =  XtraMessageBox.Show("是否覆盖原搜索码？", "信息提示", MessageBoxButtons.OKCancel);
            if (DialogResult.OK == dr)
            {
                teSearchcode.Text = pi;
            }
        }

        //得到修改后的drug
        public Drug GetDrug()
        {
            drug.cate = comboBoxEdit1.Text;
            drug.cata = comboBoxEdit2.Text;
            drug.searchcode = teSearchcode.Text;
            return drug;

        }
    }
}
