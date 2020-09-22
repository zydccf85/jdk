using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JKD.utils;
using JKD.Models;
using JKD.DB;
namespace JKD.Dialog
{
    public class DialogFactory
    {
        //配置对话框
        public static void CreateConfigControl()
        {
            XtraDialogArgs xda = new XtraDialogArgs()
            {
                Caption = "常用配置",
                Content = new ConfigControl(),
                Buttons= new DialogResult[] { DialogResult.OK, DialogResult.Cancel }
            };
            xda.Showing += (s, e) =>
            {
                e.Buttons[DialogResult.OK].Text = "确定";
                e.Buttons[DialogResult.OK].Click += (x, y) =>
                {
                    (xda.Content as ConfigControl).Reset();
                };
            };
            XtraDialog.Show(xda);
            
           
        }
        
        //搜索药品对话框
        public static void CreateSearchControl(ButtonEdit be)
        {
            XtraDialogArgs xda = new XtraDialogArgs()
            {
                Caption = "药品搜索框",
                Content = new SearchControl(be),
              //  Buttons = new DialogResult[] { DialogResult.OK, DialogResult.Cancel }
            };
            xda.Showing += (s, e) =>
            {
                e.Buttons[DialogResult.OK].Text = "确定";
                e.Buttons[DialogResult.OK].Click += (x, y) =>
                {
                    SearchControl sc = xda.Content as SearchControl;

                };
            };
            XtraDialog.Show(xda);
            
        }

        //药品明细对话框
        public static void CreateDrugEditControl(Drug d)
        {
            XtraDialogArgs xda = new XtraDialogArgs()
            {
                Caption = "药品明细框",
                Content = new DrugEditControl(d),
                Buttons = new DialogResult[] { DialogResult.OK, DialogResult.Cancel }
            };
            xda.Showing += (s, e) =>
            {
                e.Buttons[DialogResult.OK].Text = "保存";
                e.Buttons[DialogResult.Cancel].Text = "取消";
                e.Buttons[DialogResult.OK].Click += (x, y) =>
                {
                    DrugEditControl dec = xda.Content as DrugEditControl;
                    Drug dd= dec.GetDrug();
                    int count = new DrugManager().Update(dd);
                    if (count > 0)
                    {
                        XtraMessageBox.Show("更新成功", "信息提示");
                       
                    }

                };
            };
            XtraDialog.Show(xda);
        }

        //缴款单弹出对话框
        public static void CreateJKDControl(Account account)
        {
            XtraDialogArgs xda = new XtraDialogArgs()
            {
                Caption = "缴款单明细",
                Content = new JKDControl1(account),
                Buttons = new DialogResult[] { DialogResult.Cancel, DialogResult.OK },

                
            };
            xda.Showing += (s, e) =>
            {
                e.Buttons[DialogResult.OK].Text = "保存";
                e.Buttons[DialogResult.Cancel].Text = "取消";
                
                e.Buttons[DialogResult.OK].Click += (x, y) =>
                {
                    MessageBox.Show("none");

                };
            };
                XtraDialog.Show(xda);
            
        }
    }
}
