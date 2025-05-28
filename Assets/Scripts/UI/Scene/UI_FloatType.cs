using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UI_StatBar;

public class UI_FloatType : UI_Scene
{
    private TextMeshProUGUI countTxt;
    InventoryManager inventoryManager;



    [SerializeField] private FloatType type;

    enum TMP
    {
        CountTxt
    }

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(TMP));
        inventoryManager = T_PC.instance.inventoryManager;
        countTxt = Get<TextMeshProUGUI>((int)TMP.CountTxt);
        switch (type)
        {
            case FloatType.Item_ScrapIron:
                inventoryManager.OnScrapChanged += UpdateCurrent;
                break;
            case FloatType.Item_Circuit:
                inventoryManager.OnCircuitChanged += UpdateCurrent;
                break;
            case FloatType.Item_Fuel:
                inventoryManager.OnFuelChanged += UpdateCurrent;
                break;
        //    case Type.Wave:
                //_PC.OnStaminaChanged += UpdateCurrent;
             //   break;
        }
    }
    private void OnDisable()
    {
        switch (type)
        {
            case FloatType.Item_ScrapIron:
                inventoryManager.OnScrapChanged -= UpdateCurrent;
                break;
            case FloatType.Item_Circuit:
                inventoryManager.OnCircuitChanged -= UpdateCurrent;
                break;
            case FloatType.Item_Fuel:
                inventoryManager.OnFuelChanged -= UpdateCurrent;
                break;
        }
    }

    void UpdateCurrent(int stack)
    {
        countTxt.text = stack.ToString();
    }
}
