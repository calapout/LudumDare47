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
    [SerializeField] private int[] _Levels = new int[3]{0,0,0};//0 = Hp, 1 = Damage, 2 = Defense
    [SerializeField] private Camera _camera;

    public float Speed
    {
        get => _Speed;
    }

    public int Souls
    {
        get => _Souls;
        private set => _Souls = value;
    }

    public int Hp
    {
        get => PlayerEntity.Hp;
        set => PlayerEntity.IncreaseHp(PlayerEntity.Hp - value);
    }
    
    public int HpLevel
    {
        get => _Levels[0];
        private set => _Levels[0] = value;
    }
    
    public int Damage
    {
        get => PlayerEntity.Damage;
        set => PlayerEntity.IncreaseDamage(PlayerEntity.Damage - value);
    }
    
    public int DamageLevel
    {
        get => _Levels[1];
        private set => _Levels[1] = value;
    }
    
    public int Defense
    {
        get => PlayerEntity.Defense;
        set => PlayerEntity.IncreaseDefense(PlayerEntity.Defense - value);
    }
    public int DefenseLevel
    {
        get => _Levels[2];
        private set => _Levels[2] = value;
    }

    public void OnMove(InputValue value)
    {
        PlayerEntity.Move(value.Get<Vector2>() * Speed);
    }

    public void OnAttack(InputValue value)
    {
        PlayerEntity.Attack();
    }

    public void OnOrientation(InputValue value)
    {
        PlayerInput input = GetComponent<PlayerInput>();
        Vector2 direction = value.Get<Vector2>();
        Vector3 direction3D;
        if (input.currentControlScheme == "Keyboard&Mouse")
        {
            direction3D = _camera.ScreenToWorldPoint(new Vector3(direction.x, direction.y, 0)).normalized;
            direction = new Vector2(direction3D.x, direction3D.y);
        }
        PlayerEntity.Rotate(direction);
    }

    public void OnEntityDie(Bytes.Data data)
    {
        if (data is EntityData)
        {
            EntityData entityData = (EntityData)data;
            Entity entity = entityData.entity;
            this._Souls += entity.SoulValue;
            if (entity == PlayerEntity)
            {
                Respawn();
            }
        }
    }

    public int LevelUpHp()
    {
        return this.HpLevel++;
    }
    
    public int LevelUpDamage()
    {
        return this.DamageLevel++;
    }
    
    public int LevelUpDefense()
    {
        return this.DefenseLevel++;
    }
    
    public int ReduceSouls(int quantity)
    {
        return this.Souls -= quantity;
    }
    
    
    private void Awake()
    {
        EventManager.AddEventListener("EntityDie", OnEntityDie);
        PlayerEntity = GetComponent<Entity>();
    }

    private void Update()
    {
        _camera.transform.position = new Vector3(transform.position.x, transform.position.y, _camera.transform.position.z);
    }

    private void Respawn()
    {
        
    }
}
