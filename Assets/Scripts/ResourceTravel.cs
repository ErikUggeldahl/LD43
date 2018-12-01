using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTravel : MonoBehaviour
{
    public Transform destination;
    float completionRadius;

    float speed = 0.5f;

	void Start()
	{
        completionRadius = destination.GetComponent<SphereCollider>().radius;
        transform.LookAt(destination.position);
    }
	
	void Update()
	{
        if (Vector3.Distance(transform.position, destination.position) > completionRadius)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
