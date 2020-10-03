using Bytes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData : Data
{
    public int SoulsCollected { get; private set; }
    public int Cost { get; private set; }
    public int PlayerValue { get; private set; }
    public int Upgrade { get; private set; }

    /*if health was upgraded*/
    public ShopData(int soulsCollected, int cost, int playerValue, int upgrade)
    {
        SoulsCollected = soulsCollected;
        Cost = cost;
        PlayerValue = playerValue;
        Upgrade = upgrade;
    }

}
