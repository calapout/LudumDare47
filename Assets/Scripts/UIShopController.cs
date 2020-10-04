﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bytes;

public class UIShopController : MonoBehaviour
{
    
    [SerializeField] Button healthButton;
    [SerializeField] Button attackButton;
    [SerializeField] Button defenseButton;

    [SerializeField] Text playerHealth;
    [SerializeField] Text playerAttack;
    [SerializeField] Text playerDefense;

    [SerializeField] Text upgradeHealth;
    [SerializeField] Text upgradeAttack;
    [SerializeField] Text upgradeDefense;

    [SerializeField] Text playerSouls;

    private void Start()
    {
        EventManager.AddEventListener("updateHealth", UpdateHealth);
        EventManager.AddEventListener("updateAttack", UpdateAttack);
        EventManager.AddEventListener("updateDefense", UpdateDefense);
        EventManager.AddEventListener("updateSouls", UpdateSouls);
        EventManager.Dispatch("initializeValues", null);
    }

    public void BuyHealthUI()
    {
        EventManager.Dispatch("buyHealth", null);
    }

    public void BuyDefenseUI()
    {
        EventManager.Dispatch("buyDefense", null);
    }

    public void BuyAttackUI()
    {
        EventManager.Dispatch("buyAttack", null);
    }

    public void UpdateHealth(Data data)
    {
        playerHealth.text = ((ShopData)data).PlayerValue.ToString();
        upgradeHealth.text = "+"+ ((ShopData)data).Upgrade ;
        healthButton.GetComponentInChildren<Text>().text = ((ShopData)data).Cost.ToString();
    }

    public void UpdateDefense(Data data)
    {
        playerDefense.text = ((ShopData)data).PlayerValue.ToString();
        upgradeDefense.text = "+" + ((ShopData)data).Upgrade;
        defenseButton.GetComponentInChildren<Text>().text = ((ShopData)data).Cost.ToString();
    }

    public void UpdateAttack(Data data)
    {
        playerAttack.text = ((ShopData)data).PlayerValue.ToString();
        upgradeAttack.text = "+" + ((ShopData)data).Upgrade;
        attackButton.GetComponentInChildren<Text>().text = ((ShopData)data).Cost.ToString();
    }

    public void UpdateSouls(Data data)
    {
        playerSouls.text = ((ShopData)data).SoulsCollected.ToString();
    }

    

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    public void OpenWindow()
    {
        gameObject.SetActive(true);
    }

}
