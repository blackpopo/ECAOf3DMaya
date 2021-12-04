using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using UnityEngine.Assertions;

public class FaceController: MonoBehaviour
{
    // Update is called once per frame
    private CubismModel model;

    private CubismParameter param;

    private CubismParameter cheek_param;

    private bool value_change;

    private bool embarrassed;

    void LateUpdate()
    {
        if (param != null && value_change)
        {
            param.Value = 1.0f;
            if (embarrassed)
            {
                cheek_param.Value = 1.0f;
            }
        }
    }
    
    
    public void FaceChange(CubismModel input_model, string paramID)
    {
        model = input_model;
        if (paramID == "Normal")
        {
            if (!ReferenceEquals(null, param))
            {
                param.Value = 0.0f;
            }
        }
        else
        {
            param = model.Parameters.FindById("Param" + paramID);
            Assert.IsNotNull(param, $"Parameter {param} : Param ID {paramID} is not found");
            if (paramID == "Embarrassed") //頬染め
            {
                embarrassed = true;
                cheek_param = model.Parameters.FindById("ParamCheek");
                Assert.IsNotNull(cheek_param, $"Cheek Parameter {cheek_param} is not found");
            }
            value_change = true;
        }
    }

    public void FaceBack()
    {
        if (!ReferenceEquals(null, param) && value_change)
        {
            param.Value = 0.0f;
            value_change = false;
            if (embarrassed)
            {
                embarrassed = false;
                cheek_param.Value = 0.0f;
            }
        }
    }
}
