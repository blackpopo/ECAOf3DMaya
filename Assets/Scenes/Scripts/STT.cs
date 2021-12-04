using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using UnityEngine;

public class STT
{
    string subscription_key = "d52b3146de1d414597ee146b06782720";
    string region = "japanwest";
    string location = "ja-JP";
    
    // Start is called before the first frame update
    public async UniTask<string> stt(string wavFilePath) //通常用。SamplingRate = 16000[Hz]
    {
        var speechConfig = SpeechConfig.FromSubscription(subscription_key, region); 
        speechConfig.SpeechRecognitionLanguage = location; // Speech config 直下にLocationを設定して言語を設定する。
        using var audioConfig = AudioConfig.FromWavFileInput(wavFilePath);
        using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);
        var result = await recognizer.RecognizeOnceAsync();
        Debug.Log($"Recognized Line: = {result.Text}");
        return result.Text;
    }
}
