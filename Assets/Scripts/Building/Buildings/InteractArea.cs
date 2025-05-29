using UnityEngine;

// 에너지 공급이 필요한 건물 스크립트에 부착
public interface IBuildingRequireEnegy
{
    // 에너지 공급이 끊겼을 때
    public abstract void OnEnegyDown();

    // 에너지 공급이 시작될 때
    public abstract void OnEnegySupply();
}

public class InteractArea : MonoBehaviour
{
    // 상호작용 가능 영역 크기 변경
    public void ChangeRange(float range) => transform.localScale = new Vector3(range, 1, range);
}
