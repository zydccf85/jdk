using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace JKD.CenterView
{
    public class CustomeTabControl : XtraTabControl
    {
        public CustomeTabControl()
        {
            InitConfig();
        }
        private void InitConfig()
        {
            ClosePageButtonShowMode = ClosePageButtonShowMode.InActiveTabPageAndTabControlHeader;

            BackColor = System.Drawing.Color.AliceBlue;
            Dock = System.Windows.Forms.DockStyle.Fill;
            this.AppearancePage.HeaderActive.BackColor = System.Drawing.Color.Azure;
            AppearancePage.HeaderActive.Font = new Font("宋体",12.0f, FontStyle.Bold);
            AppearancePage.HeaderActive.ForeColor = Color.DarkBlue;
            AppearancePage.HeaderActive.BackColor=Color.Azure;

            Visible = false;
            CloseButtonClick += (sender, e) =>
            {
                XtraTabControl xtc = sender as XtraTabControl;
                XtraTabPage page = xtc.SelectedTabPage;
                xtc.TabPages.Remove(page);
                if (xtc.TabPages.Count == 0)
                {
                    Visible = false;
                }
            };
            HeaderButtonsShowMode = TabButtonShowMode.Always;
        }
        public void CreateTabPage(string text)
        {
            bool isExist = TabPages.Select(item => item.Text).Contains(text);
            if (isExist)
            {
                this.SelectedTabPage = TabPages.Where(item => item.Text == text).First();
            }
            else
            {
                XtraTabPage page = new XtraTabPage();
                page.Text = text;
                page.Controls.Add(UserControlFactory.CreateInstance(text));
                TabPages.Add(page);
                this.SelectedTabPage = page;
            }
            

        }
    }
}