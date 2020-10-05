using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Bytes;
using UnityEngine;
using UnityEngine.UI;

public class GameTimerUi : MonoBehaviour
{
    public Text txt;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        EventManager.AddEventListener("GameTimerUpdate", UpdateFilling);
    }

    private void UpdateFilling(Bytes.Data data)
    {
        if (!(data is GameTimerData))
        {
            return;
        }
        GameTimerData timerData = (GameTimerData) data;
        image.fillAmount = timerData.pourcent;

        float timeLeft = timerData.TimeLeft;
        int minutes = GetMinutes(timeLeft);
        int seconds = (int)((timeLeft - (float)minutes * 60));
        txt.text = minutes + ":" + seconds;
    }

    private int GetMinutes(float timeLeft)
    {
        int minutes = 0;
        while (timeLeft >= 60) { timeLeft -= 60; minutes++; }
        return minutes;
    }

}
