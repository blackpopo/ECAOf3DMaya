using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MayaBot_v01
{
    class  Utilitie
    {
        public List<string[]> ReadCSV(string file_name)
        {
            var csv_file = Resources.Load<TextAsset>(@"CSVFiles\" + file_name);
            List<string[]> res_list = new List<string[]>();
            using(var sr = new StringReader(csv_file.text))
            {
                while(sr.Peek() > -1)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(',');
                    res_list.Add(values);
                }
            }
            return res_list;
        }

        public bool IsQuote(string latter_sentence) //引用かどうかのチェック
        {
            if (latter_sentence.StartsWith("って") || latter_sentence.StartsWith("と"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsHearsay(string latter_sentence) //伝聞かどうかのチェック
        {
            if (latter_sentence.Contains("らし") || latter_sentence.Contains("そう") || latter_sentence.Contains("みたい") || latter_sentence.Contains("よう"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDeny(string latter_sentence) //否定かどうかのチェック
        {
            string[] words = { "あるまい", "あらへん", "ございません", "ありません" };
            string[] denys = { "ない", "なかった", "ず", "なかろ", "なくて", "なけれ" , "ぬ。"};
            if (latter_sentence.Contains("じゃない？") || latter_sentence.StartsWith("じゃない"))
            {
                return false;
            }
            foreach (var word in words)
            {
                if (latter_sentence.Contains(word))
                {
                    return true;
                }
            }
            foreach (var deny in denys)
            {
                if (latter_sentence.Contains(deny))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsPassive(string latter_sentence) // 受け身かどうかのチェック
        {
            if (latter_sentence.Contains("れる") || latter_sentence.Contains("れて") || latter_sentence.Contains("れた"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsAdjunct(string former_sentence)
        {
            string[] adjunct_particles = { "からには", "から", "ので", "さかい", "んで" };
            string[] adjunct_conjunctions = { "なので", "だから", "ですから", "だからこそ", "ゆえに", "故に", "そやさかい", "よって", "従って", "したがって", "んで" };
            foreach (var word in adjunct_particles)
            {
                if (former_sentence.Contains(word))
                {
                    return true;
                }
            }
            foreach(var word in adjunct_conjunctions)
            {
                if (former_sentence.Contains(word))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsConjunct(string latter_sentence)
        {
            if (latter_sentence.Contains("、"))
            {
                return true;
            }
            string te = @"て(.)";
            string[] after_check = { "い", "し", "る", "ま" };
            foreach(Match match in Regex.Matches(latter_sentence, te)){
                string after_char = match.Groups[1].Value;
                if (after_check.Contains(after_char)){
                    ;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsNotOK(string latter_sentence)
        {
            return IsQuote(latter_sentence) || IsConjunct(latter_sentence) || IsPassive(latter_sentence) || IsHearsay(latter_sentence);
        }
    }
}
