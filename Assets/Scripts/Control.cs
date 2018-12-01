using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public Picker picker;

    public Transform nodes;
    public GameObject city;
    public GameObject farm;

    enum State
    {
        Idle,
        BuildSelect,
        Picking,
    }
    State state;

    void Start()
    {
        this.city = Instantiate(this.city, Vector3.zero, Quaternion.identity);
        this.city.transform.parent = nodes;
    }

    void Update()
    {
        if (state == State.Idle)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                StartBuilding();
            }
        }
        else if (state == State.BuildSelect)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                state = State.Idle;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                var farm = Instantiate(this.farm);
                farm.GetComponent<Farm>().city = city.transform;
                farm.name = "Farm";
                StartPicking(farm);
            }
        }
        else if (state == State.Picking)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopPicking();
            }
        }
    }

    void StartBuilding()
    {
        state = State.BuildSelect;
    }

    void StartPicking(GameObject building)
    {
        state = State.Picking;
        picker.StartPicking(building, FinishPicking);
    }

    void FinishPicking(GameObject building)
    {
        state = State.Idle;
    }

    void StopPicking()
    {
        state = State.Idle;
        picker.StopPicking();
    }
}
