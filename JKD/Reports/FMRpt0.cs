using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using JKD.ViewModels;
using System.Collections.Generic;
using System.Linq;
using JKD.Models;
namespace JKD.Reports
{
    public partial class FMRpt0 : DevExpress.XtraReports.UI.XtraReport
    {
        private HuiZong HZ;
        public FMRpt0()
        {
            InitializeComponent();
        }
        //public FMRpt0(HuiZong Hz)
        //{

        //    this.HZ = Hz;
        //    InitializeComponent();
        //    this.lblId.Text = "PrintTime:" + DateTime.Now.ToString("yyyyMMddHHmmss");
        //    string maxTime = HZ.Riqi.Split('至')[1];
        //    //string minTime = cfhead.Select(it => it.opertime).Min();
        //    lbldate.Text = string.Format("统计时间段:({0})", HZ.Riqi);
        //    int d = Convert.ToInt32(DateTime.Parse(maxTime.Split(' ')[0]).Subtract(new DateTime(2020, 1, 1)).TotalDays) % 4;
        //    string[] names = { "陈春峰", "潘小琴", "胡燕", "蔡晓星" };
        //    this.lblsignature.Text = names[d];
        //    lblPrintTime.Text = "[" + DateTime.Parse(maxTime).ToString("yyyy-MM-dd dddd") + "]";
        //    lblTotalPrice.Text = string.Format("{0:C2}", HZ.TotalPrice);
        //    lblMaxPrice.Text = string.Format("{0:C2}", HZ.MaxPrice);
        //    lblMinPrice.Text = string.Format("{0:C2}", HZ.MinPrice);
        //    lblAvgPrice.Text = string.Format("{0:C2}", HZ.AvgPrice);
        //    lblCFcount.Text = HZ.CfCount.ToString(); ;
        //    lblYBcount.Text = HZ.YbCount.ToString();
        //    lblZFcount.Text = HZ.ZfCount.ToString();
        //    lblRenCiCount.Text = HZ.MzCount.ToString();
        //    lblEKcount.Text = HZ.EkCount.ToString();
        //    lblZCYcount.Text = HZ.ZcyCount.ToString();
        //    lblJEcount.Text = HZ.JeCount.ToString();
        //    lblJDcount.Text = HZ.JdCount.ToString();
        //    lblJScount.Text = HZ.JsCount.ToString();
        //    lbJzcount.Text = HZ.JzCount.ToString();
        //    lblKjycount.Text = HZ.KjyCount.ToString();
        //}
        
        public void InitData()
        {
           this.lblMaxPrice.DataBindings.Add("Text", this.DataSource, "MaxPrice");
           // this.lblMaxPrice.Text = (DataSource as HuiZong).MaxPrice.ToString();
        }
        private string[] GetString(string str)
        {
            return str.Split(new char[] { '：' });
        }

    }
}
