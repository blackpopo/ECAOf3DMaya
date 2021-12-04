using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MayaBot_v01
{
    class FixedPhrase
    {
        List<string[]> greetings = new List<string[]>();
        List<string[]> fixed_phrases = new List<string[]>();
        List<List<string>> special_phrases = new List<List<string>>();

        string[] special_responses = { "へー、そうなんだ。それで、それで？" ,
                                                         "もちろんだよ。" ,
                                                         "いいね。それから？" ,
                                                         "そうだったんだね。それから、どうしたの？" ,
                                                          "尊敬出来てかっこいいね。" ,
                                                          "とっても、頑張ってるんだね。" ,
                                                          "とっても、頑張ってたんだね。" ,
                                                          "もう少し詳しく教えて？" };

        Utilitie utilitie = new Utilitie();

        public FixedPhrase()
        {

            string greeting_file_name = @"FIXEDPHRASE\Greeting";
            var temp_greetings = utilitie.ReadCSV(greeting_file_name);
            foreach(var word in temp_greetings)
            {
                Debug.Assert(word.Length == 2);
                string[] item = { @"^" + word[0] + "(.*)", word[1] };
                greetings.Add(item);
            }

            string[] fixed_file_names = { @"FIXEDPHRASE\Personal" ,
            @"FIXEDPHRASE\Others"};
            foreach(var file_name in fixed_file_names)
            {
                var temp_fixed_phrases = utilitie.ReadCSV(file_name);
                {
                    foreach(var word in temp_fixed_phrases)
                    {
                        Debug.Assert(word.Length == 2);
                        fixed_phrases.Add(word);
                    }
                }
            }
            string[] special_file_names = { @"FIXEDPHRASE\Hearsay",
                                                            @"FIXEDPHRASE\Request" ,
                                                            @"FIXEDPHRASE\Will" ,
                                                            @"FIXEDPHRASE\WillPast" ,
                                                            @"FIXEDPHRASE\Respectable" ,
                                                            @"FIXEDPHRASE\Reward" ,
                                                            @"FIXEDPHRASE\RewardPast" ,
                                                            @"FIXEDPHRASE\Aizuchi" };

            Debug.Assert(special_file_names.Length == special_responses.Length);

            foreach(var file_name in special_file_names)
            {
                var temp_special_list = new List<string>();
                var temp_specials = utilitie.ReadCSV(file_name);
                foreach(var word in temp_specials)
                {
                    temp_special_list.Add(word[0]);
                }
                special_phrases.Add(temp_special_list);
            }

        }

        public string fixed_phrase(string sentence)
        {
            string response;

            response = IsGreeting(sentence);
            if (response != "") { return response; }

            response = IsFixedPhrase(sentence);
            if (response != "") { return response; }

            response = IsSpecial(sentence);
            if(response != "") { return response;  };

            return "";
        }

        string IsGreeting(string sentence)
        {
            foreach(var greeting_list in greetings)
            {
                var greeting = greeting_list[0];
                var response = greeting_list[1];
                var rgx_greeting = Regex.Match(sentence, greeting);
                if (rgx_greeting.Success)
                {
                    var latter_sentence = rgx_greeting.Groups[1].Value;
                    if (!utilitie.IsNotOK(latter_sentence) && !utilitie.IsDeny(latter_sentence))
                    {
                        return response;
                    }
                }

            }
            return "";
        }

        string IsFixedPhrase(string sentence)
        {
            foreach(var fixed_phrase_list in fixed_phrases)
            {
                var fixed_phrase = @fixed_phrase_list[0];
                var response = fixed_phrase_list[1];
                if (Regex.IsMatch(sentence, fixed_phrase))
                {
                    return response;
                }
            }
            return "";
        }

        string IsSpecial(string sentence)
        {
            for(var i = 0; i < special_phrases.Count; i++)
            {
                var specials = special_phrases[i];
                foreach(var word in specials)
                {
                    if (sentence.EndsWith(word))
                    {
                        return special_responses[i];
                    }
                }
            }

            return "";
        }
    }
}
