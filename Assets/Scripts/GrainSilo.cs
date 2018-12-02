using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrainSilo : MonoBehaviour, RouteHandler
{
    const int MAX_FARMS_SUPPORTED = 2;
    List<Transform> farmsSupported = new List<Transform>(MAX_FARMS_SUPPORTED);

    public bool HasAvailableRoutes()
    {
        return farmsSupported.Count < MAX_FARMS_SUPPORTED;
    }

    public bool CanRouteTo(GameObject to)
    {
        if (farmsSupported.Contains(to.transform)) return false;
        var toBuilding = to.GetComponent<Building>();
        if (toBuilding.type == Building.Type.Farm && toBuilding.GetComponent<Farm>().HasGrainSilo) return false;
        return Array.IndexOf(GetComponent<Building>().connects, toBuilding.type) != -1;
    }

    public void AddRouteTo(GameObject to)
    {
        farmsSupported.Add(to.transform);
    }

    public void AddRouteFrom(GameObject from)
    {
        throw new NotImplementedException();
    }

    public void Receieve(ResourceTravel traveller)
    {
        traveller.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        traveller.value = 2;
        traveller.Destination = traveller.origin;
    }
}
