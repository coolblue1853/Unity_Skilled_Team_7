using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ProductionBuilding", menuName = "BuildingData/Production")]
public class ProductionBuildingForm : BaseBuildingForm<BuildingData<ProductionBuildingData>>
{
    public override void CreateForm()
    {
        base.CreateForm();
        foreach (var data in dataList)
        {
            DataDic[(int)data.ID] = data;
        }
    }
}

// 벽을 상속. 생산하는 특수 기능 추가
[Serializable]
public class ProductionBuildingData : BasicBuildingData
{
    [Header("생산 기능 정보")]
    public UnityEvent Production; // 생산 기능
}