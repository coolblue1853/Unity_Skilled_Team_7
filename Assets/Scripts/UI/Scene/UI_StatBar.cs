using UnityEngine;
using UnityEngine.UI;

public class UI_StatBar : UI_Scene
{
    private PlayerEventHandler eventHandler;
    
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


    public override void Init()
    {
        base.Init();
        eventHandler = InventoryManager.instance.player.PlayerEvents;
        Bind<Image>(typeof(Images));

        switch (statBarType)
        {
            case StatBarType.Health:
                eventHandler.onChangeHealth += UpdateCurrent;
               // UpdateCurrent(_PC._maxHealth, _PC.Health);
                break;
            case StatBarType.Stamina:
                eventHandler.onChangeStamina += UpdateCurrent;
                // UpdateCurrent(_PC._maxStamina, _PC.Stamina);
                break;
            case StatBarType.Thirst:
                eventHandler.onChangeHydration += UpdateCurrent;
                //UpdateCurrent(_PC._maxStamina, _PC.Stamina);
                break;
        }
    }

    private void OnDisable()
    {
        switch (statBarType)
        {
            case StatBarType.Health:
              //  _PC.OnStaminaChanged -= UpdateCurrent;
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
