using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public enum Type
    {
        City,
        Farm,
        Market,
    }

    public Type type;
    public string displayName;
    public int cost;
    public Type[] accepts;
    public Type[] connects;
    [TextArea]
    public string description;
}
