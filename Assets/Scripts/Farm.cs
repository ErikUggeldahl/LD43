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
                sheep.GetComponent<ResourceTravel>().destination = destinations[currentDestination++];

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
        return to.tag == "City" || to.tag == "Market";
    }

    public void AddRouteTo(GameObject to)
    {
        destinations.Add(to.transform);
        state = State.Producing;
    }

    public void AddRouteFrom(GameObject from)
    {
    }

    public void Receieve(GameObject traveller)
    {
    }
}
