using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData item;
    public InventoryManager inventory;
    public GameObject parentObject; // 부모 오브젝트
    public SpawnManager spawn;

    [Header("Resource")]
    public int minCount;
    public int maxCount;
    public float farmingRate; // 자원 획득까지 걸리는 시간
    public float spawnRate; // 자원 스폰 시간

    [Header("Deduct Resource")]
    public ItemData[] deductItem; // 차감 아이템
    public int[] dedcuctCount;
    bool deduct;

    void Awake()
    {
        parentObject = transform.parent.gameObject;
        inventory = GameObject.Find("Inventory_").GetComponent<InventoryManager>();
        spawn = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    public IEnumerator GetResource(Action onCompleted)
    {
        // 소모 자원 있다면 자원 있는지 확인 하는 소스 작성
        if (deductItem.Length > 0)
        {
            for (int i = 0; i < deductItem.Length; i++)
            {
                if (inventory.itemList[deductItem[i].resourceName].count < dedcuctCount[i])
                {
                    Debug.Log("재료가 부족합니다.");
                    yield break;
                }
                else deduct = true;
            }
        }
        else deduct = false;

        int getCount = UnityEngine.Random.Range(minCount, maxCount);
        yield return new WaitForSeconds(farmingRate);

        if (deduct) inventory.DeductItem(deductItem, dedcuctCount);
        inventory.AddItem(item, getCount);
        spawn.SpawnResource(parentObject, spawnRate);
        parentObject.SetActive(false);

        onCompleted?.Invoke();
    }

    IEnumerator DigResource() //  AI 자원 채굴
    {
        int getCount = UnityEngine.Random.Range(minCount, maxCount);
        yield return new WaitForSeconds(farmingRate);
        Debug.Log(item.resourceName + " 를 " + getCount + "개 획득");
        parentObject.SetActive(false);
    }
}
