using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MonsterSpawnInfo
{
    public GameObject prefab;
    public int count;
}

[CreateAssetMenu(menuName = "Wave/WaveData")]
public class WaveData : ScriptableObject
{
    public List<MonsterSpawnInfo> spawnList;
}


public class WaveController : MonoBehaviour
{
    public List<WaveData> waves; // 웨이브 순서대로 등록
    public Transform[] spawnPoints; // 랜덤 스폰 지점

    private int currentWave = 0;

    public void StartNextWave()
    {
        if (currentWave >= waves.Count)
        {
            Debug.Log("All waves complete");
            return;
        }

        var wave = waves[currentWave];
        StartCoroutine(SpawnWave(wave));
        currentWave++;
    }

    private IEnumerator SpawnWave(WaveData wave)
    {
        foreach (var info in wave.spawnList)
        {
            for (int i = 0; i < info.count; i++)
            {
                var type = info.prefab.GetComponent<AI_Base>().enemyData.type;
                GameObject monster = ObjectPoolManager.Inst.Get(type);
                var spawnPos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                monster.transform.position = spawnPos;

                // 몬스터 초기화 필요시 호출
                //monster.GetComponent<AI_Base>()?.Initialize();

                yield return new WaitForSeconds(0.2f); // 간격 배치
            }
        }
    }
}
