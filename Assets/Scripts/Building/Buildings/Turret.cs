using UnityEngine;
public class Turret : BaseBuilding
{
    // 현재 참조중인 터렛 데이터
    public TurretData data { get; private set; }

    protected override void Init()
    {
        // 데이터 받아오기
        data = FormManager.Instance.GetForm<TurretForm>().GetDataByID((int)buildingIndex);
        // 최대 레벨
        levelMax = data.dataByLevel.Length - 1;
        SetBuildingStatus();
    }

    protected override void SetBuildingStatus()
    {
        // 레벨업으로 인한 최대 HP 증가
        hpMax = data.dataByLevel[level].hpMax;
    }

    // 건물마다 고유로 가지는 값들 반환
    public override BuildingStatus GetIndividualBuildingInfo() => new TurretStatus(level, levelMax, hpCurrent);

    public override void ResourceConsumption(int nextLevel)
    {
        ResourceRequire[] resourcesRequire = data.dataByLevel[nextLevel].resources;
        foreach (ResourceRequire resourceRequire in resourcesRequire)
        {
            inventoryManager.DeductResource(resourceRequire.resourceSort, resourceRequire.amount);
        }
    }
}

public class TurretStatus : BuildingStatus
{
    public TurretStatus(int level, int levelMax, float hpCurrent) : base(level, levelMax, hpCurrent)
    {
    }
}
