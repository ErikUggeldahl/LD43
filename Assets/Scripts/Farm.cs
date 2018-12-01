using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    public GameObject sheep;
    public Transform city;

    float sheepTimerMax = 5f;
    float sheepTimer;

	void Start()
	{
        sheepTimer = sheepTimerMax;
    }
	
	void Update()
	{
        sheepTimer -= Time.deltaTime;
        if (sheepTimer <= 0f)
        {
            var sheep = Instantiate(this.sheep, transform.position, Quaternion.identity);
            sheep.name = "Sheep";
            sheep.GetComponent<Sheep>().city = city;

            sheepTimer += sheepTimerMax;
        }
	}
}
