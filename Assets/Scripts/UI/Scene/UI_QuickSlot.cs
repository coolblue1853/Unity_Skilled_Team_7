using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class UI_QuickSlot : UI_Scene
{
    private Image iconImage;
    private Image cooldownOverlay;
    private float cooldownTime = -999;
    private float lastUsedTime = -999;
    private Sprite icon;

    public enum Images
    {
        IconBack,
        Icon,
    }

    private void Awake()
    {
        Bind<Image>(typeof(Images));
        iconImage = Get<Image>((int)Images.IconBack);
        cooldownOverlay = Get<Image>((int)Images.Icon);
    }
    public override void Init()
    {
        base.Init();
    }

    public void SetSlot(Sprite icon, float cooldown)
    {
        this.icon = icon;
        this.cooldownTime = cooldown;
        this.lastUsedTime = -cooldown; 
        iconImage.sprite = icon;
        cooldownOverlay.sprite = icon;
        cooldownOverlay.fillAmount = 1f;
    }

    public void TriggerCooldown()
    {
        lastUsedTime = Time.time;
    }

    private void Update()
    {
        if (lastUsedTime > 0)
        {
            float elapsed = Time.time - lastUsedTime;
            float ratio = Mathf.Clamp01(elapsed / cooldownTime);
            cooldownOverlay.fillAmount = ratio;
        }
    }
}
