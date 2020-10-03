using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    private int numberSouls = 100;
    private int healthLevel = 0;
    private int armorLevel = 0;
    private int strengthLevel = 0;
    private int[] upgradePrice = { 5, 10, 20, 30, 50, 80 };
    private int[] nextLevel = { 2, 5, 14, 35, 75, 150 };

    [SerializeField] Text soulsText;
    //[SerializeField] Player player;
    [SerializeField] Button[] buttons;

    // Start is called before the first frame update

    void Start()
    {
        soulsText.text = numberSouls.ToString();
        /*Find level*/
        

    }

    public void buyHealth(Text text)
    {
        /*if (numberSouls >= int.Parse(text.text))
        {
            healthLevel = buyItem(text, healthLevel, healthBar);
        }*/
        /**/

        
    }

    public void buyArmor(Text text)
    {
       /* if(numberSouls >= int.Parse(text.text))
        {
            armorLevel = buyItem(text, armorLevel, armorBar);
        }*/
    }

    public void buyStrength(Text text)
    {
      /*if (numberSouls >= int.Parse(text.text))
        {
            strengthLevel = buyItem(text, strengthLevel, strengthBar);
        }*/
    }

    public int buyItem(Text text, int level, Slider bar)
    {
        level++;
        bar.value = level * 20;
        numberSouls -=int.Parse(text.text);
        soulsText.text = numberSouls.ToString();
        if (level + 1 < upgradePrice.Length)
        {
            text.text = upgradePrice[level + 1].ToString();
        }
        return level;

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
