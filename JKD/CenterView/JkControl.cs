using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using JKD.DB;
using JKD.Custom;
using JKD.Models;
using DevExpress.XtraGrid.Columns;
using System.Linq;
namespace JKD.CenterView
{
    public partial class JkControl : XtraUserControl
    {
        private ComboBoxEdit RiqiCombo = new ComboBoxEdit();
        private ComboBoxEdit JkrCombo = new ComboBoxEdit();
        private SimpleButton BtnQuery = new SimpleButton()
        {
            Text = "查询"
        };
        private SimpleButton BtnNoneJk = new SimpleButton()
        {
            Text = "查看未缴款"
        };
        public JkControl()
        {
            InitializeComponent();
            InitView();
        }
        private void InitView()
        {
            Dictionary<string, Control> dic = new Dictionary<string, Control>();
            dic["日期快捷选择"] = RiqiCombo;
            dic["缴款人"] = JkrCombo;
            dic["查询按钮"] = BtnQuery;
            dic["未缴款按钮"] = BtnNoneJk;
            string[] riqi= {"本月","上月","本年","上一年","全部" };
            RiqiCombo.Properties.Items.AddRange(riqi);
            RiqiCombo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            new AccountManager().GetListJKR().Where(item=>item != null).ToList().ForEach(it => JkrCombo.Properties.Items.Add(it));
            JkrCombo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            List<GridColumn> lis = new List<GridColumn>();
            Vaccount.MapField.Keys.ToList().ForEach(item => {
                GridColumn gc = new GridColumn()
                {
                    FieldName = item,
                    Name = item + "Column",
                    Caption = Vaccount.MapField[item],
                    Visible = true

                };
                lis.Add(gc);

            });


            this.customTable1.AddColumns(lis);
            BtnQuery.Click += (o, e) =>
            {
                FreshData();
            };

            MyLayoutControl conditionControl = new MyLayoutControl(dic);
            
            
            this.groupControl1.Controls.Add(conditionControl);
        }
        private void FreshData()
        {
           
            string jkr = JkrCombo.EditValue == null ? " " : JkrCombo.EditValue.ToString();
            List<Vaccount> li = new VaccountManager().GetList()
               .Where(item => item.jkr.Contains(jkr))
                .ToList();
            this.customTable1.SetData(li);
        }

    }
}
