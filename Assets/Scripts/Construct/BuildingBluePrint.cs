using UnityEngine;
using System.Collections.Generic;

public class BuildingBluePrint : MonoBehaviour
{
    // 건물 청사진의 충돌 판정을 위해 리지드바디 필요
    Rigidbody rb;
    Collider col;
    MeshRenderer meshRenderer;

    Color originColor;
    readonly float tranparency = 0.5f; // 건물 청사진의 투명도

    List<Collider> colliders = new List<Collider>();

    // 레이어 명칭
    readonly string groundLayerName = "Ground", // 땅 
                    generatorZoneLayerName = "GeneratorZone"; // 발전기 작용 영역
    // 명칭으로부터 변환한 레이어 넘버
    int groundLayer, generatorZoneLayer;

    // 해당 건물이 발전기를 요구하는지 여부
    bool isNeedGenerator;
    // 발전기를 요구하는 건물들에 포함된 인터페이스
    IBuildingRequireEnegy buildingRequireEnegy;

    // 건설 가능 여부
    public bool CanConstruct {  get; private set; }

    private void Awake()
    {
        groundLayer = LayerMask.NameToLayer(groundLayerName);
        generatorZoneLayer = LayerMask.NameToLayer(generatorZoneLayerName);
    }

    private void OnEnable()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true; // 중력 영향을 받지 않도록

        // 반투명하게 표시
        meshRenderer = GetComponent<MeshRenderer>();
        MatColorChange(tranparency);
        // 반투명한 색 기억
        originColor = meshRenderer.material.color;
        // 콜라이더를 트리거로 설정 >> 트리거 이벤트로 겹친 물체 판정
        col = GetComponent<Collider>();
        if (col)
            col.isTrigger = true;

        // 해당 건물이 발전기를 필요로 하는 것이라면, 이를 기억
       if(TryGetComponent(out buildingRequireEnegy))
            isNeedGenerator = true;
    }

    // 건물 설치 완료 때 호출하여 청사진 기능 해제 및 제거
    public void WhenBuildComplete()
    {
        // 반투명 해제
        MatColorChange(1);
        // 트리거를 다시 콜라이더로 원복하여 충돌 판정 생기게
        if (col)
            col.isTrigger = false;

        // 설치가 끝났을 때 불필요한 컴포넌트들 제거
        Destroy(rb);
        Destroy(this);
    }

    // 겹치는지 여부 판정 및 색상 변화
    private void OnTriggerEnter(Collider other)
    {
        // 발전기를 필요로 하는 건물이고
        if(isNeedGenerator)
        {
            // 발전기 존과 땅이 아닌 것에 들어갔다면
            if(other.gameObject.layer != generatorZoneLayer && other.gameObject.layer != groundLayer)
            {
                // 건설 불가 판정
                ChangeNotConstructable(other);
            }
        }
        // 발전기를 필요로 하지 않고, 땅이 아닌 충돌체와 닿았다면
        else if (other.gameObject.layer != groundLayer)
        {
            // 건설 불가 판정
            ChangeNotConstructable(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 발전기를 필요로 하는 건물이고
        if (isNeedGenerator)
        {
            // 트리거 내 걸리는 다른 충돌체가 없고 발전기 영역 내라면
            if (colliders.Count.Equals(0) && other.gameObject.layer == generatorZoneLayer)
            {
                // 건설 가능
                CanConstruct = true;
                meshRenderer.material.color = originColor;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 발전기를 요구하는 건물이고
        if(isNeedGenerator)
        {
            // 땅, 발전기 적용 영역이 아닌 충돌체가 빠져나갔다면
            if(other.gameObject.layer != groundLayer && other.gameObject.layer != generatorZoneLayer)
            {
                // 트리거에서 나간 콜라이더는 리스트에서 제거
                colliders.Remove(other);
            }
        }
        // 발전기를 요구하지 않는 건물이고, 땅이 아닌 충돌체가 트리거에서 나갔다면
        else if (other.gameObject.layer != LayerMask.NameToLayer(groundLayerName))
        {
            // 트리거에서 나간 콜라이더는 리스트에서 제거
            colliders.Remove(other);
            // 트리거 내 콜라이더가 0개라면 설치 가능한 상태로
            if (colliders.Count.Equals(0))
            {
                CanConstruct = true;
                meshRenderer.material.color = originColor;
            }
        }
    }

    // 머티리얼 투명도 변화
    void MatColorChange(float transparency)
    {
        if (meshRenderer)
        {
            Color colorTransparent = meshRenderer.material.color;
            colorTransparent.a = transparency;
            meshRenderer.material.color = colorTransparent;
        }
    }

    // 건설 불가 판정 및 표시
    void ChangeNotConstructable(Collider other)
    {
        CanConstruct = false;
        meshRenderer.material.color *= Color.red;
        colliders.Add(other);
    }

    private void OnDestroy()
    {
        
    }
}
