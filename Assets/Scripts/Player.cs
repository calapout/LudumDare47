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
    private bool menuIsOpened = false;
    private bool IsMovementEnabled = true;
    [SerializeField] private int[] _Levels = new int[3] { 0, 0, 0 };//0 = Hp, 1 = Damage, 2 = Defense
    [SerializeField] private Camera _camera;

    [SerializeField] private Transform shopSpawn;
    [SerializeField] private Transform arenaSpawn;

    public Transform shopTransform;
    public bool cameraShouldFollowPlayer = true;

    public int Souls
    {
        get => _Souls;
        private set { _Souls = value; EventManager.Dispatch("playerSoulChange", new ListenText.TextChangeData(Souls.ToString())); }
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
        //Debug.Log("i attack");
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

    public void OnPlayerDies(Entity e)
    {
        EventManager.Dispatch("playerDied", null);
    }

    public void OnInterract()
    {
        if (IsMovementEnabled && !ShopIsOpen && CanOpenShop)
        {
            EventManager.Dispatch("openShop", null);
            ShopIsOpen = true;
            this.PlayerEntity.StopMoving();
            DisableMovement();
        }
    }

    public void DisableMovement()
    {
        IsMovementEnabled = false;
        PlayerEntity.StopMoving();
    }

    public void EnableMovement()
    {
        IsMovementEnabled = true;
    }

    public void OnLeaveShop()
    {
        if (ShopIsOpen)
        {
            CloseShop();
        }
        
    }

    public void OnOpenMenu()
    {
        if(!ShopIsOpen)
            EventManager.Dispatch("tryOpenCloseMenu", new TryOpenCloseMenuData(ShopIsOpen));
    }

    public void CloseShop()
    {
        EventManager.Dispatch("closeShop", null);

        // Fixes main menu opening
        Animate.Delay(0.5f, ()=> { ShopIsOpen = false; });

        EnableMovement();
        IsMovementEnabled = true;

        // Refresh stats
        EventManager.Dispatch("playerHPChange", new ListenStatFillBar.FillingBarChangeData(PlayerEntity.Hp, PlayerEntity.MaxHp));
        EventManager.Dispatch("playerAtkChange", new ListenText.TextChangeData(PlayerEntity.Damage.ToString()));
        EventManager.Dispatch("playerArmorChange", new ListenText.TextChangeData(PlayerEntity.Defense.ToString()));
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
        PlayerEntity = GetComponent<Entity>();
    }

    private void Start()
    {
        EnableMovement();

        EventManager.AddEventListener("playerGainsSoul", (Bytes.Data d)=> {
            Souls++;
        });

        EventManager.AddEventListener("placePlayerInArena", OnTeleportPlayerInArena);

        EventManager.AddEventListener("bossDefeated", (Bytes.Data d)=> { cameraShouldFollowPlayer = false; });

        PlayerEntity.OnDie.AddListener(OnPlayerDies);

        PlayerEntity.OnTakeDamage.AddListener((Entity e) => {
            EventManager.Dispatch("playerHPChange", new ListenStatFillBar.FillingBarChangeData(PlayerEntity.Hp, PlayerEntity.MaxHp));
        });

        PlayerEntity.OnDieAnimDone.AddListener((Entity e)=> {
            EventManager.Dispatch("StartFadeOut", null);
            EventManager.AddEventListener("FadedOut", Respawn);
        });

        EventManager.Dispatch("playerSoulChange", new ListenText.TextChangeData(Souls.ToString()));
        EventManager.Dispatch("playerHPChange", new ListenStatFillBar.FillingBarChangeData(PlayerEntity.Hp, PlayerEntity.MaxHp));
        EventManager.Dispatch("playerAtkChange", new ListenText.TextChangeData(PlayerEntity.Damage.ToString()));
        EventManager.Dispatch("playerArmorChange", new ListenText.TextChangeData(PlayerEntity.Defense.ToString()));
    }

    private void Update()
    {
        if (cameraShouldFollowPlayer)
            _camera.transform.position = new Vector3(transform.position.x, transform.position.y, _camera.transform.position.z);

        if (Vector2.Distance(shopTransform.transform.position, this.transform.position) <= 5)
        {
            CanOpenShop = true;
        }
        else
        {
            CanOpenShop = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "portal")
        {
            EventManager.Dispatch("startArena", null);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "shop")
        {
            CanOpenShop = false;
        }
    }

    private void Respawn(Bytes.Data data)
    {
        transform.position = shopSpawn.position;
        PlayerEntity.ResetEntity();

        EventManager.Dispatch("playerHPChange", new ListenStatFillBar.FillingBarChangeData(PlayerEntity.Hp, PlayerEntity.MaxHp));

        EventManager.Dispatch("killBoss", null);
    }

    public void OnTeleportPlayerInArena(Bytes.Data data)
    {
        transform.position = arenaSpawn.position;
    }

}
