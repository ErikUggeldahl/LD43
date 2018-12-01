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
    public GameObject market;

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

        StartIdle();
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
                StartIdle();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                BuildFarm();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                BuildMarket();
            }
        }
    }

    public void StartIdle()
    {
        state = State.Idle;
        uiAction.SetActive(true);
        uiBuild.SetActive(false);
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
        farm.name = "Farm";
        StartPicking(farm);
    }

    public void BuildMarket()
    {
        var market = Instantiate(this.market);
        market.name = "Market";
        StartPicking(market);
    }

    void StartPicking(GameObject building)
    {
        state = State.Picking;
        uiBuild.SetActive(false);

        picker.StartPicking(building);
    }

    public void StartRouting()
    {
        state = State.Routing;
        uiAction.SetActive(false);

        router.StartRouting();
    }
}
