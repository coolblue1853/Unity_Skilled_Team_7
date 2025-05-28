using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    [SerializeField] Transform buildingPrefab; // 이번에 설치할 건물 프리팹을 넣어줄 곳
    [SerializeField] LayerMask groundMask; // 지면 오브젝트의 레이어

    BuildingBluePrint buildingBluePrint; // 건물 청사진
    Vector2 mousePos; // 마우스 커서가 가리키는 현재 설치 위치
    Camera cam; // 메인 카메라
    
    private void Awake() => cam = Camera.main;

    // 건물 청사진 생성
    private void OnEnable() => CreateBluePrint();

    private void Update()
    {
        if (buildingBluePrint)
        {
            // 스크린의 마우스 위치로부터 카메라 Z포지션 절대값만큼 빔을 쏴서 맞은 땅이 있다면, 건물 예상도가 마우스를 따라다니게끔
            Ray ray = cam.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Abs(cam.transform.position.z), groundMask))
            {
                // 바닥의 두께만큼 위로 올라가서 y축을 0으로 고정
                // 대신, 에셋을 구하면 그 바닥에 좌표의 중심이 있는지 확인하기
                // 중심이 건물 바닥이 아니라면, 건물들 y축 기준이 0이 되게 빈 부모 오브젝트 두고 사용하면 됩니다. ConstructEx 프리팹 참고
                buildingBluePrint.transform.parent.position = new Vector3(hit.point.x, 0, hit.point.z);
            }
        }
    }

    // 건설할 건물 프리팹 할당
    public void SetBuilding(Transform building) => buildingPrefab = building;

    // Input System: Send Message 방식
    // 마우스가 움직일 때 마우스 좌표를 경신
    public void OnMoveInput(InputValue value) => mousePos = value.Get<Vector2>();

    // 마우스 왼쪽 버튼 클릭
    public void OnConfirmInput(InputValue value)
    {
        // 겹치는 오브젝트가 없을 때만, 건설 완료
        if(!buildingBluePrint.IsOverlap)
            CompleteBuild();
    }

    // 마우스 오른쪽 버튼 클릭 or ESC => 건설 취소
    public void OnCancelInput(InputValue value) => CancelBuild();


    // 마우스 휠 다운/업으로 건물 회전
    public void OnRotateInput(InputValue value)
    {
        float input = value.Get<float>();
        RotateBuild(input);
    }

    // 건물의 청사진 오브젝트 생성
    void CreateBluePrint()
    {
        // 프리팹이 null이 아니라면,
        if (buildingPrefab)
        {
            // 프리팹을 복제하여 마우스를 따라다닐 건물 생성
            Transform bluePrint = Instantiate(buildingPrefab);
            // 건물 청사진으로써 활동할 수 있게 스크립트 추가
            // 0번째 자식이 외형 + 콜라이더를 가지고 있음
            buildingBluePrint = bluePrint.GetChild(0).gameObject.AddComponent<BuildingBluePrint>();
        }
    }

    // 설치 완료
    void CompleteBuild()
    {
        // 자원 차감 !!!

        // 청사진 기능 해제 및 제거
        buildingBluePrint.WhenBuildComplete();
        // 설치 완료되면 건물 역할 스크립트 혹은 오브젝트 활성화 >> 그때부터 건물로써의 기능 시작 !!!

        // 설치에 관여하는 오브젝트 비활성화
        gameObject.SetActive(false);
    }

    // 설치 취소
    void CancelBuild()
    {
        // 건물 청사진 오브젝트를 파괴하고
        Destroy(buildingBluePrint.gameObject);
        // 설치에 관여하는 오브젝트 비활성화
        gameObject.SetActive(false);
    }

    // 건물 청사진 회전
    void RotateBuild(float inputValue)
    {
        if(inputValue > 0)
            buildingBluePrint.transform.parent.Rotate(90 * Vector3.up);
        else if(inputValue < 0)
            buildingBluePrint.transform.parent.Rotate(90 * Vector3.down);
    }
}
