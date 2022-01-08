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
    public partial class frmHelp : Form
    {
        public frmHelp(String fileName)
        {
            Uri browserUrl;

            InitializeComponent();

            browserUrl = new Uri(fileName);
            webHelp.Navigate(browserUrl);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
