using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayaBot_v01
{
    class Bot
    {
        Preprocess preprocess = new Preprocess();
        FixedPhrase fixed_phrase = new FixedPhrase();
        Uncompleted uncompleted = new Uncompleted();
        Question question = new Question();
        OpenQuestion open_question = new OpenQuestion();
        Emotion emotion = new Emotion();
        Aizuchi aizuchi = new Aizuchi();
        WordModelFace wmf = new WordModelFace();

        private string user_emotion = "Happy";

        public string[] Chat(string user_input)
        {
            bool apply = false;
            string sentence = preprocess.preprocess(user_input, ref apply);
            var response = sentence;
            if (!apply)
            {
                response = fixed_phrase.fixed_phrase(sentence);
                if (response == "") { response = uncompleted.uncompleted(sentence); }
                if (response == "") { response = question.question(sentence); }
                if (response == "") { response = open_question.open_question(sentence); }
                if (response == "") { response = emotion.emotion(sentence); }
                if (response == "") { response = aizuchi.aizuchi(sentence); }
            }
            var item = wmf.wmf(response, ref user_emotion);
            var model = item[0];
            var face = item[1];
            string[] res_item = {response, model, face, user_emotion};
            return res_item;
        }
    }
}
