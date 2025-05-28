using UnityEngine;

public enum BuildingIndex
{
    SignalTower,
    Wall,
    Turret,
    SlowTurret,
    Generator,
    Refinary,
    DroneManageOffice,
}

public class BaseBuilding : MonoBehaviour
{
    [SerializeField] BuildingIndex buildingIndex; // 건물 인덱스
    int level = 0; // 건물 레벨
    int levelMax; // 해당 건물 종류의 최대 레벨
    float hpCurrent; // 현재 체력
    float hpMax; // 현재 레벨의 최대 체력

    private void OnEnable()
    {
        Init();
    }

    // 건물 처음 생성 때 초기화가 필요한 것 모음
    public virtual void Init()
    {
        levelMax = FormManager.Instance.GetForm<WallForm>().GetDataByID((int)buildingIndex).buildingDatas.Length - 1;
        SetBuildingStatus();
    }

    // 레벨업 후 스테이터스 적용
    public virtual void BuildingLvUp()
    {
        if (level < levelMax)
        {
            level++;
            SetBuildingStatus();
        }
    }

    // 레벨에 맞는 스테이터스 부여
    protected virtual void SetBuildingStatus()
    {
        // 건물 종류에 맞는 데이터를 가져옴
        var buildingData = FormManager.Instance.GetForm<WallForm>().GetDataByID((int)buildingIndex);
        // 해당 레벨에 맞는 데이터
        var levelData = buildingData.buildingDatas[level];
        // 최대 체력 증가에 맞춰 풀피로 회복 >> %로 깎인 상태 유지해야 하는지 물어보기
        hpCurrent = hpMax = levelData.hpMax;
    }

    // 머지 이후 건물에 공통적으로 "대미지를 줄 수 있는" 인터페이스 넣기 !!!
    // 대미지를 받아 체력 감소 및 파괴

    public void Damage(float damage)
    {
        hpCurrent = Mathf.Clamp(hpCurrent - damage, 0, hpMax);
        if (hpCurrent <= 0)
            Destroy(gameObject);
    }

    // 수리
    public void Fix(float amount) => hpCurrent = Mathf.Clamp(hpCurrent + amount, 0, hpMax);
}
