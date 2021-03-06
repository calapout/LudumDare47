﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Bytes;
using UnityEngine;

using UnityEngine.Events;

[SerializeField] [Serializable] public class EntityEvent : UnityEvent<Entity> { }

public enum Clan { Ally, Enemy }

public class Entity : MonoBehaviour
{
    static public int sprites_render_order = 50;

    [SerializeField] private int _Hp, _MaxHp, _Damage, _Defense, _Level, _SoulValue;
    private Vector2 direction = Vector2.zero;
    private Rigidbody2D RB;
    private SpriteRenderer SR;
    private bool canAttack = true;
    private bool canTakeDamage = true;
    private bool isDead = false;
    [SerializeField] private float attackCooldown, damageTakingCooldown, _MovementSpeed, _Range;
    [SerializeField] private EntityAnimationController _animController;

    private SpriteRenderer[] spRenderers;
    private int[] spRenderersOrderBuffer;
    private int originalRenderingOrderOffset;

    public Clan clan = Clan.Enemy;
    public EntityEvent OnTakeDamage;
    public EntityEvent OnDie;
    public EntityEvent OnDieAnimDone;

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
    
    public float MovementSpeed
    {
        get => _MovementSpeed;
        private set => _MovementSpeed = value;
    }

    public int SoulValue
    {
        get => _SoulValue;
    }

    public float Range
    {
        get => _Range;
        private set => _Range = value;
    }

    public bool Dead
    {
        get;
    }

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>() ?? null;
        SR = GetComponent<SpriteRenderer>() ?? null;
        _animController = gameObject.GetComponentInChildren<EntityAnimationController>();
        //ResetEntity();

        spRenderers = transform.GetComponentsInChildren<SpriteRenderer>();

        SaveSortingOrders();

        originalRenderingOrderOffset = sprites_render_order; // Before changing value
        SetRenderingOrder();
        sprites_render_order += spRenderers.Length + 2;
    }

    #region Order
    private void SaveSortingOrders()
    {
        spRenderersOrderBuffer = new int[spRenderers.Length];
        for (int x = 0; x < spRenderers.Length; x++) { spRenderersOrderBuffer[x] = spRenderers[x].sortingOrder; }
    }

    private void LoadSortingOrders()
    {
        for (int x = 0; x < spRenderers.Length; x++) { spRenderers[x].sortingOrder = spRenderersOrderBuffer[x]; }
    }

    public void SetRenderingOrder()
    {
        int usedOffset = originalRenderingOrderOffset;
        LoadSortingOrders();
        foreach (SpriteRenderer sp in spRenderers)
        {
            sp.sortingOrder = sp.sortingOrder + usedOffset;
        }
    }
    #endregion

    public void ResetEntity()
    {
        Hp = MaxHp;
        isDead = false;
        this.direction = Vector2.zero;
        this._animController.enabled = true;
        _animController.StopMovingAnimation();
        _animController.PlayAttackAnim();
    }

    public void IncreaseDefense(int addedDefense)
    {
        this.Defense = Defense + addedDefense;
        IncreaseLevel();

    }

    public void IncreaseHp(int addedHp)
    {
        this.Hp = Hp + addedHp;
        this.MaxHp = MaxHp + addedHp;
        IncreaseLevel();

    }

    public void IncreaseDamage(int addedDamage)
    {
        this.Damage = Damage + addedDamage;
        IncreaseLevel();
    }

    private void IncreaseLevel()
    {
        this.Level++;
    }

    private void TakeDamage(Entity from)
    {
        if (isDead) { return; }

        //print("damage: "+ from.Damage);
        this.canTakeDamage = false;
        Invoke("EnableDamageTaking", damageTakingCooldown);
        int totalDmg = Mathf.Clamp(from.Damage - Defense, 1, 9999);
        this.Hp -= totalDmg;
        EventManager.Dispatch("createDamageText", new ObjectCreationManager.DamageTextData(totalDmg, this.transform.position + new Vector3(0, 0.1f, 0), 1f));
        OnTakeDamage?.Invoke(from);
        if (this.Hp <= 0)
        {
            this.Hp = 0;
            Die(true, true);
        }
    }
    
    public void Die(bool soulCreation = true, bool sound = false)
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        ThrowDieEvent(soulCreation, sound);
        /*if (SR != null)
        {
            SR.color = new Color(1f,1f,1f,0f);
        }*/
    }

    public void Move(Vector2 direction)
    {
        if (isDead) { return; }

        this.direction = direction * this.MovementSpeed;
        
        if (direction == Vector2.zero)
        {
            _animController?.StopMovingAnimation();
            return;
        }
        _animController?.StartMovingAnimation();
    }

    public void Rotate(Vector2 direction)
    {
        if (isDead) { return; }

        if (direction == Vector2.zero)
        {
            return;
        }
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Attack()
    {
        if (!canAttack || isDead)
        {
            return;
        }
        _animController?.PlayAttackAnim();
        DisableAttack();
        Invoke("EnableAttack", attackCooldown);
    }

    private void FixedUpdate()
    {
        if (isDead) { direction = Vector2.zero; }

        if (RB != null)
        { 
            RB.velocity = direction;
        }
    }

    private void EnableAttack()
    {
        this.canAttack = true;
    }
    
    private void DisableAttack()
    {
        this.canAttack = false;
    }

    private void EnableDamageTaking()
    {
        canTakeDamage = true;
    }

    public void StopMoving()
    {
        this.direction = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collider2d)
    {
        GameObject colliderObject = collider2d.gameObject;
        GameObject root = collider2d.transform.root.gameObject;
        if (colliderObject.CompareTag("canDamage") && root != gameObject && canTakeDamage)
        {
            Entity from = colliderObject.GetComponentInParent<Entity>();
            if (from.clan != this.clan) { TakeDamage(from); }
        }
    }

    private void ThrowDieEvent(bool soulCreation = true, bool sound = false)
    {
        OnDie?.Invoke(this);
        _animController?.PlayDieAnim(()=> {
            EventManager.Dispatch("EntityDie", new EntityData(this));
            OnDieAnimDone?.Invoke(this);
        }, sound);
        if(soulCreation) EventManager.Dispatch("createSoulsplosion", new ObjectCreationManager.SoulsplosionData(this.SoulValue, new Vector2(transform.position.x, transform.position.y)));
    }

    public Vector2 GetDirectionFrom(Transform targetTransform)
    {
        return (targetTransform.position - this.transform.position).normalized;
    }

    public float GetDistanceFrom(Transform targetTransform)
    {
        return Vector2.Distance(this.transform.position, targetTransform.position);
    }

};