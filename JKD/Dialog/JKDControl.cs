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
using JKD.utils;
using DevExpress.XtraLayout;
using JKD.Service;
using DevExpress.XtraReports.UI;
using JKD.Reports;
using JKD.Models;
using JKD.DB;

namespace JKD.Dialog
{
    public partial class JKDControl1 : DevExpress.XtraEditors.XtraUserControl
    {
        public DataTable dt = SqlHelper.ExecuteTable("select aid 编号,hm 缴款单位,zh 帐号,yh 银行名称 from acctitle");
        public Account myaccount;
        public JKDControl1()
        {
            InitializeComponent();
            //  Init();
        }
        public JKDControl1(Account account)
        {
            InitializeComponent();
            myaccount = account;
            this.jkdBindingSource.DataSource = account;
            Init();

        }
        public void Init()
        {
            this.lueZH.Properties.DataSource = dt;
            this.lueZH.Properties.ValueMember = "编号";
            this.lueZH.Properties.DisplayMember = "编号";
           // lueZH.EditValue =Convert.ToInt32( dt.Rows[0][0].ToString().Trim());
            foreach(DataRow mydr in SqlHelper.ExecuteTable("select distinct kxly from account ").Rows)
            {
                this.teLY.Properties.Items.Add(mydr["kxly"]);
            }
            teLY.SelectedItem = "业务收入";
            foreach (DataRow mydr in SqlHelper.ExecuteTable("select distinct jkr from account ").Rows)
            {
                this.teJKR.Properties.Items.Add(mydr["jkr"]);
            }
            //单击头部自定义按钮
            groupControl1.CustomButtonClick += (s, e) =>
            {
                string caption = e.Button.Properties.Caption;
                switch (caption)
                {
                    case "设计报表":
                        sbPrint_Click(true);
                        break;
                    case "打印":
                        sbPrint_Click(false);
                        break;
                    case "保存":
                        simpleButton1_Click(this, new EventArgs());
                        break;
                    default:
                        break;
                }
            };
        }
      

        private void lueZH_EditValueChanged(object sender, EventArgs e)
        {
            DataRow dr = dt.Select("编号 = " + this.lueZH.Text)[0];
            this.teDW.Text = dr["缴款单位"].ToString();
            this.teZH.Text = dr["帐号"].ToString();
            this.teMC.Text = dr["银行名称"].ToString();
            myaccount.bz= "人民币";
        }

        private void teJEXX_Properties_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            
           
            
        }

        private void teJEXX_Properties_EditValueChanged(object sender, EventArgs e)
        {
            if (this.teJEXX.EditValue != null && !string.IsNullOrEmpty(this.teJEXX.EditValue.ToString()))
            {
                System.Diagnostics.Debug.WriteLine(Convert.ToString(this.teJEXX.EditValue));
                this.teJEDX.Text = new MoneyConvertChinese().MoneyToChinese(Convert.ToString(this.teJEXX.EditValue));
            }
        }
        //单击保存
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string[] strs = { lueZH.Text,teJKR.Text,teLY.Text,teJEXX.Text};
            
            if (strs.ToList().TrueForAll(item => !string.IsNullOrEmpty(item)))
            {
                myaccount.lrsj = DateTime.Now;
                if(myaccount.id != null)
                {
                    int count = new AccountManager().Update(myaccount);
                    if (count > 0)
                    {
                        XtraMessageBox.Show("数据更新成功", "信息提示框", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    int myid = new AccountManager().Save(myaccount);
                    if (myid > 0)
                    {
                        XtraMessageBox.Show("数据添加成功", "信息提示框", MessageBoxButtons.OK);
                        teID.Text = myid.ToString();
                    }
                }
               

            }
            else
            {
                XtraMessageBox.Show("带*为必填项,请输入完整!", "信息提示框", MessageBoxButtons.OK);
            }
        }

        private void sbPrint_Click(bool flag)
        {
            myaccount.dysj = DateTime.Now;
            new AccountManager().Update(myaccount);
            XtraReport xr = new JdkRep();
            List<Jkd> myjkd = new JkdManager().GetList(int.Parse(teID.Text.Trim()));
            MoneyConvertChinese mcc = new MoneyConvertChinese();
            myjkd.ForEach(item => item.JeDX = mcc.MoneyToChinese(item.Je.ToString()));
            xr.DataSource = myjkd;
            new MyReport(xr, "JdkRep.repx", flag);
        }

        private void teID_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void jkdBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void teZH_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
