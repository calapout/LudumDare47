using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bytes;

public class ObjectCreationManager : MonoBehaviour
{
    public GameObject soulPrefab;
    public GameObject damageTextPrefab;

    private void Awake()
    {
        EventManager.AddEventListener("createSoulsplosion", CreateSoulsplosionAt);
        EventManager.AddEventListener("createDamageText", CreateDamageTextAt);
    }

    private void Start()
    {
        /*Animate.Repeat(1f, () =>
        {
            EventManager.Dispatch("createSoulsplosion", new SoulsplosionData(Random.Range(3, 7), new Vector2(Random.Range(-5, 5), Random.Range(-5, 5))));
            return true;
        }, 12);*/
    }

    public void CreateSoulsplosionAt(Bytes.Data data)
    {
        SoulsplosionData soulData = (SoulsplosionData)data;
        for (int i = 0; i < soulData.Amount; i++)
        {
            var g = InstantiatePrefab(soulPrefab, soulData.Position);
        }
    }

    public void CreateDamageTextAt(Bytes.Data data)
    {
        DamageTextData dmgTxtData = (DamageTextData)data;
        CreateDamageTextAt(dmgTxtData.Amount, dmgTxtData.Position, dmgTxtData.TimeToLive);
    }

    private void CreateSoulsplosionAt(int amount, Vector2 position)
    {
        for (int i = 0; i < amount; i++)
        {
            var g = InstantiatePrefab(soulPrefab, position);
        }
    }

    private void CreateDamageTextAt(int dmgAmount, Vector2 position, float timeToLive = 3f)
    {
        var g = InstantiatePrefab(soulPrefab, position, timeToLive);
        
    }

    private GameObject InstantiatePrefab(GameObject prefab, Vector2 position, float timeToLive = -1)
    {
        var g = Instantiate(prefab, position, Quaternion.identity);

        if (timeToLive != -1) { Animate.Delay(timeToLive, ()=> { Destroy(g); }); }

        return g;
    }

    public class SoulsplosionData : Bytes.Data
    {
        public SoulsplosionData(int amount, Vector2 position) { Amount = amount; Position = position; }
        public int Amount { get; private set; }
        public Vector2 Position { get; private set; }
    }
    public class DamageTextData : Bytes.Data
    {
        public DamageTextData(int amount, Vector2 position, float timeToLive = 3f) { Amount = amount; Position = position; TimeToLive = timeToLive; }
        public int Amount { get; private set; }
        public Vector2 Position { get; private set; }
        public float TimeToLive { get; private set; }
    }
}
