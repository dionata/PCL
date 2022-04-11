
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using PCLLib;
using System.Windows.Media.Media3D;
using OpenTK;
using MaterialSkin.Controls;
using System.IO;
//using PCLLib.View;

namespace PCLLib
{
    public partial class SplashScreenProgressBarView : MaterialForm
    {
        private delegate void CloseDelegateScreenProgress();
        private delegate void UpdateDelegateScreenProgress(string txt);

        private SplashScreenProgressBarView splashScreenProgressBarView;

        public SplashScreenProgressBarView(int max)
        {
            InitializeComponent();
            progressBar1.Minimum = 0;
            progressBar1.Maximum = max;
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowForm(object max)
        {
            splashScreenProgressBarView = new SplashScreenProgressBarView(Convert.ToInt32(max));
            Application.Run(splashScreenProgressBarView);
        }

        public void ShowSplashScreen(int max)
        {
            if (splashScreenProgressBarView != null)
                return;
            Thread thread = new Thread(ShowForm);
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(max);
        }

        public bool Ready()
        {
            return (splashScreenProgressBarView != null) && (splashScreenProgressBarView.Created);
        }

        public void UpdateProgress(string txt)
        {
            splashScreenProgressBarView.Invoke(new UpdateDelegateScreenProgress(UpdateProgressInternal), txt);

        }

        public void CloseForm()
        {
            splashScreenProgressBarView.Invoke(new CloseDelegateScreenProgress(CloseFormInternal));
        }

        public void UpdateProgressInternal(string txt)
        {
            splashScreenProgressBarView.progressBar1.Value++;
            splashScreenProgressBarView.progressBar1.Text = txt;
        }

        private void CloseFormInternal()
        {
            splashScreenProgressBarView.Close();           
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
     
        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.Join();// Abort();
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void updateInitFinishBlocos()
        {
            
        }

        private void materialRaisedButton2_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
