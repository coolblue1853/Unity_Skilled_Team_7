using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Idle,
    Walk,
    Dash,
    Gathering,
    Throw,
    Vehicle,
    Death
}

public class Player : MonoBehaviour, IDamageable
{
    private Rigidbody _rigidbody;
    private PlayerInput playerInput; // PlayerInput
    private Animator anim;

    // Player Data
    [SerializeField] private PlayerDataSO playerDataSO;
    public PlayerDataSO PlayerDataSO { get => playerDataSO; }


    private PlayerController playerController; // 플레이어 인풋 관련
    private PlayerStatus playerStatus; // 플레이어 스탯 관련
    private PlayerMovement playerMovement; // 플레이어 이동 관련
    private PlayerVehicleHandler playerVehicle; // 탈 것 관리
    private PlayerWeaponHandler playerWeapon; // 무기 관리
    private PlayerAnimationHandler playerAnimation; // 애니메이션 관리

    [SerializeField] PlayerState curState;
    public PlayerState CurState { get => curState; } // 플레이어 상태

    // Player Events
    public PlayerEventHandler PlayerEvents { get; private set; }

    public bool CanMove => CurState != PlayerState.Vehicle && CurState != PlayerState.Gathering && CurState != PlayerState.Death && CurState != PlayerState.Throw;
    public bool CanDash { get; set; }
    public bool IsDead { get; private set; }
    public bool OnBattle { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponentInChildren<Animator>();

        PlayerEvents = new PlayerEventHandler();

        playerAnimation = GetComponent<PlayerAnimationHandler>();
        playerController = GetComponent<PlayerController>();
        playerStatus = GetComponent<PlayerStatus>();
        playerMovement = GetComponent<PlayerMovement>();
        playerVehicle = GetComponent<PlayerVehicleHandler>();
        playerWeapon = GetComponentInChildren<PlayerWeaponHandler>();

        if (playerAnimation)
            playerAnimation.Init(anim);
        if (playerController)
            playerController.Init(this);
        if (playerStatus)
            playerStatus.Init(this);
        if (playerMovement)
            playerMovement.Init(this, _rigidbody, playerAnimation);
        if (playerVehicle)
            playerVehicle.Init(this, playerAnimation);
        if (playerWeapon)
            playerWeapon.Init(this, playerStatus, playerAnimation);


        curState = PlayerState.Idle;

        CanDash = true;
        IsDead = false;
        OnBattle = true;
        
        playerAnimation.SetDash(IsDead);
    }

    void FixedUpdate()
    {
        if (CanMove)
            playerMovement.Move(playerController.MoveDirection, playerStatus.MoveSpeed);
    }

    void Update()
    {
        if (CanMove)
            playerMovement.Rotate(playerController.LookDirection);
    }

    public void ChangeState(PlayerState state)
    {
        curState = state;
    }

    public void Dash()
    {
        if (CanDash && CurState != PlayerState.Vehicle && playerStatus.UseStaminaAndHydration(playerDataSO.DashStamina, playerDataSO.DashHydration))
        {
            float dashSpeed = playerDataSO.DashSpeed;
            float duration = playerDataSO.DashDuration;
            float cooldown = playerDataSO.DashCoolDown;
            PlayerEvents.RaisedDash();
            StartCoroutine(playerMovement.DashCoroutine(playerController.LookDirection, dashSpeed, duration, cooldown));
        }
    }

    public void CallVehicle()
    {
        playerVehicle.CallVehicle();
    }

    public void SetVehicle(VehicleController vehicle)
    {
        _rigidbody.isKinematic = curState != PlayerState.Vehicle;
        playerVehicle.SetVehicle(vehicle, playerInput);
    }

    public void GatheringResource(Resource resource)
    {
        if (playerStatus.UseStamina(playerDataSO.GatherStamina))
        {
            ChangeState(PlayerState.Gathering);
            playerAnimation.SetGathering(true);

            StartCoroutine(resource.GetResource(() =>
            {
                ChangeState(PlayerState.Idle);
                playerAnimation.SetGathering(false);
            }));
        }
    }

    public void ConsumeItem(ItemDataConsumable consumable)
    {
        playerStatus.SetItemStat(consumable.type, consumable.value);
    }

    public void ThrowGrenade()
    {
        playerWeapon.CheckThrow();
    }

    public void UnlockWeapon(int idx)
    {
        playerWeapon.UnlockWeapon(idx);
    }

    public void SelectWeapon(int idx)
    {
        playerWeapon.ChangeWeapon(idx);
    }

    public void StartAiming()
    {
        playerWeapon.StartAiming();
    }

    public void StopAiming()
    {
        playerWeapon.StopAiming();
    }

    public void TakeDamage(float amount)
    {
        playerStatus.TakeDamage(amount);
        playerAnimation.Hit();
    }

    public void Dead()
    {
        StopAllCoroutines();
        IsDead = true;
        playerAnimation.SetDeath();

        ChangeState(PlayerState.Death);
    }
}
