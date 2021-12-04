using System;
using System.Collections;
using System.Collections.Generic;
using Live2D.Cubism.Core;
using MayaBot_v01;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{

    private Bot maya;

    private FaceController faceController;

    private ModelController modelcontroller;

    private VoiceController voiceController;

    private MICController micController;

    private bool changed = false;

    private CubismModel model;

    private string response;

    private string model_name;

    private string face;

    private string user_emotion;

    [SerializeField] private Text input_text;

    [SerializeField] private Text output_text;

    void Start()
    {
        faceController = gameObject.GetComponent<FaceController>();
        modelcontroller = gameObject.GetComponent<ModelController>();
        voiceController = gameObject.GetComponent<VoiceController>();
        micController = gameObject.GetComponent<MICController>();
        maya = new Bot();
        StartCoroutine("StartTalk");
    }

    void Update()
    {
        if (micController.sentence != null) 
        {
            var input = micController.sentence;
            var item = maya.Chat(input);
            input_text.text = input;
            output_text.text = item[0];
            if (!micController.isSpeaking && input != "うん。")
            {
                Debug.Log("Talk");
                Talk(item);
                changed = true;
            }
            else
            {
                Debug.Log("Nod");
                modelcontroller.Nod();
            }
            micController.sentence = null;
        }
        
        var isModelPlaying = voiceController.isAudioPlaying();
        if (changed && !isModelPlaying) //音声の再生後に0.5s経ってからモデルを戻す。
        {
            changed = false;
            faceController.FaceBack();
            model = modelcontroller.SetModel("W", false);
            faceController.FaceChange(model, user_emotion);
        }
    }
    

    private void Talk( string[] item)
    {
        response = item[0];
        model_name = item[1];
        face = item[2];
        user_emotion = item[3];
        model = modelcontroller.SetModel(model_name, true);
        faceController.FaceChange(model, face);
        voiceController.Play(response);
        if (model_name == "G")
        {
            Quit();
        }
    }
    

    private void Quit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
              UnityEngine.Application.Quit();
        #endif
    }
    

        IEnumerator StartTalk()
    {
        yield return new WaitForSeconds(1.0f);
        voiceController.Play("こんにちは");
    }
}
