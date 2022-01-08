using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPrincipalParts
{
    public class classComplexProcessing
    {

        /*=========================================================================*
         *   Variables for cleaning the source data (Greek):                       *
         *   ==============================================                        *
         *=========================================================================*/

        SortedDictionary<Char, Char> baseCharLookup = new SortedDictionary<char, char>();
        SortedDictionary<Char, Char> initialConv = new SortedDictionary<char, char>();
        SortedSet<Char> allowedChars = new SortedSet<char>();

        /*------------------------------------------------------------------------------------------------------------*
         *                                                                                                            *
         *                                            GravToAccute                                                    *
         *                                            ------------                                                    *
         *                                                                                                            *
         *  Because we are looking at words as they are used in flowing text, there will be contexts where some words *
         *    are given grave accents.  When these words are consiodered on their own (as lexical items), those       *
         *    accents are naturally accute.  This conversion table enables the substitution of accute accents for     *
         *    graves.                                                                                                 *
         *                                                                                                            *
         *  Key:   A specific character carrying a grave accent.                                                      *
         *  Value: The equivalent character (including e.g. breathing or iota subscript) with an accute accent.       *
         *                                                                                                            *
         *------------------------------------------------------------------------------------------------------------*/
        SortedDictionary<Char, Char> GravToAccute = new SortedDictionary<char, char>();

        /*------------------------------------------------------------------------------------------------------------*
         *                                                                                                            *
         *                                             GravToBase                                                     *
         *                                             ----------                                                     *
         *                                                                                                            *
         *  This is much as GravToAccute but replaces the grave accented character with a native character.  This is  *
         *    used where the grave is a second accent on a word.                                                      *
         *                                                                                                            *
         *  Key:   A specific character carrying a grave accent.                                                      *
         *  Value: The equivalent character (including e.g. breathing or iota subscript) with no accent.              *
         *                                                                                                            *
         *------------------------------------------------------------------------------------------------------------*/
        SortedDictionary<Char, Char> GravToBase = new SortedDictionary<char, char>();

        /*=========================================================================*
         *   Variables for parse-encoding the source data code:                    *
         *   ============================================                          *
         *=========================================================================*/
        int principalPartCode = 0;
        /*-------------------------------------------------------------------------*
         *   grammarBreakdown:                                                     *
         *   ----------------                                                      *
         *   Stores the code value for each parse point (e.g. 1/2/3rd person,      *
         *     sing/plur, tense, etc.).  Items (positions in Tuple):               *
         *                                                                         *
         *      1   Person (values 0 to 3)                                         *
         *      2   Number (values 1 = singular, 2 = plural)                       *
         *      3   Case (values 1 to 5 and include vocative [=2])                 *
         *      4   Gender (values 1 to 3)                                         *
         *      5   Tense (values 1 to 6)                                          *
         *      6   Mood (values 1 to 6)                                           *
         *      7   Voice (values 1 to 3)                                          *
         *-------------------------------------------------------------------------*/
        Tuple<int, int, int, int, int, int, int> grammarBreakdown;

        public Tuple<int, int, int, int, int, int, int> GrammarBreakdown { get => grammarBreakdown; set => grammarBreakdown = value; }
        public int PrincipalPartCode { get => principalPartCode; set => principalPartCode = value; }
        public SortedDictionary<char, char> BaseCharLookup { get => baseCharLookup; set => baseCharLookup = value; }

        public void initialiseLookup()
        {
            /*=================================================================================================================*
             *                                                                                                                 *
             *                                           initialiseLookup                                                      *
             *                                           ================                                                      *
             *                                                                                                                 *
             *  The purpose of the resulting lookup "table" is to provide a means of sorting Greek words in a useful way.      *
             *  This will not only allow case-insensitive sorts (which can't be done with a simple sort) but also a sort that  *
             *  ignores accents, breathings and other diacritics.                                                              *
             *                                                                                                                 *
             *  Essentially, a base character is identified for every possible Greek character.  So, for example, 'ἁ', 'ἂ',    *
             *  'ἃ', 'ἄ', 'ἅ', 'Ἄ', 'Ἅ', 'Ἆ' and 'Ἇ' will all translate to 'α'.                                                *
             *                                                                                                                 *
             *=================================================================================================================*/

            int idx, indexMax, totalChars, jdx, runningCount = 0;
            Char[] gkLetters = { 'ἀ', 'ἁ', 'ἂ', 'ἃ', 'ἄ', 'ἅ', 'ἆ', 'ἇ', 'Ἀ', 'Ἁ', 'Ἂ', 'Ἃ', 'Ἄ', 'Ἅ', 'Ἆ', 'Ἇ', 'ᾀ', 'ᾁ', 'ᾂ', 'ᾃ', 'ᾄ', 'ᾅ', 'ᾆ', 'ᾇ',
                                  'ᾈ', 'ᾉ', 'ᾊ', 'ᾋ', 'ᾌ', 'ᾍ', 'ᾎ', 'ᾏ', 'ᾰ', 'ᾱ', 'ᾲ', 'ᾳ', 'ᾴ', '᾵', 'ᾶ', 'ᾷ', 'Ᾰ', 'Ᾱ', 'Ὰ', 'Ά', 'ᾼ', 'ὰ', 'ά',
                                  'ἐ', 'ἑ', 'ἒ', 'ἓ', 'ἔ', 'ἕ', '἖', '἗', 'Ἐ', 'Ἑ', 'Ἒ', 'Ἓ', 'Ἔ', 'Ἕ', 'ὲ', 'έ', 'Ὲ', 'Έ',
                                  'ἠ', 'ἡ', 'ἢ', 'ἣ', 'ἤ', 'ἥ', 'ἦ', 'ἧ', 'Ἠ', 'Ἡ', 'Ἢ', 'Ἣ', 'Ἤ', 'Ἥ', 'Ἦ', 'Ἧ', 'ᾐ', 'ᾑ', 'ᾒ', 'ᾓ', 'ᾔ', 'ᾕ', 'ᾖ',
                                  'ᾗ', 'ᾘ', 'ᾙ', 'ᾚ', 'ᾛ', 'ᾜ', 'ᾝ', 'ᾞ', 'ᾟ', 'ῂ', 'ῃ', 'ῄ', 'ῆ', 'ῇ', 'Ὴ', 'Ή', 'ῌ', 'ὴ', 'ή',
                                  'ἰ', 'ἱ', 'ἲ', 'ἳ', 'ἴ', 'ἵ', 'ἶ', 'ἷ', 'Ἰ', 'Ἱ', 'Ἲ', 'Ἳ', 'Ἴ', 'Ἵ', 'Ἶ', 'Ἷ', 'ῐ', 'ῑ', 'ῒ', 'ΐ', 'ῖ', 'ῗ', 'Ῐ', 'Ῑ', 'Ὶ', 'Ί', 'ὶ', 'ί',
                                  'ὀ', 'ὁ', 'ὂ', 'ὃ', 'ὄ', 'ὅ', 'Ὀ', 'Ὁ', 'Ὂ', 'Ὃ', 'Ὄ', 'Ὅ', 'ὸ', 'ό',
                                  'ὐ', 'ὑ', 'ὒ', 'ὓ', 'ὔ', 'ὕ', 'ὖ', 'ὗ', 'Ὑ', 'Ὓ', 'Ὕ', 'Ὗ', 'ῠ', 'ῡ', 'ῢ', 'ΰ', 'ῦ', 'ῧ', 'Ῠ', 'Ῡ', 'Ὺ', 'Ύ', 'ὺ', 'ύ',
                                  'ὠ', 'ὡ', 'ὢ', 'ὣ', 'ὤ', 'ὥ', 'ὦ', 'ὧ', 'Ὠ', 'Ὡ', 'Ὢ', 'Ὣ', 'Ὤ', 'Ὥ', 'Ὦ', 'Ὧ', 'ᾠ', 'ᾡ', 'ᾢ', 'ᾣ', 'ᾤ', 'ᾥ', 'ᾦ',
                                  'ᾧ', 'ᾨ', 'ᾩ', 'ᾪ', 'ᾫ', 'ᾬ', 'ᾭ', 'ᾮ', 'ᾯ', 'ῲ', 'ῳ', 'ῴ', 'ῶ', 'ῷ', 'Ὸ', 'Ό', 'Ὼ', 'Ώ', 'ῼ', 'ὼ', 'ώ',
                                  'ῤ', 'ῥ', 'Ῥ' };
            Char[] gkBaseLetters = { 'α', 'ε', 'η', 'ι', 'ο', 'υ', 'ω', 'ρ' };

            int[] gkLetterCounts = { 47, 18, 42, 28, 14, 24, 44, 3 };
            Char[] majiscules = { 'Α', 'Β', 'Γ', 'Δ', 'Ε', 'Ζ', 'Η', 'Θ', 'Ι', 'Κ', 'Λ', 'Μ', 'Ν', 'Ξ', 'Ο', 'Π', 'Ρ', 'Σ', 'Τ', 'Υ', 'Φ', 'Χ', 'Ψ', 'Ω' };
            Char[] miniscules = { 'α', 'β', 'γ', 'δ', 'ε', 'ζ', 'η', 'θ', 'ι', 'κ', 'λ', 'μ', 'ν', 'ξ', 'ο', 'π', 'ρ', 'σ', 'τ', 'υ', 'φ', 'χ', 'ψ', 'ω' };
            Char[] restToBeChanged = { 'Ά', 'Έ', 'Ή', 'Ί', 'Ό', 'Ύ', 'Ώ', 'Ϊ', 'Ϋ', 'ά', 'έ', 'ή', 'ί', 'ΰ', 'ϊ', 'ϋ', 'ό', 'ύ', 'ώ' };
            Char[] restComparable = { 'α', 'ε', 'η', 'ι', 'ο', 'υ', 'ω', 'ι', 'υ', 'α', 'ε', 'η', 'ι', 'ο', 'ι', 'υ', 'ο', 'υ', 'ω' };
            Char[] simpleAlphabet = { 'α', 'β', 'γ', 'δ', 'ε', 'ζ', 'η', 'θ', 'ι', 'κ', 'λ', 'μ', 'ν', 'ξ', 'ο', 'π', 'ρ', 'ς', 'σ', 'τ', 'υ', 'φ', 'χ', 'ψ', 'ω' };
            Char[] possibleCaps = { 'Α', 'Ἀ', 'Ἁ', 'Ἂ', 'Ἃ', 'Ἄ', 'Ἅ', 'Β', 'Γ', 'Δ', 'Ε', 'Ἐ', 'Ἑ', 'Ἒ', 'Ἓ', 'Ἔ', 'Ἕ', 'Ζ', 'Η', 'Ἠ', 'Ἡ', 'Ἢ', 'Ἣ', 'Ἤ', 'Ἥ', 'Θ',
                                  'Ι', 'Ἰ', 'Ἱ', 'Ἲ', 'Ἳ', 'Ἴ', 'Ἵ', 'Κ', 'Λ', 'Μ', 'Ν', 'Ξ', 'Ο', 'Ὀ', 'Ὁ', 'Ὂ', 'Ὃ', 'Ὄ', 'Ὅ', 'Π', 'Ρ', 'Σ', 'Τ',
                                  'Υ', 'Ὑ', 'Ὓ', 'Ὕ', 'Φ', 'Χ', 'Ψ', 'Ω', 'Ὠ', 'Ὡ', 'Ὢ', 'Ὣ', 'Ὤ', 'Ὥ' };
            Char[] replacementMins = { 'α', 'ἀ', 'ἁ', 'ἂ', 'ἃ', 'ἄ', 'ἅ', 'β', 'γ', 'δ', 'ε', 'ἐ', 'ἑ', 'ἒ', 'ἓ', 'ἔ', 'ἕ', 'ζ', 'η', 'ἠ', 'ἡ', 'ἢ', 'ἣ', 'ἤ', 'ἥ', 'θ',
                                     'ι', 'ἰ', 'ἱ', 'ἲ', 'ἳ', 'ἴ', 'ἵ', 'κ', 'λ', 'μ', 'ν', 'ξ', 'ο', 'ὀ', 'ὁ', 'ὂ', 'ὃ', 'ὄ', 'ὅ', 'π', 'ρ', 'σ', 'τ',
                                     'υ', 'ὑ', 'ὓ', 'ὕ', 'φ', 'χ', 'ψ', 'ω', 'ὠ', 'ὡ', 'ὢ', 'ὣ', 'ὤ', 'ὥ' };
            Char[] gkVowelsWithAcc = { 'ἄ', 'ἅ', 'Ἄ', 'Ἅ', 'ᾄ', 'ᾅ','ᾌ', 'ᾍ', 'ᾴ', 'Ά', 'ά',
                                  'ἔ', 'ἕ', 'Ἔ', 'Ἕ', 'έ', 'Έ',
                                  'ἤ', 'ἥ', 'Ἤ', 'Ἥ', 'ᾔ', 'ᾕ', 'ᾜ', 'ᾝ', 'ῄ', 'Ή', 'ή',
                                  'ἴ', 'ἵ', 'Ἴ', 'Ἵ', 'ΐ', 'Ί', 'ί',
                                  'ὄ', 'ὅ', 'Ὄ', 'Ὅ', 'ό',
                                  'ὔ', 'ὕ', 'Ὕ', 'ΰ', 'Ύ', 'ύ',
                                  'ὤ', 'ὥ', 'Ὤ', 'Ὥ', 'ᾤ', 'ᾥ', 'ᾬ', 'ᾭ', 'ῴ', 'Ό', 'Ώ', 'ώ' };
            Char[] gkVowelWithGrav = { 'ἂ', 'ἃ', 'Ἂ', 'Ἃ', 'ᾂ', 'ᾃ', 'ᾊ', 'ᾋ', 'ᾲ', 'Ὰ', 'ὰ',
                                  'ἒ', 'ἓ', 'Ἒ', 'Ἓ', 'έ', 'Ὲ',
                                  'ἢ', 'ἣ', 'Ἢ', 'Ἣ', 'ᾒ', 'ᾓ', 'ᾚ', 'ᾛ', 'ῂ', 'Ὴ', 'ὴ',
                                  'ἲ', 'ἳ', 'Ἲ', 'Ἳ', 'ῒ', 'Ὶ', 'ὶ',
                                  'ὂ', 'ὃ', 'Ὂ', 'Ὃ', 'ὸ',
                                  'ὒ', 'ὓ', 'Ὓ', 'ῢ', 'Ὺ', 'ὺ',
                                  'ὢ', 'ὣ', 'Ὢ', 'Ὣ', 'ᾢ', 'ᾣ', 'ᾪ', 'ᾫ', 'ῲ', 'Ὸ', 'Ὼ', 'ὼ' };
            Char[] gkVowelWithNone = { 'ἀ', 'ἁ', 'Ἀ', 'Ἁ', 'ᾀ', 'ᾁ', 'ᾈ', 'ᾉ', 'ᾳ', 'Α', 'α',
                                       'ἐ', 'ἑ', 'Ἐ', 'Ἑ', 'ε', 'Ε',
                                        'ἠ', 'ἡ', 'Ἠ', 'Ἡ', 'ᾐ', 'ᾑ', 'ᾘ', 'ᾙ', 'ῃ', 'Η', 'η',
                                        'ἰ', 'ἱ', 'Ἰ', 'Ἱ', 'ϊ', 'Ι', 'ι',
                                        'ὀ', 'ὁ', 'Ὀ', 'Ὁ', 'ο',
                                        'ὐ', 'ὑ', 'Ὑ', 'ϋ', 'Υ', 'υ',
                                        'ὠ', 'ὡ', 'Ὠ', 'Ὡ', 'ᾠ', 'ᾡ', 'ᾨ', 'ᾩ', 'ῳ', 'Ο', 'Ω', 'ω' };

            /*------------------------------------------------------------------------------------------------------------*
             *                                                                                                            *
             *                                       Populate: baseCharLookup                                             *
             *                                       ------------------------                                             *
             *                                                                                                            *
             *  Purpose: to create a lookup "table" which converts Greek vowels (and rho) with accents etc to simple,     *
             *           unadorned equivalents (e.g. ἁ to α).                                                             *
             *                                                                                                            *
             *  Key data is provided by the array, gkLetters.                                                             *
             *  Equivalent bare characters are given in gkBaseLetters.                                                    *
             *  The array gkLetterCounts tells us how many alpha-equivalent characters are in gkLetters, how many         *
             *    epsilon-equivalent characters, and so on.  This means that we only need to store the base characters    *
             *    in gkBaseLetters once rather than match the non-base occurrences.                                       *
             *                                                                                                            *
             *------------------------------------------------------------------------------------------------------------*/
            indexMax = gkLetterCounts.Length;
            for (jdx = 0; jdx < indexMax; jdx++)
            {
                for (idx = 0; idx < gkLetterCounts[jdx]; idx++)
                {
                    baseCharLookup.Add(gkLetters[runningCount++], gkBaseLetters[jdx]);
                }
            }

            /*------------------------------------------------------------------------------------------------------------*
             *                                                                                                            *
             *                                       Populate: baseCharLookup (continued)                                 *
             *                                       ------------------------                                             *
             *                                                                                                            *
             *  Purpose: This adds base majiscules to the key data.  So baseCharLookup will also convert majiscules to    *
             *           base miniscules.  Note that this includes the conversion of consonants.                          *
             *                                                                                                            *
             *  Key data is provided by the array, majiscules.                                                            *
             *  Equivalent bare characters are given in miniscules.                                                       *
             *                                                                                                            *
             *------------------------------------------------------------------------------------------------------------*/
            indexMax = majiscules.Length;
            for (idx = 0; idx < indexMax; idx++)
            {
                baseCharLookup.Add(majiscules[idx], miniscules[idx]);
            }

            /*------------------------------------------------------------------------------------------------------------*
             *                                                                                                            *
             *                                       Populate: baseCharLookup (continued)                                 *
             *                                       ------------------------                                             *
             *                                                                                                            *
             *  Purpose: This adds a collection of characters that occur in the lower range of Unicode values.            *
             *                                                                                                            *
             *  Key data is provided by the array, restToBeChanged.                                                       *
             *  Equivalent bare characters are given in restComparable.                                                   *
             *                                                                                                            *
             *------------------------------------------------------------------------------------------------------------*/
            indexMax = restToBeChanged.Length;
            for (idx = 0; idx < indexMax; idx++)
            {
                baseCharLookup.Add(restToBeChanged[idx], restComparable[idx]);
            }

            /*------------------------------------------------------------------------------------------------------------*
             *                                                                                                            *
             *                                       Populate: baseCharLookup (continued)                                 *
             *                                       ------------------------                                             *
             *                                                                                                            *
             *  Purpose: Finally, we include the base characters themselves as both key and value members of the lookup   *
             *           "table".                                                                                         *
             *                                                                                                            *
             *  Key data and equivalent (value) characters are provided by the array, simpleAlphabet.                     *
             *                                                                                                            *
             *------------------------------------------------------------------------------------------------------------*/
            indexMax = simpleAlphabet.Length;
            for (idx = 0; idx < indexMax; idx++)
            {
                baseCharLookup.Add(simpleAlphabet[idx], simpleAlphabet[idx]);
            }

            /*------------------------------------------------------------------------------------------------------------*
             *                                                                                                            *
             *                                       Populate: initialConv                                                *
             *                                       ---------------------                                                *
             *                                                                                                            *
             *  Purpose: To enable conversion of initial capitals to lower-case equivalents.  This is needed because some *
             *           actual occurrences begin sentences and, as a result, start with an uncial.  When making a list   *
             *           of equivalent words, we may get both a word with a leading majiscule and a word that is          *
             *           identical except that it starts with a miniscule.                                                *
             *                                                                                                            *
             *                                                                                                            *
             *  Key data is provided by the array, possibleCaps.                                                          *
             *  Equivalent bare characters are given in replacementMins.                                                  *
             *                                                                                                            *
             *------------------------------------------------------------------------------------------------------------*/
            totalChars = possibleCaps.Length;
            for (idx = 0; idx < totalChars; idx++)
            {
                initialConv.Add(possibleCaps[idx], replacementMins[idx]);
            }
            for (idx = 0x0374; idx < 0x0400; idx++)
            {
                allowedChars.Add((char)idx);
            }
            for (idx = 0x1f00; idx < 0x2000; idx++)
            {
                allowedChars.Add((char)idx);
            }

            totalChars = gkVowelWithGrav.Length;
            for (idx = 0; idx < totalChars; idx++)
            {
                GravToAccute.Add(gkVowelWithGrav[idx], gkVowelsWithAcc[idx]);
            }
            totalChars = gkVowelWithGrav.Length;
            for (idx = 0; idx < totalChars; idx++)
            {
                GravToBase.Add(gkVowelWithGrav[idx], gkVowelWithNone[idx]);
            }
        }

        public String removeSpuriousAccents(String sourceWord)
        {
            int idx, noOfChars, accentCount = 0;
            Char currentCharacter, replacementChar;
            String finalWord = "";
            Char[] allAccents = { 'ἂ', 'ἃ', 'ἄ', 'ἅ', 'ἆ', 'ἇ', 'Ἀ', 'Ἁ', 'Ἂ', 'Ἃ', 'Ἄ', 'Ἅ', 'Ἆ', 'Ἇ', 'ᾂ', 'ᾃ', 'ᾄ', 'ᾅ', 'ᾆ', 'ᾇ',
                                  'ᾈ', 'ᾉ', 'ᾊ', 'ᾋ', 'ᾌ', 'ᾍ', 'ᾎ', 'ᾏ', 'ᾲ', 'ᾴ', 'ᾶ', 'ᾷ', 'Ὰ', 'Ά', 'ὰ', 'ά',
                                  'ἒ', 'ἓ', 'ἔ', 'ἕ', 'Ἐ', 'Ἑ', 'Ἒ', 'Ἓ', 'Ἔ', 'Ἕ', 'ὲ', 'έ', 'Ὲ', 'Έ',
                                  'ἢ', 'ἣ', 'ἤ', 'ἥ', 'ἦ', 'ἧ', 'Ἠ', 'Ἡ', 'Ἢ', 'Ἣ', 'Ἤ', 'Ἥ', 'Ἦ', 'Ἧ', 'ᾒ', 'ᾓ', 'ᾔ', 'ᾕ', 'ᾖ',
                                  'ᾗ', 'ᾚ', 'ᾛ', 'ᾜ', 'ᾝ', 'ᾞ', 'ᾟ', 'ῂ', 'ῄ', 'ῆ', 'ῇ', 'Ὴ', 'Ή', 'ὴ', 'ή',
                                  'ἲ', 'ἳ', 'ἴ', 'ἵ', 'ἶ', 'ἷ', 'Ἲ', 'Ἳ', 'Ἴ', 'Ἵ', 'Ἶ', 'Ἷ', 'ῒ', 'ΐ', 'ῖ', 'ῗ', 'Ὶ', 'Ί', 'ὶ', 'ί',
                                  'ὂ', 'ὃ', 'ὄ', 'ὅ', 'Ὂ', 'Ὃ', 'Ὄ', 'Ὅ', 'ὸ', 'ό',
                                  'ὒ', 'ὓ', 'ὔ', 'ὕ', 'ὖ', 'ὗ', 'Ὕ', 'Ὗ', 'ῢ', 'ΰ', 'ῦ', 'ῧ', 'Ὺ', 'Ύ', 'ὺ', 'ύ',
                                  'ὢ', 'ὣ', 'ὤ', 'ὥ', 'ὦ', 'ὧ', 'Ὢ', 'Ὣ', 'Ὤ', 'Ὥ', 'Ὦ', 'Ὧ', 'ᾢ', 'ᾣ', 'ᾤ', 'ᾥ', 'ᾦ',
                                  'ᾧ', 'ᾪ', 'ᾫ', 'ᾬ', 'ᾭ', 'ᾮ', 'ᾯ', 'ῲ', 'ῳ', 'ῴ', 'ῶ', 'ῷ', 'Ὸ', 'Ό', 'Ὼ', 'Ώ', 'ὼ', 'ώ' };

            noOfChars = sourceWord.Length;
            for (idx = 0; idx < noOfChars; idx++)
            {
                currentCharacter = sourceWord[idx];
                if (allAccents.Contains(currentCharacter))
                {
                    if (++accentCount > 1)
                    {
                        if (GravToAccute.ContainsKey(currentCharacter))
                        {
                            GravToBase.TryGetValue(currentCharacter, out replacementChar);
                            finalWord += replacementChar;
                        }
                        else finalWord += currentCharacter;
                    }
                    else
                    {
                        if (GravToAccute.ContainsKey(currentCharacter))
                        {
                            GravToAccute.TryGetValue(currentCharacter, out replacementChar);
                            finalWord += replacementChar;
                        }
                        else finalWord += currentCharacter;
                    }
                }
                else finalWord += currentCharacter;
            }
            return finalWord;
        }

        public String checkAndReplaceLeadingMajiscule(String incomingWord)
        {
            Char replacement;
            String outgoingWord;

            if (initialConv.ContainsKey(incomingWord[0]))
            {
                initialConv.TryGetValue(incomingWord[0], out replacement);
                outgoingWord = replacement.ToString() + incomingWord.Substring(1);
            }
            else outgoingWord = incomingWord;
            return outgoingWord;
        }

        public int getNTParseInfo(String inParseInfo)
        {
            /*=================================================================================================================*
             *                                                                                                                 *
             *                                            getNTParseInfo                                                       *
             *                                            ==============                                                       *
             *                                                                                                                 *
             *  Decode the Parse Code for NT source data and convert it into an integer value that uniquely identifies its     *
             *  grammatical function.  The numeric code is formed as follows:                                                  *
             *                                                                                                                 *
             *    Person:  1, 2 or 3 according to person                                                                       *
             *             Plural: add 3 to person value                                                                       *
             *             If the source code is -, Singular = 7 and Plural = 8                                                *
             *    Tense:   Add 1 - 6 times 10, (so Present adds 10, imperfect adds 20, aorist adds 30, etc.)                   *
             *    Mood:    add 1 - 6 times 100, (so indicative adds 100, imperative adds 200, etc.)                            *
             *    Voice:   add 1 - 3 times 1000, according to voice                                                            *
             *    Case:    add 1 - 5 times 10000, according to case                                                            *
             *    Gender:  add 1 - 3 times 100000, according to gender                                                         *
             *                                                                                                                 *
             *  The method also adds the same coded values to grammarStore and uses this information to identify the principal *
             *  part related to the specific word form.                                                                        *
             *                                                                                                                 *
             *=================================================================================================================*/

            const int noOfCategories = 7;
            int idx, parseCode = 0, voiceCode;
            int[] grammarStore = new int[noOfCategories];

            for (idx = 0; idx < noOfCategories; idx++) grammarStore[idx] = 0;
            switch (inParseInfo[0])
            {
                case '1':
                    grammarStore[0] = 1;
                    if (inParseInfo[5] == 'S')
                    {
                        parseCode = 1;
                        grammarStore[1] = 1;
                    }
                    else
                    {
                        parseCode = 4;
                        grammarStore[1] = 2;
                    }
                    break;
                case '2':
                    grammarStore[0] = 2;
                    if (inParseInfo[5] == 'S')
                    {
                        parseCode = 2;
                        grammarStore[1] = 1;
                    }
                    else
                    {
                        parseCode = 5;
                        grammarStore[1] = 2;
                    }
                    break;
                case '3':
                    grammarStore[0] = 3;
                    if (inParseInfo[5] == 'S')
                    {
                        parseCode = 3;
                        grammarStore[1] = 1;
                    }
                    else
                    {
                        parseCode = 6;
                        grammarStore[1] = 2;
                    }
                    break;
                case '-':
                    grammarStore[0] = 0;
                    if (inParseInfo[5] == 'S')
                    {
                        parseCode = 7;
                        grammarStore[1] = 1;
                    }
                    if (inParseInfo[5] == 'P')
                    {
                        parseCode = 8;
                        grammarStore[1] = 2;
                    }
                    break;
            }
            switch (inParseInfo[4])
            {
                case 'N': parseCode += 10000; grammarStore[2] = 1; break;    // Nominative
                case 'V': parseCode += 20000; grammarStore[2] = 2; break;    // Vocative
                case 'A': parseCode += 30000; grammarStore[2] = 3; break;    // Accusative
                case 'G': parseCode += 40000; grammarStore[2] = 4; break;    // Genitive
                case 'D': parseCode += 50000; grammarStore[2] = 5; break;    // Dative
                default: break;
            }
            switch (inParseInfo[6])
            {
                case 'M': parseCode += 100000; grammarStore[3] = 1; break;    // Masculine
                case 'N': parseCode += 200000; grammarStore[3] = 2; break;    // Neuter
                case 'F': parseCode += 300000; grammarStore[3] = 3; break;    // Feminine
                default: break;
            }
            switch (inParseInfo[1])
            {
                case 'P': parseCode += 10; grammarStore[4] = 1; break;    // Present
                case 'I': parseCode += 20; grammarStore[4] = 2; break;    // Imperfect
                case 'A': parseCode += 30; grammarStore[4] = 3; break;    // Aorist
                case 'X': parseCode += 40; grammarStore[4] = 4; break;    // Perfect
                case 'Y': parseCode += 50; grammarStore[4] = 5; break;    // Pluperfect
                case 'F': parseCode += 60; grammarStore[4] = 6; break;    // Future
                default: break;
            }
            switch (inParseInfo[3])
            {
                case 'I': parseCode += 100; grammarStore[5] = 1; break;    // Indicative
                case 'D': parseCode += 200; grammarStore[5] = 2; break;    // Imperative
                case 'S': parseCode += 300; grammarStore[5] = 3; break;    // Subjunctive
                case 'O': parseCode += 400; grammarStore[5] = 4; break;    // Optative
                case 'N': parseCode += 500; grammarStore[5] = 5; break;    // Infinitive
                case 'P': parseCode += 600; grammarStore[5] = 6; break;    // Participle
                default: break;
            }
            switch (inParseInfo[2])
            {
                case 'A': parseCode += 1000; grammarStore[6] = 1; break;    // Active
                case 'M': parseCode += 2000; grammarStore[6] = 2; break;    // Middle
                case 'P': parseCode += 3000; grammarStore[6] = 3; break;    // Passive
                default: break;
            }
            switch (inParseInfo[7])
            {
                default: break;
            }
            grammarBreakdown = new Tuple<int, int, int, int, int, int, int>(grammarStore[0], grammarStore[1], grammarStore[2],
                grammarStore[3], grammarStore[4], grammarStore[5], grammarStore[6]);
            voiceCode = (parseCode % 10000) / 1000;
            switch (grammarBreakdown.Item5)
            {
                case 1:
                case 2: principalPartCode = 1; break;
                case 3:
                    if (grammarBreakdown.Item7 < 3) principalPartCode = 3;
                    else principalPartCode = 6;
                    break;
                case 4:
                case 5:
                    if (grammarBreakdown.Item7 == 1) principalPartCode = 4;
                    else principalPartCode = 5;
                    break;
                case 6:
                    if (grammarBreakdown.Item7 < 3) principalPartCode = 2;
                    else principalPartCode = 6;
                    break;
            }
            return parseCode;
        }

        public int getLXXParseInfo(String inParseInfo)
        {
            /*=================================================================================================================*
             *                                                                                                                 *
             *                                            getLXXParseInfo                                                      *
             *                                            ===============                                                      *
             *                                                                                                                 *
             *  Decode the Parse Code for LXX source data and convert it into an integer value that uniquely identifies its    *
             *  grammatical function.  The numeric code is formed as follows:                                                  *
             *                                                                                                                 *
             *    Person:  1, 2 or 3 according to person                                                                       *
             *             Plural: add 3 to person value                                                                       *
             *             If the source code is -, Singular = 7 and Plural = 8                                                *
             *    Tense:   Add 1 - 6 times 10, (so Present adds 10, imperfect adds 20, aorist adds 30, etc.)                   *
             *    Mood:    add 1 - 6 times 100, (so indicative adds 100, imperative adds 200, etc.)                            *
             *    Voice:   add 1 - 3 times 1000, according to voice                                                            *
             *    Case:    add 1 - 5 times 10000, according to case                                                            *
             *    Gender:  add 1 - 3 times 100000, according to gender                                                         *
             *                                                                                                                 *
             *  The method also adds the same coded values to grammarStore and uses this information to identify the principal *
             *  part related to the specific word form.                                                                        *
             *                                                                                                                 *
             *=================================================================================================================*/

            const int noOfCategories = 7;
            int idx, parseCode = 0, voiceCode;
            int[] grammarStore = new int[noOfCategories];

            for (idx = 0; idx < noOfCategories; idx++) grammarStore[idx] = 0;
            switch (inParseInfo[0])
            {
                case 'P': parseCode += 10; grammarStore[4] = 1; break;    // Present
                case 'I': parseCode += 20; grammarStore[4] = 2; break;    // Imperfect
                case 'A': parseCode += 30; grammarStore[4] = 3; break;    // Aorist
                case 'X': parseCode += 40; grammarStore[4] = 4; break;    // Perfect
                case 'Y': parseCode += 50; grammarStore[4] = 5; break;    // Pluperfect
                case 'F': parseCode += 60; grammarStore[4] = 6; break;    // Future
                default: break;
            }
            switch (inParseInfo[1])
            {
                case 'A': parseCode += 1000; grammarStore[6] = 1; break;    // Active
                case 'M': parseCode += 2000; grammarStore[6] = 2; break;    // Middle
                case 'P': parseCode += 3000; grammarStore[6] = 3; break;    // Passive
                default: break;
            }
            switch (inParseInfo[2])
            {
                case 'I': parseCode += 100; grammarStore[5] = 1; break;    // Indicative
                case 'D': parseCode += 200; grammarStore[5] = 2; break;    // Imperative
                case 'S': parseCode += 300; grammarStore[5] = 3; break;    // Subjunctive
                case 'O': parseCode += 400; grammarStore[5] = 4; break;    // Optative
                case 'N': parseCode += 500; grammarStore[5] = 5; break;    // Infinitive
                case 'P': parseCode += 600; grammarStore[5] = 6; break;    // Participle
                default: break;
            }
            if (inParseInfo[2] == 'P')  // the occurrence is a participle
            {
                if (inParseInfo.Length > 3)
                {
                    switch (inParseInfo[3])
                    {
                        case 'N': parseCode += 10000; grammarStore[2] = 1; break;    // Nominative
                        case 'V': parseCode += 20000; grammarStore[2] = 2; break;    // Vocative
                        case 'A': parseCode += 30000; grammarStore[2] = 3; break;    // Accusative
                        case 'G': parseCode += 40000; grammarStore[2] = 4; break;    // Genitive
                        case 'D': parseCode += 50000; grammarStore[2] = 5; break;    // Dative
                        default: break;
                    }
                }
                if (inParseInfo.Length > 4)
                {
                    switch (inParseInfo[4])
                    {
                        case 'S': parseCode += 7; grammarStore[0] = 0; grammarStore[1] = 1; break;    // Singular
                        case 'D': break;    // Dual
                        case 'P': parseCode += 8; grammarStore[0] = 0; grammarStore[1] = 2; break;    // Plural
                        default: break;
                    }
                }
                if (inParseInfo.Length > 5)
                {
                    switch (inParseInfo[5])
                    {
                        case 'M': parseCode += 100000; grammarStore[3] = 1; break;    // Masculine
                        case 'N': parseCode += 200000; grammarStore[3] = 2; break;    // Neuter
                        case 'F': parseCode += 300000; grammarStore[3] = 3; break;    // Feminine
                        default: break;
                    }
                }
            }
            else
            {
                if (inParseInfo.Length > 3)
                {
                    switch (inParseInfo[3])
                    {
                        case '1':
                            grammarStore[0] = 1;
                            switch (inParseInfo[4])
                            {
                                case 'S': parseCode += 1; grammarStore[1] = 1; break;
                                case 'D': break;
                                case 'P': parseCode += 4; grammarStore[1] = 2; break;
                            }
                            break;
                        case '2':
                            grammarStore[0] = 2;
                            switch (inParseInfo[4])
                            {
                                case 'S': parseCode += 2; grammarStore[1] = 1; break;
                                case 'D': break;
                                case 'P': parseCode += 5; grammarStore[1] = 2; break;
                            }
                            break;
                        case '3':
                            grammarStore[0] = 3;
                            switch (inParseInfo[4])
                            {
                                case 'S': parseCode += 3; grammarStore[1] = 1; break;
                                case 'D': break;
                                case 'P': parseCode += 6; grammarStore[1] = 2; break;
                            }
                            break;
                    }
                }
            }
            grammarBreakdown = new Tuple<int, int, int, int, int, int, int>(grammarStore[0], grammarStore[1], grammarStore[2],
                grammarStore[3], grammarStore[4], grammarStore[5], grammarStore[6]);
            voiceCode = (parseCode % 10000) / 1000;
            switch (grammarBreakdown.Item5)
            {
                case 1:
                case 2: principalPartCode = 1; break;
                case 3:
                    if (grammarBreakdown.Item7 < 3) principalPartCode = 3;
                    else principalPartCode = 6;
                    break;
                case 4:
                case 5:
                    if (grammarBreakdown.Item7 == 1) principalPartCode = 4;
                    else principalPartCode = 5;
                    break;
                case 6:
                    if (grammarBreakdown.Item7 < 3) principalPartCode = 2;
                    else principalPartCode = 6;
                    break;
            }
            return parseCode;
        }
    }
}
