using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public Transform city;
    float completionRadius;

    float speed = 0.5f;

	void Start()
	{
        completionRadius = city.GetComponent<SphereCollider>().radius;
        transform.LookAt(city.position);
    }
	
	void Update()
	{
        if (Vector3.Distance(transform.position, city.position) > completionRadius)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
