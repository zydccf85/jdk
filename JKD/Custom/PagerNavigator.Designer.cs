namespace JKD.Custom
{
    partial class PagerNavigator
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataNavigator1 = new DevExpress.XtraEditors.DataNavigator();
            this.SuspendLayout();
            // 
            // dataNavigator1
            // 
            this.dataNavigator1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataNavigator1.Appearance.BackColor2 = System.Drawing.Color.White;
            this.dataNavigator1.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.dataNavigator1.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.dataNavigator1.Appearance.Options.UseBackColor = true;
            this.dataNavigator1.Appearance.Options.UseForeColor = true;
            this.dataNavigator1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.dataNavigator1.Buttons.Append.Visible = false;
            this.dataNavigator1.Buttons.CancelEdit.Visible = false;
            this.dataNavigator1.Buttons.EndEdit.Visible = false;
            this.dataNavigator1.Buttons.First.Visible = false;
            this.dataNavigator1.Buttons.Last.Visible = false;
            this.dataNavigator1.Buttons.Next.Visible = false;
            this.dataNavigator1.Buttons.NextPage.Visible = false;
            this.dataNavigator1.Buttons.Prev.Visible = false;
            this.dataNavigator1.Buttons.PrevPage.Visible = false;
            this.dataNavigator1.Buttons.Remove.Visible = false;
            this.dataNavigator1.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(0, 0, "首页"),
            new DevExpress.XtraEditors.NavigatorCustomButton(1, 1, "上一页"),
            new DevExpress.XtraEditors.NavigatorCustomButton(3, 4, "下一页"),
            new DevExpress.XtraEditors.NavigatorCustomButton(4, 5, "末页")});
            this.dataNavigator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataNavigator1.Location = new System.Drawing.Point(0, 0);
            this.dataNavigator1.Name = "dataNavigator1";
            this.dataNavigator1.Size = new System.Drawing.Size(573, 37);
            this.dataNavigator1.TabIndex = 0;
            this.dataNavigator1.Text = "dataNavigator1";
            this.dataNavigator1.TextLocation = DevExpress.XtraEditors.NavigatorButtonsTextLocation.Begin;
            // 
            // PagerNavigator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataNavigator1);
            this.Name = "PagerNavigator";
            this.Size = new System.Drawing.Size(573, 37);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.DataNavigator dataNavigator1;
    }
}
