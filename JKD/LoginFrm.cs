﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using JKD.DB;
using System.Configuration;
using System.IO;
using JKD.CenterView;
using JKD.Service;
using JKD.ViewModels;

namespace JKD
{
    public partial class LoginFrm : DevExpress.XtraEditors.XtraForm
    {
        public bool isSuccess = false;
        
        public LoginFrm()
        {
            InitializeComponent();
            mvvmContext1.ViewModelType = typeof(LoginFrmViewModel);
            Init();
        }
        public void Init()
        {
            mvvmContext1.SetBinding(teUsername, e => e.Text, "Username");
            mvvmContext1.GetViewModel<LoginFrmViewModel>().Usernames.ForEach(item=>teUsername.Properties.Items.Add(item));
            mvvmContext1.SetBinding(tePsw, e => e.Text, "Password");
            mvvmContext1.SetBinding(teRname, e => e.Text, "RName");
            mvvmContext1.SetBinding(teRusername, e => e.Text, "RUsername");
            mvvmContext1.SetBinding(teRpsw, e => e.Text, "RPassword");
            mvvmContext1.SetBinding(teRrepsw, e => e.Text, "RRepeatPassword");
            mvvmContext1.BindCommand<LoginFrmViewModel,LoginFrm>(btnOK, (x,p)=> x.handleOk(p),x=>this);
            mvvmContext1.BindCommand<LoginFrmViewModel>(btnRegister, x=>x.handleRegister());
            mvvmContext1.BindCommand<LoginFrmViewModel>(btnReset, x => x.handleReset());
            sbCancel.Click += (s, e) =>
            {
                this.Close();
            };

        }
        public void SelectAll(string tename)
        {
            if (tename == "teUsername")
            {
                teUsername.Focus();
                teUsername.SelectAll();
            }else if (tename == "tePsw")
            {
                tePsw.Focus();
                tePsw.SelectAll();
            }
            
        }

        private void tePsw_Properties_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void tePsw_Properties_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnOK.Focus();
            }
        }
    }
}