using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Minimap : UI_Scene
{
    [SerializeField] GameObject minimapObj;

    public void OnMinimap()
    {
        minimapObj.SetActive(!minimapObj.activeSelf);
    }

}
