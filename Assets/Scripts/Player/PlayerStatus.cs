using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] SurvivalConditionSO conditionSO;
    Player player;
    PlayerDataSO playerDataSO;
    PlayerEventHandler playerEvents;

    #region 플레이어 스탯 관련 변수 & 프로퍼티
    public float MoveSpeed { get => IsDanger ? conditionSO.MoveSpeedPenalty : playerDataSO.MoveSpeed; }

    [Header("Player Max Stats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxStamina;

    // 현재 스탯들
    [Header("Player Current Stats")]
    [SerializeField] private float curHealth;
    [SerializeField] private float curStamina;
    [SerializeField] private float curHydration;

    public float CurHealth
    {
        get => curHealth;
        set
        {
            curHealth = Mathf.Clamp(value, 0, maxHealth);
            playerEvents?.RaisedChangeHealth(maxHealth, curHealth);
        }
    }
    public float CurStamina
    {
        get => curStamina;
        set
        {
            curStamina = Mathf.Clamp(value, 0, maxStamina);
            playerEvents?.RaisedChangeStamina(maxStamina, curStamina);
        }
    }
    public float CurHydration
    {
        get => curHydration;
        set
        {
            curHydration = Mathf.Clamp(value, 0, playerDataSO.MaxHydration);
            playerEvents?.RaisedChangeHydration(playerDataSO.MaxHydration, curHydration);
        }
    }

    // 스탯 감소량
    private float healthDecayPerInterval;
    private float staminaDecayPerInterval;
    private float HydrationDecayPerInterval => player.OnBattle ? conditionSO.HydrationDecayPerInterval * 5 : conditionSO.HydrationDecayPerInterval;
    private float interval;

    private float healthRecoverPerInterval;
    #endregion

    private bool CanHeal => curStamina >= conditionSO.MinConditionToHeal && curHealth < maxHealth;
    private bool IsDanger => curStamina <= conditionSO.MinStaminaToDecay || curHydration <= conditionSO.MinHydrationToDecay;

    public void Init(Player player)
    {
        this.player = player;
        playerDataSO = player.PlayerDataSO;
        playerEvents = player.PlayerEvents;

        maxHealth = playerDataSO.MaxHealth;
        curHealth = maxHealth;
        maxStamina = playerDataSO.MaxStamina;
        curStamina = maxStamina;
        curHydration = playerDataSO.MaxHydration;

        healthDecayPerInterval = conditionSO.HealthDecayPerInterval;
        staminaDecayPerInterval = conditionSO.StaminaDecayPerInterval;
        //hydrationDecayPerInterval = conditionSO.HydrationDecayPerInterval;

        healthRecoverPerInterval = conditionSO.HealthRecoverPerInterval;

        interval = conditionSO.Interval;

        StartCoroutine(DecayPerIntervalCoroutine());
    }

    IEnumerator DecayPerIntervalCoroutine()
    {
        while (!player.IsDead)
        {
            CurHydration -= HydrationDecayPerInterval;

            if (player.CurState == PlayerState.Walk)
                CurStamina -= staminaDecayPerInterval;

            if (CanHeal)
            {
                CurStamina -= staminaDecayPerInterval;
                CurHealth += healthRecoverPerInterval;
            }

            if (IsDanger)
            {
                CurHealth -= healthDecayPerInterval;
            }

            yield return new WaitForSeconds(interval);
        }
    }

    public bool UseStamina(float amount)
    {
        if (curStamina >= amount)
        {
            CurStamina -= amount;
            return true;
        }

        return false;
    }

    public bool UseHydration(float amount)
    {
        if (curHydration >= amount)
        {
            CurHydration -= amount;
            return true;
        }

        return false;
    }

    public bool UseStaminaAndHydration(float staminaAmount, float hydrationAmount)
    {
        if (curStamina >= staminaAmount && curHydration >= hydrationAmount)
        {
            CurStamina -= staminaAmount;
            CurHydration -= hydrationAmount;
            return true;
        }

        return false;
    }
}
