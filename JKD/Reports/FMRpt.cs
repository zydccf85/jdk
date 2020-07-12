using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using JKD.ViewModels;
using System.Timers;
namespace JKD.Reports
{
    public partial class FMRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private FyFormViewModel ffvm;
        public FMRpt(FyFormViewModel ffvm)
        {
            this.ffvm = ffvm;
            InitializeComponent();
             this.TopMargin.HeightF = 10.0f;
            lbldate.Text = "（"+DateTime.Now.ToString("yyyy-MM-dd")+")";
            this.lblTotalPrice.Text = ffvm.Totalprice;
            this.lblcfcount.Text = ffvm.Cfcount;
            lblMzcount.Text = ffvm.Mzcount;
            lblZcycount.Text = ffvm.Zcycount;
            lblJdcount.Text = ffvm.Jdcount;
            lblEkcount.Text = ffvm.Ekcount;
            lblJecount.Text = ffvm.Jeycount;
            lblJscount.Text = ffvm.Jscount;
            this.xrTable1.Rows[0].Cells[0].Text = "ccf";
        }

    }
}
