using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bytes
{

    public class ShopController : MonoBehaviour
    {
        private int numberSouls;
        private int playerHealth;
        private int playerDefense;
        private int playerAttack;
        private int healthIndex;
        private int defenseIndex;
        private int attackIndex;
        private int[] upgradePrice = { 5, 10, 20, 30, 50, 80 };
        private int[] playerLevel = { 5, 14, 35, 75, 150, 275 };

        [SerializeField] Player player;


        // Start is called before the first frame update

        void Start()
        {
            EventManager.AddEventListener("buyHealth", BuyHealth);
            EventManager.AddEventListener("buyDefense", BuyDefense);
            EventManager.AddEventListener("buyAttack", BuyAttack);
            initialiseValues();
        }

        public void BuyHealth(Data data)
        {
            Debug.Log("hello i'm buy health");
            int tempIndex = BuyItem(healthIndex, upgradePrice[healthIndex]);
            if (tempIndex != -1)
            {
                healthIndex = tempIndex;
                RefreshHealthValues();
            }
        
        
        }

        public void BuyDefense(Data data)
        {
            Debug.Log("hello i'm buying defense");
            int tempIndex = BuyItem(defenseIndex, upgradePrice[defenseIndex]);
            if (tempIndex != -1)
            {
                defenseIndex = tempIndex;
                RefreshDefenseValues();
            }
        }

        public void BuyAttack(Data data)
        {
            Debug.Log("hello i'm buying attack");
            int tempIndex = BuyItem(attackIndex, upgradePrice[attackIndex]);
            if (tempIndex != -1)
            {
                attackIndex = tempIndex;
               RefreshAttackValues();
            }
        }

        private int BuyItem(int level, int price)
        {
            if (numberSouls >= price)
            {
                level++;
                player.ReduceSouls(price);
                return level;
            }
            return -1;

        }

        private void initialiseValues()
        {
            numberSouls = player.Souls;
            playerHealth = player.Hp;
            playerDefense = player.Defense;
            playerAttack = player.Damage;
            healthIndex = FindUpgradeIndex(playerHealth);
            attackIndex = FindUpgradeIndex(playerAttack);
            defenseIndex = FindUpgradeIndex(playerDefense);
        }

        private void RefreshHealthValues()
        {
            /*Get the info of the player*/
            numberSouls = player.Souls;
            playerHealth = player.Hp;

        }

        private void RefreshDefenseValues()
        {
            numberSouls = player.Souls;
            playerDefense = player.Defense;
        }

        private void RefreshAttackValues()
        {
            numberSouls = player.Souls;
            playerAttack = player.Damage;
        }

        private int FindUpgradeIndex(int playerValue)
        {
            for (int i =0; i < playerLevel.Length; i++)
            {
                if (playerLevel[i] == playerValue)
                {
                    return i;
                }
            }
            return -1;
        }
    
    }
}
