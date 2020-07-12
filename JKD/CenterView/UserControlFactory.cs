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
        //public static Dictionary<string, Control> dic = new Dictionary<string, Control>()
        //{
        //    {"处方明细",new ChufangControl() },
        //    {"缴款记录",new  JkControl() },
        //    { "药品明细",new DrugUserControl()}
        //};
        public static Control CreateInstance(string name)
        {
            //Control con = null;
            //if( dic.TryGetValue(name,out con))
            //{
            //    con.Dock = DockStyle.Fill;
            //    return con;
            //}
            //return null;
            Control con = null;

            switch (name)
            {
                case "处方明细":
                    con = new ChufangControl();
                    break;
                case "缴款记录":
                    con = new JkControl();
                    break;
                case "药品明细":
                    con = new DrugUserControl();
                    break;
                case "处方统计":
                    con = new CftongjiControl1();
                    break;

            }
            if (con != null)
            {
                con.Dock = DockStyle.Fill;
            }
            return con;

        }
    }
    
}
