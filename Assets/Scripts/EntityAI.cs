using System;
using System.Collections;
using System.Collections.Generic;
using Bytes;
using UnityEngine;

public class EntityAI : MonoBehaviour
{
    public Entity controlledEntity;
    [SerializeField] private string creatureCampName;
    [SerializeField] private Entity target;
    [SerializeField] private bool initOnStart = false;

    [SerializeField] private int alphaStatDamage = 1;
    [SerializeField] private int alphaStatHP = 5;
    [SerializeField] private int alphaStatArmor = 1;

    protected virtual void Start()
    {
        if (initOnStart)
            Initialize(creatureCampName);
    }

    public void Initialize (string creatureCampName, bool isAlpha = false)
    {
        this.creatureCampName = creatureCampName;

        //EventManager.AddEventListener("EntityOnDamageTaken", OnTakeDamage);
        // Receives aggro from its mates
        EventManager.AddEventListener("EntityOnAggro_" + creatureCampName, OnAggro);
        controlledEntity = GetComponent<Entity>();
        controlledEntity.OnTakeDamage.AddListener(OnTakeDamage);

        // Remove Aggro event listener
        controlledEntity.OnDie.AddListener((Entity e)=> {
            EventManager.RemoveEventListener("EntityOnAggro_" + creatureCampName, OnAggro);
        });

        controlledEntity.OnDieAnimDone.AddListener((Entity e) => {
            Destroy(this.gameObject);
        });

        controlledEntity.OnTakeDamage.AddListener((Entity e)=> {
            EventManager.Dispatch("playSound", new PlaySoundData("takeDamage"));
        });

        if (isAlpha) { SetAsAlphaMonster(); }
    }

    private void Update()
    {
        if (target != null && !controlledEntity.Dead && !target.Dead)
        {
            if (WalkInRange(target.transform))
            {
                controlledEntity.Attack();
            }
        }
    }

    private bool WalkInRange(Transform target)
    {
        // We still do rotation
        Vector2 dir = controlledEntity.GetDirectionFrom(target);
        controlledEntity.Rotate(dir);

        bool inRange = IsInRange();
        if (inRange) { dir = Vector2.zero; }
        controlledEntity.Move(dir);

        return inRange;
    }

    private bool IsInRange()
    {
        float dis = controlledEntity.GetDistanceFrom(target.transform);
        return dis <= controlledEntity.Range;
    }

    private void OnTakeDamage(Entity from)
    {
        if (from != null)
        {
            // Debug.Log("i received dmg!");
            EventManager.Dispatch("EntityOnAggro_" + creatureCampName, new EntityData(from));
            return;
        }
    }

    private void OnAggro(Bytes.Data data)
    {
        Aggro(((EntityData)data).entity);
    }

    protected void Aggro(Entity target)
    {
        // Can't aggro again if already the target
        if (target != null && this.target == target) { return; }

        //Debug.Log(this.name + " received aggro from camp named: " + creatureCampName + " and target is: " + target.name);
        this.target = target;
    }

    private void SetAsAlphaMonster()
    {
        controlledEntity.IncreaseDamage(alphaStatDamage);
        controlledEntity.IncreaseHp(alphaStatHP);
        controlledEntity.IncreaseDefense(alphaStatArmor);

        transform.localScale *= 1.25f;
    }

}