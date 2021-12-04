using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayaBot_v01
{
    class Aizuchi
    {
        List<string> aizuchis = new List<string>();
        Utilitie utilitie = new Utilitie();
        Random rnd = new Random();
        public Aizuchi()
        {
            string aizuchi_file_name = @"AIZUCHI\AizuchiResponse";
            var temp_aizuchis = utilitie.ReadCSV(aizuchi_file_name);
            foreach(var word in temp_aizuchis)
            {
                aizuchis.Add(word[0]);
            }
        }
        
        public string aizuchi(string sentence)
        {
            return aizuchis[rnd.Next(0, aizuchis.Count)]; //Random Includes Max Value
        }
    }
}
