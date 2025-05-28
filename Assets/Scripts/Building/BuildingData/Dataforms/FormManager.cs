using System.Collections.Generic;
using UnityEngine;
using System;

public class FormManager : Singleton<FormManager>
{
    [SerializeField] List<ScriptableObject> formList = new List<ScriptableObject>();

    private Dictionary<Type, IForm> formDic = new Dictionary<Type, IForm>();

    void Awake()
    {
        foreach (var formObj in formList)
        {
            if (formObj is IForm form)
            {
                form.CreateForm();
                formDic[form.Type] = form;
            }
        }
    }

    /// <summary>
    /// 등록된 테이블을 가져오는 함수 
    /// </summary>
    /// <typeparam name="T">사용할 Table</typeparam>
    /// <returns></returns>
    public T GetForm<T>() where T : class
    {
        return formDic[typeof(T)] as T;
    }

#if UNITY_EDITOR
    public void AutoAssignTables()
    {
        formList.Clear();

        // 빌딩 데이터의 경로로부터 스크립터블 오브젝트만 찾기
        string[] guids =
            UnityEditor.AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Scripts/Construct/BuildingData" });

        foreach (string guid in guids)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            if (asset is IForm)
            {
                if (!formList.Contains(asset))
                {
                    formList.Add(asset);
                }
            }
        }

        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}
