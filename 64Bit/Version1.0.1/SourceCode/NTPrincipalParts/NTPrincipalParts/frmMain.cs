using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace NTPrincipalParts
{
    public partial class frmMain : Form
    {
        bool isNotActivated = true;
        int ntCount, totalBookCount, rootWordCount = 0;
        String keyEntryWorkspace = "";
        Button[,] gkKeys;
        ToolTip[,] keyToolTips;

        Thread loadThread;
        private delegate void performProgressIncrement(String message);
        private delegate void performPanelModification(Panel panelToBeModified, Button buttonToBeAdded);
        private delegate void performSetToolTip(ToolTip ttTarget, Button targetButton, String message);
        private delegate void performListboxAddition(ListBox lbTarget, String text);

        /*======================================================*
         *  Dictionaries and Lists                              *
         *======================================================*/

        /*----------------------------------------------------------------------------------------------------*
         *  bookList                                                                                          *
         *  --------                                                                                          *
         *                                                                                                    *
         *  A list of all book names, abbreviations and source file names.                                    *
         *                                                                                                    *
         *  Ironically, New Testament books come first in sequence.  So                                       *
         *     1-27  Matthew to Revelation                                                                    *
         *     28-86 Genesis to Susanna (Theodotion)                                                          *
         *           (Note: effective order is 1 to 59)                                                       *
         *                                                                                                    *
         *  key:   Integer book code                                                                          *
         *  value: The class instance for the relevant book                                                   *
         *----------------------------------------------------------------------------------------------------*/
        SortedList<int, classBooks> bookList = new SortedList<int, classBooks>();

        /*----------------------------------------------------------------------------------------------------*
         *  listOfRoots                                                                                       *
         *  -----------                                                                                       *
         *                                                                                                    *
         *  Identifies the classRoot instance for a specific root word.                                       *
         *                                                                                                    *
         *  key:   A specifc root word                                                                        *
         *  value: The class instance for that word                                                           *
         *----------------------------------------------------------------------------------------------------*/
        SortedList<String, classRoot> listOfRoots = new SortedList<string, classRoot>();

        /*----------------------------------------------------------------------------------------------------*
         *  rootWordIndex                                                                                     *
         *  -------------                                                                                     *
         *                                                                                                    *
         *  An enabling lookup list for rootWordList                                                          *
         *                                                                                                    *
         *  key:   A sequential index, formed purely on the order of appearance of the word - effectively     *
         *         random                                                                                     *
         *  value: The root word                                                                              *
         *----------------------------------------------------------------------------------------------------*/
        SortedList<int, String> rootWordIndex = new SortedList<int, string>();

        /*----------------------------------------------------------------------------------------------------*
         *  frequencyTable                                                                                    *
         *  --------------                                                                                    *
         *                                                                                                    *
         *  Effectively provides a list of words and their frequency of occurrence in the New Testament.  The *
         *  only sensible way of sring this information is a list but this creates a problem: for any given   *
         *  number of occurrences there is likely to be several words with the same frequency (especially at  *
         *  low frequencies).  So, for each number of occurrences (1, 2, and so on) we have a class instance  *
         *  that stores all words with that frequency.  This list provides access to these instances.         *
         *                                                                                                    *
         *  key:   The frequency of occurrence of words (i.e. an integer)                                     *
         *  value: The class instance of the list of words with that frequency                                *
         *----------------------------------------------------------------------------------------------------*/
        SortedList<int, classFrequency> frequencyTable = new SortedList<int, classFrequency>();

        /*======================================================*
         *  Global class definitions                            *
         *======================================================*/
        classGlobal globalVars;
        frmInitialisation initDialog;
        classComplexProcessing complexProcessing;

        public frmMain()
        {
            int idx, headingCount;
            String[] summaryHeadings = { "1 Present/Imperfect", "2 Future Active/Middle", "3 Aorist Active/Middle", "4 Perfect/Pluperfect Active",
                                         "5 Perfect/Pluperfect Middle", "6 Aorist/Future Passive", "No. of times used in the NT" };

            InitializeComponent();
            globalVars = new classGlobal();
            globalVars.initialiseRegistry();
            complexProcessing = new classComplexProcessing();
            headingCount = summaryHeadings.Length;
            dgvSummary.RowCount = headingCount;
            for (idx = 0; idx < headingCount; idx++)
            {
                dgvSummary.Rows[idx].Cells[0].Value = summaryHeadings[idx];
            }
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            if (isNotActivated)
            {
                this.Visible = false;
                initDialog = new frmInitialisation();
                initDialog.Show();

                loadThread = new Thread(new ThreadStart(bespokeInitialisation));
                loadThread.IsBackground = true;
                loadThread.Start();
                tmrInitial.Enabled = true;
                isNotActivated = false;
            }
        }

        private void bespokeInitialisation()
        {
            /*----------------------------------------------------------------------------------------------*
             *                                                                                              *
             *                                   bespokeInitialisation                                      *
             *                                   ---------------------                                      *
             *                                                                                              *
             *  Effectively, the main, controlling method for the intialisation of the application.  It is  *
             *  called as a background thread by frmMain_Activated.                                         *
             *                                                                                              *
             *----------------------------------------------------------------------------------------------*/

            initDialog.Invoke(new performProgressIncrement(incrementProgress), "Setting up greek character conversion");
            complexProcessing.initialiseLookup();

            /*-----------------------------------------------------------------------*
             *   Store book titles and associated details.                           *
             *-----------------------------------------------------------------------*/
            initDialog.Invoke(new performProgressIncrement(incrementProgress), "Getting book titles");
            processBookTitles();

            /*-----------------------------------------------------------------------*
             *   Store the actual text.                                              *
             *-----------------------------------------------------------------------*/
            storeNTText();
            storeLXXText();

            /*-----------------------------------------------------------------------*
             *  Create a keyboard as part of access to the listbox                   *
             *-----------------------------------------------------------------------*/
            initDialog.Invoke(new performProgressIncrement(incrementProgress), "Setting up the user interface - virtual keyboard");
            setupKeyboard();
            initDialog.Invoke(new performProgressIncrement(incrementProgress), "Setting up the user interface - list of verbs");
            populateListbox();
            initDialog.Invoke(new performProgressIncrement(incrementProgress), "Final initialisation tasks");
            initialiseFrequencyTable();
        }

        private void processBookTitles()
        {
            /*----------------------------------------------------------------------------------------------*
             *                                                                                              *
             *                                     processBookTitles                                        *
             *                                     -----------------                                        *
             *                                                                                              *
             *  Gets the details of biblical books from file and stores the relevant information in the     *
             *  class: classBook.  This includes file names for the method that actually loads the text.    *
             *                                                                                              *
             *  It occurs in two halves: one for the NT and one for the LXX.                                *
             *                                                                                              *
             *----------------------------------------------------------------------------------------------*/

            int titleCount = 0;
            String fullFileName, textBuffer;
            String[] fileContents;
            Char[] splitParams = { '\t' };
            StreamReader srTitles;
            classBooks newBook;

            fullFileName = globalVars.BaseDirectory + @"\" + globalVars.NtTitlesFile;
            srTitles = new StreamReader(fullFileName);
            textBuffer = srTitles.ReadLine();
            while (textBuffer != null)
            {
                fileContents = textBuffer.Split(splitParams);
                newBook = new classBooks();
                newBook.BookName = fileContents[0];
                newBook.BookShortName = fileContents[1];
                newBook.BookFileName = fileContents[2];
                newBook.IsNTBook = true;
                newBook.ActualBookNumber = titleCount;
                bookList.Add(titleCount++, newBook);
                textBuffer = srTitles.ReadLine();
            }
            srTitles.Close();
            ntCount = titleCount;
            fullFileName = globalVars.BaseDirectory + @"\" + globalVars.LxxTitlesFile;
            srTitles = new StreamReader(fullFileName);
            textBuffer = srTitles.ReadLine();
            while (textBuffer != null)
            {
                if (textBuffer[0] != ';')
                {
                    fileContents = textBuffer.Split(splitParams);
                    newBook = new classBooks();
                    newBook.BookName = fileContents[1];
                    newBook.BookShortName = fileContents[0];
                    newBook.BookFileName = fileContents[3];
                    newBook.IsNTBook = false;
                    newBook.ActualBookNumber = titleCount;
                    bookList.Add(titleCount++, newBook);
                }
                textBuffer = srTitles.ReadLine();
            }
            srTitles.Close();
            srTitles.Dispose();
            totalBookCount = titleCount;
        }

        private void storeNTText()
        {
            /*----------------------------------------------------------------------------------------------*
             *                                                                                              *
             *                                       storeNTText                                            *
             *                                       -----------                                            *
             *                                                                                              *
             *  Now to get the actual text - this method is specific to NT.                                 *
             *                                                                                              *
             *----------------------------------------------------------------------------------------------*/

            int idx, chapterSeq, verseSeq, parseCode, principalPartCode = 0;
            String fullFileName, textBuffer, bookName, chapterNo, verseNo, prevChapNo, prevVerseNo, wordInstance, associatedRoot;
            String[] fileContents;
            Char[] splitParams = { '\t' };
            StreamReader srText;
            Tuple<int, int, int, int, int, int, int> grammarBreakdown;
            classBooks currentBook;
            classChapter currentChapter = null;
            classVerse currentVerse = null;
            classRoot newRoot;

            for (idx = 0; idx < ntCount; idx++)
            {
                if (bookList.ContainsKey(idx))
                {
                    bookList.TryGetValue(idx, out currentBook);
                    bookName = currentBook.BookName;
                    initDialog.Invoke(new performProgressIncrement(incrementProgress), "Storing " + bookName);
                    fullFileName = globalVars.BaseDirectory + @"\" + globalVars.NtTextFolder + @"\" + currentBook.BookFileName;
                    srText = new StreamReader(fullFileName);
                    textBuffer = srText.ReadLine();
                    prevChapNo = "?";
                    chapterSeq = 0;
                    prevVerseNo = "?";
                    verseSeq = 0;
                    while (textBuffer != null)
                    {
                        fileContents = textBuffer.Split(splitParams);
                        chapterNo = fileContents[0];
                        if (String.Compare(chapterNo, prevChapNo) != 0)
                        {
                            chapterSeq++;
                            prevChapNo = chapterNo;
                            prevVerseNo = "?";
                            verseSeq = 0;
                            currentChapter = currentBook.addChapter(chapterNo, chapterSeq);
                        }
                        verseNo = fileContents[1];
                        currentBook.addDecodeInformation(chapterNo, verseNo, chapterSeq);
                        if (String.Compare(verseNo, prevVerseNo) != 0)
                        {
                            verseSeq++;
                            prevVerseNo = verseNo;
                            currentVerse = currentChapter.addVerse(verseNo, verseSeq);
                        }
                        currentVerse.addWord(fileContents[5], fileContents[9], fileContents[10], fileContents[11]);
                        if (String.Compare(fileContents[2], "V-") == 0)
                        {
                            /*---------------------------------------------------------------------------------------*
                             *  Key to parameters:                                                                   *
                             *  -------------                                                                        *
                             *                                                                                       *
                             *  1   The book number/code                                                             *
                             *  2   simple chapter number (in string form) - allows out of sequence                  *
                             *  3   simple verse number (in string form) - dito                                      *
                             *  4   Full parse code (as normal)                                                      *
                             *  5   Word as appears in text (with punctuation already removed)                       *
                             *  6   Same word form but with accents, etc removed (but not breathings)                *
                             *  7   Root form of word                                                                *
                             *                                                                                       *
                             *  bookList:  The list containing the class instance in which the word occurs           *
                             *---------------------------------------------------------------------------------------*/
                            parseCode = complexProcessing.getNTParseInfo(fileContents[3]);
                            wordInstance = fileContents[5];
                            wordInstance = complexProcessing.checkAndReplaceLeadingMajiscule(wordInstance);
                            associatedRoot = fileContents[8];
                            principalPartCode = complexProcessing.PrincipalPartCode;
                            grammarBreakdown = complexProcessing.GrammarBreakdown;
                            if( listOfRoots.ContainsKey( associatedRoot) )
                            {
                                listOfRoots.TryGetValue(associatedRoot, out newRoot);
                            }
                            else
                            {
                                newRoot = new classRoot();
                                listOfRoots.Add(associatedRoot, newRoot);
                                rootWordIndex.Add(rootWordCount++, associatedRoot);
                                rootWordCount++;
                            }
                            newRoot.addRootInformation(true, wordInstance, fileContents[6], parseCode, principalPartCode, bookName, chapterNo, verseNo, idx + 1, chapterSeq, verseSeq,
                                grammarBreakdown, complexProcessing);
                        }
                        textBuffer = srText.ReadLine();
                    }
                    srText.Close();
                }
            }
        }

        private void storeLXXText()
        {
            /*----------------------------------------------------------------------------------------------*
             *                                                                                              *
             *                                       storeLXXText                                           *
             *                                       ------------                                           *
             *                                                                                              *
             *  Now to get the actual text - duplicate of storeNTText but specific to LXX.                  *
             *                                                                                              *
             *----------------------------------------------------------------------------------------------*/

            int idx, chapterSeq, verseSeq, parseCode, principalPartCode = 0;
            String fullFileName, textBuffer, bookName, chapterNo, verseNo, prevChapNo, prevVerseNo, wordInstance, associatedRoot;
            String[] fileContents;
            Char[] splitParams = { '\t' };
            StreamReader srText;
            Tuple<int, int, int, int, int, int, int> grammarBreakdown;
            classBooks currentBook;
            classChapter currentChapter = null;
            classVerse currentVerse = null;
            classRoot newRoot;

            for (idx = ntCount; idx < totalBookCount; idx++)
            {
                if (bookList.ContainsKey(idx))
                {
                    bookList.TryGetValue(idx, out currentBook);
                    bookName = currentBook.BookName;
                    initDialog.Invoke(new performProgressIncrement(incrementProgress), "Storing " + bookName);
                    fullFileName = globalVars.BaseDirectory + @"\" + globalVars.LxxTextFolder + @"\" + currentBook.BookFileName;
                    srText = new StreamReader(fullFileName);
                    textBuffer = srText.ReadLine();
                    prevChapNo = "?";
                    chapterSeq = 0;
                    prevVerseNo = "?";
                    verseSeq = 0;
                    while (textBuffer != null)
                    {
                        fileContents = textBuffer.Split(splitParams);
                        chapterNo = fileContents[0];
                        if (String.Compare(chapterNo, prevChapNo) != 0)
                        {
                            chapterSeq++;
                            prevChapNo = chapterNo;
                            prevVerseNo = "?";
                            verseSeq = 0;
                            currentChapter = currentBook.addChapter(chapterNo, chapterSeq);
                        }
                        verseNo = fileContents[1];
                        currentBook.addDecodeInformation(chapterNo, verseNo, chapterSeq);
                        if (String.Compare(verseNo, prevVerseNo) != 0)
                        {
                            verseSeq++;
                            prevVerseNo = verseNo;
                            currentVerse = currentChapter.addVerse(verseNo, verseSeq);
                        }
                        currentVerse.addWord(fileContents[5], fileContents[9], fileContents[10], fileContents[11]);
                        if ((fileContents[2].Length > 0) && (String.Compare(fileContents[2].Substring(0, 1), "V") == 0))
                        {
                            /*---------------------------------------------------------------------------------------*
                             *  Key to parameters:                                                                   *
                             *  -------------                                                                        *
                             *                                                                                       *
                             *  1   The book number/code                                                             *
                             *  2   simple chapter number (in string form) - allows out of sequence                  *
                             *  3   simple verse number (in string form) - dito                                      *
                             *  4   Full parse code (as normal)                                                      *
                             *  5   Word as appears in text (with punctuation already removed)                       *
                             *  6   Same word form but with accents, etc removed (but not breathings)                *
                             *  7   Root form of word                                                                *
                             *                                                                                       *
                             *  bookList:  The list containing the class instance in which the word occurs           *
                             *---------------------------------------------------------------------------------------*/
                            parseCode = complexProcessing.getLXXParseInfo(fileContents[3]);
                            wordInstance = fileContents[5];
                            wordInstance = complexProcessing.checkAndReplaceLeadingMajiscule(wordInstance);
                            associatedRoot = fileContents[8];
                            principalPartCode = complexProcessing.PrincipalPartCode;
                            grammarBreakdown = complexProcessing.GrammarBreakdown;
                            if (listOfRoots.ContainsKey(associatedRoot))
                            {
                                listOfRoots.TryGetValue(associatedRoot, out newRoot);
                                // The main difference from the NT load is that we are not storing _new_ roots - only existing ones.
                                newRoot.addRootInformation(false, wordInstance, fileContents[6], parseCode, principalPartCode, bookName, chapterNo, verseNo, idx + 1, chapterSeq, verseSeq,
                                    grammarBreakdown, complexProcessing);
                            }
                        }
                        textBuffer = srText.ReadLine();
                    }
                    srText.Close();
                }
            }
        }

        private void setupKeyboard()
        {
            int keyHeight = 34, keyWidth = 34, keyBigWidth = 48, keyTop = 4, keyLeft = 10, keyGap = 4, keyRows = 3, keyCols = 10, kRowIdx, kColIdx, keySeq = 0;
            Button currentButton;
            String[,] keyFaces = {{ "α", "β", "γ", "δ", "ε", "ζ", "η", "θ", "ι", "BkSp" },
                                   { "κ", "λ", "μ", "ν", "ξ", "ο", "π", "ρ", "σ", "Clear" },
                                   { "ς", "τ", "υ", "φ", "χ", "ψ", "ω", " ", " ", " " } };
            String[,] gkKeyHints = { { "alpha", "beta", "gamma", "delta", "epsilon", "zeta", "eta", "theta", "iota", "Backspace" },
                            { "kappa", "lambda", "mu", "nu", "xi", "omicron", "pi", "rho", "sigma", "Clear" },
                            { "final sigma", "tau", "upsilon", "phi", "chi", "psi", "omega", " ", " ", " " } };
            Font buttonFont = new Font("Times New Roman", 12, FontStyle.Regular);

            gkKeys = new Button[keyRows, keyCols];
            keyToolTips = new ToolTip[keyRows, keyCols];
            for (kRowIdx = 0; kRowIdx < keyRows; kRowIdx++)
            {
                for (kColIdx = 0; kColIdx < keyCols; kColIdx++)
                {
                    currentButton = new Button();
                    currentButton.Height = keyHeight;
                    if ((kColIdx == keyCols - 1) && (kRowIdx < 2))
                    {
                        currentButton.Width = keyBigWidth;
                    }
                    else
                    {
                        currentButton.Width = keyWidth;
                        currentButton.Font = buttonFont;
                    }
                    currentButton.Top = keyTop + kRowIdx * (keyHeight + keyGap);
                    currentButton.Left = keyLeft + kColIdx * (keyWidth + keyGap);
                    currentButton.Name = "Key_Row" + kRowIdx.ToString() + "_Col" + kColIdx.ToString();
                    currentButton.Tag = keySeq++;
                    currentButton.Text = keyFaces[kRowIdx, kColIdx];
                    currentButton.Click += keyButton_Click;
                    pnlKeys.Invoke(new performPanelModification(addControlToPanel), pnlKeys, currentButton);
                    gkKeys[kRowIdx, kColIdx] = currentButton;

                    keyToolTips[kRowIdx, kColIdx] = new ToolTip();
                    keyToolTips[kRowIdx, kColIdx].ToolTipTitle = "Key Value";
                    pnlKeys.Invoke(new performSetToolTip(setToolTip), keyToolTips[kRowIdx, kColIdx], gkKeys[kRowIdx, kColIdx], gkKeyHints[kRowIdx, kColIdx]);
                    if ((kColIdx == 6) && (kRowIdx == 2)) break;
                }
            }
        }

        void keyButton_Click(object sender, EventArgs e)
        {
            int tagValue;
            Button currentButton;

            currentButton = (Button)sender;
            tagValue = Convert.ToInt32(currentButton.Tag);
            switch (tagValue)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26: processAddedChar(currentButton); break;


                case 9: removeLastChar(); break;
                case 19: clearConstraints(); break;
                default: break;
            }
        }

        private void processAddedChar(Button pressedButton)
        {
            int removalPstn, idx, limit;
            Char targetChar, compareChar, inspectedChar;
            String inspectedEntry;
            SortedDictionary<Char, Char> gkFlattener = new SortedDictionary<Char, Char>();

            removalPstn = keyEntryWorkspace.Length;
            targetChar = pressedButton.Text[0];
            limit = lbAvailableWords.Items.Count;
            gkFlattener = complexProcessing.BaseCharLookup;
            for (idx = limit - 1; idx >= 0; idx--)
            {
                inspectedEntry = lbAvailableWords.Items[idx].ToString();
                if (inspectedEntry.Length <= removalPstn)
                {
                    lbAvailableWords.Items.RemoveAt(idx);
                    continue;
                }
                inspectedChar = inspectedEntry[removalPstn];
                if (gkFlattener.ContainsKey(inspectedChar))
                {
                    gkFlattener.TryGetValue(inspectedChar, out compareChar);
                    if (compareChar != targetChar)
                    {
                        lbAvailableWords.Items.RemoveAt(idx);
                        continue;
                    }
                }
                else
                {
                    lbAvailableWords.Items.RemoveAt(idx);
                    continue;
                }
            }
            keyEntryWorkspace += pressedButton.Text;
            labTextEnteredMsg.Text = keyEntryWorkspace;
        }

        private void removeLastChar()
        {
            bool isNoMatch;
            int removalPstn, idx, jdx, limit;
            Char targetChar, compareChar;
            String rootWord;
            classRoot rootData;
            SortedDictionary<Char, Char> gkFlattener = new SortedDictionary<Char, Char>();

            gkFlattener = complexProcessing.BaseCharLookup;
            removalPstn = keyEntryWorkspace.Length;
            if (removalPstn == 0) return;
            if (removalPstn == 1)
            {
                clearConstraints();
                return;
            }
            keyEntryWorkspace = keyEntryWorkspace.Substring(0, keyEntryWorkspace.Length - 1);
            removalPstn--;
            lbAvailableWords.Items.Clear();
            for (idx = 1; idx <= rootWordCount; idx++)
            {
                isNoMatch = false;
                if (rootWordIndex.ContainsKey(idx))
                {
                    rootWordIndex.TryGetValue(idx, out rootWord);
                    listOfRoots.TryGetValue(rootWord, out rootData);
                    limit = rootWord.Length;
                    if (limit < removalPstn) continue;
                    for (jdx = 0; jdx < removalPstn; jdx++)
                    {
                        targetChar = rootWord[jdx];
                        if (gkFlattener.ContainsKey(targetChar))
                        {
                            gkFlattener.TryGetValue(targetChar, out compareChar);
                            if (compareChar != keyEntryWorkspace[jdx])
                            {
                                isNoMatch = true;
                                break;
                            }
                        }
                        else
                        {
                            isNoMatch = true;
                            break;
                        }
                    }
                    if (isNoMatch) continue;
                    lbAvailableWords.Items.Add(rootWord);
                }
            }
            if (lbAvailableWords.Items.Count > 0) lbAvailableWords.SelectedIndex = 0;
            labTextEnteredMsg.Text = keyEntryWorkspace;
        }

        private void clearConstraints()
        {
            int idx;
            String rootWord;

            keyEntryWorkspace = "";
            lbAvailableWords.Items.Clear();
            for (idx = 1; idx <= rootWordCount; idx++)
            {
                if (rootWordIndex.ContainsKey(idx))
                {
                    rootWordIndex.TryGetValue(idx, out rootWord);
                    lbAvailableWords.Items.Add(rootWord);
                }
            }
            if (lbAvailableWords.Items.Count > 0) lbAvailableWords.SelectedIndex = 0;
            labTextEnteredMsg.Text = "None";
        }

        private void populateListbox()
        {
            int idx;
            String rootWord;

            for (idx = 0; idx < rootWordCount; idx++)
            {
                if (rootWordIndex.ContainsKey(idx))
                {
                    rootWordIndex.TryGetValue(idx, out rootWord);
                    lbAvailableWords.Invoke(new performListboxAddition(addListboxEntry), lbAvailableWords, rootWord);
                }
            }
        }

        private void initialiseFrequencyTable()
        {
            int rootCount;
            String rootWord;
            classRoot rootClass;
            classFrequency freqInstance;

            foreach (KeyValuePair<String, classRoot> parsePair in listOfRoots)
            {
                rootClass = parsePair.Value;
                rootWord = parsePair.Key;
                rootCount = rootClass.FrequencyInNT;
                if (frequencyTable.ContainsKey(rootCount))
                {
                    frequencyTable.TryGetValue(rootCount, out freqInstance);
                }
                else
                {
                    freqInstance = new classFrequency();
                    frequencyTable.Add(rootCount, freqInstance);
                }
                freqInstance.addRootEntry(rootWord);
            }
        }

        private void lbAvailableWords_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx, noOfUses, pPartCode;
            String targetWord;
            bool[] isPPartUsed = new bool[6];
            classRoot rootWord;
            classParseDetail parseElement;

            if (lbAvailableWords.SelectedItem == null)
            {
                lbAvailableWords.SelectedIndex = 0;
                return;
            }
            targetWord = lbAvailableWords.SelectedItem.ToString();
            if (listOfRoots.ContainsKey(targetWord))
            {
                for (idx = 0; idx < 6; idx++) isPPartUsed[idx] = false;
                listOfRoots.TryGetValue(lbAvailableWords.SelectedItem.ToString(), out rootWord);
                labWordSummaryMsg.Text = targetWord;
                noOfUses = rootWord.orderByParseCode();
                for (idx = 0; idx < noOfUses; idx++)
                {
                    parseElement = rootWord.getParseElementByIndex(idx);
                    pPartCode = parseElement.PrincipalPartCode;
                    if (pPartCode == 0) continue;
                    if (!isPPartUsed[pPartCode - 1])
                    {
                        dgvSummary.Rows[pPartCode - 1].Cells[1].Value = parseElement.getWordFormByIndex(0);
                        isPPartUsed[pPartCode - 1] = true;
                    }
                }
                for (idx = 0; idx < 6; idx++)
                {
                    if (!isPPartUsed[idx]) dgvSummary.Rows[idx].Cells[1].Value = "";
                }
                dgvSummary.Rows[6].Cells[1].Value = rootWord.FrequencyInNT;
            }
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            int idx;
            String[] rowContents = new String[6];
            frmDetails frmVerbMaster;
            classRoot rootWord;

            for (idx = 0; idx < 6; idx++)
            {
                if (dgvSummary.Rows[idx].Cells[1].Value == null) rowContents[idx] = "";
                else rowContents[idx] = dgvSummary.Rows[idx].Cells[1].Value.ToString();
            }
            listOfRoots.TryGetValue(lbAvailableWords.SelectedItem.ToString(), out rootWord);
            frmVerbMaster = new frmDetails();
            frmVerbMaster.initialiseVerbDetails(rowContents, rootWord, bookList, globalVars);
            frmVerbMaster.Show();
        }

        private void orderEntryCheckedChanged(object sender, EventArgs e)
        {
            int tagValue;
            RadioButton currentButton = (RadioButton)sender;

            tagValue = Convert.ToInt32(currentButton.Tag);
            switch (tagValue)
            {
                case 1: lbAvailableWords.Sorted = true; break;
                case 2: populateGridByUse(); break;
                case 3: populateGridByNonUse(); break;
            }
        }

        private void populateGridByUse()
        {
            String[] wordsOfAGivenFrequency;

            lbAvailableWords.Sorted = false;
            lbAvailableWords.Items.Clear();
            foreach (KeyValuePair<int, classFrequency> frequencyPair in frequencyTable)
            {
                wordsOfAGivenFrequency = frequencyPair.Value.ListOfRootWords.Values.ToArray();
                Array.Sort(wordsOfAGivenFrequency, StringComparer.InvariantCulture);
                lbAvailableWords.Items.AddRange(wordsOfAGivenFrequency);
            }
        }

        private void populateGridByNonUse()
        {
            int[] frequencyValues;
            String[] wordsOfAGivenFrequency;
            SortedList<int, String> listOfRootWords = new SortedList<int, string>();
            classFrequency frequencyInstance;

            lbAvailableWords.Sorted = false;
            lbAvailableWords.Items.Clear();
            frequencyValues = frequencyTable.Keys.ToArray();
            Array.Reverse(frequencyValues);
            foreach (int frequency in frequencyValues)
            {
                frequencyTable.TryGetValue(frequency, out frequencyInstance);
                listOfRootWords = frequencyInstance.ListOfRootWords;
                wordsOfAGivenFrequency = listOfRootWords.Values.ToArray();
                Array.Sort(wordsOfAGivenFrequency, StringComparer.InvariantCulture);
                lbAvailableWords.Items.AddRange(wordsOfAGivenFrequency);
            }
        }

        private void incrementProgress(String progressMessage)
        {
            initDialog.updateProgress(progressMessage);
        }

        private void addControlToPanel(Panel pnlTarget, Button btnTarget)
        {
            pnlTarget.Controls.Add(btnTarget);
        }

        private void setToolTip(ToolTip ttTarget, Button targetButton, String actualTip)
        {
            ttTarget.SetToolTip(targetButton, actualTip);

        }

        private void tmrInitial_Tick(object sender, EventArgs e)
        {
            if (loadThread.IsAlive)
            {
                tmrInitial.Enabled = true;
                return;
            }
            tmrInitial.Enabled = false;
            if (lbAvailableWords.Items.Count > 0) lbAvailableWords.SelectedIndex = 0;
            this.Visible = true;
            initDialog.Close();
        }

        private void addListboxEntry(ListBox lbTarget, String entry)
        {
            lbTarget.Items.Add(entry);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            String fileName;
            frmHelp pPartsHelp;

            fileName = globalVars.BaseDirectory + globalVars.HelpDirectory + @"\Help.html";
            pPartsHelp = new frmHelp(fileName);
            pPartsHelp.Show();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmAbout aboutForm;

            aboutForm = new frmAbout();
            aboutForm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
