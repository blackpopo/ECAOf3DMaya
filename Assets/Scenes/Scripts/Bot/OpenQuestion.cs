using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace MayaBot_v01
{
    class OpenQuestion
    {
        List<string> one_phrase_open_questions = new List<string>();
        List<string> wants = new List<string>();
        List<string[]> thinks = new List<string[]>();
        List<string> ikenais = new List<string>();
        List<string> guesses = new List<string>();
        List<string> shoulds = new List<string>();
        List<string[]> wishes = new List<string[]>();
        List<string> judges = new List<string>();
        List<string> superlatives = new List<string>();

        Utilitie utilitie = new Utilitie();

        public OpenQuestion()
        {
            //ただのリスト
            string one_phrase_open_question_file_name = @"OPENQUESTION\OnePhraseOpenQuestion";
            var temp_one_phrase_open_questions = utilitie.ReadCSV(one_phrase_open_question_file_name);
            foreach (var word in temp_one_phrase_open_questions)
            {
                Debug.Assert(word.Length == 1);
                one_phrase_open_questions.Add(word[0]);
            }

            //3つのグループ
            string want_file_name = @"OPENQUESTION\Want";
            var temp_wants = utilitie.ReadCSV(want_file_name);
            foreach (var word in temp_wants)
            {
                Debug.Assert(word.Length == 1);
                wants.Add(@"(.*)(" + word[0] + ")(.*)");
            }

            // 2つのグループとレスポンス
            string think_file_name = @"OPENQUESTION\Think";
            var temp_thinks = utilitie.ReadCSV(think_file_name);
            foreach (var word_list in temp_thinks)
            {
                Debug.Assert(word_list.Length == 2);
                var word = word_list[0];
                var response = word_list[1];
                string[] item = { @"(.*)" + word + "(.*)", response };
                thinks.Add(item);
            }


            //　3つのグループ
            string ikenai_file_name = @"OPENQUESTION\Ikenai";
            var temp_ikenais = utilitie.ReadCSV(ikenai_file_name);

            string ikenai_prefix_file_name = @"OPENQUESTION\IkenaiConjunctions";
            var temp_ikenai_prefixs = utilitie.ReadCSV(ikenai_prefix_file_name);
            foreach (var prefix in temp_ikenai_prefixs)
            {
                foreach (var word in temp_ikenais)
                {
                    Debug.Assert(prefix.Length == 1);
                    Debug.Assert(word.Length == 1);
                    ikenais.Add(@"(.*)(" + prefix[0] + word[0] + ")(.*)");
                }
            }

            //2つのグループ
            string guess_file_name = @"OPENQUESTION\Guess";
            var temp_guesses = utilitie.ReadCSV(guess_file_name);
            foreach (var word in temp_guesses)
            {
                Debug.Assert(word.Length == 1);
                guesses.Add(@"(.*)" + word[0] + "(.*)");
            }

            //3つのグループ
            string should_file_name = @"OPENQUESTION\Should";
            var temp_shoulds = utilitie.ReadCSV(should_file_name);
            foreach (var word in temp_shoulds)
            {
                Debug.Assert(word.Length == 1);
                shoulds.Add(@"(.*)(" + word[0] + ")(.*)");
            }

            //1つのグループとレスポンス
            string wish_file_name = @"OPENQUESTION\Wish";
            var temp_wishes = utilitie.ReadCSV(wish_file_name);
            foreach (var word_list in temp_wishes)
            {
                Debug.Assert(word_list.Length == 2);
                var word = word_list[0];
                var response = word_list[1];
                string[] item = { @"(.*)" + word,  response};
                wishes.Add(item);
            }

            //3つのグループ
            string judge_file_name = @"OPENQUESTION\Judge";
            var temp_judges = utilitie.ReadCSV(judge_file_name);
            foreach (var word in temp_judges)
            {
                Debug.Assert(word.Length == 1);
                judges.Add(@"(.*)(" + word[0] + ")(.*)");
            }

            //3つのグループ
            string superlative_file_name = @"OPENQUESTION\Superlative";
            var temp_superlatives = utilitie.ReadCSV(superlative_file_name);
            foreach (var word in temp_superlatives)
            {
                Debug.Assert(word.Length == 1);
                superlatives.Add(@"(.*)(" + word[0] + ")(.*)");
            }
        }

        public string open_question(string sentence)
        {
            string response = IsOpenQuestionOnePhrase(sentence);
            if (response != "") { return response;  }

            response = IsWant(sentence);
            if (response != "") { return response; }

            response = IsThink(sentence);
            if (response != "") { return response; }

            response = IsIkenai(sentence);
            if (response != "") {  return response; }

            response = IsGuess(sentence);
            if (response != "") {return response; }

            response = IsShould(sentence);
            if (response != "") { return response; }

            response = IsWish(sentence);
            if (response != "") {  return response; }

            response = IsJudge(sentence);
            if (response != "") {  return response; }

            response = IsSuperlative(sentence);
            if (response != "") {return response; }

            return "";
        }

        string IsOpenQuestionOnePhrase(string sentence)
        {
            foreach(var word in one_phrase_open_questions)
            {
                if (sentence.EndsWith(word))
                {
                    return "そうなんだね。それで？";
                }
            }
            return "";
        }

        string IsWant(string sentence)
        {
            foreach (var word in wants)
            {
                var rgx_want = Regex.Match(sentence, word);
                if (rgx_want.Success)
                {
                    var former_sentence = rgx_want.Groups[1].Value;
                    var middle_sentence = rgx_want.Groups[2].Value;
                    var latter_sentence = rgx_want.Groups[3].Value;
                    if (! utilitie.IsNotOK(latter_sentence)  && !utilitie.IsAdjunct(former_sentence))
                    {
                        if (middle_sentence.Contains("な") || utilitie.IsDeny(latter_sentence))
                        {
                            if (middle_sentence.EndsWith("た") || latter_sentence.Contains("た"))
                            {
                                return "どうして、そうして欲しくなかったの？";
                            }
                            else
                            {
                                return "どうして、そうして欲しくないの？";
                            }
                        }
                        else
                        {
                            if (middle_sentence.EndsWith("た") || latter_sentence.Contains("た"))
                            {
                                return "どうして、そうして欲しかったの？";
                            }
                            else
                            {
                                return "どうして、そうして欲しいの？";
                            }
                        }
                    }
                }
            }
            return "";
        }
        string IsThink(string sentence)
        {
            foreach (var word_list in thinks)
            {
                var word = word_list[0];
                var response = word_list[1];
                var rgx_think = Regex.Match(sentence, word);
                if (rgx_think.Success)
                {
                    var former_sentence = rgx_think.Groups[1].Value;
                    var latter_sentence = rgx_think.Groups[2].Value;
                    if (!utilitie.IsNotOK(latter_sentence) && !utilitie.IsDeny(latter_sentence) && !utilitie.IsAdjunct(former_sentence))
                    {
                        return response;
                    }
                }
            }
            return "";
        }
        string IsIkenai(string sentence)
        {
            foreach(var word in ikenais)
            {
                var rgx_ikenai = Regex.Match(sentence, word);
                if (rgx_ikenai.Success)
                {
                    var former_sentence = rgx_ikenai.Groups[1].Value;
                    var middle_sentence = rgx_ikenai.Groups[2].Value;
                    var latter_sentence = rgx_ikenai.Groups[3].Value;
                    if (!utilitie.IsNotOK(latter_sentence) && !utilitie.IsDeny(latter_sentence) && !utilitie.IsAdjunct(former_sentence))
                    {
                        if (middle_sentence.Contains("ば"))
                        {
                            if (middle_sentence.EndsWith("た") || latter_sentence.Contains("た"))
                            {
                                return "どうして、そうしなければいけなかったの？";
                            }
                            else
                            {
                                return "どうして、そうしなければいけないの？";
                            }
                        }
                        else
                        {
                            if (middle_sentence.EndsWith("た") || latter_sentence.Contains("た"))
                            {
                                return "どうして、そうしなくてはいけなかったの？";
                            }
                            else
                            {
                                return "どうして、そうしなくてはいけないの？";
                            }
                        }
                    }
                }
            }
            return "";
        }
        string IsGuess(string sentence)
        {
            foreach(var word in guesses)
            {
                var rgx_guess = Regex.Match(sentence, word);
                if (rgx_guess.Success)
                {
                    var former_sentence = rgx_guess.Groups[1].Value;
                    var latter_sentence = rgx_guess.Groups[2].Value;
                    if (!utilitie.IsAdjunct(former_sentence) && !utilitie.IsNotOK(latter_sentence) && !utilitie.IsDeny(latter_sentence))
                    {
                        return "どうして、そう思うの？";
                    }
                }
            }
            return "";
        }
        string IsShould(string sentence)
        {
            foreach(var word in shoulds)
            {
                var rgx_should = Regex.Match(sentence, word);
                if (rgx_should.Success)
                {
                    var former_sentence = rgx_should.Groups[1].Value;
                    var middle_sentence = rgx_should.Groups[2].Value;
                    var latter_sentence = rgx_should.Groups[3].Value;
                    if (! utilitie.IsNotOK(latter_sentence) && !utilitie.IsAdjunct(former_sentence))
                    {
                        if (middle_sentence.EndsWith("じゃ") || middle_sentence.EndsWith("では"))
                        {
                            if (latter_sentence.Contains("た"))
                            {
                                return "どうして、そうするべきじゃなかったの？";
                            }
                            else
                            {
                                return "どうして、そうするべきじゃないの？";
                            }
                        }
                        else
                        {
                            if (middle_sentence.Contains("た"))
                            {
                                return "どうして、そうするべきだったの？";
                            }
                            else
                            {
                                return "どうして、そうするべきなの？";
                            }
                        }
                    }
                }
            }
            return "";
        }
        string IsWish(string sentence)
        {
            string prechars = "いきしちにひりぎじぢび来居見えけせてね得寝経へめげぜでべ";
            foreach ( var word_list in wishes)
            {
                var word = word_list[0];
                var response = word_list[1];
                var rgx_wish = Regex.Match(sentence, word);
                if (rgx_wish.Success)
                {
                    var former_sentence = rgx_wish.Groups[1].Value;
                    if (former_sentence.Length > 0 && prechars.Contains(former_sentence.Substring(former_sentence.Length -1)) && ! utilitie.IsAdjunct(former_sentence))
                    {
                        return response;
                    }
                }
            }
            return "";
        }
        string IsJudge(string sentence)
        {
            foreach(var word in judges)
            {
                var rgx_judge = Regex.Match(sentence, word);
                if (rgx_judge.Success)
                {
                    var former_sentence = rgx_judge.Groups[1].Value;
                    var middle_sentence = rgx_judge.Groups[2].Value;
                    var latter_sentence = rgx_judge.Groups[3].Value;
                    if (!utilitie.IsAdjunct(former_sentence) && !former_sentence.EndsWith("が") && !utilitie.IsNotOK(latter_sentence) && !utilitie.IsDeny(latter_sentence))
                    {
                        if (middle_sentence.EndsWith("し。"))
                        {
                            middle_sentence = middle_sentence.Substring(0, middle_sentence.Length - 2);
                        }
                        else if (middle_sentence.EndsWith("。"))
                        {
                            middle_sentence = middle_sentence.Substring(0, middle_sentence.Length - 1);
                        }
                        return "へーそうなんだ。どんなところが、" + middle_sentence + "の？";
                    }
                }
            }
            return "";
        }
        string IsSuperlative(string sentence)
        {
            foreach(var word in superlatives)
            {
                var rgx_superlative = Regex.Match(sentence, word);
                if (rgx_superlative.Success)
                {
                    var former_sentence = rgx_superlative.Groups[1].Value;
                    var middle_sentence = rgx_superlative.Groups[2].Value;
                    var latter_sentence = rgx_superlative.Groups[3].Value;
                    if (!utilitie.IsAdjunct(former_sentence) && !utilitie.IsNotOK(latter_sentence))
                    {
                        if (utilitie.IsDeny(latter_sentence))
                        {
                            if (latter_sentence.Contains("た"))
                            {
                                return "どんなところが、" + middle_sentence + "じゃなかったの？";
                            }
                            else
                            {
                                return "どんなところが、" + middle_sentence + "じゃないの？";
                            }
                        }
                        else
                        {
                            if (latter_sentence.Contains("た"))
                            {
                                return "どんなところが、" + middle_sentence + "だったの？";
                            }
                            else
                            {
                                return "どんなところが、" + middle_sentence + "なの？";
                            }
                        }
                    }
                }
            }
            return "";
        }

    }
}
