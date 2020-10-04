using System;
using System.Collections;
using System.Collections.Generic;
using Bytes;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    private void Start()
    {
        EventManager.AddEventListener("StartFadeOut", StartAnimation);
    }

    private void StartAnimation(Bytes.Data data)
    {
        GetComponent<Animation>().Play();
    }

    public void FadedOut()
    {
        EventManager.Dispatch("FadedOut", null);
    }
    
    public void FadedDone()
    {
        EventManager.Dispatch("FadedDone", null);
    }
}
