using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret", menuName = "BuildingData/Turret")]
public class TurretForm : BaseBuildingForm<BuildingData<TurretData>>
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

// 벽을 상속, 공격 요소를 추가한 터렛
[Serializable]
public class TurretData : BasicBuildingData
{
    [Header("자동 포탑 정보")]
    // 타워에 필요한 요소인 공격력, 공격 딜레이, 사거리 추가
    public float atk;
    public float atkDelay;
    public float range;
}
