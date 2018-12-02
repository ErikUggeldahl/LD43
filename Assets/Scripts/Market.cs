using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour, RouteHandler
{
    public GameObject coin;

    List<Transform> destinations = new List<Transform>();

    float cointTimerMax = 1.25f;
    float coinTimer = 0;

    int sheepCount = 0;

    enum State
    {
        Idle,
        Producing,
    }
    State state = State.Idle;
	
	void Update()
	{
        if (state == State.Producing)
        {
            if (coinTimer > 0f)
            {
                coinTimer -= Time.deltaTime * DebugControl.Instance.speedMultiplier;
            }
            if (sheepCount > 0 && coinTimer <= 0f)
            {
                var coin = Instantiate(this.coin, transform.position, Quaternion.identity);
                coin.name = "Coin";
                coin.GetComponent<ResourceTravel>().destination = destinations[0];

                coinTimer += cointTimerMax;
                sheepCount--;
            }
        }
    }

    public bool HasAvailableRoutes()
    {
        return true;
    }

    public bool CanRouteTo(GameObject to)
    {
        if (destinations.Contains(to.transform)) return false;
        return Array.IndexOf(GetComponent<Building>().connects, to.GetComponent<Building>().type) != -1;
    }

    public void AddRouteTo(GameObject to)
    {
        destinations.Add(to.transform);
        state = State.Producing;
    }

    public void AddRouteFrom(GameObject from)
    {
    }

    public void Receieve(ResourceTravel traveller)
    {
        if (traveller.tag == "Sheep")
        {
            sheepCount += traveller.value;
        }
    }
}
