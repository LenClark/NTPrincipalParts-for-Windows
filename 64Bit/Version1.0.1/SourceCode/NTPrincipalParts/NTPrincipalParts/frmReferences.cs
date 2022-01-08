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
    public partial class frmReferences : Form
    {
        int clickedRow = 0, totalRows;
        String controlingWord;
        frmDetails parentUsage;
        SortedList<int, classBooks> bookList;
        classGlobal globalVars;
        frmDetails callingForm;

        public SortedList<int, classBooks> BookList { get => bookList; set => bookList = value; }

        public frmReferences(classGlobal inGlobal, frmDetails callingForm)
        {
            InitializeComponent();
            globalVars = inGlobal;
            parentUsage = callingForm;
            dgvReferences.Columns[2].Width = dgvReferences.Width - (dgvReferences.Columns[0].Width + dgvReferences.Columns[1].Width);
        }

        public void displayReferences(SortedList<int, Tuple<String, String, String, String>> sortedReferenceList)
        {
            int rowNum = 0;

            dgvReferences.Rows.Clear();
            dgvReferences.RowCount = sortedReferenceList.Count;
            foreach( KeyValuePair<int, Tuple<String, String, String, String>> referenceItem in sortedReferenceList)
            {
                dgvReferences.Rows[rowNum].Cells[0].Value = referenceItem.Value.Item2;
                dgvReferences.Rows[rowNum].Cells[1].Value = referenceItem.Value.Item3;
                dgvReferences.Rows[rowNum].Cells[2].Value = referenceItem.Value.Item1 + " " + referenceItem.Value.Item3 + ":" + referenceItem.Value.Item4;
                rowNum++;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
/*            mnuRightShow.Visible = true;
            mnuRightOnlyShow.Visible = false;
            mnuRightKeep.Visible = true; */
            this.Close();
            this.Dispose();
        }

        private void dgvReferences_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int y;

            y = e.RowIndex;
            handleDisplay(y);
        }

        private void displayButton_Click(object sender, EventArgs e)
        {
            int idx, y;
            DataGridViewSelectedRowCollection selectedRows;

            selectedRows = dgvReferences.SelectedRows;
            y = selectedRows[0].Index;
            handleDisplay(y);
        }

        private void handleDisplay(int y)
        {
            bool isFirst = true;
            int idx, noOfItems, retrievedSeqNo, prevSeqNo;
            String fullreference, bookName = "", chapterNo, verseText = "", verseRef;
            int[] arrayOfValues;
            Char[] bookSplit = { ' ' }, chapterSplit = { ':' }, verseSplit = { ',' };
            String[] bookAndOther, bookChapterAndVerses, versesOnly, verseList;
            classBooks currentBook = null;
            classChapter currentChapter;
            frmChapter chapterDisplay;

            dgvReferences.ClearSelection();
            dgvReferences.Rows[y].Selected = true;
            fullreference = dgvReferences.Rows[y].Cells[2].Value.ToString();
            bookChapterAndVerses = fullreference.Split(chapterSplit);
            bookAndOther = bookChapterAndVerses[0].Split(bookSplit);
            noOfItems = bookAndOther.Length;
            for (idx = 0; idx < noOfItems - 1; idx++)
            {
                if (idx == 0) bookName = bookAndOther[0];
                else bookName += " " + bookAndOther[idx];
            }
            chapterNo = bookAndOther[noOfItems - 1];
            versesOnly = bookChapterAndVerses[1].Split(verseSplit);
            noOfItems = versesOnly.Length;
            verseList = new String[noOfItems];
            for (idx = 0; idx < noOfItems; idx++)
            {
                verseList[idx] = versesOnly[idx].Trim();
            }
            // Let's get the book details
            foreach (KeyValuePair<int, classBooks> bookPair in bookList)
            {
                if (String.Compare(bookName, bookPair.Value.BookName) == 0)
                {
                    currentBook = bookPair.Value;
                    break;
                }
            }
            if (currentBook == null) return;
            prevSeqNo = -1;
            foreach (KeyValuePair<String, classChapterDecode> decodePair in currentBook.ChapterDecode)
            {
                if (String.Compare(decodePair.Key, chapterNo) != 0) continue;
                arrayOfValues = decodePair.Value.ChapterAndVerseDecode.Values.ToArray().Distinct().ToArray();
                Array.Sort(arrayOfValues);
                foreach (int candidateSequence in arrayOfValues)
                {
                    currentChapter = currentBook.getChapterBySequence(candidateSequence);
                    noOfItems = currentChapter.NoOfVerses;
                    for (idx = 1; idx <= noOfItems; idx++)
                    {
                        verseRef = currentChapter.getVerseRefBySequence(idx);
                        if (isFirst)
                        {
                            verseText = verseRef + ": ";
                            isFirst = false;
                        }
                        else verseText += "\n" + verseRef + ": ";
                        verseText += currentChapter.getVerseText(idx);
                    }
                }
            }
            chapterDisplay = new frmChapter( parentUsage, this);
            chapterDisplay.displayChapter(verseText, fullreference, verseList);
            chapterDisplay.Show();
        }

        private void btnClosePrev_Click(object sender, EventArgs e)
        {
            parentUsage.Close();
            this.Close();
            this.Dispose();
        }

        private void saveCSV_Click(object sender, EventArgs e)
        {
            int idx, jdx;
            String fileName;
            String[] col = new String[3];
            StreamWriter swCSV;

            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                fileName = dlgSave.FileName;
                swCSV = new StreamWriter(fileName);
                for (idx = 0; idx < totalRows; idx++)
                {
                    for (jdx = 0; jdx < 3; jdx++)
                    {
                        if (dgvReferences.Rows[idx].Cells[jdx].Value == null)
                        {
                            col[jdx] = "";
                        }
                        else
                        {
                            col[jdx] = dgvReferences.Rows[idx].Cells[jdx].Value.ToString();
                        }
                    }
                    swCSV.WriteLine(col[0] + "\t" + col[1] + "\t" + col[2]);
                }
                swCSV.Close();
                swCSV.Dispose();
                MessageBox.Show("The references for the word: " + controlingWord + " have been saved as CSV\n\tin: " + fileName, "Save as CSV successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
