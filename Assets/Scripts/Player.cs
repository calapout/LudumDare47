using System;
using System.Collections;
using System.Collections.Generic;
using Bytes;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _Speed = 1;
    [SerializeField] private int _Souls = 0;
    private Entity PlayerEntity;

    public float Speed
    {
        get => _Speed;
    }

    public int Souls
    {
        get => _Souls;
        private set => _Souls = value;
    }

    public int Damage
    {
        get => PlayerEntity.Damage;
        set => PlayerEntity.IncreaseDamage(PlayerEntity.Damage - value);
    }
    
    public int Defense
    {
        get => PlayerEntity.Defense;
        set => PlayerEntity.IncreaseDefense(PlayerEntity.Defense - value);
    }
    
    public int Hp
    {
        get => PlayerEntity.Hp;
        set => PlayerEntity.IncreaseHp(PlayerEntity.Hp - value);
    }

    public void OnMove(InputValue value)
    {
        PlayerEntity.Move(value.Get<Vector2>() * Speed);
    }

    public void OnAttack(InputValue value)
    {
        PlayerEntity.Attack();
    }

    public void OnEntityDie(Bytes.Data data)
    {
        if (data is EntityData)
        {
            EntityData entityData = (EntityData)data;
            Entity entity = entityData.entity;
            this._Souls += entityData.entity.SoulValue;
            if (entity == PlayerEntity)
            {
                Respawn();
            }
        }
    }

    private void Awake()
    {
        EventManager.AddEventListener("EntityDie", OnEntityDie);
        PlayerEntity = GetComponent<Entity>();
    }

    private void Respawn()
    {
        
    }

    public int ReduceSouls(int quantity)
    {
        return this.Souls -= quantity;
    }
}
