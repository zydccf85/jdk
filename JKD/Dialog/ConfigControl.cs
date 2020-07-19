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
using JKD.utils;
using JKD.DB;
using MySql.Data.MySqlClient;

namespace JKD.Dialog
{
    public partial class ConfigControl : DevExpress.XtraEditors.XtraUserControl
    {
        public ConfigControl()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            buttonEdit1.Text = AppconfigHelper.GetAppConfig("importXmlPath");
            buttonEdit2.Text = AppconfigHelper.GetAppConfig("importExcelPath");
            buttonEdit1.Click += (s, e) =>
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog()
                {
                    Description = "选择文件夹"
                };
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    buttonEdit1.Text = fbd.SelectedPath;
                    AppconfigHelper.UpdateAppConfig("importXmlPath", fbd.SelectedPath);
                }
            };
            InitDb();
        }
        public void InitDb()
        {
            List<string> li = new DbContext().Db.SqlQueryable<Object>("select schema_name from information_schema.schemata").Select<string>().ToList();
            cbeDb.Properties.Items.AddRange(li);
            cbeDb.SelectedItem = "wsy";
            string conn = AppconfigHelper.GetConnectionStringsConfig("mysqlConnectionStr");
            // server = localhost; uid = root; pwd = 541800; database = wsy;
            this.teServer.Text = conn.Split(';')[0].Split('=')[1].Trim();
            this.teUser.Text = conn.Split(';')[1].Split('=')[1].Trim();
            this.tePsw.Text = conn.Split(';')[2].Split('=')[1].Trim();
        }
        public void Reset()
        {
            AppconfigHelper.UpdateAppConfig("importXmlPath", this.buttonEdit1.Text);
            AppconfigHelper.UpdateAppConfig("importExcelPath", this.buttonEdit2.Text);
            
            
            AppconfigHelper.UpdateConnectionStringsConfig("mysqlConnectionStr",getConnectStr());
        }
        private String getConnectStr()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("server=" + teServer.Text + ";");
            sb.Append("uid=" + teUser.Text + ";");
            sb.Append("pwd=" + tePsw.Text + ";");
            sb.Append("database=" + cbeDb.Text + ";");
            return sb.ToString();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(getConnectStr());
            try
            {
                conn.Open();
                XtraMessageBox.Show("连接成功","信息提示",MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("连接失败", "信息提示", MessageBoxButtons.OK);
            }
            finally
            {
                conn.Close();
            }
            
          

        }
    }
}
