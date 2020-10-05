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
        private int[] upgradePrice = { 3, 8, 16, 24, 36, 50, 70, 90, 120 };

        private int[] hpGrowthLevel = { 10, 15, 20, 20, 25, 35, 40, 45, 50 };
        private int[] atkGrowthLevel = { 1, 2, 2, 2, 3, 3, 4, 5, 5 };
        private int[] armorGrowthLevel = { 1, 1, 1, 1, 2, 2, 3, 3, 4 };

        [SerializeField] Player player;


        // Start is called before the first frame update

        void Start()
        {
            EventManager.AddEventListener("initializeValues", InitialiseValues);
            EventManager.AddEventListener("buyHealth", BuyHealth);
            EventManager.AddEventListener("buyDefense", BuyDefense);
            EventManager.AddEventListener("buyAttack", BuyAttack);
            EventManager.AddEventListener("closeShopController", CloseShopController);
            //InitialiseValues();
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
            player.Hp = PlayerHealth + hpGrowthLevel[HealthIndex];
            RefreshHealthValues();

            PlayAccordingSound("buy");
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
            player.Defense = PlayerDefense + armorGrowthLevel[DefenseIndex];
            RefreshDefenseValues();

            PlayAccordingSound("buy");
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
            player.Damage = PlayerAttack + atkGrowthLevel[AttackIndex];
            RefreshAttackValues();

            PlayAccordingSound("buy");
        }

        public void InitialiseValues(Bytes.Data data)
        {
            NumberSouls = player.Souls;
            PlayerHealth = player.Hp;
            PlayerDefense = player.Defense;
            PlayerAttack = player.Damage;
            HealthIndex = player.HpLevel;
            AttackIndex = player.DamageLevel;
            DefenseIndex = player.DefenseLevel;

            EventManager.Dispatch("updateSouls", new ShopData(NumberSouls));
            EventManager.Dispatch("updateHealth", new ShopData(upgradePrice[HealthIndex], PlayerHealth, hpGrowthLevel[HealthIndex]));
            EventManager.Dispatch("updateAttack", new ShopData(upgradePrice[AttackIndex], PlayerAttack, atkGrowthLevel[AttackIndex]));
            EventManager.Dispatch("updateDefense", new ShopData(upgradePrice[DefenseIndex], PlayerDefense, armorGrowthLevel[DefenseIndex]));
        }

        public void CloseShopController(Data data)
        {
            player.CloseShop();

            PlayAccordingSound("UIGenericBtn");
        }

        private void RefreshHealthValues()
        {
            /*Get the info of the player*/
            NumberSouls = player.Souls;
            PlayerHealth = player.Hp;
            HealthIndex = player.HpLevel;
            Debug.Log("Health Index " + HealthIndex);

            EventManager.Dispatch("updateSouls", new ShopData(NumberSouls));
            EventManager.Dispatch("updateHealth", new ShopData(upgradePrice[HealthIndex], PlayerHealth, hpGrowthLevel[HealthIndex], (HealthIndex == hpGrowthLevel.Length - 1)));

        }

        private void RefreshDefenseValues()
        {
            NumberSouls = player.Souls;
            PlayerDefense = player.Defense; 
            DefenseIndex = player.DefenseLevel;
            EventManager.Dispatch("updateSouls", new ShopData(NumberSouls));
            EventManager.Dispatch("updateDefense", new ShopData(upgradePrice[DefenseIndex], PlayerDefense, armorGrowthLevel[DefenseIndex], (DefenseIndex == armorGrowthLevel.Length - 1)));
        }

        private void RefreshAttackValues()
        {
            NumberSouls = player.Souls;
            PlayerAttack = player.Damage;
            AttackIndex = player.DamageLevel;
            EventManager.Dispatch("updateSouls", new ShopData(NumberSouls));
            EventManager.Dispatch("updateAttack", new ShopData(upgradePrice[AttackIndex], PlayerAttack, atkGrowthLevel[AttackIndex], (AttackIndex == atkGrowthLevel.Length - 1) ));
        }


        private void PlayAccordingSound(string suffix)
        {
            EventManager.Dispatch("playSound", new PlaySoundData(suffix));
        }

    }
}
