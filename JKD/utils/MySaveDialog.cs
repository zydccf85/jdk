using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JKD.utils
{
    public class MySaveFileDialog
    {
        public static string Show()
        {
            XtraSaveFileDialog xsfd = new XtraSaveFileDialog();
            xsfd.Filter = "Excel文件(*.xlsx)|.xlsx";
            DialogResult dr = xsfd.ShowDialog();
            if(dr == DialogResult.OK)
            {
                return xsfd.FileName;
            }
            return null;
        }
    }
}
