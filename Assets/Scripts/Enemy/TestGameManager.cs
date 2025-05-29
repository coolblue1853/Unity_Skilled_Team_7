using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    public GameObject testPlayer;
       public WaveController waveController;

    public static TestGameManager Inst { get; private set; }
    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject); 
            return;
        }

        Inst = this;
        DontDestroyOnLoad(gameObject); 
    }

    private void Start()
    {
        waveController.StartNextWave();
    }
}
