using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace MayaBot_v01
{
    class Question
    {
        List<string> open_keywords = new List<string>();
        List<string> open_ends = new List<string>();
        List<string> question_ends = new List<string>();

        Utilitie utilitie = new Utilitie();

        string[] open_head_keywords = { "なに", "何", "だれ", "誰", "どこ", "何処", "どう", "いつ", "どちら" , "どっち"};

        public Question()
        {
            string open_question_keyword_file_name = @"QUESTION\OpenQuestionKeywords";
            var temp_open_keywords = utilitie.ReadCSV(open_question_keyword_file_name);
            foreach (var word in temp_open_keywords)
            {
                Debug.Assert(word.Length == 1);
                open_keywords.Add(@"(.*)" + word[0] + "(.*)");
            }

            string open_end_file_name = @"QUESTION\OpenEnds";
            var temp_open_ends = utilitie.ReadCSV(open_end_file_name);
            foreach (var word in temp_open_ends)
            {
                Debug.Assert(word.Length == 1);
                open_ends.Add(word[0]);
            }

            string question_end_file_name = @"QUESTION\QuestionEnds";
            var temp_question_ends = utilitie.ReadCSV(question_end_file_name);
            foreach (var word in temp_question_ends)
            {
                Debug.Assert(word.Length == 1);
                question_ends.Add(word[0]);
            }
        }

        public string question(string sentence)
        {
            string response = IsOpenQuestion(sentence);
            if (response != "") {  return response;  }

            response = IsExclamation(sentence);
            if (response != "") { return response; }

            response = IsQuestion(sentence);
            if (response != "") { return response; }

            return "";
        }
        string IsOpenQuestion(string sentence)
        {
            foreach (var word in open_ends)
            {
                var not_end = "から" + word;
                if (sentence.Contains(word))
                {
                    if (!sentence.Contains(not_end))
                    {
                        foreach ( var keyword in open_keywords)
                        {
                            var rgx_keyword = Regex.Match(sentence, keyword);
                            if (rgx_keyword.Success)
                            {
                                string former_sentence = rgx_keyword.Groups[1].Value;
                                string latter_sentence = rgx_keyword.Groups[2].Value;
                                if (! latter_sentence.StartsWith("も") && ! latter_sentence.StartsWith("か") && ! latter_sentence.StartsWith("す") && !utilitie.IsAdjunct(former_sentence))
                                {
                                    return "あなたはどう思っているの？";
                                }
                                else
                                {
                                    return "へー、そうなんだ。それで？";
                                }
                            }
                        }

                    }
                    else
                    {
                        return "へー、そうなんだ。それで？";
                    }
                }
            }
            return "";
        }

        string IsExclamation(string sentence)
        {
            foreach(var word in question_ends)
            {
                var q_end = word + "？";
                var p_end = word + "。";
                if (sentence.EndsWith(q_end) || sentence.EndsWith(p_end))
                {
                    return "どうして、そう思うの？";
                }
            }
            return "";
        }

        string IsQuestion(string sentence)
        {
            var q_end = "か？";
            var not_q_end = "う" + q_end;
            if (sentence.EndsWith(q_end) && !sentence.EndsWith(not_q_end))
            {
                foreach( var keyword in open_head_keywords)
                {
                    var not_keyword = keyword + "も";
                    if (sentence.Contains(keyword) && !sentence.Contains(not_keyword))
                    {
                        return "ごめんね。わかんないや。";
                    }
                    else
                    {
                        return "調べてみる？";
                    }
                }
            }
            return ""; 
        }
    }
}
