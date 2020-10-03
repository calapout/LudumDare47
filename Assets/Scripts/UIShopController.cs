using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bytes;

public class UIShopController : MonoBehaviour
{

    private void Start()
    {
        
    }

    public void buyHealth()
    {
        EventManager.Dispatch("buyHealth", null);
    }

    public void buyDefense()
    {
        EventManager.Dispatch("buyDefense", null);
    }

    public void buyStrength()
    {
        EventManager.Dispatch("buyAttack", null);
    }

    public void closeWindow()
    {
        gameObject.SetActive(false);
    }

    public void openWindow()
    {
        gameObject.SetActive(true);
    }

}
