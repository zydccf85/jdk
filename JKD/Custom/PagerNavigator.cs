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
using System.Collections.Generic;
using System.Collections;
using DevExpress.XtraGrid;
using JKD.Models;

namespace JKD.Custom
{
    public partial class PagerNavigator: DevExpress.XtraEditors.XtraUserControl
    {
        public GridControl GC
        {
            get; set;
        }

        public int PageNo { get; set; } 
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int Pages { get; set; }
        public List<Drug> Datas { get; set; }
        public PagerNavigator()
        {
            
            InitializeComponent();
            
            Init();
            //GC = gc;
        }
        public void Init()
        {
            dataNavigator1.ButtonClick += (s, e) =>
            {
               if( e.Button.ButtonType == NavigatorButtonType.Custom)
                {
                    if (e.Button.Hint == "首页")
                    {
                        PageNo = 1;
                        js();

                    }else if(e.Button.Hint == "上一页")
                    {
                        PageNo -= 1;
                        js();
                    }
                    else if (e.Button.Hint == "下一页")
                    {
                        PageNo += 1;
                        js();
                    }
                    else if (e.Button.Hint == "末页")
                    {
                        PageNo = Pages;
                        js();
                    }
                }

            };
        }
        public void SetData(List<Drug> ds)
        {
            Datas = ds;
            PageNumber = Datas.Count();
            this.PageNo = 1;
            this.PageSize = 25;
            js();
        }
        private void js()
        {
           
            this.Pages = (int)(Math.Ceiling(PageNumber/ (double)this.PageSize));
            int skipCount = (this.PageNo - 1) * this.PageSize;
            GC.DataSource = this.Datas.Skip(skipCount).Take(this.PageSize).ToList();
            if (this.Datas.Count == 0)
            {
                this.dataNavigator1.TextStringFormat = "无记录";
                foreach (NavigatorCustomButton ncb in dataNavigator1.Buttons.CustomButtons)
                {
                    ncb.Visible = false;

                }

                 return;
            }
            else
            {
                foreach (NavigatorCustomButton ncb in dataNavigator1.Buttons.CustomButtons)
                {
                    ncb.Visible = true;

                }
            }
            this.dataNavigator1.TextStringFormat = string.Format(" {0:###}/{1:###}页，共{2:###}条，每页{3:N0}条", PageNo,Pages,PageNumber,PageSize);
            if(this.PageNo == 1)
            {
                dataNavigator1.Buttons.CustomButtons[0].Enabled = dataNavigator1.Buttons.CustomButtons[1].Enabled =  false;
            }else
            {
                dataNavigator1.Buttons.CustomButtons[0].Enabled = dataNavigator1.Buttons.CustomButtons[1].Enabled = true;
            }
            
            if (this.PageNo == this.Pages)
            {
                dataNavigator1.Buttons.CustomButtons[2].Enabled = dataNavigator1.Buttons.CustomButtons[3].Enabled = false;
            }
            else
            {
                dataNavigator1.Buttons.CustomButtons[2].Enabled = dataNavigator1.Buttons.CustomButtons[3].Enabled = true;
            }
        }
    }
}
