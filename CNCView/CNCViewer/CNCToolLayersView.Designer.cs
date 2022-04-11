namespace CNCViewer
{
    partial class FrmToolLayers
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tvTools = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // tvTools
            // 
            this.tvTools.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvTools.CheckBoxes = true;
            this.tvTools.Location = new System.Drawing.Point(0, -1);
            this.tvTools.Name = "tvTools";
            this.tvTools.ShowLines = false;
            this.tvTools.ShowPlusMinus = false;
            this.tvTools.ShowRootLines = false;
            this.tvTools.Size = new System.Drawing.Size(132, 0);
            this.tvTools.TabIndex = 2;
            this.tvTools.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TvTools_AfterCheck);
            this.tvTools.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.TvTools_BeforeSelect);
            // 
            // FrmToolLayers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.ClientSize = new System.Drawing.Size(131, 60);
            this.ControlBox = false;
            this.Controls.Add(this.tvTools);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmToolLayers";
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.TreeView tvTools;


    }
}