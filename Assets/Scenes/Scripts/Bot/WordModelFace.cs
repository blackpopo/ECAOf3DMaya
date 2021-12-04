using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayaBot_v01
{
    class WordModelFace
    {
        Dictionary<string, string[]> WMF_dict = new Dictionary<string, string[]>();

        string[] positives = { "Surprised", "Embarrassed", "Anger", "Happy", "Fun" };

        string[] negatives = { "Disgust", "Sad", "Fear" };

        Utilitie utilitie = new Utilitie();
        public WordModelFace()
        {
            string WMF_file_name = @"WORDMODELFACE\Maya_WMF";
            var temp_WMF = utilitie.ReadCSV(WMF_file_name);
            foreach (string[] word in temp_WMF)
            {
                Debug.Assert(word.Length == 3);
                string[] item = { word[1], word[2] };
                WMF_dict[word[0]] = item;
            }
        }
        public string[] wmf(string response, ref string user_emotion)
        {
            string face = "Normal";
            string model = "Waiting";
            if (WMF_dict.ContainsKey(response))
            {
                var item = WMF_dict[response];
                model = item[0];
                face = item[1];
                if (face == "Null")
                {
                    face = user_emotion;
                }
                else
                {
                    if (positives.Contains(face))
                    {
                        user_emotion = "Happy";
                    }
                    else if (negatives.Contains(face))
                    {
                        user_emotion = "Sad";
                    }
                }
            }
            string[] res_item = {model, face };
            return res_item;
        }
    }
}
