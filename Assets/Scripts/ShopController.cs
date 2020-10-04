using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bytes
{

    public class ShopController : MonoBehaviour
    {   private int NumberSouls { get; set; }
        private int PlayerHealth { get; set; }
        private int PlayerDefense { get; set; }
        private int PlayerAttack { get; set; }
        private int HealthIndex { get; set; }
        private int DefenseIndex { get; set; }
        private int AttackIndex { get; set; }
        private int[] upgradePrice = { 5, 10, 20, 30, 50, 80 };
        private int[] playerLevel = { 10, 14, 35, 75, 150, 275 };

        [SerializeField] Player player;


        // Start is called before the first frame update

        void Start()
        {
            EventManager.AddEventListener("initializeValues", InitialiseValues);
            EventManager.AddEventListener("buyHealth", BuyHealth);
            EventManager.AddEventListener("buyDefense", BuyDefense);
            EventManager.AddEventListener("buyAttack", BuyAttack);
            //EventManager.Dispatch("initializeValues", null);
            InitialiseValues();
        }

        public void BuyHealth(Data data)
        {
            Debug.Log("i am buying health");
            int price = upgradePrice[HealthIndex];
            if ( HealthIndex >= (upgradePrice.Length -1) || NumberSouls < price)
            {
                return;
            }
                player.LevelUpHp();
                player.ReduceSouls(price);
                player.Hp = PlayerHealth + playerLevel[HealthIndex];
                RefreshHealthValues();
        
        }

        public void BuyDefense(Data data)
        {
            int price = upgradePrice[DefenseIndex];
            if ( DefenseIndex >= (upgradePrice.Length - 1) || NumberSouls < price)
            {
                return;
            }
                player.LevelUpDefense();
                player.ReduceSouls(price);
                player.Defense = PlayerDefense + playerLevel[DefenseIndex];
                RefreshDefenseValues();
        }

        public void BuyAttack(Data data)
        {
            int price = upgradePrice[AttackIndex];
            if (AttackIndex >= (upgradePrice.Length - 1) || NumberSouls < price)
            {
                return;
            }
                player.LevelUpDamage();
                player.ReduceSouls(price);
                player.Damage = PlayerAttack + playerLevel[AttackIndex];
                RefreshAttackValues();
        }

        public void InitialiseValues()
        {
            NumberSouls = player.Souls;
            PlayerHealth = player.Hp;
            PlayerDefense = player.Defense;
            PlayerAttack = player.Damage;
            HealthIndex = player.HpLevel;
            AttackIndex = player.DamageLevel;
            DefenseIndex = player.DefenseLevel;

            EventManager.Dispatch("updateSouls", new ShopData(NumberSouls));
            EventManager.Dispatch("updateHealth", new ShopData(upgradePrice[HealthIndex], PlayerHealth, playerLevel[HealthIndex]));
            EventManager.Dispatch("updateAttack", new ShopData(upgradePrice[AttackIndex], PlayerAttack, playerLevel[AttackIndex]));
            EventManager.Dispatch("updateDefense", new ShopData(upgradePrice[DefenseIndex], PlayerDefense, playerLevel[DefenseIndex]));
        }

        private void RefreshHealthValues()
        {
            /*Get the info of the player*/
            NumberSouls = player.Souls;
            PlayerHealth = player.Hp;
            HealthIndex = player.HpLevel;
            Debug.Log("Health Index " + HealthIndex);

            EventManager.Dispatch("updateSouls", new ShopData(NumberSouls));
            EventManager.Dispatch("updateHealth", new ShopData(upgradePrice[HealthIndex], PlayerHealth, playerLevel[HealthIndex]));

        }

        private void RefreshDefenseValues()
        {
            NumberSouls = player.Souls;
            PlayerDefense = player.Defense; 
            DefenseIndex = player.DefenseLevel;
            EventManager.Dispatch("updateSouls", new ShopData(NumberSouls));
            EventManager.Dispatch("updateDefense", new ShopData(upgradePrice[DefenseIndex], PlayerDefense, playerLevel[DefenseIndex]));
        }

        private void RefreshAttackValues()
        {
            NumberSouls = player.Souls;
            PlayerAttack = player.Damage;
            AttackIndex = player.DamageLevel;
            EventManager.Dispatch("updateSouls", new ShopData(NumberSouls));
            EventManager.Dispatch("updateAttack", new ShopData(upgradePrice[AttackIndex], PlayerAttack, playerLevel[AttackIndex]));
        }
    
    }
}
