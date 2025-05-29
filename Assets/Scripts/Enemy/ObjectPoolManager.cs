using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Inst;

    [System.Serializable]
    public class Pool
    {

        [HideInInspector] public EENEMYTYPE type;
        public GameObject prefab;
        public int initialSize;
    }

    public List<Pool> pools;
    private Dictionary<EENEMYTYPE, Queue<GameObject>> poolDict;

    private void Awake()
    {
        Inst = this;
        poolDict = new Dictionary<EENEMYTYPE, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            var type = pool.prefab.GetComponent<AI_Base>().enemyData.type;
            pool.type = type;

            var queue = new Queue<GameObject>();
            for (int i = 0; i < pool.initialSize; i++)
            {
                var obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }

            poolDict.Add(type, queue);
        }
    }

    public GameObject Get(EENEMYTYPE type)
    {
        if (poolDict[type].Count == 0)
        {
            // 부족하면 동적으로 추가
            var prefab = pools.Find(p => p.type == type).prefab;
            var obj = Instantiate(prefab);
            obj.SetActive(false);
            poolDict[type].Enqueue(obj);
        }

        var go = poolDict[type].Dequeue();
        go.SetActive(true);
        return go;
    }

    public void Return(EENEMYTYPE type, GameObject obj)
    {
        obj.SetActive(false);
        poolDict[type].Enqueue(obj);
    }
}
