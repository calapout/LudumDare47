using System;
using System.Collections;
using System.Collections.Generic;
using Bytes;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private int _Souls = 0;
    private Entity PlayerEntity;
    private bool CanOpenShop = false;
    private bool ShopIsOpen = false;
    private bool IsMovementEnabled = true;
    [SerializeField] private int[] _Levels = new int[3] { 0, 0, 0 };//0 = Hp, 1 = Damage, 2 = Defense
    [SerializeField] private Camera _camera;

    [SerializeField] private Transform shopSpawn;
    [SerializeField] private Transform arenaSpawn;

    public int Souls
    {
        get => _Souls;
        private set => _Souls = value;
    }

    public int Hp
    {
        get => PlayerEntity.Hp;
        set => PlayerEntity.IncreaseHp(value - PlayerEntity.Hp);
    }

    public int HpLevel
    {
        get => _Levels[0];
        private set => _Levels[0] = value;
    }

    public int Damage
    {
        get => PlayerEntity.Damage;
        set => PlayerEntity.IncreaseDamage(value - PlayerEntity.Damage);
    }

    public int DamageLevel
    {
        get => _Levels[1];
        private set => _Levels[1] = value;
    }

    public int Defense
    {
        get => PlayerEntity.Defense;
        set => PlayerEntity.IncreaseDefense(value - PlayerEntity.Defense);
    }
    public int DefenseLevel
    {
        get => _Levels[2];
        private set => _Levels[2] = value;
    }

    public void OnMove(InputValue value)
    { 
        if (!IsMovementEnabled)
        {
            return;
        }
        Vector2 movement = value.Get<Vector2>();
        PlayerEntity.Move(movement);
    }

    public void OnAttack(InputValue value)
    {
        Debug.Log("i attack");
        PlayerEntity.Attack();
    }

    public void OnOrientation(InputValue value)
    {
        if (!IsMovementEnabled)
        {
            return;
        }

        PlayerInput input = GetComponent<PlayerInput>();
        Vector2 direction = value.Get<Vector2>();
        Vector3 direction3D;
        if (input.currentControlScheme == "Keyboard&Mouse")
        {
            direction3D = _camera.ScreenToWorldPoint(new Vector3(direction.x, direction.y, 0));
            direction = new Vector2(direction3D.x - gameObject.transform.position.x, direction3D.y - gameObject.transform.position.y).normalized;
        }
        PlayerEntity.Rotate(direction);
    }

    public void OnEntityDie(Bytes.Data data)
    {
        if (data is EntityData)
        {
            EntityData entityData = (EntityData)data;
            Entity entity = entityData.entity;
        }
    }

    public void OnInterract()
    {
        if (!ShopIsOpen && CanOpenShop)
        {
            EventManager.Dispatch("openShop", null);
            ShopIsOpen = true;
            IsMovementEnabled = false;
        }
    }

    public void OnLeaveShop()
    {
        if (ShopIsOpen)
        {
            CloseShop();
        }
        
    }

    public void CloseShop()
    {
        EventManager.Dispatch("closeShop", null);
        ShopIsOpen = false;
        IsMovementEnabled = true;
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

        PlayerEntity.OnDieAnimDone.AddListener((Entity e)=> {
            Animate.Delay(1.2f, Respawn);
        });
    }

    private void Update()
    {
        _camera.transform.position = new Vector3(transform.position.x, transform.position.y, _camera.transform.position.z);
    }

    private void Respawn()
    {
        transform.position = shopSpawn.position;
        PlayerEntity.ResetEntity();
    }

}
