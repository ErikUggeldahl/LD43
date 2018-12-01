using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public Picker picker;
    public Router router;

    public Transform nodes;
    public GameObject city;
    public GameObject farm;

    public GameObject uiAction;
    public GameObject uiBuild;

    enum State
    {
        Idle,
        BuildSelect,
        Picking,
        Routing,
    }
    State state;

    void Start()
    {
        city = Instantiate(city, Vector3.zero, Quaternion.identity);
        city.name = "City";
        city.transform.parent = nodes;

        uiAction.SetActive(true);
        uiBuild.SetActive(false);
    }

    void Update()
    {
        if (state == State.Idle)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                StartBuilding();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                StartRouting();
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

    public void StartBuilding()
    {
        state = State.BuildSelect;
        uiAction.SetActive(false);
        uiBuild.SetActive(true);
    }

    public void BuildFarm()
    {
        var farm = Instantiate(this.farm);
        farm.GetComponent<Farm>().city = city.transform;
        farm.name = "Farm";
        StartPicking(farm);
    }

    void StartPicking(GameObject building)
    {
        state = State.Picking;
        uiBuild.SetActive(false);

        picker.StartPicking(building, FinishPicking); 
    }

    void FinishPicking(GameObject building)
    {
        state = State.Idle;
        uiAction.SetActive(true);
    }

    void StopPicking()
    {
        state = State.Idle;
        uiAction.SetActive(true);

        picker.StopPicking();
    }

    public void StartRouting()
    {
        state = State.Routing;
        uiAction.SetActive(false);

        router.StartRouting(FinishRouting);
    }

    void FinishRouting()
    {
        state = State.Idle;
        uiAction.SetActive(true);
    }
}
