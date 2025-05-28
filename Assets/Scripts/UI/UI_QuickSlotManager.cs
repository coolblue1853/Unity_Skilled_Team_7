using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class UI_QuickSlotManager : MonoBehaviour
{
    // 대쉬 이미지는 고정
    [SerializeField] private Sprite dashSprite;
    [SerializeField] private UI_QuickSlot[] itemSlots;

    private int dashIndex = 4;
    private void Start()
    {
        SetDashSlot(2.0f); // 추후 변경
    }

    public void SetItemSlot(int index, Sprite sprite, float cooldown)
    {
        // 아이템 관련 슬롯 설정
    }
    public void SetDashSlot(float cooldown)
    {
        itemSlots[dashIndex].SetSlot(dashSprite, cooldown);
    }

    public void OnSlotPressed(int index)
    {
        itemSlots[index].TriggerCooldown();
    }


    //테스트
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            itemSlots[dashIndex].TriggerCooldown();
        }
    }
}
