using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using JKD.Models;
using JKD.DB;
using System.IO;
using JKD.utils;
using MySql.Data.MySqlClient;

namespace JKD.CenterView
{
    public partial class EmployeeControl : DevExpress.XtraEditors.XtraUserControl
    {
        public List<Employee> li = new EmployeeManager().GetList();
        public EmployeeControl()
        {
            li = new EmployeeManager().GetList();
            InitializeComponent();
            gridControl1.DataSource = li;
            pictureBox1.Image = Image.FromFile(@"C:\Users\Administrator\Desktop\0A9A6992.JPG");
          
        }

        private void textEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FileDialog fd = new OpenFileDialog();
            DialogResult ds = fd.ShowDialog();
            if(ds == DialogResult.OK)
            {
                ImageConvert ic = new ImageConvert();
                System.Windows.Forms.MessageBox.Show(fd.FileName);
                byte[] bbs= ic.GetPictureData(fd.FileName);
                FileStream fs = new FileStream(fd.FileName, FileMode.Open);
                MySqlParameter p1 = new MySqlParameter("@pic", MySqlDbType.LongBlob);
                p1.Value = bbs;
                int cont = SqlHelper.ExecuteNoQuery("update employee set pic = @pic where id =3",p1);
                System.Windows.Forms.MessageBox.Show(cont.ToString());

            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }
    }
}
