using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Bytes;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private int _Hp, _MaxHp, _Damage, _Defense, _Level, _SoulValue;
    private Vector2 direction = Vector2.zero;
    private Rigidbody2D RB;
    private SpriteRenderer SR;
    private bool canAttack = true;
    private bool canTakeDamage = true;
    private bool isDead = false;
    [SerializeField] private float attackCooldown, damageTakingCooldown;
    [SerializeField] private EntityAnimationController _animController;

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
        IncreaseLevel();

    }

    public void IncreaseHp(int addedHp)
    {
        this.Hp += addedHp;
        this.MaxHp += addedHp;
        IncreaseLevel();

    }

    public void IncreaseDamage(int addedDamage)
    {
        this.Damage += Damage;
        IncreaseLevel();
    }

    private void IncreaseLevel()
    {
        this.Level++;
    }

    private void TakeDamage(int takenDamage)
    {
        print("damage: "+ takenDamage);
        this.canTakeDamage = false;
        Invoke("EnableDamageTaking", damageTakingCooldown);
        this.Hp -= Mathf.Clamp(takenDamage - Defense, 11, 9999);
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
        _animController = gameObject.GetComponentInChildren<EntityAnimationController>();
        ResetEntity();
    }
    
    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        _animController?.PlayDieAnim(()=>EventManager.Dispatch("EntityDie", new EntityData(this)));
        EventManager.Dispatch("playerGainsSoul", new ObjectCreationManager.SoulsplosionData(this.SoulValue, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)));
        if (SR != null)
        {
            SR.color = new Color(1f,1f,1f,0f);
        }
    }

    public void Move(Vector2 direction)
    {
        this.direction = direction;
        
        if (direction == Vector2.zero)
        {
            _animController?.StopMovingAnimation();
            return;
        }
        _animController?.StartMovingAnimation();
    }

    public void Rotate(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            return;
        }
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Attack()
    {
        if (!canAttack)
        {
            return;
        }
        _animController?.PlayAttackAnim();
        DisableAttack();
        Invoke("EnableAttack", attackCooldown);
    }

    private void FixedUpdate()
    {
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

    private void OnTriggerEnter2D(Collider2D collider2d)
    {
        GameObject colliderObject = collider2d.gameObject;
        GameObject root = collider2d.transform.root.gameObject;
        if (colliderObject.CompareTag("canDamage") && root != gameObject && canTakeDamage)
        {
            TakeDamage(colliderObject.GetComponentInParent<Entity>().Damage);
        }
    }
};