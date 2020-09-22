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
using JKD.Dialog;
using JKD.DB;
using JKD.Models;

namespace JKD.CenterView
{
    public partial class MyJKControl : DevExpress.XtraEditors.XtraUserControl
    {
        public Account SelectedAccount  ;
        public MyJKControl()
        {
            InitializeComponent();
            Init();
            
        }
        public void Init()
        {
            this.deBegin.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.deEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.sbQuery.Click += RefeshData;
            this.cbeJKR.Properties.Items.Add("所有");
            foreach(DataRow datarow in SqlHelper.ExecuteTable("select distinct jkr from account").Rows)
            {
                this.cbeJKR.Properties.Items.Add(datarow["jkr"]);
            }
            this.cbeJKR.SelectedIndex = 0;
            this.groupControl1.CustomButtonClick += (s, e) =>
            {
                if (e.Button.Properties.Caption == "新增") {
                    SelectedAccount = new Account();
                    DialogFactory.CreateJKDControl(SelectedAccount);
                }else if (e.Button.Properties.Caption == "查看")
                {
                    DialogFactory.CreateJKDControl(SelectedAccount);
                }else if (e.Button.Properties.Caption == "删除")
                {
                    int count = new AccountManager().Delete(SelectedAccount.id);
                    if (count > 0)
                    {
                        XtraMessageBox.Show($"Id为{SelectedAccount.id}的记录删除成功");
                    }
                }
                this.RefeshData(this, new EventArgs());
            };

        }
        public void RefeshData(Object sender,EventArgs e)
        {
            //string sqlstr = $"select * from acctitle a inner join account b on a.aid = b.accid and" +
            //    $" lrsj between '{deBegin.Text} 00:00:00' and '{deEnd.Text} 23:59:59' and jkr like '%{((cbeJKR.Text=="所有" ||string.IsNullOrEmpty(cbeJKR.Text))?"":cbeJKR.Text)}%'";
            //gridControl1.DataSource = SqlHelper.ExecuteTable(sqlstr);
            gridControl1.DataSource = new JkdManager().GetList(deBegin.Text + " 00:00:00", deEnd.Text + " 23:59:59", cbeJKR.Text);

        }
        

        private void gridView1_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            //获得选中的行，如果是单选模式，则直接取第一个
            int selectRow = gridView1.GetSelectedRows()[0];
            //获得绑定的行数据
            //  DataRow dataRow = gridView1.GetDataRow(selectRow);
            Jkd myjkd= gridView1.GetRow(selectRow)  as Jkd;
            SelectedAccount = new Account()
            {
                id = myjkd.Id,
                accid   =myjkd.Accid,
                dysj=myjkd.Dysj,
                jkrq=myjkd.Jkrq,
                je=myjkd.Je,
                kxly=myjkd.Kxly,
                lrsj=myjkd.Lrsj,
                jkr=myjkd.Jkr,
                isdelete=myjkd.Isdelete,
                bz=myjkd.Bz

            };
            

        }
    }
}
