using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour, RouteHandler
{
    public GameObject sheep;

    List<Transform> destinations = new List<Transform>();
    int currentDestination = 0;

    float sheepTimerMax = 2.5f;
    float sheepTimer = 0;

    bool grainFed = false;

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
                var resourceTravel = sheep.GetComponent<ResourceTravel>();
                resourceTravel.destination = destinations[currentDestination++];
                if (grainFed)
                {
                    sheep.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    resourceTravel.value = 2;
                }

                currentDestination %= destinations.Count;
                sheepTimer += sheepTimerMax;
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
        if (from.GetComponent<Building>().type == Building.Type.GrainSilo)
        {
            grainFed = true;
        }
    }

    public void Receieve(ResourceTravel traveller)
    {
    }
}
