using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThievesDen : MonoBehaviour, RouteHandler
{
    public GameObject thief;

    Transform destination;

    const float THIEF_TIMER_MAX = 1f;
    float thiefTimer = 0;

    const float THIEF_SPEED = 4;

    const int THIEF_COST = 3;
    int coinStore = 0;

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
            if (thiefTimer > 0f)
            {
                thiefTimer -= Time.deltaTime * DebugControl.Instance.speedMultiplier;
            }
            if (coinStore >= THIEF_COST && thiefTimer <= 0f && destination)
            {
                var thief = Instantiate(this.thief, transform.position, Quaternion.identity);
                thief.name = "Thief";
                var resourceTraveler = thief.GetComponent<ResourceTravel>();
                resourceTraveler.origin = transform;
                resourceTraveler.Destination = destination;
                resourceTraveler.speed = THIEF_SPEED;

                thiefTimer += THIEF_TIMER_MAX;
                coinStore -= THIEF_COST;
            }
        }
    }

    public bool HasAvailableRoutes()
    {
        return destination == null;
    }

    public bool CanRouteTo(GameObject to)
    {
        var building = GetComponent<Building>();
        var toBuilding = to.GetComponent<Building>();
        return Array.IndexOf(building.connects, toBuilding.type) != -1 &&
            building.owner != toBuilding.owner;
    }

    public void AddRouteTo(GameObject to)
    {
        destination = to.transform;
    }

    public void AddRouteFrom(GameObject from)
    {
        state = State.Producing;
    }

    public void Receieve(ResourceTravel traveller)
    {
        if (traveller.tag == "Coin")
        {
            coinStore += traveller.value;
        }
        Destroy(traveller.gameObject);
    }
}
