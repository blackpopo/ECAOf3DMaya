using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class VoiceController : MonoBehaviour
{
    [SerializeField] private Animator waitingAnimator;
    
    // private List<string[]> VoiceFileNames;

    [SerializeField] private AudioSource voiceSource;

    private bool isModelPlaying;

    private List<string[]> voice_names;
    

    // Update is called once per frame
    
    void Update()
    {
        if (ReferenceEquals(null, voiceSource))
        {
            ;
        }
        else if (!voiceSource.isPlaying && isModelPlaying) //しゃべり終わってから実行。
        {
            StartCoroutine("Timer", 1.0f);　//一秒後に stop state から抜ける。
        }
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        waitingAnimator.SetBool("stop", false);
        isModelPlaying = false;
    }

    public void Play(string clip_name)
    {
        if (!voiceSource.isPlaying)
        {
            Resources.UnloadUnusedAssets();
            var clip = Resources.Load<AudioClip>("Voices/" + clip_name);
            Assert.IsFalse(ReferenceEquals(null, clip));
            waitingAnimator.SetBool("stop", true); 
            voiceSource.clip = clip;
            voiceSource.Play();
            isModelPlaying = true;
        }
        else
        {
            Debug.Log("voice source is now playing...");
        }
    }

    public bool isAudioPlaying()
    {
        return isModelPlaying;
    }
}
