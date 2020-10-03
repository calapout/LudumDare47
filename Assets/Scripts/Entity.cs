using System;
using System.Collections;
using System.Collections.Generic;
using Bytes;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private int _Hp, _MaxHp, _Damage, _Defense, _Level, _SoulValue;
    private Vector2 direction = Vector2.zero;
    private Rigidbody2D RB;
    private SpriteRenderer SR;
    public int Hp
    {
        get => _Hp;
        private set => _Hp = value;
    }

    public int MaxHp
    {
        get => _MaxHp;
        private set => _MaxHp = value;
    }

    public int Damage
    {
        get => _Damage;
        private set => _Damage = value;
    }

    public int Defense
    {
        get => _Defense;
        private set => _Defense = value;
    }

    public int Level
    {
        get => _Level;
        private set => _Level = value;
    }

    public int SoulValue
    {
        get => _SoulValue;
    }

    public void ResetEntity()
    {
        Hp = MaxHp;
        if (SR != null)
        {
            SR.color = new Color(1f,1f,1f,1f);
        }
    }

    public void IncreaseDefense(int addedDefense)
    {
        this.Defense += addedDefense;
    }

    public void IncreaseHp(int addedHp)
    {
        this.Hp += addedHp;
        this.MaxHp += addedHp;
    }

    public void IncreaseDamage(int addedDamage)
    {
        this.Damage += Damage;
    }

    private void IncreaseLevel()
    {
        this.Level++;
    }

    private void TakeDamage(int takenDamage)
    {
        this.Hp -= takenDamage;
        if (this.Hp <= 0)
        {
            this.Hp = 0;
            Die();
        }
    }

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>() ?? null;
        SR = GetComponent<SpriteRenderer>() ?? null;
        ResetEntity();
    }
    
    private void Die()
    {
        EventManager.Dispatch("EntityDie", new EntityData(this));
        if (SR != null)
        {
            SR.color = new Color(1f,1f,1f,0f);
        }
    }

    public void Move(Vector2 direction)
    {
        this.direction = direction;
    }

    public void Attack()
    {
        
    }

    private void FixedUpdate()
    {
        if (direction == Vector2.zero)
        {
            return;
        }
        RB?.AddRelativeForce(direction);
        direction = Vector2.zero;
    }
};