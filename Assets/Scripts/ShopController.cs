using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    private int numberSouls = 5;
    private int healthLevel = 0;
    private int armorLevel = 0;
    private int strengthLevel = 0;
    private int[] upgradePrice = { 5, 10, 20, 30, 50, 80 };

    [SerializeField] Text soulsText;
    [SerializeField] Slider healthBar;
    [SerializeField] Slider armorBar;
    [SerializeField] Slider strengthBar;

    // Start is called before the first frame update

    void Start()
    {
        /*Take values from player*/
        soulsText.text = numberSouls.ToString();
        /*Get number of souls from player*/
        /*Get level for each category*/

    }

    public void buyHealth(Text text)
    {
        if (numberSouls >= int.Parse(text.text))
        {
            healthLevel++;
            healthBar.value = healthLevel * 10;
            if (healthLevel + 1 < upgradePrice.Length)
            {
                text.text = upgradePrice[healthLevel + 1].ToString();
            }
        }
        
    }

    public void buyArmor(Text text)
    {
        armorLevel++;
        armorBar.value = armorLevel * 10;
        if (armorLevel + 1 < upgradePrice.Length)
        {
            text.text = upgradePrice[armorLevel + 1].ToString();
        }
    }

    public void buyStrength(Text text)
    {
        strengthLevel++;
        strengthBar.value = strengthLevel * 10;
        if (strengthLevel + 1 < upgradePrice.Length)
        {
            text.text = upgradePrice[strengthLevel + 1].ToString();
        }
    }

    
}
