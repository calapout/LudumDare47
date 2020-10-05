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
        EventManager.AddEventListener("openShop", OpenShop);
        EventManager.AddEventListener("closeShop", CloseShop);

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

    public void ButtonClosePressed()
    {
        EventManager.Dispatch("closeShopController", null);
    }
    

    public void UpdateHealth(Bytes.Data data)
    {
        playerHealth.text = ((ShopData)data).PlayerValue.ToString();

        string upgradeTxt = "+" + ((ShopData)data).Upgrade;
        if (((ShopData)data).IsMaxIndex) { upgradeTxt = "-"; }
        upgradeHealth.text = upgradeTxt;

        healthButton.GetComponentInChildren<Text>().text = ((ShopData)data).Cost.ToString();
    }

    public void UpdateDefense(Bytes.Data data)
    {
        playerDefense.text = ((ShopData)data).PlayerValue.ToString();

        string upgradeTxt = "+" + ((ShopData)data).Upgrade;
        if (((ShopData)data).IsMaxIndex) { upgradeTxt = "-"; }
        upgradeDefense.text = upgradeTxt;

        defenseButton.GetComponentInChildren<Text>().text = ((ShopData)data).Cost.ToString();
    }

    public void UpdateAttack(Bytes.Data data)
    {
        playerAttack.text = ((ShopData)data).PlayerValue.ToString();

        string upgradeTxt = "+" + ((ShopData)data).Upgrade;
        if (((ShopData)data).IsMaxIndex) { upgradeTxt = "-"; }
        upgradeAttack.text = upgradeTxt;

        attackButton.GetComponentInChildren<Text>().text = ((ShopData)data).Cost.ToString();
    }

    public void UpdateSouls(Bytes.Data data)
    {
        playerSouls.text = ((ShopData)data).SoulsCollected.ToString();
    }

    

    public void CloseShop(Data data)
    {
        this.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
    }

    public void OpenShop(Data data)
    {
        this.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
        EventManager.Dispatch("initializeValues", null);
    }

}
