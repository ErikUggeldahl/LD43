using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour, RouteHandler
{
    public GameObject coin;

    List<Transform> destinations = new List<Transform>();
    int currentDestination = 0;

    const float COIN_TIMER_MAX = 1f;
    float coinTimer = 0;

    int sheepStore = 0;

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
            if (sheepStore > 0 && coinTimer <= 0f)
            {
                var coin = Instantiate(this.coin, transform.position, Quaternion.identity);
                coin.name = "Coin";
                coin.GetComponent<ResourceTravel>().Destination = destinations[NextDestination()];

                coinTimer += COIN_TIMER_MAX;
                sheepStore--;
            }
        }
    }

    int NextDestination()
    {
        currentDestination++;
        currentDestination %= destinations.Count;
        return currentDestination;
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
            sheepStore += traveller.value;
        }
        Destroy(traveller.gameObject);
    }
}
