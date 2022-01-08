using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPrincipalParts
{
    class classFrequency
    {
        int listCount = 0;
        SortedList<int, String> listOfRootWords = new SortedList<int, string>();

        public int ListCount { get => listCount; }
        public SortedList<int, string> ListOfRootWords { get => listOfRootWords; set => listOfRootWords = value; }

        public void addRootEntry(String rootName)
        {
            listOfRootWords.Add(listCount++, rootName);
        }

        public String getRootNameByIndex(int index)
        {
            String retrievedRoot;

            if (!listOfRootWords.ContainsKey(index)) return "";
            listOfRootWords.TryGetValue(index, out retrievedRoot);
            return retrievedRoot;
        }
    }
}
