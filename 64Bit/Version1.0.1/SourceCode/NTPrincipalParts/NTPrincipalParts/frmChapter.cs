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
    public partial class frmChapter : Form
    {
        frmDetails grandParentUsage;
        frmReferences parentUsage;

        public frmChapter( frmDetails inheritedForm, frmReferences callingForm)
        {
            InitializeComponent();
            grandParentUsage = inheritedForm;
            parentUsage = callingForm;
        }

        public void displayChapter( String text, String reference, String[] arrayOfVerses)
        {
            int nStart, nEnd;
            Font fntBold;

            this.Text = reference;
            fntBold = new Font(rtxtChapter.Font.Name, 14, FontStyle.Bold);
            rtxtChapter.Text = text;
            foreach( String verseNo in arrayOfVerses)
            {
                nStart = rtxtChapter.Text.IndexOf(verseNo + ":");
                if( nStart > -1 )
                {
                    nEnd = rtxtChapter.Text.IndexOf('\n', nStart);
                    if (nEnd == -1) nEnd = rtxtChapter.Text.Length;
                    rtxtChapter.SelectionStart = nStart;
                    rtxtChapter.SelectionLength = nEnd - nStart;
                    rtxtChapter.SelectionFont = fntBold;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private void btnBackToBase_Click(object sender, EventArgs e)
        {
            parentUsage.Close();
            grandParentUsage.Close();
            this.Close();
            this.Dispose();
        }
    }
}
