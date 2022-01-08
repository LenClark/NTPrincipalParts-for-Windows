using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPrincipalParts
{
    public class classRoot
    {
        /*=========================================================================================================*
         *                                                                                                         *
         *                                              classRoot                                                  *
         *                                              =========                                                  *
         *                                                                                                         *
         *  This class holds all information relating to the root word.  In practice, we hold very little          *
         *    information specifically relating to the root.  Only:                                                *
         *                                                                                                         *
         *  rootWord            The word provided by the source data as root                                       *
         *  frequencyInNT       The number of times the entire word is used in the NT                              *
         *  frequencyInLXX      The number of times the entire word is used in the LXX                             *
         *  parsedDetails       A list of each specific form used for the root (see below)                         *
         *                                                                                                         *
         *  All other data is effectively passed through to one or other parseDetail instance.                     *
         *                                                                                                         *
         *=========================================================================================================*/
        bool isParseOrdered = false;
        int frequencyInNT = 0, frequencyInOT = 0, parseCount = 0;
        String rootWord;

        /*---------------------------------------------------------------------------------------------------------*
         *                                                                                                         *
         *                                            parsedDetails                                                *
         *                                            =============                                                *
         *                                                                                                         *
         *  A list of all parsedDetail instances created for the given root, identified by parseCode               *
         *                                                                                                         *
         *  key:   the parseCode of the current word form                                                          *
         *  value: the parsedDetail instance for the keyed parseCode                                               *
         *                                                                                                         *
         *---------------------------------------------------------------------------------------------------------*/
        SortedList<int, classParseDetail> parsedDetails = new SortedList<int, classParseDetail>();
        SortedList<int, int> parseCodeDecode = new SortedList<int, int>();

        public int FrequencyInNT { get => frequencyInNT; set => frequencyInNT = value; }
        public int FrequencyInOT { get => frequencyInOT; set => frequencyInOT = value; }
        public int ParseCount { get => parseCount; set => parseCount = value; }
        public SortedList<int, classParseDetail> ParsedDetails { get => parsedDetails; set => parsedDetails = value; }

        public void addRootInformation( bool isNT, String specificWord, String noAccent, int parseCode, int principalPartCode, 
            String bookName, String chapter, String verse, int bookNo, int chapterSeq, int verseSeq,
            Tuple<int, int, int, int, int, int, int> inBreakdown, classComplexProcessing gkMethod)
        {
            /*=========================================================================================================*
             *                                                                                                         *
             *                                           addNTRootInformation                                          *
             *                                           ====================                                          *
             *                                                                                                         *
             *  The actual root form is contained in rootWordList and rootWordIndex in the frmMain code.               *
             *                                                                                                         *
             *  Parameters:                                                                                            *
             *  ==========                                                                                             *
             *                                                                                                         *
             *  specificWord        The word as used in the text                                                       *
             *  parseCode           The parse code (processed)                                                         *
             *  principalPartCode   A code that identifies to which Principal Part this specific word belongs          *
             *  bookName            The name of the book                                                               *
             *  chapter             String value of chapter                                                            *
             *  verse               String value of verse                                                              *
             *  inBreakdown         A breakdown of the parse code, as follows:                                         *
             *                        item                 meaning                                                     *
             *                         0      person - 0 (none), 1, 2 or 3                                             *
             *                         1      singular or plural - 1 or 2                                              *
             *                         2      case - 1 - 5 (Nom., Voc., Acc., Gen., Dat.)                              *
             *                         3      gender - 1 = masc, 2 = neut, 3 = fem                                     *
             *                         4      tense - 1 = present                                                      *
             *                                        2 = imperfect                                                    *
             *                                        3 = aorist                                                       *
             *                                        4 = perfect                                                      *
             *                                        5 = pluperfect                                                   *
             *                                        6 = future                                                       *
             *                         5      Mood - 1 = indicative                                                    *
             *                                       2 = imperative                                                    *
             *                                       3 = subjunctive                                                   *
             *                                       4 = optative                                                      *
             *                                       5 = infinitive                                                    *
             *                                       6 = participle                                                    *
             *                         6      Voice - 1 - 3 (Active, Middle, Passive)                                  *
             *                                                                                                         *
             *=========================================================================================================*/

            classParseDetail newParseElement;

            if (isNT) frequencyInNT++;
            else frequencyInOT++;
            if (!parsedDetails.ContainsKey(parseCode))
            {
                newParseElement = new classParseDetail();
                parsedDetails.Add(parseCode, newParseElement);
            }
            else
            {
                parsedDetails.TryGetValue(parseCode, out newParseElement);
            }
            newParseElement.addParseElement( specificWord, noAccent, principalPartCode, parseCode, bookName, chapter, verse, bookNo, chapterSeq, verseSeq, gkMethod, isNT);
            newParseElement.GrammarBreakdown = inBreakdown;
        }

        public int orderByParseCode()
        {
            if (isParseOrdered) return parseCount;
            foreach (KeyValuePair<int, classParseDetail> sortPair in parsedDetails)
            {
                parseCodeDecode.Add(parseCount++, sortPair.Key);
            }
            isParseOrdered = true;
            return parseCount;
        }

        public classParseDetail getParseElementByIndex(int index)
        {
            int parseCode;
            classParseDetail parseElement;

            if (!parseCodeDecode.ContainsKey(index)) return null;
            parseCodeDecode.TryGetValue(index, out parseCode);
            if (!parsedDetails.ContainsKey(parseCode)) return null;
            parsedDetails.TryGetValue(parseCode, out parseElement);
            return parseElement;
        }
    }
}
