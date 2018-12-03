using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugControl : MonoBehaviour
{
    static DebugControl instance = null;
    
    public static DebugControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DebugControl>();
            }

            return instance;
        }
    }

    void Start()
    {
        speedMultiplier = Difficulty.setting == Difficulty.Setting.Hard ? 2.25f : 1f;
    }

    void OnApplicationQuit()
    {
        instance = null;
    }

    [Range(0.25f, 10f)]
    public float speedMultiplier = 1f;

    public bool decisionsAtSpeed = false;

    public bool unlimitedCoin = false;

    [Range(1f, 5f)]
    public float tutorialSpeed = 1f;
}
