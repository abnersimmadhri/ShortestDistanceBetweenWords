using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ShortestDistanceBetweenWords
{
    class Program
    {
        static void Main(string[] args)
        {     

            Console.Write("Please enter the first word ");
            string searchWord1 = Console.ReadLine();
            Console.Write("Please enter the second word ");
            string searchWord2 = Console.ReadLine();
            string path = "sampleText.txt";

            var wordsDict = parseText(path);

            Console.Write($"The shortest distance between '{searchWord1}' and '{searchWord2}' is: ");
            Console.WriteLine(getDistance(wordsDict, searchWord1, searchWord2));
            Console.WriteLine();
            Console.ReadLine();

        }

        /// <summary>
        /// gets the shortest distance between two words. throws an error if the word is not in the dictionary.
        /// </summary>
        /// <param name="wordDict"></param>
        /// <param name="word1"></param>
        /// <param name="word2"></param>
        /// <returns></returns>
        public static int getDistance(Dictionary<string, List<int>> wordDict, string word1, string word2)
        {
            int distance = 0;
            
            var firstWordList = wordDict[word1.Trim().ToLower()];
            var secondWordList = wordDict[word2.Trim().ToLower()];

            firstWordList.Sort();
            secondWordList.Sort();

            if (word1 == word2)
            {
                if (firstWordList.Count <= 1)
                    return 0;

                distance = Math.Abs(firstWordList[1] - firstWordList[0]);
                for (int i = 1; i < firstWordList.Count(); i++)
                {
                    int nextDistance = Math.Abs(firstWordList[i-1] - firstWordList[i]);
                    if (nextDistance < distance)
                        distance = nextDistance;
                }
                //0 based so decrease distance.
                return distance -1;
            }

            //To Do.. change from nested for loop. 

            distance = Math.Abs(firstWordList[0] - secondWordList[0]);
            for (int i = 0; i < firstWordList.Count(); i++)
            {
                for (int j = 0; j < secondWordList.Count(); j++)
                {
                    int nextDistance = Math.Abs(firstWordList[i] - secondWordList[j]);
                    if (nextDistance < distance)
                        distance = nextDistance;

                }
            }

            //0 based so decrease distance.
            return distance -1;
        }

        /// <summary>
        /// read in the text from the spcified path and converts it to a dictionary of words. 
        /// throws an error if the file is not found.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Dictionary<string, List<int>> parseText(string path)
        {

            Dictionary<string, List<int>> keyValuePairs = new Dictionary<string, List<int>>();
            string fileText = File.ReadAllText(path);
            fileText = Regex.Replace(fileText, @"\t|\n|\r", " ");
            var wordArray = fileText.Split(' ');
            int count = 0;
            for (int i = 0; i < wordArray.Length; i++)
            {
                string plainText = wordArray[i].Trim().ToLower();
                plainText = Regex.Replace(plainText, @"[^\w\s]", "");
                
                if (string.IsNullOrEmpty(plainText))
                    continue;
                if (!keyValuePairs.ContainsKey(plainText))
                {
                    keyValuePairs.Add(plainText, new List<int> { count });
                }
                else
                {
                    keyValuePairs[plainText].Add(count);
                }

                count++;
            }

            return keyValuePairs;

        }

        /// <summary>
        /// prints each uniques word contained in the text.
        /// </summary>
        /// <param name="wordsDictionary"></param>
        public static void printWords(Dictionary<string, List<int>> wordsDictionary)
        {
            for(int i = 0; i<wordsDictionary.Count(); i++)
            {
                Console.WriteLine(wordsDictionary.ElementAt(i).Key);

            }
        }

        /// <summary>
        /// prints the text in the original order without punctuations, line breaks and additional spaces.
        /// </summary>
        /// <param name="wordsDictionary"></param>
        public static void printTextWithWords(Dictionary<string, List<int>> wordsDictionary)
        {
            Dictionary<int, string> words = new Dictionary<int, string>();

            for(int i = 0; i < wordsDictionary.Count(); i ++ )
            {
                for (int j = 0; j < wordsDictionary.ElementAt(i).Value.Count; j++)
                    words.Add(wordsDictionary.ElementAt(i).Value[j], wordsDictionary.ElementAt(i).Key.ToString());   
            }

            for(int i = 0; i< words.Count(); i++ )
            {
                Console.Write(words[i] + " ");
            }
        }
    }
}
