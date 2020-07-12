using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Linq;
using DevExpress.XtraTab;
using JKD.CenterView;
using DevExpress.XtraSplashScreen;
using System.Threading;

namespace JKD
{
    public partial class NaviFrm : DevExpress.XtraEditors.XtraForm
    {
        public CustomeTabControl CTC = new CustomeTabControl();
        public NaviFrm()
        {
          

            InitializeComponent();
            this.Load += (o,e)=>{
                SplashScreenManager.CloseForm();
            };
            panel1.Controls.Add(CTC);
            foreach (ToolStripMenuItem menu in menuStrip1.Items)
            {
                foreach(ToolStripItem item in menu.DropDownItems)
                {
                    item.Click += (s, e) =>
                    {
                        SplashScreenManager.ShowForm(typeof(WaitForm1));
                        CTC.Visible = true;
                        ToolStripItem tsi = s as ToolStripMenuItem;
                        CTC.CreateTabPage(tsi.Text);
                        SplashScreenManager.CloseForm();


                    };
                }
            }
            InitStatusTool();

        }
        private void InitStatusTool()
        {
            tsslLoginTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void xtraTabControl1_CustomHeaderButtonClick(object sender, DevExpress.XtraTab.ViewInfo.CustomHeaderButtonEventArgs e)
        {
            XtraTabControl xtc = sender as XtraTabControl;
            XtraTabPage page = e.ActivePage as XtraTabPage;
            
            xtc.TabPages.Remove(page);
        }

        private void xtraTabPage1_MouseHover(object sender, EventArgs e)
        {
           // xtp.ImageOptions.Image = new Bitmap(@"C:\Users\Public\Pictures\Sample Pictures\沙漠.jpg");
        }

        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            MouseEventArgs mea = e as MouseEventArgs;
            XtraTabControl xtc = sender as XtraTabControl;
            XtraTabPage page = xtc.SelectedTabPage;
                  xtc.TabPages.Remove(page);
            MessageBox.Show(sender.GetType().ToString());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}