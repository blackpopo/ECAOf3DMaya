using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace MayaBot_v01
{
    class Preprocess
    {
        string last_sentece = @"^[。？！].+?[。？！]";
        string ends = @"[。？！]$";
        Utilitie utilitie = new Utilitie();
        List<string> speak_orders = new List<string>();
        List<string> topics = new List<string>();

        Random rnd = new Random();

        static string speak_order_file_name;
        static string topics_file_name;

        public Preprocess()
        {
            speak_order_file_name = @"PREPROCESS\SpeakOrder";
            topics_file_name = @"PREPROCESS\Topic";

            var temp_speak_orders = utilitie.ReadCSV(speak_order_file_name);
            foreach(var word in temp_speak_orders)
            {
                Debug.Assert(word.Length == 1);
                speak_orders.Add(word[0]);
            }

            var temp_topics = utilitie.ReadCSV(topics_file_name);
            foreach (var word in temp_topics)
            {
                Debug.Assert(word.Length == 1);
                topics.Add(word[0]);
            }

        }

        public string preprocess(string sentence, ref bool apply)
        {
            apply = false;
            sentence = ExtractLastSentence(sentence);
            var response = SpeakInitialization(sentence);
            if (response != ""){
                apply = true;
                return response;
            }
            return sentence;
        }

        private string ExtractLastSentence(string sentence)
        {
            if(sentence == null)
            {
                return "話して。";
            }
            if(! Regex.IsMatch(sentence, ends))
            {
                sentence = sentence + "。";
            }
            sentence = string.Concat(sentence.Reverse()) + "。";
            sentence = Regex.Match(sentence, last_sentece).Value;
            sentence = sentence.Substring(0, sentence.Length - 1);
            sentence = string.Concat(sentence.Reverse());
            return sentence;

        }

        private string SpeakInitialization(string sentence)
        {
            foreach (var word in speak_orders)
            {
                if (sentence.StartsWith(word))
                {
                    return topics[rnd.Next(0, topics.Count)];
                }
            }
            return "";
        }
    }
}
