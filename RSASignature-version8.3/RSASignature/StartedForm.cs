using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSASignature
{
    public partial class StartedForm : Form
    {
        #region Event form

        public StartedForm()
        {
            InitializeComponent();
        }
        private void Started_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want close?", "Notification", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void btOpenTaoChuKy_Click(object sender, EventArgs e)
        {
            CreateForm f1 = new CreateForm();
            this.Visible = false;
            f1.ShowDialog();
            this.Visible=true;
        }

        private void btOpenXacThucChuKy_Click(object sender, EventArgs e)
        {
            Verification f2 = new Verification();
            this.Visible = false;
            f2.ShowDialog();
            this.Visible = true;
        }

        #endregion

       
    }
}
