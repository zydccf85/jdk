using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using JKD.Service;
using JKD.CenterView;
using JKD.DB;
using DevExpress.XtraEditors;
using JKD.Models;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JKD.ViewModels
{
    [POCOViewModel]
    public class LoginFrmViewModel
    {
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public LoginService ls = new LoginService();
        public virtual string RName { get; set; }
        public virtual string RUsername { get; set; }
        public virtual string RPassword { get; set; }
        public virtual string RRepeatPassword { get; set; }
        public UserManager um = new UserManager();
        public virtual List<string> Usernames { get; set; } =new  List<string>();
        public LoginFrmViewModel()
        {
            ReadUsername();
        }
        #region 登录
        public void handleOk(LoginFrm form)
        {
            LoginStatus status = ls.login(Username, Password);
            switch (status)
            {
                case LoginStatus.Success:
                    form.Hide();
                    IninConfig();
                    MainFrm mf = new MainFrm();
                    mf.FormClosed += (s, e) =>
                    {
                        form.Close();
                    };
                    mf.Show();
                    Usernames.Add(Username);
                    Usernames = Usernames.Distinct<string>().ToList();
                    WriteUsername();
                    break;
                case LoginStatus.UsernameErr:
                    XtraMessageBox.Show("用户名错误，请重新输入", "信息提示");
                    form.SelectAll("teUsername");
                    break;
                case LoginStatus.PasswordErr:
                    XtraMessageBox.Show("密码错误，请重新输入", "信息提示");
                    form.SelectAll("tePsw");
                    break;
                case LoginStatus.ConnectionErr:
                    XtraMessageBox.Show("不能连接数据库，请查看原因", "信息提示");
                    break;
                default:
                    break;
            }

        }
        #endregion

        #region 注册
         public void handleRegister()
        {
            if(RPassword != RRepeatPassword)
            {
                XtraMessageBox.Show("确认密码不一致，请重新输入", "信息提示");
                return;
            }
             RegisterStatus rs =ls.Register(RName,RUsername);
            switch (rs)
            {
                case RegisterStatus.NameDuplicate:
                    XtraMessageBox.Show("真实姓名已有，请重新输入", "信息提示");
                    break;
                case RegisterStatus.UsernameDuplicate:
                    XtraMessageBox.Show("用户名已有，请重新输入", "信息提示");
                    break;
                case RegisterStatus.Nullerr:
                    XtraMessageBox.Show("真实姓名或用户名不能为空，请重新输入", "信息提示");
                    break;
                case RegisterStatus.Success:
                    User u = new User()
                    {
                        name = RName,
                        username = RUsername,
                        psw=RPassword
                    };
                    if (um.Insert(u) > 0)
                    {
                        XtraMessageBox.Show("注册成功", "信息提示");
                        handleReset();
                    }
                    break;
                default:
                    break;
            }

        }
        #endregion
        #region 重置
        public void handleReset()
        {
            RName = RUsername = RPassword = RRepeatPassword = string.Empty;
        }
        #endregion

        #region 初始化SplashScreen
        private void IninConfig()
        {
            SplashScreenManager.ShowForm(typeof(SplashScreen1));
            string maxTime = new CfheadManager().GetMaxTime();
            SplashScreenManager.Default.SendCommand(null, string.Format("最新更新时间为{0},1", maxTime));

            string maxDate = maxTime.Split(' ')[0].Replace("-", "").Trim();
            new ImportData().AutoImport();


            SplashScreenManager.Default.SendCommand(null, "内容面板预加载，开始,10");
            UserControlFactory.CreateInstance("");
            SplashScreenManager.Default.SendCommand(null, "内容面板预加载，结束,50");

            SplashScreenManager.Default.SendCommand(null, "数据更新完成,100");
            SplashScreenManager.CloseForm();
        }
        #endregion

        #region 从指定文件读取用户名
        public void ReadUsername()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string mypath = Path.Combine(baseDir, "username.txt");
            if (!File.Exists(mypath))
            {
                File.Create(mypath);
            }
            else
            {
                using (StreamReader sr = new StreamReader(mypath))
                {
                    List<string> u = new List<string>();
                    while (sr.Peek() > 0)
                    {
                        u.Add(sr.ReadLine());
                    }
                    u.Reverse();
                    Usernames.AddRange(u);
                }

            }
        }
        #endregion
        #region 将用户名写入指定文件中
        public void WriteUsername()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string mypath = Path.Combine(baseDir, "username.txt");

            using (StreamWriter sw = new StreamWriter(mypath))
            {
                Usernames.ForEach(item => sw.WriteLine(item));
            }

        }
        #endregion


    }
}