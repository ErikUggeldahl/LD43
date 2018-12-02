using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTravel : MonoBehaviour
{
    public Transform Destination
    {
        set
        {
            destination = value;
            if (destination)
            {
                completionRadius = destination.GetComponent<SphereCollider>().radius;
                transform.LookAt(destination.position);
            }
        }
    }
    Transform destination;
    float completionRadius;

    public Transform origin;

    public int value = 1;

    public float speed = 1.75f;

    void Update()
    {
        if (!destination) return;

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
