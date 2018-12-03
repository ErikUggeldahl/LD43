using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public enum Setting
    {
        Normal,
        Hard,
    }
    public static Setting setting = Setting.Normal;
}
