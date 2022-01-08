using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace NTPrincipalParts
{
    public partial class frmDetails : Form
    {
        int cellLastEnteredRowIndex = -1, cellLastEnteredColIndex = -1, rowsToDisplay, displayCode = 0;
        String[] rowContents;
        String[] col0titles = { "First person", "Second person", "Third person", "", "Imperative", "", "Infinitive", "",
                                    "Participles:", "   Masculine Nominative", "   Vocative", "   Accusative", "   Genitive", "   Dative", "",
                                    "   Neuter Nominative", "   Vocative", "   Accusative", "   Genitive", "   Dative", "",
                                    "   Feminine Nominative", "   Vocative", "   Accusative", "   Genitive", "   Dative", "",
                                    "Subjunctive:", "   First person", "   Second person", "   Third person", "",
                                    "Optative:", "   First person", "   Second person", "   Third person" };
        DataGridView[] dgvPPart;
        SortedList<int, classBooks> bookList;
        classGlobal globalVars;
        classParseDetail[,,] parseNetwork;
        classRoot rootInstance;

        public frmDetails()
        {
            InitializeComponent();
        }

        public void initialiseVerbDetails(String[] inContents, classRoot inRootWord, SortedList<int, classBooks> inList, classGlobal inGlobal)
        {
            int idx, jdx, kdx, refCount = 0;

            globalVars = inGlobal;
            // Make the six data grid views indexable
            dgvPPart = new DataGridView[6];
            dgvPPart[0] = dgvPPart1;
            dgvPPart[1] = dgvPPart2;
            dgvPPart[2] = dgvPPart3;
            dgvPPart[3] = dgvPPart4;
            dgvPPart[4] = dgvPPart5;
            dgvPPart[5] = dgvPPart6;
            rootInstance = inRootWord;
            // Allocate space for references - *** untested
            parseNetwork = new classParseDetail[6, col0titles.Length, 13];
            for (idx = 0; idx < 6; idx++)
            {
                for (jdx = 0; jdx < col0titles.Length; jdx++)
                {
                    for (kdx = 0; kdx < 13; kdx++)
                    {
                        parseNetwork[idx, jdx, kdx] = null;
                    }
                }
            }
            rowContents = inContents;

            bookList = inList;
            this.Text = "Verb details: " + rowContents[0];
            displayData();
        }

        private void displayData()
        {
            int idx, jdx, rowNo, principalPartCode, parseCode, wordFormCount = 0, targetTab, targetColumn = 0, targetRow = -1, subjunctiveStartRow = 26, regKeyValue;
            String wordEntry;
            System.Drawing.Font italicFont, regularFont;
            classParseDetail parseElements;
            /*=========================================================================*
             *   grammarBreakdown:                                                     *
             *   ================                                                      *
             *   Stores the code value for each parse point (e.g. 1/2/3rd person,      *
             *     sing/plur, tense, etc.).  Items (positions in Tuple):               *
             *                                                                         *
             *      0   Person (values 0 to 3)                                         *
             *      1   Number (values 1 = singular, 2 = plural)                       *
             *      2   Case (values 1 to 5 and include vocative [=2])                 *
             *      3   Gender (values 1 to 3)                                         *
             *      4   Tense (values 1 to 6)                                          *
             *      5   Mood (values 1 to 6)                                           *
             *      6   Voice (values 1 to 3)                                          *
             *      7   Comparative (=1)/Superlative (=2)                              *
             *=========================================================================*/
            Tuple<int, int, int, int, int, int, int> grammarBreakdown;

            // Initialise radiobutton and checkbox settings first
            regKeyValue = globalVars.getRegSetting(0);
            switch( regKeyValue)
            {
                case 0:
                    {
                        rbtnNoPtcpls.Checked = true;
                        rbtnNomAccMasc.Checked = false;
                        rbtnAllPtcpls.Checked = false;
                    }
                    break;
                case 1:
                    {
                        rbtnNoPtcpls.Checked = false;
                        rbtnNomAccMasc.Checked = true;
                        rbtnAllPtcpls.Checked = false;
                    }
                    break;
                case 2:
                    {
                        rbtnNoPtcpls.Checked = false;
                        rbtnNomAccMasc.Checked = false;
                        rbtnAllPtcpls.Checked = true;
                    }
                    break;
                default: break;
            }
            regKeyValue = globalVars.getRegSetting(1);
            if (regKeyValue == 0) chkSubj.Checked = false;
            else chkSubj.Checked = true;
            regKeyValue = globalVars.getRegSetting(2);
            idx = regKeyValue % 2;
            if (idx == 0) chkPresent.Checked = false;
            else chkPresent.Checked = true;
            regKeyValue /= 2;
            idx = regKeyValue % 2;
            if (idx == 0) chkFuture.Checked = false;
            else chkFuture.Checked = true;
            regKeyValue /= 2;
            idx = regKeyValue % 2;
            if (idx == 0) chkAorist.Checked = false;
            else chkAorist.Checked = true;
            regKeyValue /= 2;
            idx = regKeyValue % 2;
            if (idx == 0) chkPerfectActive.Checked = false;
            else chkPerfectActive.Checked = true;
            regKeyValue /= 2;
            idx = regKeyValue % 2;
            if (idx == 0) chkPerfectPassive.Checked = false;
            else chkPerfectPassive.Checked = true;
            regKeyValue /= 2;
            idx = regKeyValue % 2;
            if (idx == 0) chkAoristPassive.Checked = false;
            else chkAoristPassive.Checked = true;

            // Check on what is going to be displayed
            rowsToDisplay = 7; // These will always be displayed
            displayCode = 0;
            subjunctiveStartRow = 7;
            if (rbtnAllPtcpls.Checked)
            {
                rowsToDisplay += 19;
                displayCode = 1;
                subjunctiveStartRow = 26;
            }
            if (rbtnNomAccMasc.Checked)
            {
                rowsToDisplay += 3;
                displayCode = 2;
                subjunctiveStartRow = 10;
            }
            if (chkSubj.Checked)
            {
                rowsToDisplay += 10;
                displayCode += 4;
            }
            /*----------------------------------------------------------------------------------------------------*
             *   displayCode and rowsToDisplay:                                                                   *
             *   -----------------------------                                                                    *
             *   So, from this point on, these variables have the following values and purposes                   *
             *     displayCode    rowsToDisplay                  Meaning                                          *
             *         0               7             No participles or Subjunctive Mood                           *
             *         1              26             Base moods plus Participle but not Subjunctive/Optative      *
             *         2              10             Base moods plus indicatve single value of Participle         *
             *         4              17             Base moods plus Subjunctive and Optative but no Participles  *
             *         5              36             Full display of all moods, including all Participles         *
             *         6              20             All moods but only an indicative value for Participle        *
             *----------------------------------------------------------------------------------------------------*/
            // Make sure fonts are defined
            italicFont = new System.Drawing.Font("Times New Roman", 12, FontStyle.Italic);
            regularFont = new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular);
            // Ensure sufficient rows have been allocated for each Data Grid View (for each Principal Part)
            for (idx = 0; idx < 6; idx++)
            {
                dgvPPart[idx].Rows.Clear();
                dgvPPart[idx].RowCount = rowsToDisplay;
                dgvPPart[idx].DefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular);
            }
            // Create the side titles for each Data Grid View
            for (jdx = 0; jdx < 6; jdx++)
            {
                rowNo = 0;
                for (idx = 0; idx < 7; idx++)
                {
                    dgvPPart[jdx].Rows[idx].Cells[0].Value = col0titles[idx];
                    rowNo++;
                }
                switch (displayCode)
                {
                    case 1:
                        for (idx = 0; idx < 19; idx++) dgvPPart[jdx].Rows[rowNo++].Cells[0].Value = col0titles[idx + 7];
                        break;
                    case 2:
                        for (idx = 0; idx < 3; idx++) dgvPPart[jdx].Rows[rowNo++].Cells[0].Value = col0titles[idx + 7];
                        break;
                    case 4:
                        for (idx = 0; idx < 10; idx++) dgvPPart[jdx].Rows[rowNo++].Cells[0].Value = col0titles[idx + 26];
                        break;
                    case 5:
                        for (idx = 0; idx < 19; idx++) dgvPPart[jdx].Rows[rowNo++].Cells[0].Value = col0titles[idx + 7];
                        for (idx = 0; idx < 10; idx++) dgvPPart[jdx].Rows[rowNo++].Cells[0].Value = col0titles[idx + 26];
                        break;
                    case 6:
                        for (idx = 0; idx < 3; idx++) dgvPPart[jdx].Rows[rowNo++].Cells[0].Value = col0titles[idx + 7];
                        for (idx = 0; idx < 10; idx++) dgvPPart[jdx].Rows[rowNo++].Cells[0].Value = col0titles[idx + 26];
                        break;
                    default: break;
                }
            }
            // So, noOfElements will provide the total number of different forms for the root
//            noOfElements = rootInstance.ParseCount;
            // Now let's iterate through each of the forms of the root (verb)
//            for (idx = 0; idx < noOfElements; idx++)
            foreach( KeyValuePair<int, classParseDetail> parsePair in rootInstance.ParsedDetails)
            {
                targetTab = 0;
                targetColumn = 0;
                targetRow = 0;
//                parseElements = rootInstance.getParseElementByIndex(idx);
                parseElements = parsePair.Value;
                principalPartCode = parseElements.PrincipalPartCode;
                targetTab = principalPartCode - 1;
                parseCode = parseElements.ParseCode;
                // Total number of variant forms (really ought always to be 1 but often = 2 or more)
                wordFormCount = parseElements.WordFormCount;
                grammarBreakdown = parseElements.GrammarBreakdown;
                wordEntry = "";
                // Sort out the form to be displayed
                for (jdx = 0; jdx < wordFormCount; jdx++)
                {
                    if (wordEntry.Length == 0) wordEntry = parseElements.getWordFormByIndex(jdx);
                    else wordEntry += ", " + parseElements.getWordFormByIndex(jdx);
                }
                // The first thing we need to check is whether we are displaying the current item
                switch (grammarBreakdown.Item6)
                {
                    case 1:        // Indicative
                    case 2:        // Imperative
                    case 5: break; // Infinitive - aqlways display these three
                    case 3:                                         // Subjunctive
                    case 4: if (displayCode < 4) continue; break;   // Optative
                    case 6:                                         // Participle - a bit more complex
                        jdx = displayCode % 4;
                        if (jdx == 0) continue;
                        if (jdx == 2)
                        {
                            if (grammarBreakdown.Item4 > 1) continue;
                            if (grammarBreakdown.Item3 > 1) continue;
                            if (grammarBreakdown.Item2 > 1) continue;
                        }
                        break;
                }
                // Basically, the whole of the switch works out which cell to put the result in
                switch (grammarBreakdown.Item5)
                {
                    case 1:
                    case 2:   // Present and Imperfect are treated the same for all except column
                        switch (grammarBreakdown.Item7)  // i.e. active, middle or passive
                        {
                            case 1:
                                if (grammarBreakdown.Item5 == 1)
                                {
                                    if (grammarBreakdown.Item2 < 2) targetColumn = 1; else targetColumn = 2;
                                }
                                else
                                {
                                    if (grammarBreakdown.Item2 < 2) targetColumn = 3; else targetColumn = 4;
                                }
                                break;
                            case 2:
                                if (grammarBreakdown.Item5 == 1)
                                {
                                    if (grammarBreakdown.Item2 < 2) targetColumn = 5; else targetColumn = 6;
                                }
                                else
                                {
                                    if (grammarBreakdown.Item2 < 2) targetColumn = 7; else targetColumn = 8;
                                }
                                break;
                            case 3:
                                if (grammarBreakdown.Item5 == 1)
                                {
                                    if (grammarBreakdown.Item2 < 2) targetColumn = 9; else targetColumn = 10;
                                }
                                else
                                {
                                    if (grammarBreakdown.Item2 < 2) targetColumn = 11; else targetColumn = 12;
                                }
                                break;
                        }
                        // Columns are easy; rows a bit more awkward
                        switch (grammarBreakdown.Item6)
                        {
                            case 1: targetRow = grammarBreakdown.Item1 - 1; break;   // indicative
                            case 2: targetRow = 4; break;                            // Imperative
                            case 3: targetRow = subjunctiveStartRow + grammarBreakdown.Item1; break;  // Subjunctive
                            case 4: targetRow = subjunctiveStartRow + 5 + grammarBreakdown.Item1; break;  // Optative
                            case 5: targetRow = 6; break;                            // Infinitive
                            case 6:                                                  // Participle - a bit more complex
                                switch (grammarBreakdown.Item4)
                                {
                                    case 1: targetRow = 8 + grammarBreakdown.Item3; break;
                                    case 2: targetRow = 14 + grammarBreakdown.Item3; break;
                                    case 3: targetRow = 20 + grammarBreakdown.Item3; break;
                                }
                                break;
                        }
                        break;
                    case 3:  // Aorist
                        switch (grammarBreakdown.Item7)
                        {
                            case 1: if (grammarBreakdown.Item2 < 2) targetColumn = 1; else targetColumn = 2; break;
                            case 2: if (grammarBreakdown.Item2 < 2) targetColumn = 3; else targetColumn = 4; break;
                            case 3:
                                if (grammarBreakdown.Item2 < 2) targetColumn = 1; else targetColumn = 2;  // Different tab
                                break;
                        }
                        switch (grammarBreakdown.Item6)
                        {
                            case 1: targetRow = grammarBreakdown.Item1 - 1; break;
                            case 2: targetRow = 4; break;
                            case 3: targetRow = subjunctiveStartRow + grammarBreakdown.Item1; break;
                            case 4: targetRow = subjunctiveStartRow + 5 + grammarBreakdown.Item1; break;
                            case 5: targetRow = 6; break;
                            case 6:
                                switch (grammarBreakdown.Item4)
                                {
                                    case 1: targetRow = 8 + grammarBreakdown.Item3; break;
                                    case 2: targetRow = 14 + grammarBreakdown.Item3; break;
                                    case 3: targetRow = 20 + grammarBreakdown.Item3; break;
                                }
                                break;
                        }
                        break;
                    case 4:
                    case 5:
                        switch (grammarBreakdown.Item7)
                        {
                            case 1:
                                if (grammarBreakdown.Item5 == 4)
                                {
                                    if (grammarBreakdown.Item2 < 2) targetColumn = 1; else targetColumn = 2;
                                }
                                else
                                {
                                    if (grammarBreakdown.Item2 < 2) targetColumn = 3; else targetColumn = 4;
                                }
                                break;
                            case 2:
                                if (grammarBreakdown.Item5 == 4)
                                {
                                    if (grammarBreakdown.Item2 < 2) targetColumn = 1; else targetColumn = 2;
                                }
                                else
                                {
                                    if (grammarBreakdown.Item2 < 2) targetColumn = 3; else targetColumn = 4;
                                }
                                break;
                            case 3:
                                if (grammarBreakdown.Item5 == 4)
                                {
                                    if (grammarBreakdown.Item2 < 2) targetColumn = 5; else targetColumn = 6;
                                }
                                else
                                {
                                    if (grammarBreakdown.Item2 < 2) targetColumn = 7; else targetColumn = 8;
                                }
                                break;
                        }
                        switch (grammarBreakdown.Item6)
                        {
                            case 1: targetRow = grammarBreakdown.Item1 - 1; break;
                            case 2: targetRow = 4; break;
                            case 3: targetRow = subjunctiveStartRow + grammarBreakdown.Item1; break;
                            case 4: targetRow = subjunctiveStartRow + 5 + grammarBreakdown.Item1; break;
                            case 5: targetRow = 6; break;
                            case 6:
                                switch (grammarBreakdown.Item4)
                                {
                                    case 1: targetRow = 8 + grammarBreakdown.Item3; break;
                                    case 2: targetRow = 14 + grammarBreakdown.Item3; break;
                                    case 3: targetRow = 20 + grammarBreakdown.Item3; break;
                                }
                                break;
                        }
                        break;
                    case 6:
                        switch (grammarBreakdown.Item7)
                        {
                            case 1: if (grammarBreakdown.Item2 < 2) targetColumn = 1; else targetColumn = 2; break;
                            case 2: if (grammarBreakdown.Item2 < 2) targetColumn = 3; else targetColumn = 4; break;
                            case 3: if (grammarBreakdown.Item2 < 2) targetColumn = 3; else targetColumn = 4; break;
                        }
                        switch (grammarBreakdown.Item6)
                        {
                            case 1: targetRow = grammarBreakdown.Item1 - 1; break;
                            case 2: targetRow = 4; break;
                            case 3: targetRow = subjunctiveStartRow + grammarBreakdown.Item1; break;
                            case 4: targetRow = subjunctiveStartRow + 5 + grammarBreakdown.Item1; break;
                            case 5: targetRow = 6; break;
                            case 6:
                                switch (grammarBreakdown.Item4)
                                {
                                    case 1: targetRow = 8 + grammarBreakdown.Item3; break;
                                    case 2: targetRow = 14 + grammarBreakdown.Item3; break;
                                    case 3: targetRow = 20 + grammarBreakdown.Item3; break;
                                }
                                break;
                        }
                        break;
                }
                if (targetRow > 999) continue;
                if (targetRow < 0) continue;
                // Actually display the form
                dgvPPart[targetTab].Rows[targetRow].Cells[targetColumn].Value = wordEntry;
                // If the form comes from the NT, display in bold
                switch(parseElements.NtAndLxxCode)
                {
                    case 1:
                        dgvPPart[targetTab].Rows[targetRow].Cells[targetColumn].Style.Font = regularFont;
                        dgvPPart[targetTab].Rows[targetRow].Cells[targetColumn].Style.ForeColor = Color.Black;
                        break;
                    case 2:
                        dgvPPart[targetTab].Rows[targetRow].Cells[targetColumn].Style.Font = italicFont;
                        dgvPPart[targetTab].Rows[targetRow].Cells[targetColumn].Style.ForeColor = Color.Gray;
                        break;
                    case 3:
                        dgvPPart[targetTab].Rows[targetRow].Cells[targetColumn].Style.Font = italicFont;
                        dgvPPart[targetTab].Rows[targetRow].Cells[targetColumn].Style.ForeColor = Color.Red;
                        break;
                    default: break;
                }
                parseNetwork[targetTab, targetRow, targetColumn] = parseElements;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private void dgvPPart1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                dgvMainHdr1.HorizontalScrollingOffset = e.NewValue;
            }
        }

        private void dgvPPart2_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                dgvMainHdr2.HorizontalScrollingOffset = e.NewValue;
            }
        }

        private void dgvPPart3_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                dgvMainHdr3.HorizontalScrollingOffset = e.NewValue;
            }
        }

        private void dgvPPart4_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                dgvMainHdr4.HorizontalScrollingOffset = e.NewValue;
            }
        }

        private void dgvPPart5_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                dgvMainHdr5.HorizontalScrollingOffset = e.NewValue;
            }
        }

        private void dgvPPart6_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                dgvMainHdr6.HorizontalScrollingOffset = e.NewValue;
            }
        }

        private void mnuRefShow_Click(object sender, EventArgs e)
        {

        }

        private void btnPParts_Click(object sender, EventArgs e)
        {
            if (String.Compare(btnPParts.Text, "Select All") == 0)
            {
                chkPresent.Checked = true;
                chkFuture.Checked = true;
                chkAorist.Checked = true;
                chkPerfectActive.Checked = true;
                chkPerfectPassive.Checked = true;
                chkAoristPassive.Checked = true;
                btnPParts.Text = "Uncheck All";
            }
            else
            {
                chkPresent.Checked = false;
                chkFuture.Checked = false;
                chkAorist.Checked = false;
                chkPerfectActive.Checked = false;
                chkPerfectPassive.Checked = false;
                chkAoristPassive.Checked = false;
                btnPParts.Text = "Select All";
            }
        }

        private void btnTextPrint_Click(object sender, EventArgs e)
        {
            const int subjStart = 26, subjNoOfRows = 10;

            int noOfRows, idx, jdx, rdx, extDot;
            String sideHeader, mainName, extension;
            StreamWriter swVerbFile;
            bool[] selectedParts = new bool[6];
            int[] headerItemCount = { 7, 3, 3, 3, 5, 3 };
            int[] columnCount = { 13, 5, 5, 5, 9, 5 };
            DataGridView[] dgvHeaders = new DataGridView[6];

            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                extDot = dlgSave.FileName.LastIndexOf('.');
                if (extDot > -1)
                {
                    mainName = dlgSave.FileName.Substring(0, extDot);
                    extension = dlgSave.FileName.Substring(extDot);
                }
                else
                {
                    mainName = dlgSave.FileName;
                    extension = "";
                }
                noOfRows = 7;
                if (rbtnNomAccMasc.Checked) noOfRows = 10;
                if (rbtnAllPtcpls.Checked) noOfRows = 26;
                selectedParts[0] = chkPresent.Checked;
                selectedParts[1] = chkFuture.Checked;
                selectedParts[2] = chkAorist.Checked;
                selectedParts[3] = chkPerfectActive.Checked;
                selectedParts[4] = chkPerfectPassive.Checked;
                selectedParts[5] = chkAoristPassive.Checked;
                dgvHeaders[0] = dgvMainHdr1;
                dgvHeaders[1] = dgvMainHdr2;
                dgvHeaders[2] = dgvMainHdr3;
                dgvHeaders[3] = dgvMainHdr4;
                dgvHeaders[4] = dgvMainHdr5;
                dgvHeaders[5] = dgvMainHdr6;
                for (idx = 0; idx < 6; idx++)
                {
                    if (selectedParts[idx])
                    {
                        swVerbFile = new StreamWriter(mainName + "P" + idx.ToString() + extension);
                        for (jdx = 1; jdx < headerItemCount[idx]; jdx++)
                        {
                            swVerbFile.Write("\t" + dgvHeaders[idx].Columns[jdx].HeaderText);
                        }
                        swVerbFile.WriteLine();
                        for (jdx = 1; jdx < columnCount[idx]; jdx++)
                        {
                            swVerbFile.Write("\t" + dgvPPart[idx].Columns[jdx].HeaderText);
                        }
                        swVerbFile.WriteLine();
                        for (rdx = 0; rdx < noOfRows; rdx++)
                        {
                            sideHeader = col0titles[rdx];
                            if ((sideHeader.Length == 0) || (sideHeader.Contains(':')))
                            {
                                swVerbFile.WriteLine(sideHeader);
                            }
                            else
                            {
                                swVerbFile.Write(sideHeader);
                                for (jdx = 1; jdx < columnCount[idx]; jdx++)
                                {
                                    if (dgvPPart[idx].Rows[rdx].Cells[jdx].Value == null) swVerbFile.Write("\t");
                                    else swVerbFile.Write("\t" + dgvPPart[idx].Rows[rdx].Cells[jdx].Value.ToString());
                                }
                                swVerbFile.WriteLine();
                            }
                        }
                        if (chkSubj.Checked)
                        {
                            for (rdx = subjStart; rdx < subjStart + subjNoOfRows; rdx++)
                            {
                                sideHeader = col0titles[rdx];
                                if ((sideHeader.Length == 0) || (sideHeader.Contains(':')))
                                {
                                    swVerbFile.WriteLine(sideHeader);
                                }
                                else
                                {
                                    swVerbFile.Write(sideHeader);
                                    for (jdx = 1; jdx < columnCount[idx]; jdx++)
                                    {
                                        if (dgvPPart[idx].Rows[rdx].Cells[jdx].Value == null) swVerbFile.Write("\t");
                                        else swVerbFile.Write("\t" + dgvPPart[idx].Rows[rdx].Cells[jdx].Value.ToString());
                                    }
                                    swVerbFile.WriteLine();
                                }
                            }
                        }
                        swVerbFile.Close();
                    }
                }
            }
        }

        private void dgvPPart1_MouseClick(object sender, MouseEventArgs e)
        {
            int x, y, clientX, clientY, tpX, tpY, tcX, tcY, absX, absY;
            System.Drawing.Point coord;
            DataGridView currentDGV;
            TabPage parentTabPage;
            TabControl parentTabCtrl;

            x = e.X;
            y = e.Y;

            currentDGV = dgvPPart[tabCtlVerbDetail.SelectedIndex];
            // (clientX, clientY) = coordinate of the DataGridView in the TabCtrl
            clientX = currentDGV.Left;
            clientY = currentDGV.Top;
            parentTabPage = (TabPage)currentDGV.Parent;
            // (tpX, tpY) = coordinate of the TabPage in its parent
            tpX = parentTabPage.Left;
            tpY = parentTabPage.Top;
            parentTabCtrl = (TabControl)parentTabPage.Parent;
            // (tcX, tcY) = coordinate of the TabControl in the form client
            tcX = parentTabCtrl.Left;
            tcY = parentTabCtrl.Top;
            absX = currentDGV.TopLevelControl.Left + clientX + tpX + tcX + x;
            absY = currentDGV.TopLevelControl.Top + clientY + tpY + tcY + y;
            currentDGV.Rows[cellLastEnteredRowIndex].Cells[cellLastEnteredColIndex].Selected = true;
            coord = new System.Drawing.Point(absX, absY);
            mnuReferences.Show(coord);
        }

        private void chkTense_CheckedChanged(object sender, EventArgs e)
        {
            int chkCode = 0;

            if (chkPresent.Checked) chkCode += 1;
            if (chkFuture.Checked) chkCode += 2;
            if (chkAorist.Checked) chkCode += 4;
            if (chkPerfectActive.Checked) chkCode += 8;
            if (chkPerfectPassive.Checked) chkCode += 16;
            if (chkAoristPassive.Checked) chkCode += 32;
            globalVars.updateRegSetting(2, chkCode);
        }

        private void chkBoxCheckChanged(object sender, EventArgs e)
        {
            CheckBox activeButton;

            activeButton = (CheckBox)sender;
            if (activeButton.Checked) globalVars.updateRegSetting(1, 1);
            else globalVars.updateRegSetting(1, 0);
            displayData();
        }

        private void rbtnCheckChanged(object sender, EventArgs e)
        {
            RadioButton activeButton;

            activeButton = (RadioButton)sender;
            if (!activeButton.Checked) return;
            if( activeButton == rbtnNoPtcpls) globalVars.updateRegSetting(0, 0);
            if (activeButton == rbtnNomAccMasc) globalVars.updateRegSetting(0, 1);
            if (activeButton == rbtnAllPtcpls) globalVars.updateRegSetting(0, 2);
            displayData();
        }

        private void dataGridCellDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            /*====================================================================================================================*
             *                                                                                                                    *
             *                                            dataGridCellDoubleClick                                                 *
             *                                            =======================                                                 *
             *                                                                                                                    *
             *  This method codes the response to a mouse double click on a specific data grid view cell.  The aim is to          *
             *  (a) obtain a list of references associated with that cell;                                                        *
             *  (b) sort them into a meaningful order                                                                             *
             *  (c) send the sorted list to frmReferences                                                                         *
             *                                                                                                                    *
             *  Since arrays are fixed size, we can't simply add and insert values in arrays to create the list.  Also, since     *
             *    there are three variables (book, chapter and verse), the exercise is complex.  In order to complete the         *
             *    process, we start by assigning an integer value to each reference.  This will be of the form:                   *
             *                                                                                                                    *
             *                   bookNo * 1 000 000 + chapterSeq * 1000 + verseSeq                                                *
             *                                                                                                                    *
             *    (The largest number of verses in any chapter is 150; the largest number of chapters in any book is 150.  So     *
             *    this ensures the calculated number will be unique and in order.)                                                *
             *                                                                                                                    *
             *  We create a sorted list, codedReferenceList, with the key and value:                                              *
             *                                                                                                                    *
             *     key:   the calculated code for the reference                                                                   *
             *     value: the class instance relating to the coded key value                                                      *
             *                                                                                                                    *
             *  Once completed, we can send this to frmReferences for processing.                                                 *
             *                                                                                                                    *
             *====================================================================================================================*/

            int targetTab, targetRow, targetColumn;
            DataGridView currentDataGridView;

            currentDataGridView = (DataGridView)sender;
            targetTab = Convert.ToInt32(currentDataGridView.Tag);
            targetRow = e.RowIndex;
            targetColumn = e.ColumnIndex;
            detailDisplay(targetTab, targetRow, targetColumn);
        }

        private void dgvPPart1_Scroll_1(object sender, ScrollEventArgs e)
        {
            if( e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                //                MessageBox.Show(e.OldValue.ToString() + " ==> " + e.NewValue.ToString(), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvMainHdr1.HorizontalScrollingOffset = e.NewValue;
            }
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            int targetTab, targetRow, targetColumn;
            TabPage selectedTabPage;
            DataGridView currentDataGridView;
            DataGridViewSelectedCellCollection currentCell;

            selectedTabPage = tabCtlVerbDetail.SelectedTab;
            targetTab = Convert.ToInt32(selectedTabPage.Tag) - 1;
            currentDataGridView = dgvPPart[targetTab];
            currentCell = currentDataGridView.SelectedCells;
            targetRow = currentCell[0].RowIndex;
            targetColumn = currentCell[0].ColumnIndex;
            if ((targetRow == -1) || (targetColumn == -1)) return;
            detailDisplay(targetTab, targetRow, targetColumn);
        }

        private void detailDisplay( int targetTab, int targetRow, int targetColumn )
        {
            int codedValue, lastValue = 0, chapterFocus, bookFocus, prevChapterFocus = 0, prevBookFocus = 0;
            frmReferences currentRef;
            SortedList<int, classReference> codedReferenceList = new SortedList<int, classReference>();
            SortedList<int, Tuple<String, String, String, String>> displayList = new SortedList<int, Tuple<String, string, string, string>>();
            classParseDetail selectedParseDetail;
            Tuple<String, String, String, String> currentEntry;

            selectedParseDetail = parseNetwork[targetTab, targetRow, targetColumn];
            foreach (KeyValuePair<int, classReference> specificReference in selectedParseDetail.ReferenceList)
            {
                codedValue = specificReference.Value.BookNo * 1000000 + specificReference.Value.ChapterSeq * 1000 + specificReference.Value.VerseSeq;
                if (!codedReferenceList.ContainsKey(codedValue)) codedReferenceList.Add(codedValue, specificReference.Value);
            }
            // Now lets tidy the presentation
            foreach (KeyValuePair<int, classReference> referenceItem in codedReferenceList)
            {
                chapterFocus = referenceItem.Key / 1000;  // Get rid of varying verses
                if (chapterFocus == prevChapterFocus) // this and the previous entry refer to the same chapter
                {
                    displayList.TryGetValue(lastValue, out currentEntry);
                    currentEntry = new Tuple<String, string, string, string>(currentEntry.Item1, currentEntry.Item2, currentEntry.Item3, currentEntry.Item4 + ", " + referenceItem.Value.Verse);
                    displayList.Remove(lastValue);
                    displayList.Add(lastValue, currentEntry);
                }
                else
                {
                    bookFocus = referenceItem.Key / 1000000;
                    if (bookFocus == prevBookFocus)
                    {
                        currentEntry = new Tuple<String, string, string, string>(referenceItem.Value.BookName, "", referenceItem.Value.Chapter, referenceItem.Value.Verse);
                    }
                    else
                    {
                        currentEntry = new Tuple<String, string, string, string>(referenceItem.Value.BookName, referenceItem.Value.BookName, referenceItem.Value.Chapter, referenceItem.Value.Verse);
                        prevBookFocus = bookFocus;
                    }
                    prevChapterFocus = chapterFocus;
                    lastValue = referenceItem.Key;
                    displayList.Add(lastValue, currentEntry);
                }
            }
            currentRef = new frmReferences(globalVars, this);
            currentRef.BookList = bookList;
            currentRef.displayReferences(displayList);
            currentRef.Show();
        }

        private void dgvPPart1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            cellLastEnteredColIndex = e.ColumnIndex;
            cellLastEnteredRowIndex = e.RowIndex;
        }
    }
}
