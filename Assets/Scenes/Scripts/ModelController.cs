using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Live2D.Cubism.Core;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class ModelController: MonoBehaviour
{
    // Start is called before the first frame update
    // Set model Waiting, Goodby, Lookingup, Question
    private string[] tag_name_index = {"W", "G", "L", "Q"};

    [SerializeField] private CubismModel[] models;

    [SerializeField] private Animator waiting_animator;

    private GameObject[] model_objects;
    

    private void Start()
    {
        model_objects = new GameObject[models.Length];
        for (int i = 0; i < models.Length; i++)
        {
            model_objects[i] = models[i].gameObject;
            model_objects[i].SetActive(false);
        }
        model_objects[0].SetActive(true);
        Assert.IsTrue(models.Length == tag_name_index.Length);
    }

    // Update is called once per frame

    public CubismModel SetModel(string model_name_index, bool isStop)
    {
        Assert.IsTrue(tag_name_index.Contains(model_name_index), $"model name index is {model_name_index}");
        var index = Array.IndexOf(tag_name_index, model_name_index);
        model_objects[index].SetActive(true);
        for (var i = 0; i < models.Length; i++)
        {
            if(i != index){model_objects[i].SetActive(false);}
        }

        if (model_name_index == "W")
        {
            if (isStop)
            {
                waiting_animator.SetBool("stop", true);
            }
            else
            {
                waiting_animator.SetBool("stop", false);
            }
            
        }

        return models[index];
    }

    public void Nod()
    {
        if (!model_objects[0].activeSelf)
        {
            model_objects[0].SetActive(true);
            for (var i = 0; i < models.Length; i++)
            {
                if(i != 0){model_objects[i].SetActive(false);}
            }
        }
        waiting_animator.SetBool("nod", true);
    }
}
