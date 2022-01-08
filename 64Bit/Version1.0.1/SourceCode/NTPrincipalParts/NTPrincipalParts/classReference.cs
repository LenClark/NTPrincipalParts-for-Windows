using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPrincipalParts
{
    public class classReference
    {
        int bookNo, chapterSeq, verseSeq;
        String bookName, chapter, verse;

        public int BookNo { get => bookNo; set => bookNo = value; }
        public int ChapterSeq { get => chapterSeq; set => chapterSeq = value; }
        public int VerseSeq { get => verseSeq; set => verseSeq = value; }
        public string BookName { get => bookName; set => bookName = value; }
        public string Chapter { get => chapter; set => chapter = value; }
        public string Verse { get => verse; set => verse = value; }
    }
}
