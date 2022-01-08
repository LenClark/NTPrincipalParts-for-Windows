using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPrincipalParts
{
    public class classChapter
    {
        /*==================================================================================*
         *                                                                                  *
         *                                 classChapter                                     *
         *                                 ============                                     *
         *                                                                                  *
         *  In the LXX we have two specific phenomena:                                      *
         *    (a) consecutive verses such as 22, 22a, 22b ...                               *
         *    (b) chapters can be out of sequence (e.g. chapter 24 follwed by chapter 30)   *
         *    (c) out-of-sequence repeats of chapters - e.g. chapters 24, 30, 24 (although  *
         *        _verses_ are never repeated within chapters - other than point a)         *
         *                                                                                  *
         *  So, our processing must cater for this.                                         *
         *                                                                                  *
         *  All access will be by sequential numbers, created in the code.  However, we     *
         *    need to be able to identify the correct chapter reference from the sequence.  *
         *    So, we will also reference a chapter selection class that allows us to        *
         *    recover the correct chapter sequence number from a chapter reference and      *
         *    verse reference.                                                              *
         *                                                                                  *
         *==================================================================================*/

        int noOfVerses = 0;
        SortedList<String, classVerse> verseList = new SortedList<String, classVerse>();
        SortedList<int, String> verseLookup = new SortedList<int, String>();

        public int NoOfVerses { get => noOfVerses; set => noOfVerses = value; }

        public classVerse addVerse(String verseNo, int verseSeq)
        {
            classVerse currentVerse;

            if (verseList.ContainsKey(verseNo))
            {
                verseList.TryGetValue(verseNo, out currentVerse);
            }
            else
            {
                currentVerse = new classVerse();
                verseList.Add(verseNo, currentVerse);
                verseLookup.Add(verseSeq, verseNo);
                if (verseSeq > noOfVerses) noOfVerses = verseSeq;
            }
            return currentVerse;
        }

        public String getVerseText(int verseSeq)
        {
            String verseRef;
            classVerse currentVerse = null;

            if (verseLookup.ContainsKey(verseSeq))
            {
                verseLookup.TryGetValue(verseSeq, out verseRef);
                if (verseList.ContainsKey(verseRef))
                {
                    verseList.TryGetValue(verseRef, out currentVerse);
                }
            }
            if (currentVerse == null) return "";
            else return currentVerse.TextOfVerse;
        }

        public String getVerseRefBySequence(int seq)
        {
            String verseRef = "";

            if (verseLookup.ContainsKey(seq))
            {
                verseLookup.TryGetValue(seq, out verseRef);
            }
            return verseRef;
        }
    }
}
