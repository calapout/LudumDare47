using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCamp : MonoBehaviour
{
    public GameObject monsterToSpawn;
    public EntityAI[] monsters;
    public Transform[] spawnPoints;
    public string monsterCampName;
    private void Start()
    {
        RespawnCreatures();

        //Bytes.Animate.Delay(5f, RespawnCreatures);
    }
    public void RespawnCreatures()
    {
        int i = 0;
        for (int r = 0; r < monsters.Length; r++)
        {
            if (monsters[r] != null)
            {
                monsters[r].controlledEntity.Die(false);
            }
        }
        monsters = new EntityAI[spawnPoints.Length];
        foreach (Transform spawnPoint in spawnPoints)
        {
            monsters[i] = CreateCreature(spawnPoint);
            monsters[i].Initialize(monsterCampName, isAlpha: (i == 1));
            i++;
        }
    }
    private EntityAI CreateCreature(Transform tr)
    {
        var e = InstantiatePrefab(monsterToSpawn, tr).GetComponent<EntityAI>();
        return e;
    }

    private GameObject InstantiatePrefab(GameObject prefab, Transform tr, float timeToLive = -1)
    {
        var g = Instantiate(prefab, tr.position, this.transform.rotation);
        g.transform.SetParent(tr);
        g.transform.position = tr.position;
        
        return g;
    }
}
