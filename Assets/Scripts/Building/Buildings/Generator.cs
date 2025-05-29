using UnityEngine;

public class Generator : BaseBuilding
{
    [SerializeField]
    InteractArea interactArea;
    public GeneratorData data { get; private set; }

    protected override void Init()
    {
        // 데이터 받아오기
        data = FormManager.Instance.GetForm<GeneratorForm>().GetDataByID((int)buildingIndex);
        // 최대 레벨
        levelMax = data.dataByLevel.Length - 1;
        SetBuildingStatus();
    }

    protected override void SetBuildingStatus()
    {
        // 레벨업으로 인한 최대 HP 증가
        hpMax = data.dataByLevel[level].hpMax;
        // 발전기 적용 범위 변경
        interactArea.ChangeRange(data.dataByLevel[level].activeRange);
    }

    // 건설 시작/종료 때 호출 >> 상호작용 영역 및 표시 활성화/비활성화
    public void StartConstruct() => interactArea.gameObject.SetActive(true);
    public void EndConstruct() => interactArea.gameObject.SetActive(false);

    // 건물마다 고유로 가지는 값들 반환
    public override BuildingStatus GetIndividualBuildingInfo() => new GeneratorStatus(level, levelMax, hpMax);

    public override void ResourceConsumption(int nextLevel)
    {
        ResourceRequire[] resourcesRequire = data.dataByLevel[nextLevel].resources;
        foreach (ResourceRequire resourceRequire in resourcesRequire)
        {
            inventoryManager.DeductResource(resourceRequire.resourceSort, resourceRequire.amount);
        }
    }
}

public class GeneratorStatus : BuildingStatus
{
    public GeneratorStatus(int level, int levelMax, float hpCurrent) : base(level, levelMax, hpCurrent)
    {
    }
}
