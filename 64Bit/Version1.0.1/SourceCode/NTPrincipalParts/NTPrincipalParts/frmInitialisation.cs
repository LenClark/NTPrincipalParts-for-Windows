using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NTPrincipalParts
{
    public partial class frmInitialisation : Form
    {
        public frmInitialisation()
        {
            InitializeComponent();
        }

        public void updateProgress(String message)
        {
            pbProgress.Increment(1);
            labProgressMsg.Text = message;
        }
    }
}
