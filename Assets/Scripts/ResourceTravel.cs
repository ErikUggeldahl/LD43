using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTravel : MonoBehaviour
{
    public Transform Destination { set { destination = value; transform.LookAt(destination.position); } }
    Transform destination;
    float completionRadius;

    public Transform travellingFrom;

    public int value = 1;

    float speed = 1.75f;

	void Start()
	{
        completionRadius = destination.GetComponent<SphereCollider>().radius;
        transform.LookAt(destination.position);
    }
	
	void Update()
	{
        if (Vector3.Distance(transform.position, destination.position) > completionRadius)
        {
            transform.Translate(Vector3.forward * speed * DebugControl.Instance.speedMultiplier * Time.deltaTime, Space.Self);
        }
        else
        {
            destination.GetComponent<RouteHandler>().Receieve(this);
        }
	}
}
