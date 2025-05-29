public class Wall : BaseBuilding
{
    public BasicBuildingData data { get; private set; }

    protected override void Init()
    {
        // 데이터 받아오기
        data = FormManager.Instance.GetForm<WallForm>().GetDataByID((int)buildingIndex);
        // 최대 레벨
        levelMax = data.dataByLevel.Length - 1;
        SetBuildingStatus();
    }

    protected override void SetBuildingStatus()
    {
        // 레벨업으로 인한 최대 HP 증가
        hpMax = data.dataByLevel[level].hpMax;
    }

    public override void ResourceConsumption(int nextLevel)
    {
        ResourceRequire[] resourcesRequire = data.dataByLevel[nextLevel].resources;
        foreach (ResourceRequire resourceRequire in resourcesRequire)
        {
            inventoryManager.DeductResource(resourceRequire.resourceSort, resourceRequire.amount);
        }
    }
}


