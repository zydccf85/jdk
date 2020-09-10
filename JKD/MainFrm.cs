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
            //XmlDocument doc = new XmlDocument();
            //doc.Load("config/MenuConfig.xml");
            //XmlNode root = doc.SelectSingleNode("menus");
            //foreach (XmlNode node in root.ChildNodes)
            //{
            //   string title =  node.Attributes["name"].Value;
            //    BarSubItem bi = new BarSubItem();
            //    bi.Caption = title;
            //    if (!node.HasChildNodes)
            //    {
            //        bi.ItemClick += (s, e) =>
            //        {
            //            DialogFactory.CreateConfigControl();
            //        };
            //    }
            //    foreach (XmlNode child in node.ChildNodes)
            //    {
            //        string title02 = child.InnerText;
            //        BarButtonItem bii = new BarButtonItem();
            //        bii.Caption = title02;
            //        bii.ItemClick += (s, e) =>
            //        {
            //            Control cc = UserControlFactory.CreateInstance(title02);
            //            cc.Text = title02;
            //            documentManager1.View.AddOrActivateDocument(item => item.Caption==cc.Text, ()=>cc);
            //        };
            //        bi.AddItem(bii);
            //    }
            //    this.barManager1.MainMenu.AddItem(bi);
               
                
            //}

            DataTable dt = SqlHelper.ExecuteTable("select * from menu");
            User u = AppDomain.CurrentDomain.GetData("user") as User;
            foreach (DataRow item in dt.Select("upid is null and admin ="+u.isadmin))
            {
                BarSubItem bi = new BarSubItem();
                bi.Caption = item.Field<string>("title");
                if(dt.Select("upid =" + item.Field<int>("id")).Length == 0)
                {
                    bi.ItemClick += (s, e) =>
                    {
                        DialogFactory.CreateConfigControl();
                    };
                }
                foreach (DataRow item02 in dt.Select("upid ="+item.Field<int>("id")))
                {
                    BarButtonItem bii = new BarButtonItem();
                    bii.Caption = item02.Field<string>("title");
                    bii.ItemClick += (s, e) =>
                    {
                        Control cc = UserControlFactory.CreateInstance(bii.Caption);
                        cc.Text = bii.Caption;
                        documentManager1.View.AddOrActivateDocument(ite => ite.Caption == cc.Text, () => cc);
                    };
                    bi.AddItem(bii);
                }
                barManager1.MainMenu.AddItem(bi);
                //System.Diagnostics.Debug.WriteLine(item.Field<string>("title"));
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