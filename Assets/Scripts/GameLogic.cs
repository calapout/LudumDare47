using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bytes;

public class GameLogic : MonoBehaviour
{
    //public MonsterCamp bossCamp;
    MonsterCamp[] camps;
    private void Start()
    {
        camps = GameObject.FindObjectsOfType<MonsterCamp>(true);

        //Animate.Delay(0.5f, StartArena);

        EventManager.AddEventListener("startArena", StartArenaNewRunWithFade);
        EventManager.AddEventListener("GameTimerTimeOut", SpawnBoss);
        EventManager.AddEventListener("playerDied", StopArena);
    }
    public void StartArena()
    {
        EventManager.Dispatch("GameTimerStart", null);
        RespawnCamps();
    }
    public void StopArena(Bytes.Data data)
    {
        KillAllCreatures();
        EventManager.Dispatch("GameTimerStop", null);
    }
    private void StartArenaNewRunWithFade(Bytes.Data data)
    {
        EventManager.Dispatch("StartFadeOut", null);
        EventManager.AddEventListener("FadedOut", StartArenaNewRun);
    }
    private void StartArenaNewRun(Bytes.Data data)
    {
        EventManager.RemoveEventListener("FadedOut", StartArenaNewRun);
        EventManager.Dispatch("placePlayerInArena", null);
        StartArena();
    }

    private void SpawnBoss(Bytes.Data data)
    {
        KillAllCreatures();
        print("Spawn boss!");
    }

    private void RespawnCamps()
    {
        foreach (MonsterCamp camp in camps)
        {
            camp.RespawnCreatures();
        }
    }

    private void KillAllCreatures()
    {
        foreach (MonsterCamp camp in camps)
        {
            camp.KillCreatures();
        }
    }

}
