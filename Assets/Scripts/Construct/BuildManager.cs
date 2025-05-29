using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class BuildManager : MonoBehaviour
{
    [SerializeField] Transform buildingPrefab; // 이번에 설치할 건물 프리팹을 넣어줄 곳
    [SerializeField] LayerMask groundMask; // 지면 오브젝트의 레이어
    GeneratorManager generatorManager;

    BaseBuilding buildingScript;
    BuildingBluePrint buildingBluePrint; // 건물 청사진
    Vector2 mousePos; // 마우스 커서가 가리키는 현재 설치 위치
    Camera cam; // 메인 카메라

    private void Awake()
    {
        cam = Camera.main;
        TryGetComponent(out generatorManager);
    }

    private void OnEnable()
    {
        // 건설 가능한 자원이 충분한지 확인할 필요가 있음 !!! >> 불충분하면 건설 취소 및 자원 부족 알림

        // 발전기들의 전력 공급 가능 범위 표시
        generatorManager.StartConstruct();
        // 지을려는 건물의 청사진 생성
        CreateBluePrint();
    }

    private void Update()
    {
        if (buildingBluePrint)
        {
            // 스크린의 마우스 위치로부터 카메라 Z포지션 절대값만큼 빔을 쏴서 맞은 땅이 있다면, 건물 예상도가 마우스를 따라다니게끔
            Ray ray = cam.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Abs(cam.transform.position.z), groundMask))
            {
                buildingBluePrint.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
            }
        }
    }

    private void OnDisable()
    {
        // 데이터 초기화
        buildingPrefab = null;
        buildingBluePrint = null;
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
        if(buildingBluePrint.CanConstruct)
            StartCoroutine(CompleteBuild());
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
            // 프리팹을 복제하여 마우스를 따라다닐 건물 생성 + 건물의 데이터에 접근하기 위한 길 터두기
            // 에러 발생: 제네릭은 상속되지 않아 BasicBuildingData는 이를 상속하는 클래스들의 공통분모로 인식되지 않음 >> 다중 제네릭은 서비스 종료다..
            buildingScript = Instantiate(buildingPrefab).GetComponent<BaseBuilding>();
            // 건물 청사진으로써 활동할 수 있게 스크립트 추가
            buildingBluePrint = buildingScript.gameObject.AddComponent<BuildingBluePrint>();
            // 건물이 제 역할을 하지 않도록 스크립트 비활성화
            buildingScript.enabled = false;
        }
    }

    // 설치 완료
    IEnumerator CompleteBuild()
    {
        // 설치 완료되면 건물 역할 스크립트 혹은 오브젝트 활성화 >> 그때부터 건물로써의 기능 시작 !!!
        buildingScript.enabled = true;
        // enabled를 true로 만들고 바로 자원을 차감하려면 먹히지 않기에 1프레임 대기
        yield return null;
        // 건설 자원 차감 
        buildingScript.ResourceConsumption(0);
        // 청사진 기능 해제 및 제거
        buildingBluePrint.WhenBuildComplete();
        // 발전기들의 전력 공급 가능 범위 표시 끄기
        generatorManager.EndConstruct();
        // 설치에 관여하는 오브젝트 비활성화
        gameObject.SetActive(false);
    }

    // 설치 취소
    void CancelBuild()
    {
        // 건물 청사진 오브젝트를 파괴하고
        if(buildingBluePrint != null)
            Destroy(buildingBluePrint.gameObject);
        // 발전기들의 전력 공급 가능 범위 표시 끄기
        generatorManager.EndConstruct();
        // 설치에 관여하는 오브젝트 비활성화
        gameObject.SetActive(false);
    }

    // 건물 청사진 회전
    void RotateBuild(float inputValue)
    {
        if(inputValue > 0)
            buildingBluePrint.transform.Rotate(90 * Vector3.up);
        else if(inputValue < 0)
            buildingBluePrint.transform.Rotate(90 * Vector3.down);
    }
}
