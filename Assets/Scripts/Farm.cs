using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
public class Farm : MonoBehaviour, RouteHandler
{
    public GameObject sheep;

    List<Transform> destinations = new List<Transform>();
    int currentDestination = 0;

    float sheepTimerMax = 2.5f;
    float sheepTimer = 0;

    Transform grainSilo = null;
    public bool HasGrainSilo { get { return grainSilo; } }

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
            sheepTimer -= Time.deltaTime * DebugControl.Instance.speedMultiplier;
            if (sheepTimer <= 0f)
            {
                var sheep = Instantiate(this.sheep, transform.position, Quaternion.identity);
                sheep.name = "Sheep";
                var traveller = sheep.GetComponent<ResourceTravel>();
                if (grainSilo)
                {
                    traveller.Destination = grainSilo;
                    traveller.travellingFrom = transform;
                }
                else
                {
                    traveller.Destination = destinations[NextDestination()];
                }

                sheepTimer += sheepTimerMax;
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
        if (from.GetComponent<Building>().type == Building.Type.GrainSilo)
        {
            grainSilo = from.transform;
        }
    }

    public void Receieve(ResourceTravel traveller)
    {
        traveller.Destination = destinations[NextDestination()];
    }
}
