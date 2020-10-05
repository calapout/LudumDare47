using Bytes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData : Bytes.Data
{
    public int SoulsCollected { get; private set; }
    public int Cost { get; private set; }
    public int PlayerValue { get; private set; }
    public int Upgrade { get; private set; }
    public bool IsMaxIndex { get; private set; }
    public ShopData(int soulsCollected)
    {
        SoulsCollected = soulsCollected;
    }

    public ShopData(int cost, int playerValue, int upgrade, bool isMaxIndex = false)
    {
        Cost = cost;
        PlayerValue = playerValue;
        Upgrade = upgrade;
        IsMaxIndex = isMaxIndex;
    }

    

}
