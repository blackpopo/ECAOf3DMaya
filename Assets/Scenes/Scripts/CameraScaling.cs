using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScaling : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image scalingImage;

    private bool upscale = false;

    private void Start()
    {
        scalingImage.color = Color.cyan;
    }

    [SerializeField] private Transform[] models;
    public void buttonClick()
    {
        if (upscale)
        {
            upscale = false;
            foreach (var model in models)
            {
                model.localScale = Vector3.one;
                model.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            }
            scalingImage.color = Color.cyan;
        }
        else
        {
            upscale = true;
            foreach (var model in models)
            {
                model.localScale = new Vector3(2.0f, 2.0f, 1.0f);
                model.transform.position = new Vector3(0.0f, -0.5f, 0.0f);
            }
            scalingImage.color = Color.magenta;
        }
        
    }
}
