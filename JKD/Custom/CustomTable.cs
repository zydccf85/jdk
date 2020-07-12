using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Menu;
using DevExpress.Utils.Menu;

namespace JKD.Custom
{
    public partial class CustomTable : UserControl
    {
        public CustomTable()
        {
            InitializeComponent();
            InitConfig();
        }
        private void InitConfig()
        {
            #region 设置指示器
            this.gridView1.IndicatorWidth = 30;
            this.gridView1.CustomDrawRowIndicator += (sender, e) =>
            {
                GridView view = (GridView)sender;
                //Check whether the indicator cell belongs to a data row
                if (e.Info.IsRowIndicator && e.RowHandle >= 0 )
                {
                    e.Info.DisplayText =  (e.RowHandle+1).ToString();
                }
            };
            #endregion
            #region 
            this.gridControl1.MouseDown += gridControl1_MouseDown;
            #endregion
            this.gridView1.OptionsView.BestFitMode = GridBestFitMode.Fast;
            
        }
        public void SetHiddenColumn(int[] hidden)
        {
            for(int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                if (hidden.Contains(i))
                {
                    this.gridView1.Columns[i].Visible = false;
                }
            }
        }
        public void AddColumns(List<GridColumn> li)
        {
            this.gridView1.Columns.AddRange(li.ToArray());
            foreach(GridColumn gc in this.gridView1.Columns)
            {
                gc.OptionsFilter.AllowAutoFilter = false;
                gc.OptionsFilter.AllowFilter = false;
            }
        }
        public void AddRowClick(RowClickEventHandler ev)
        {
            this.gridView1.RowClick += ev;
        }
        private void gridControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
            // Check if the end-user has right clicked the grid control.?
            if (e.Button == MouseButtons.Right)
                     DoShowMenu(gridView1.CalcHitInfo(new Point(e.X, e.Y)));
        }

        void DoShowMenu(DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi)
        {
            // Create the menu.?
            DevExpress.XtraGrid.Menu.GridViewMenu menu = null;
            // Check whether the header panel button has been clicked.?
            //if (hi.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.ColumnButton)
            if(hi.InRow && this.gridView1.FocusedRowHandle == hi.RowHandle)
            {
                menu = new GridViewColumnButtonMenu(gridView1);
                
                menu.Init(hi);
                // Display the menu.?
                menu.Show(hi.HitPoint);
            }
        }


        public void SetData(Object datasource)
        {
            this.gridControl1.DataSource = datasource;
            
        }
        public void SetColumns(Dictionary<string,string> dic)
        {
            foreach(GridColumn gc in this.gridView1.Columns)
            {
                string val = gc.FieldName;
                if (dic.TryGetValue(gc.FieldName,out val))
                {
                    gc.Caption = val;
                }
                gc.OptionsFilter.AllowFilter = false;
               
            }
        }
    }
    class GridViewColumnButtonMenu : GridViewMenu
    {
        public event EventHandler EV;
        
        public GridViewColumnButtonMenu(DevExpress.XtraGrid.Views.Grid.GridView view) : base(view) { }
        // Create menu items.?
        // This method is automatically called by the menu's public Init method.?
      

            protected override void CreateItems()
        {
            Items.Clear();
            DXSubMenuItem columnsItem = new DXSubMenuItem("删除");
            columnsItem.Click += EV;
            Items.Add(columnsItem);
            //Items.Add(columnsItem);
            //Items.Add(CreateMenuItem("Runtime Column Customization", GridMenuImages.Column.Images[3],
            //  "Customization", true));
            //foreach (GridColumn column in View.Columns)
            //{
            //    if (column.OptionsColumn.ShowInCustomizationForm)
            //        columnsItem.Items.Add(CreateMenuCheckItem(column.Caption, column.VisibleIndex >= 0,
            //          null, column, true));
            //}
        }
        protected  void OnMenuItemClick(object sender, EventArgs e)
        {
            
            if (RaiseClickEvent(sender, null)) return;
            DXMenuItem item = sender as DXMenuItem;
            if (item.Tag == null) return;
            if (item.Tag is GridColumn)
            {
                GridColumn column = item.Tag as GridColumn;
                column.VisibleIndex = column.VisibleIndex >= 0 ? -1 : View.VisibleColumns.Count;
            }else if(item.Tag.ToString() == "Customization") {
                View.ShowCustomization();
            } 
   }
}

}
