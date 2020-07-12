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
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;

namespace JKD.Custom
{
    public partial class CustomLayoutControl : DevExpress.XtraEditors.XtraUserControl
    {
        public CustomLayoutControl(Dictionary<string, Control> dic)
        {
            InitializeComponent();
            CreateDicItem(dic);
        }
        //public MyLayoutControl(Dictionary<string, Control> dic)
        //{
        //    this.Dock = System.Windows.Forms.DockStyle.Fill;
        //    // Dic = dic;
        //    CreateDicItem(dic);
        //}
        public void CreateDicItem(Dictionary<string, Control> dic)
        {
            foreach (string key in dic.Keys)
            {
                CreateControlItem(key, dic[key]);
            }
        }
        private void CreateControlItem(string labelName, Control control)
        {
            LayoutControlItem item = new LayoutControlItem();
            item.Parent = this.layoutControlGroup1;
            item.Name = labelName;
            item.Control = control;
            item.Text = labelName;
            //item2.Move(item1, InsertType.Right);
            if (this.layoutControlGroup1.Items.IndexOf(item) > 0)
            {
                item.Move(this.layoutControlGroup1.Items[this.layoutControlGroup1.Items.IndexOf(item) - 1], InsertType.Right);
            }

        }
    }
}
