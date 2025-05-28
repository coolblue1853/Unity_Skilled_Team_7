using UnityEngine;
using UnityEngine.UI;

public class UI_StatBar : UI_Scene
{
    
    public enum StatBarType
    {
        Health,
        Stamina,
        Thirst,
    }

    [SerializeField] private StatBarType statBarType;

    enum Images
    {
        EmptyBar,
        FillBar
    }

    private T_PC _PC;

    public override void Init()
    {
        base.Init();
        _PC = T_PC.instance; // 추후 연결 변경
        Bind<Image>(typeof(Images));

        switch (statBarType)
        {
            case StatBarType.Health:
                _PC.OnHealthChanged += UpdateCurrent;
                UpdateCurrent(_PC._maxHealth, _PC.Health);
                break;
            case StatBarType.Stamina:
                _PC.OnStaminaChanged += UpdateCurrent;
                UpdateCurrent(_PC._maxStamina, _PC.Stamina);
                break;
            case StatBarType.Thirst:
                //_PC.OnStaminaChanged += UpdateCurrent;
                //UpdateCurrent(_PC._maxStamina, _PC.Stamina);
                break;
        }
    }

    private void OnDisable()
    {
        switch (statBarType)
        {
            case StatBarType.Health:
                _PC.OnStaminaChanged -= UpdateCurrent;
                break;
        }
    }

    void UpdateCurrent(float max,float current)
    {
        var image = Get<Image>((int)Images.FillBar);
        if (image != null)
        {
            image.fillAmount = current / max;
        }
    }
}
