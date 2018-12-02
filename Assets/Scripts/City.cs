using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour, RouteHandler
{
    public Resources resources;

    public bool HasAvailableRoutes()
    {
        return false;
    }

    public bool CanRouteTo(GameObject to)
    {
        throw new System.NotImplementedException();
    }

    public void AddRouteTo(GameObject to)
    {
        throw new System.NotImplementedException();
    }

    public void AddRouteFrom(GameObject from)
    {
    }

    public void Receieve(ResourceTravel traveller)
    {
        if (traveller.tag == "Sheep")
        {
            resources.AddSacrifice(traveller.value);
        }
        else if (traveller.tag == "Coin" || traveller.tag == "Thief")
        {
            resources.AddCoin(traveller.value);
        }
        Destroy(traveller.gameObject);
    }
}
