using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UI_StatBar;

public class UI_FloatType : UI_Scene
{
    private TextMeshProUGUI countTxt;

    public enum Type
    {
        Scrap,
        Circuit,
        Fuel,
        Wave,
    }

    [SerializeField] private Type type;

    enum TMP
    {
        CountTxt
    }

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(TMP));


        switch (type)
        {
            case Type.Scrap:
               // _PC.OnHealthChanged += UpdateCurrent;
               // UpdateCurrent(_PC._maxHealth, _PC.Health);
                break;
            case Type.Circuit:
               // _PC.OnStaminaChanged += UpdateCurrent;
               // UpdateCurrent(_PC._maxStamina, _PC.Stamina);
                break;
            case Type.Fuel:
                //_PC.OnStaminaChanged += UpdateCurrent;
                //UpdateCurrent(_PC._maxStamina, _PC.Stamina);
                break;
            case Type.Wave:
                //_PC.OnStaminaChanged += UpdateCurrent;
                //UpdateCurrent(_PC._maxStamina, _PC.Stamina);
                break;
        }
    }
    private void OnDisable()
    {
        switch (type)
        {
            case Type.Scrap:
               // _PC.OnStaminaChanged -= UpdateCurrent;
                break;
        }
    }

    void UpdateCurrent(float stack)
    {
        countTxt.text = stack.ToString();
    }
}
