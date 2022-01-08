using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPrincipalParts
{
    class classChapterDecode
    {
        SortedList<String, int> chapterAndVerseDecode = new SortedList<string, int>();

        public SortedList<string, int> ChapterAndVerseDecode { get => chapterAndVerseDecode; set => chapterAndVerseDecode = value; }

        public void addDecodeEntry( String verseRef, int chapterSequence)
        {
            if (!chapterAndVerseDecode.ContainsKey(verseRef)) chapterAndVerseDecode.Add(verseRef, chapterSequence);
        }

        public int getDecodeEntryForVerseReference( String verseRef )
        {
            int foundVerseSeq = -1;
            chapterAndVerseDecode.TryGetValue(verseRef, out foundVerseSeq);
            return foundVerseSeq;
        }
    }
}
