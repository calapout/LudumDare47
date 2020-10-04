using System.Collections;
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
        EventManager.AddEventListener("openWindow", OpenWindow);
        EventManager.AddEventListener("closeWindow", CloseWindow);

    }

    private void Awake()
    {
        
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

    public void UpdateHealth(Bytes.Data data)
    {
        playerHealth.text = ((ShopData)data).PlayerValue.ToString();
        upgradeHealth.text = "+"+ ((ShopData)data).Upgrade ;
        healthButton.GetComponentInChildren<Text>().text = ((ShopData)data).Cost.ToString();
    }

    public void UpdateDefense(Bytes.Data data)
    {
        playerDefense.text = ((ShopData)data).PlayerValue.ToString();
        upgradeDefense.text = "+" + ((ShopData)data).Upgrade;
        defenseButton.GetComponentInChildren<Text>().text = ((ShopData)data).Cost.ToString();
    }

    public void UpdateAttack(Bytes.Data data)
    {
        playerAttack.text = ((ShopData)data).PlayerValue.ToString();
        upgradeAttack.text = "+" + ((ShopData)data).Upgrade;
        attackButton.GetComponentInChildren<Text>().text = ((ShopData)data).Cost.ToString();
    }

    public void UpdateSouls(Bytes.Data data)
    {
        playerSouls.text = ((ShopData)data).SoulsCollected.ToString();
    }

    

    public void CloseWindow(Data data)
    {
        gameObject.SetActive(false);
    }

    public void OpenWindow(Data data)
    {
        gameObject.SetActive(true);
    }

}
