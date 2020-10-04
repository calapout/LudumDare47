using System;
using System.Collections;
using System.Collections.Generic;
using Bytes;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float timeLeft;
    [SerializeField] private float timerLength = 120f;

    [SerializeField] private bool isTimerEnable = false;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddEventListener("GameTimerStart", EnableTimer);
        EventManager.AddEventListener("GameTimerStop", DisableTimer);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTimerEnable)
        {
            return;
        }
        timeLeft -= Time.deltaTime;
        EventManager.Dispatch("GameTimerUpdate", new GameTimerData(Mathf.Clamp(timeLeft/timerLength, 0, 1)));
        if (timeLeft <= 0)
        {
            TimeOut();
        }
    }

    private void SetTime(float time)
    {
        this.timeLeft = time;
    }

    private void EnableTimer(Bytes.Data data)
    {
        SetTime(timerLength);
        this.isTimerEnable = true;
    }

    private void DisableTimer(Bytes.Data data)
    {
        this.isTimerEnable = false;
    }

    private void TimeOut()
    {
        this.isTimerEnable = false;
        EventManager.Dispatch("GameTimerTimeOut", null);
    }
}
