using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using static UnityEditor.Progress;

public class UI_QuickSlotManager : MonoBehaviour
{
    private PlayerEventHandler eventHandler;
    private PlayerDataSO playerData;
    // 대쉬 이미지는 고정
    [SerializeField] private Sprite dashSprite;
    [SerializeField] private UI_QuickSlot[] itemSlots;

    private int dashIndex = 4;

    private void Start()
    {
        playerData = InventoryManager.instance.player.PlayerDataSO;
        eventHandler= InventoryManager.instance.player.PlayerEvents;
        InventoryManager.instance.quickSlotManager = this;
        SetDashSlot(playerData.DashCoolDown);

        eventHandler.onDash += OnDash;
    }

    public void SetItemSlot(int index, ItemInfo info, float cooldown = 0)
    {
        itemSlots[index].SetSlot(info, cooldown);
    }
    public void ClearItemSlot(int index)
    {
        itemSlots[index].ClearSlot();
    }

    public void SetDashSlot(float cooldown)
    {
        itemSlots[dashIndex].SetSlot(dashSprite, cooldown);
    }

    public void OnSlotPressed(int index)
    {
        itemSlots[index].TriggerCooldown();
    }
    // 추후 플레이어컨트롤러와 연결해주어야함.
    public void OnDash()
    {
        itemSlots[dashIndex].TriggerCooldown();
    }
    public bool CheckQuick(int index, bool isEnd = false)
    {
         return itemSlots[index].TriggerCooldown(isEnd);
    }
    public void UpdateStack(int index, ItemInfo info)
    {
        itemSlots[index].UpdateStack(info);
    }


}
