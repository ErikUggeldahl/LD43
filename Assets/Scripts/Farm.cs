using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour, RouteHandler
{
    public GameObject sheep;

    List<Transform> destinations = new List<Transform>();

    float sheepTimerMax = 5f;
    float sheepTimer;

    enum State
    {
        Idle,
        Producing,
    }
    State state = State.Idle;

	void Start()
	{
        sheepTimer = sheepTimerMax;
    }
	
	void Update()
	{
        if (state == State.Producing)
        {
            sheepTimer -= Time.deltaTime;
            if (sheepTimer <= 0f)
            {
                var sheep = Instantiate(this.sheep, transform.position, Quaternion.identity);
                sheep.name = "Sheep";
                sheep.GetComponent<Sheep>().destination = destinations[0];

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
        return to.tag == "City";
    }

    public void AddRouteTo(GameObject to)
    {
        destinations.Add(to.transform);
        state = State.Producing;
    }

    public void AddRouteFrom(GameObject from)
    {
    }
}
