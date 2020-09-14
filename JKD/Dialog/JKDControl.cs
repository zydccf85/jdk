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

namespace JKD.Dialog
{
    public partial class JKDControl1 : DevExpress.XtraEditors.XtraUserControl
    {
        public DataTable dt = SqlHelper.ExecuteTable("select aid 编号,hm 缴款单位,zh 帐号,yh 银行名称 from acctitle");
        public JKDControl1()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            this.lueZH.Properties.DataSource = dt;
            this.lueZH.Properties.ValueMember = "编号";
            this.lueZH.Properties.DisplayMember = "编号";
            foreach(DataRow mydr in SqlHelper.ExecuteTable("select distinct kxly from account ").Rows)
            {
                this.teLY.Properties.Items.Add(mydr["kxly"]);
            }
            foreach (DataRow mydr in SqlHelper.ExecuteTable("select distinct jkr from account ").Rows)
            {
                this.teJKR.Properties.Items.Add(mydr["jkr"]);
            }

        }

        private void lueZH_EditValueChanged(object sender, EventArgs e)
        {
            DataRow dr = dt.Select("编号 = " + this.lueZH.Text)[0];
            this.teDW.Text = dr["缴款单位"].ToString();
            this.teZH.Text = dr["帐号"].ToString();
            this.teMC.Text = dr["银行名称"].ToString();
            this.teBZ.Text = "人民币";
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
                string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string sqlstr = $"insert into account(accid,kxly,bz,lrsj,jkr,je) values({lueZH.Text},'{teLY.Text}','{teBZ.Text}','{nowtime}','{teJKR.Text}',{teJEXX.EditValue.ToString()})";
               int coun = SqlHelper.ExecuteNoQuery(sqlstr);
                if (coun > 0)
                {
                    XtraMessageBox.Show("数据更新成功", "信息提示框", MessageBoxButtons.OK);
                    teID.Text = SqlHelper.ExecuteScaler("select max(id) from account").ToString();
                    sbPrint.Enabled = true;
                    simpleButton1.Enabled = false;
                    foreach(LayoutControlItem c in this.layoutControlGroup3.Items)
                    {
                        c.Enabled = false;
                        
                    }


                }

            }
            else
            {
                XtraMessageBox.Show("带*为必填项,请输入完整!", "信息提示框", MessageBoxButtons.OK);
            }
        }
    }
}
