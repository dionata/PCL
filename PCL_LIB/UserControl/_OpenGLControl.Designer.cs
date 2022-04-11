using OpenTK;

namespace PCLLib
{
    partial class OpenGLControl
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
            this.glControl1 = new OpenTK.GLControl();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl1.BackColor = System.Drawing.Color.White;
            this.glControl1.Location = new System.Drawing.Point(0, 0);
            this.glControl1.Margin = new System.Windows.Forms.Padding(5);
            this.glControl1.Name = "glControl1";
           // this.glControl1.Size = new System.Drawing.Size(854, 453); //(256, 218);
            this.glControl1.TabIndex = 12;
            this.glControl1.VSync = false;

            // 
            // OpenGLControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.glControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "OpenGLControl";
            //this.Size = new System.Drawing.Size(854, 453);//(256, 218);
            this.Load += new System.EventHandler(this.OpenGLControl_Load);
            this.ResumeLayout(false);

            this.SuspendLayout();
            // 
            // OpenGLControl
            //         
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Marcacao);
            this.ResumeLayout(false);

        }

        #endregion

        public GLControl glControl1;
    }
}
