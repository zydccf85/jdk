using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JKD.CenterView
{
    public class UserControlFactory
    {
        
        public static Control CreateInstance(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            Control con = null;
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                {"处方明细","ChufangControl" }, {"缴款记录","JkControl" }, {"药品明细","DrugUserControl" },
                 {"处方统计","CftongjiControl" }, {"药品使用情况统计","DrugTJControl" }, {"人员明细","EmployeeControl" },
                {"我的缴款","MyJKControl" }
            };
            Type t = Type.GetType("JKD.CenterView." + dic[name]);
            con = Activator.CreateInstance(t) as Control;
            //switch (name)
            //{
            //    case "处方明细":
            //        con = new ChufangControl();
            //        break;
            //    case "缴款记录":
            //        con = new JkControl();
            //        break;
            //    case "药品明细":
            //        con = new DrugUserControl();
            //        break;
            //    case "处方统计":
            //        con = new CftongjiControl();
            //        break;
            //    case "药品使用情况统计":
            //        con = new DrugTJControl();
            //        break;
            //    case "人员明细":
            //        con = new EmployeeControl();
            //        break;
            //    case "我的缴款":
            //        con = new MyJKControl();
            //        break;

            //}
            if (con != null)
            {
                con.Dock = DockStyle.Fill;
            }
            return con;

        }
    }
    
}
