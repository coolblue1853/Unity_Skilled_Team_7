using UnityEngine;

// 상호작용 가능한 건물들에 부여
public interface IInteractactble
{
    public abstract void OnInteract();
}

public class Refinery : BaseBuilding, IInteractactble, IBuildingRequireEnegy
{
    public ProductionBuildingData data { get; private set; }

    // 제작 진행 시간, 제작에 필요한 시간
    float progressTime, requireTime;
    // 생산된 양
    int productAmount;

    protected override void Init()
    {
        // 데이터 받아오기
        data = FormManager.Instance.GetForm<ProductionBuildingForm>().GetDataByID((int)buildingIndex);
        // 최대 레벨
        levelMax = data.dataByLevel.Length - 1;
        SetBuildingStatus();
    }

    protected override void Start()
    {
        base.Start();
        // 업데이트에서 매번 찾기보다 변수에 담아두는 게 좋지 않을까
        requireTime = data.dataByLevel[level].productionTime;
    }

    private void Update()
    {
        // 시간을 세어주다, 생산 필요 시간을 넘었다면 생산!
        progressTime += Time.deltaTime;
        if(progressTime > requireTime)
        {
            progressTime = 0; // 시간 초기화
            Production(); // 생산
        }
    }

    protected override void SetBuildingStatus()
    {
        // 레벨업으로 인한 최대 HP 증가
        hpMax = data.dataByLevel[level].hpMax;
        // 레벨업으로 변한 생산 시간 반영
        requireTime = data.dataByLevel[level].productionTime;
    }

    // 생산량만큼 적재
    void Production() => productAmount = Mathf.Clamp(productAmount + data.dataByLevel[level].amount, 0, data.dataByLevel[level].capacity);
    
    // 생산한 아이템을 인벤토리에 넣게끔
    public void OnInteract() => inventoryManager.AddResource(data.dataByLevel[level].product, productAmount);

    // 건물마다 고유로 가지는 값들 반환
    public override BuildingStatus GetIndividualBuildingInfo() => new ProductBuildingStatus(level, levelMax, hpCurrent, progressTime, productAmount);

    public override void ResourceConsumption(int nextLevel)
    {
        ResourceRequire[] resourcesRequire = data.dataByLevel[nextLevel].resources;
        foreach (ResourceRequire resourceRequire in resourcesRequire)
        {
            inventoryManager.DeductResource(resourceRequire.resourceSort, resourceRequire.amount);
        }
    }

    public void OnEnegyDown()
    {
        throw new System.NotImplementedException();
    }

    public void OnEnegySupply()
    {
        throw new System.NotImplementedException();
    }
}

public class ProductBuildingStatus : BuildingStatus
{
    public float progressTime; // 제작 진행 시간
    public int productAmount; // 생산된 양

    public ProductBuildingStatus(int level, int levelMax, float hpCurrent, float progressTime, int productAmount) : base(level, levelMax, hpCurrent)
    {
        this.progressTime = progressTime;
        this.productAmount = productAmount;
    }
}
