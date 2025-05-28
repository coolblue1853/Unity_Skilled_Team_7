using UnityEngine;

[CreateAssetMenu(fileName = "SurvivalCondition", menuName = "Player/new SurvivalCondition")]
public class SurvivalConditionSO : ScriptableObject
{
    [Header("Stat Decay Per Interval")]
    [SerializeField] float interval;
    [SerializeField] float healthDecayPerInterval;
    [SerializeField] float staminaDecayPerInterval;
    [SerializeField] float hydrationDecayPerInterval;
    public float Interval { get => interval; }
    public float HealthDecayPerInterval { get => healthDecayPerInterval; }
    public float StaminaDecayPerInterval { get => staminaDecayPerInterval; }
    public float HydrationDecayPerInterval { get => hydrationDecayPerInterval; }

    [Header("Stat Recover Per Interval")]
    [SerializeField] float healthRecoverPerInterval;
    public float HealthRecoverPerInterval { get => healthRecoverPerInterval; }

    [Header("Health Recover Condition")]
    [SerializeField] float minConditionToHeal;
    public float MinConditionToHeal { get => minConditionToHeal; }

    [Header("Health Decay Condition")]
    [SerializeField] float minStaminaToDecay;
    [SerializeField] float minHydrationToDecay;
    public float MinStaminaToDecay { get => minStaminaToDecay; }
    public float MinHydrationToDecay { get => minHydrationToDecay; }

    [Header("MoveSpeed Penalty")]
    [SerializeField] float moveSpeedPenalty;
    public float MoveSpeedPenalty { get => moveSpeedPenalty; }
}
