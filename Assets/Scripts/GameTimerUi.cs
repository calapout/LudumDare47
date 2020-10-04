using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Bytes;
using UnityEngine;
using UnityEngine.UI;

public class GameTimerUi : MonoBehaviour
{
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
    }


}
