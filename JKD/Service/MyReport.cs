using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.XtraSplashScreen;
using JKD.Reports;

namespace JKD.Service
{
    public class MyReport
    {
        public XtraReport xr;
      //  public XRDesignFormEx xrd = new XRDesignFormEx();
        public MyReport(XtraReport xr,string filePath,bool flag)
        {
            SplashScreenManager.ShowForm(typeof(WaitForm1));
            string root = @"C:/jdk/reports";
            if (File.Exists(Path.Combine(root,filePath)))
            {
                object ds = xr.DataSource;
                xr = new XtraReport();
                xr.LoadLayout(Path.Combine(root, filePath));
                xr.DataSource = ds;
                
            }
            else
            {
               // xr = new FMRpt0();
                
            }
           // xr.DataSource = datasource;
            SplashScreenManager.CloseForm();
            if (flag== false)
            {
                xr.ShowPreviewDialog();
                return;
            }
           
            XRDesignFormEx xrd = new XRDesignFormEx();
            xrd.FormClosing+= (s, e) =>
            {
                if(!Directory.Exists(root)){
                    Directory.CreateDirectory(root);
                 }
                xr.SaveLayout(Path.Combine(root, filePath));

            };
            xrd.ReportStateChanged += (s, e) =>
            {
                if (e.ReportState == ReportState.Changed)
                {
                    ((XRDesignFormEx)s).DesignPanel.ReportState = ReportState.Saved;
                }
                
            };
            xrd.OpenReport(xr);
            

            xrd.ShowDialog();
            xrd.Dispose();
        }



    }
}
