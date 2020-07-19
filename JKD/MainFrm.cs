using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using System.Xml;
using System.Diagnostics;
using JKD.CenterView;
using DevExpress.XtraBars.Docking2010.Views;
using JKD.Dialog;
using JKD.Models;
namespace JKD
{
    public partial class MainFrm : DevExpress.XtraEditors.XtraForm
    {
        public MainFrm()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            InitMenu();
            InitSatus();
        }
        public void InitMenu()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("config/MenuConfig.xml");
            XmlNode root = doc.SelectSingleNode("menus");
            foreach (XmlNode node in root.ChildNodes)
            {
               string title =  node.Attributes["name"].Value;
                BarSubItem bi = new BarSubItem();
                bi.Caption = title;
                if (!node.HasChildNodes)
                {
                    bi.ItemClick += (s, e) =>
                    {
                        DialogFactory.CreateConfigControl();
                    };
                }
                foreach (XmlNode child in node.ChildNodes)
                {
                    string title02 = child.InnerText;
                    BarButtonItem bii = new BarButtonItem();
                    bii.Caption = title02;
                    bii.ItemClick += (s, e) =>
                    {
                        Control cc = UserControlFactory.CreateInstance(title02);
                        cc.Text = title02;
                        documentManager1.View.AddOrActivateDocument(item => item.Caption==cc.Text, ()=>cc);
                    };
                    bi.AddItem(bii);
                }
                this.barManager1.MainMenu.AddItem(bi);
            }





        }
        public void InitSatus()
        {
            User u = AppDomain.CurrentDomain.GetData("user") as User;
            biUser.Caption = u.name;
            biUserType.Caption = u.isadmin == 0 ? "普通用户" : "超级用户";
            biLoginTime.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}