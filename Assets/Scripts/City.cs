using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour, RouteHandler
{
    void Start()
	{
	}
	
	void Update()
	{
	}

    public bool HasAvailableRoutes()
    {
        return false;
    }

    public bool CanRouteTo(GameObject to)
    {
        return true;
    }

    public void AddRouteTo(GameObject to)
    {
        throw new System.NotImplementedException();
    }

    public void AddRouteFrom(GameObject from)
    {
    }

    public void Receieve(GameObject traveller)
    {
    }
}
