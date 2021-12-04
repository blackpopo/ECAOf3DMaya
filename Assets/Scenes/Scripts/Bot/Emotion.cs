using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MayaBot_v01
{
    class Emotion
    {
        List<string[]> emotions = new List<string[]>();
        Dictionary<string, string[]> emtions2lines = new Dictionary<string, string[]>();
        Dictionary<string, string[]> emtions2lines_past = new Dictionary<string, string[]>();

        Utilitie utilitie = new Utilitie();

        int middle_start_index;
        int middle_end_index;
        string former_sentence;
        string middle_sentence;
        string latter_sentence;
        string key;

        Random rnd = new Random();
        
        public Emotion()
        {
            string emtion_base_file_name = @"EMOTION\FeelingBases";
            var temp_emotions = utilitie.ReadCSV(emtion_base_file_name);
            foreach(var word_list in temp_emotions)
            {
                Debug.Assert(word_list.Length == 2);
                var word = word_list[0];
                var emotion = word_list[1];
                string[] item = { @"(.*)(" + word + ")(.*)",  emotion };
                emotions.Add(item);
            }

            string emtions2lines_file_name = @"EMOTION\Feelings2Lines";
            var temp_emtions2lines = utilitie.ReadCSV(emtions2lines_file_name);
            foreach(var word_list in temp_emtions2lines)
            {
                Debug.Assert(word_list.Length >= 2); //emtion, line1, line2, ...
                var key = word_list[0];
                string[] lines = new string[word_list.Length - 1];
                Array.Copy(word_list, 1, lines, 0, word_list.Length - 1);
                emtions2lines[key] = lines;
            }

            string emtions2lines_past_file_name = @"EMOTION\Feelings2Lines_past";
            var temp_emtions2lines_past = utilitie.ReadCSV(emtions2lines_past_file_name);
            foreach (var word_list in temp_emtions2lines_past)
            {
                Debug.Assert(word_list.Length >= 2); //emtion, line1, line2, ...
                var key = word_list[0];
                string[] lines = new string[word_list.Length - 1];
                Array.Copy(word_list, 1, lines, 0, word_list.Length - 1);
                emtions2lines_past[key] = lines;
            }
        }

        public string emotion(string sentence)
        {
            SetParameters("", sentence.Length, 0, "", "", "");
            foreach(var word_list in emotions)
            {
                var word = word_list[0];
                var temp_key = word_list[1];
                var rgx_emotion = Regex.Match(sentence, word);
                if (rgx_emotion.Success)
                {
                    var former_sentence = rgx_emotion.Groups[1].Value;
                    var middle_sentence = rgx_emotion.Groups[2].Value;
                    var latter_sentence = rgx_emotion.Groups[3].Value;
                    var start_index = rgx_emotion.Index;
                    var end_index = start_index + rgx_emotion.Length;
                    if (end_index > this.middle_end_index)
                    {
                        SetParameters(temp_key, start_index, end_index, former_sentence, middle_sentence, latter_sentence);
                    }
                    else if (end_index == this.middle_end_index && start_index < this.middle_start_index)
                    {
                        SetParameters(temp_key, start_index, end_index, former_sentence, middle_sentence, latter_sentence);
                    }
                }
            }
            if (this.middle_sentence != "")
            {
                if (!utilitie.IsDeny(this.latter_sentence) && !utilitie.IsNotOK(this.latter_sentence))
                {
                    if (this.latter_sentence.Contains("た"))
                    {
                        string[] responses = emtions2lines_past[this.key];
                        return responses[rnd.Next(0, responses.Length)];
                    }
                    else
                    {
                        string[] responses = emtions2lines[this.key];
                        return responses[rnd.Next(0, responses.Length)];
                    }
                }
            }
            return "";
        }

        void SetParameters(string temp_key, int temp_middle_start_index, int temp_middle_end_index, string temp_former_sentence, 
            string temp_middle_sentece, string temp_latter_sentence)
        {
            this.key = temp_key;
            this.middle_start_index = temp_middle_start_index;
            this.middle_end_index = temp_middle_end_index;
            this.former_sentence = temp_former_sentence;
            this.middle_sentence = temp_middle_sentece;
            this.latter_sentence = temp_latter_sentence;
        }
    }
}
