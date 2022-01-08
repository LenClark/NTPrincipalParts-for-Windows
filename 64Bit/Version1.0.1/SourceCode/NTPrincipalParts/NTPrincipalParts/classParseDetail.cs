using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPrincipalParts
{
    public class classParseDetail
    {
        /*======================================================================================================================*
         *                                                                                                                      *
         *                                                 classParseElement                                                    *
         *                                                 =================                                                    *
         *                                                                                                                      *
         *  This class will instantiate a specific grammatical element of a verb. So, for example, there will be a single entry *
         *     for present imperative active singular and another for aorist active infinitive.                                 *
         *                                                                                                                      *
         *  The class contains the following data:                                                                              *
         *                                                                                                                      *
         *  specificWordForm: In theory, this ought always to have only a single entry but, in practice, occurrences can vary   *
         *                    in accent and, rarely, dialectical variation.  There can also be grammatical variations (for      *
         *                    example, omission of the final sigma before a following vowel).                                   *
         *  plainWordForm:    This is simply the plain form version of each specific word form (i.e. no accents, etc but does   *
         *                    include breathings).  *These* are the bases of deciding which specific words are valid.           *
         *                                                                                                                      *
         *======================================================================================================================*/

        bool isFirstOccurrence = true;
        int wordFormCount = 0, refCount = 0, parseCode = 0, principalPartCode;
        /*======================================================================================================================*
         *                                                                                                                      *
         *                                                   ntAndLxxCode                                                       *
         *                                                   ============                                                       *
         *                                                                                                                      *
         *  This tells us whether the form is only found in the NT, only found in the LXX or found in both.  The code telling   *
         *     us this is:                                                                                                      *
         *                                                                                                                      *
         *    Code                                            Meaning                                                           *
         *                                                                                                                      *
         *     0    Not allocated to any text                                                                                   *
         *     1    Only found in the New Testament                                                                             *
         *     2    Only found in the LXX                                                                                       *
         *     3    Found in both                                                                                               *
         *                                                                                                                      *
         *======================================================================================================================*/
        int ntAndLxxCode;

        /*----------------------------------------------------------------------------------------------------------------------*
         *                                                                                                                      *
         *                                                  specificWordForm                                                    *
         *                                                  ================                                                    *
         *                                                                                                                      *
         *  This is the form of the word which we are actually interested in.  It will vary for different grammatical functions *
         *     - e.g. 1st person singular present indicative will be different from second person.  Each class instance is for  *
         *     a specific "grammatical function".  However, in some cases there may be several variants of the same grammatical *
         *     function - the same parse code.  So, the list, specificWordForm, will capture one or more forms of that unit.    *
         *                                                                                                                      *
         *  The list is formed as follows:                                                                                      *
         *    key:   a fairly meaningless (sequential) count                                                                    *
         *    value: the specific word form                                                                                     *
         *                                                                                                                      *
         *----------------------------------------------------------------------------------------------------------------------*/
        SortedList<int, String> specificWordForm = new SortedList<int, string>();

        /*----------------------------------------------------------------------------------------------------------------------*
         *                                                                                                                      *
         *                                                   plainWordForm                                                      *
         *                                                   =============                                                      *
         *                                                                                                                      *
         *  The simplified ("plain") form of the current word form is added to this list.  So, future occurrences can be        *
         *     compared with it to assess whether the form has occurred before.                                                 *
         *                                                                                                                      *
         *  We need to get rid of extraneous character furniture for several reasons:                                           *
         *     (a) most significant, for some characters (notably vowels with accute accents) there are potentially two Unicode *
         *           representations.  This means that words that are visually identical may have different electronic forms.   *
         *           Stripping the accents will remove this anomaly.                                                            *
         *     (b) some words may take varying accents when used in varying contexts.  This process will also remove their      *
         *           effect.                                                                                                    *
         *                                                                                                                      *
         *  Note: we do _not_ key these entries on the parse code because the parse code may have several different forms.  So, *
         *          this process allows us to handle variations in a given parse code.                                          *
         *                                                                                                                      *
         *  The list is formed as follows:                                                                                      *
         *    key:   a fairly meaningless (sequential) count - the same value as used in specificWordForm                       *
         *    value: the stripped version of the current word form                                                              *
         *                                                                                                                      *
         *----------------------------------------------------------------------------------------------------------------------*/
        SortedList<int, String> plainWordForm = new SortedList<int, string>();
        SortedList<int, classReference> referenceList = new SortedList<int, classReference>();
        SortedList<String, int> referenceControl = new SortedList<string, int>();
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
        classComplexProcessing complexProcessing;

        public int WordFormCount { get => wordFormCount; }
        public int RefCount { get => refCount; }
        public int ParseCode { get => parseCode; set => parseCode = value; }
        public int PrincipalPartCode { get => principalPartCode; set => principalPartCode = value; }
        public int NtAndLxxCode { get => ntAndLxxCode; set => ntAndLxxCode = value; }
        public Tuple<int, int, int, int, int, int, int> GrammarBreakdown { get => grammarBreakdown; set => grammarBreakdown = value; }
        internal SortedList<int, classReference> ReferenceList { get => referenceList; set => referenceList = value; }

        public void addParseElement(String currentWord, String noAccent, int pPartCode, int pCode, 
            String inBook, String inChap, String inVerse, int bookNo, int chapterSeq, int verseSeq,
            classComplexProcessing gkMethod, bool isFromNT)
        {
            String refString, modifiedWordForm;
            classReference reference;

            if (isFromNT)
            {
                if (ntAndLxxCode == 0) ntAndLxxCode = 1;
            }
            else
            {
                if (ntAndLxxCode < 2) ntAndLxxCode += 2;
            }
            if( isFirstOccurrence )
            {
                complexProcessing = gkMethod;
                principalPartCode = pPartCode; // I.e. which of the principal part categories the current form belongs to
                parseCode = pCode;  // I.e. the integer representation of the source parse code
                isFirstOccurrence = false;
            }
            if (!plainWordForm.ContainsValue(noAccent))
            {
                // Does our list of words contain the stripped word.  If not, it's a first!
                plainWordForm.Add(wordFormCount, noAccent);
                // We also want the accented word but *not* if it has graves or more than one accent
                modifiedWordForm = gkMethod.removeSpuriousAccents(currentWord);
                specificWordForm.Add(wordFormCount++, modifiedWordForm);
            }
            refString = inBook + " " + inChap + ":" + inVerse;
            if (referenceControl.ContainsKey(refString)) return;
            // So, if we get here, it is a new reference
            reference = new classReference();
            reference.BookName = inBook;
            reference.Chapter = inChap;
            reference.Verse = inVerse;
            reference.BookNo = bookNo;
            reference.ChapterSeq = chapterSeq;
            reference.VerseSeq = verseSeq;
            referenceControl.Add(refString, refCount);
            referenceList.Add(refCount++, reference);
        }

        public String getWordFormByIndex(int index)
        {
            String acqWord;

            if (!specificWordForm.ContainsKey(index)) return "";
            specificWordForm.TryGetValue(index, out acqWord);
            return acqWord;
        }

        public String getReferenceByIndex(int index)
        {
            classReference thisRef;

            if (!referenceList.ContainsKey(index)) return "";
            referenceList.TryGetValue(index, out thisRef);
            return thisRef.BookName + " " + thisRef.Chapter + ":" + thisRef.Verse;
        }
    }
}
