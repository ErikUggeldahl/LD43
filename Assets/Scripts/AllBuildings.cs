using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllBuildings : MonoBehaviour
{
    public GameObject road;

    public GameObject farm;
    public GameObject market;
    public GameObject grainSilo;

    int weightedTotal;
    List<GameObject> all;
    public IEnumerable<GameObject> All { get { return all;  } }

    void Start()
    {
        all = new List<GameObject>
        {
            farm,
            market,
            grainSilo,
        };
        weightedTotal = All.Aggregate(0, (sum, go) => sum + go.GetComponent<Building>().randomWeight);
    }

    public GameObject WeightedRandom()
    {
        var selection = Random.Range(0, weightedTotal);
        var currentSum = 0;
        foreach (var building in All)
        {
            currentSum += building.GetComponent<Building>().randomWeight;
            if (selection < currentSum)
            {
                return building;
            }
        }
        throw new System.NotImplementedException();
    }
}
