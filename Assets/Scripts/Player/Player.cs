using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Idle,
    Walk,
    Dash,
    Gathering
}

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private PlayerInput playerInput; // PlayerInput

    // Player Data
    [SerializeField] private PlayerDataSO playerDataSO;
    public PlayerDataSO PlayerDataSO { get => playerDataSO; }

    
    private PlayerController playerController; // 플레이어 인풋 관련
    private PlayerStatus playerStatus; // 플레이어 스탯 관련
    private PlayerMovement playerMovement; // 플레이어 이동 관련
    private PlayerVehicleHandler playerVehicle; // 탈 것 관리
    private PlayerWeaponHandler playerWeapon; // 무기 관리

    public PlayerState CurState { get; private set; } // 플레이어 상태

    // Player Events
    public PlayerEventHandler PlayerEvents { get; private set; }

    public bool CanDash { get; set; }
    public bool IsDead { get; private set; }
    public bool OnVehicle { get; set; }
    public bool OnBattle { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        PlayerEvents = new PlayerEventHandler();

        playerController = GetComponent<PlayerController>();
        playerStatus = GetComponent<PlayerStatus>();
        playerMovement = GetComponent<PlayerMovement>();
        playerVehicle = GetComponent<PlayerVehicleHandler>();
        playerWeapon = GetComponent<PlayerWeaponHandler>();

        if (playerController)
            playerController.Init(this);
        if (playerStatus)
            playerStatus.Init(this);
        if (playerMovement)
            playerMovement.Init(this, _rigidbody);
        if (playerVehicle)
            playerVehicle.Init(this);
        if (playerWeapon)
            playerWeapon.Init(this);

        CurState = PlayerState.Idle;

        CanDash = true;
        IsDead = false;
        OnVehicle = false;
        OnBattle = true;

        PlayerEvents.onSelectSlot += SelectSlotTest;
    }

    public void SelectSlotTest(int idx)
    {
        Debug.Log(idx);
    }

    void FixedUpdate()
    {
        if (!OnVehicle)
            playerMovement.Move(playerController.MoveDirection, playerStatus.MoveSpeed);
    }

    void Update()
    {
        if (!OnVehicle)
            playerMovement.Rotate(playerController.LookDirection);
    }

    public void ChangeState(PlayerState state)
    {
        CurState = state;
    }

    public void Dash()
    {
        if (CanDash && !OnVehicle && playerStatus.UseStaminaAndHydration(playerDataSO.DashStamina, playerDataSO.DashHydration))
        {
            float dashSpeed = playerDataSO.DashSpeed;
            float duration = playerDataSO.DashDuration;
            float cooldown = playerDataSO.DashCoolDown;

            StartCoroutine(playerMovement.DashCoroutine(playerController.LookDirection, dashSpeed, duration, cooldown));
        }
    }

    public void SetVehicle(VehicleController vehicle)
    {
        _rigidbody.isKinematic = !OnVehicle;
        playerVehicle.SetVehicle(vehicle, playerInput);
    }

    public void Dead()
    {
        IsDead = true;
    }
}
