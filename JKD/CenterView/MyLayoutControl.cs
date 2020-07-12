using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JKD.CenterView
{
   public  class MyLayoutControl:LayoutControl
    {
       // Dictionary<string, Control> Dic = new Dictionary<string, Control>();
        public MyLayoutControl(Dictionary<string, Control> dic)
        {
            this.Dock = System.Windows.Forms.DockStyle.Fill;
           // Dic = dic;
            CreateDicItem(dic);
        }
        public void CreateDicItem(Dictionary<string, Control> dic)
        {
            foreach(string key in dic.Keys)
            {
                CreateControlItem(key, dic[key]);
            }
        }
        private void CreateControlItem(string labelName,Control control)
        {
            LayoutControlItem item = new LayoutControlItem();
            item.Parent = this.Root;
            item.Name = labelName;
            item.Control = control;
            item.Text = labelName;
            //item2.Move(item1, InsertType.Right);
            if (this.Items.IndexOf(item) > 0)
            {
                item.Move(this.Items[this.Items.IndexOf(item) - 1], InsertType.Right);
            }
                
        }
        
    }
}
