using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CNCViewer
{
    public partial class FrmToolLayers : MaterialForm
    {
        public FrmToolLayers()
        {
            InitializeComponent();
        }

        private void TvTools_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown)
                return;
            ((clsToolLayer)e.Node.Tag).Hidden = !e.Node.Checked;
        }

        private void TvTools_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void BtnDone_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
