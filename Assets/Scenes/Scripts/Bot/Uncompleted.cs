using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MayaBot_v01
{
    class Uncompleted
    {
        List<string> one_phrase_uncompleteds = new List<string>();
        List<string[]> uncompleteds = new List<string[]>();

        Utilitie utilitie = new Utilitie();
        public Uncompleted()
        {
            string one_phrase_uncompleted_file_name = @"UNCOMPLETED\OnePhraseUncompleted";
            var temp_one_phrase_uncompleted = utilitie.ReadCSV(one_phrase_uncompleted_file_name);
            foreach(var word in temp_one_phrase_uncompleted)
            {
                Debug.Assert(word.Length == 1);
                one_phrase_uncompleteds.Add(word[0]);
            }

            string uncompleted_file_name = @"UNCOMPLETED\uncompleted";
            var temp_uncompleted = utilitie.ReadCSV(uncompleted_file_name);
            foreach(var word in temp_uncompleted)
            {
                Debug.Assert(word.Length == 2);
                uncompleteds.Add(word);
            }

        }
        public string uncompleted(string sentence)
        {
            var response = IsUncompletedOnePhrase(sentence);
            if (response != "") { return response;  }

            response = IsUncompleted(sentence);
            if (response != "") { return response; }

            return "";
        }
        private string IsUncompletedOnePhrase(string sentence)
        {
            foreach(var word in one_phrase_uncompleteds)
            {
                if (sentence.Contains(word))
                {
                    string response = word.Substring(0, word.Length - 1) + "？";
                    return response;
                }
            }
            return "";
        }
        private string IsUncompleted(string sentence)
        {
            foreach(var item in uncompleteds)
            {
                var word = item[0];
                var response = item[1];
                if (sentence.Contains(word))
                {
                    return response;
                }
            }
            return "";
        }
    }
}
