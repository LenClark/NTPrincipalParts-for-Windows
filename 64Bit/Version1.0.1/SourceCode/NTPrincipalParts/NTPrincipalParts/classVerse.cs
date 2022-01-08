using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPrincipalParts
{
    public class classVerse
    {
        String textOfVerse = "";

        public string TextOfVerse { get => textOfVerse; set => textOfVerse = value; }

        public void addWord(String word, String preChars, String followingChars, String punctuation)
        {
            if (textOfVerse.Length == 0) textOfVerse = preChars + word + followingChars + punctuation;
            else textOfVerse += " " + preChars + word + followingChars + punctuation;
        }
    }
}
