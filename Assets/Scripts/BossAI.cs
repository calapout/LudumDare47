using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Bytes;

public class BossAI : EntityAI
{
    protected override void Start()
    {
        base.Start();

        Aggro(GameObject.Find("Player").GetComponent<Entity>());

        controlledEntity.OnDie.AddListener((Entity e)=> {
            EventManager.Dispatch("bossDefeated", null);
        });
    }
    public void CancelBoss()
    {
        controlledEntity.OnDie.RemoveAllListeners();
        controlledEntity.Die(false);
    }
}
