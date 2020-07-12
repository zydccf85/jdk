using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using System.Threading;

namespace JKD
{
    public partial class SplashScreen1 : SplashScreen
    {
        public SplashScreen1()
        {
            InitializeComponent();
        }
        #region Overrides
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
            string args = Convert.ToString(arg);

            this.labelControl2.Text = args.Split(',')[0];
            this.marqueeProgressBarControl1.EditValue = Convert.ToInt32(args.Split(',')[1]);
            
            
        }
       
        #endregion

        public enum SplashScreenCommand
        {
        }
    }
}