using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPrincipalParts
{
    public class classBooks
    {
        /*=========================================================================================================*
         *                                                                                                         *
         *                                               classBooks                                                *
         *                                               ==========                                                *
         *                                                                                                         *
         *  This class serves two purposes:                                                                        *
         *    (a) simple repository of information about each book of the Bible and the relevant text file;        *
         *    (b) the root class for accessing references (book --> chapter --> verse)                             *
         *                                                                                                         *
         *  This second purpose carries some significant implications.  In the LXX we have two specific phenomena: *
         *    (a) consecutive verses such as 22, 22a, 22b ...                                                      *
         *    (b) chapters can be out of sequence (e.g. chapter 24 follwed by chapter 30)                          *
         *    (c) out-of-sequence repeats of chapters - e.g. chapters 24, 30, 24 (although _verses_ are never      *
         *        repeated within chapters - other than point a)                                                   *
         *                                                                                                         *
         *  So, our processing must cater for this.                                                                *
         *                                                                                                         *
         *  All access will be by sequential numbers, created in the code.  However, we need to be able to         *
         *    identify the correct chapter reference from the sequence. So, we will also reference a chapter       *
         *    selection class that allows us to recover the correct chapter sequence number from a chapter         *
         *    reference and verse reference.                                                                       *
         *                                                                                                         *
         *=========================================================================================================*/

        bool isNTBook;
        int noOfChapters = 0, actualBookNumber;
        String bookName, bookShortName, bookFileName;

        /*---------------------------------------------------------------------------------------------------------*
         *                                                                                                         *
         *                                              chapterList                                                *
         *                                              -----------                                                *
         *                                                                                                         *
         *  This list allows us to store and identify a specific chapter within a book from its internally         *
         *    generated sequence number.                                                                           *
         *                                                                                                         *
         *  Key:   The sequence number;                                                                            *
         *  Value: The address of the instance of the relevant chapter.                                            *
         *                                                                                                         *
         *---------------------------------------------------------------------------------------------------------*/
        SortedList<int, classChapter> chapterList = new SortedList<int, classChapter>();

        /*---------------------------------------------------------------------------------------------------------*
         *                                                                                                         *
         *                                           chapterReferenceList                                          *
         *                                           --------------------                                          *
         *                                                                                                         *
         *  This list allows us to store the chapter number as provided by the source data (in string form).       *
         *    While the sequence number is a simple sequence, chapter references may be out of sequence and may    *
         *    even be repeated after a break.                                                                      *
         *                                                                                                         *
         *  Key:   The sequence number;                                                                            *
         *  Value: The string chapter number as provided by the source data.                                       *
         *                                                                                                         *
         *---------------------------------------------------------------------------------------------------------*/
        SortedList<int, String> chapterReferenceList = new SortedList<int, String>();

        /*---------------------------------------------------------------------------------------------------------*
         *                                                                                                         *
         *                                              chapterDecode                                              *
         *                                              -------------                                              *
         *                                                                                                         *
         *  This list returns an instance for a given chapter reference.  Once acquired, the coder must use        *
         *    getDecodeEntryForVerseReference to get the chapter sequence that applies to a given verse (and which *
         *    allows him or her to get the verse details).                                                         *
         *                                                                                                         *
         *  Key:   The chapter referece,as provided by the source data;                                            *
         *  Value: The address of a decode instance, allowing retrieval of the code-generated sequence number.     *
         *                                                                                                         *
         *---------------------------------------------------------------------------------------------------------*/
        SortedList<String, classChapterDecode> chapterDecode = new SortedList<string, classChapterDecode>();

        public int NoOfChapters { get => noOfChapters; set => noOfChapters = value; }
        public int ActualBookNumber { get => actualBookNumber; set => actualBookNumber = value; }
        public string BookName { get => bookName; set => bookName = value; }
        public string BookShortName { get => bookShortName; set => bookShortName = value; }
        public string BookFileName { get => bookFileName; set => bookFileName = value; }
        public bool IsNTBook { get => isNTBook; set => isNTBook = value; }
        internal SortedList<string, classChapterDecode> ChapterDecode { get => chapterDecode; set => chapterDecode = value; }

        public classChapter addChapter(String chapterNo, int chapterSeq)
        {
            classChapter currentChapter;

            if (chapterList.ContainsKey(chapterSeq))
            {
                chapterList.TryGetValue(chapterSeq, out currentChapter);
            }
            else
            {
                currentChapter = new classChapter();
                chapterList.Add(chapterSeq, currentChapter);
                chapterReferenceList.Add(chapterSeq, chapterNo);
                if (chapterSeq > noOfChapters) noOfChapters = chapterSeq;
            }
            return currentChapter;
        }

        public void addDecodeInformation( String chapterNo, String verseNo, int chapterSeq)
        {
            classChapterDecode currentDecode;

            if (chapterDecode.ContainsKey( chapterNo) )
            {
                chapterDecode.TryGetValue(chapterNo, out currentDecode);
            }
            else
            {
                currentDecode = new classChapterDecode();
                chapterDecode.Add(chapterNo, currentDecode);
            }
            currentDecode.addDecodeEntry(verseNo, chapterSeq);
        }

        public classChapter getChapterBySequence(int seq)
        {
            classChapter currentChapter = null;

            if (chapterList.ContainsKey(seq))
            {
                chapterList.TryGetValue(seq, out currentChapter);
            }
            return currentChapter;
        }

        public String getChapterRefBySequence( int seq)
        {
            String chapRef = "";

            if( chapterReferenceList.ContainsKey( seq) )
            {
                chapterReferenceList.TryGetValue(seq, out chapRef);
            }
            return chapRef;
        }

        public classChapter getChapterByChapterRef( String chapterRef, String relevantVerseRef )
        {
            int seqNo = -1;
            classChapterDecode currentDecode = null;

            if (chapterDecode.ContainsKey(chapterRef))
            {
                chapterDecode.TryGetValue(chapterRef, out currentDecode);
                if (currentDecode != null) seqNo = currentDecode.getDecodeEntryForVerseReference(relevantVerseRef);
                return getChapterBySequence(seqNo);
            }
            return null;
        }
    }
}
