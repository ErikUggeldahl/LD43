﻿using System.Collections;
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

    void OnApplicationQuit()
    {
        instance = null;
    }

    [Range(0.25f, 10f)]
    public float speedMultiplier = 1f;

    public bool decisionsAtSpeed = true;

    public bool unlimitedCoin = false;
}
