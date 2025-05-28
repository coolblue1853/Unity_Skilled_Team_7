using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkerOffice", menuName = "BuildingData/WorkerOffice")]
public class WorkerOfficeForm : BaseBuildingForm<BuildingData<WorkerOfficeData>>
{
    public override void CreateForm()
    {
        base.CreateForm();
        foreach(var data in dataList)
        {
            DataDic[(int)data.ID] = data;
        }
    }
}

// 일꾼을 관리하는 사무소
[Serializable]
public class WorkerOfficeData : BasicBuildingData
{
    [Header("일꾼 정보")]
    public GameObject WorkerPrefab;
    public int workerCount;
}