using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control : MonoBehaviour
{
    public Resources resources;
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
        resources.CoinChanged += UpdateBuildButtonInteraction;

        city = Instantiate(city, Vector3.zero, Quaternion.identity);
        city.name = "City";
        city.GetComponent<City>().resources = resources;
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

        UpdateBuildButtonInteraction();
    }

    void UpdateBuildButtonInteraction()
    {
        foreach (var button in uiBuild.GetComponentsInChildren<Button>())
        {
            button.interactable = CanBuild(button.GetComponent<TooltipHover>().represent);
        }
    }

    bool CanBuild(GameObject building)
    {
        return building.GetComponent<Building>().cost <= resources.Coins;
    }

    public void BuildFarm()
    {
        StartPicking(farm);
    }

    public void BuildMarket()
    {
        StartPicking(market);
    }

    void StartPicking(GameObject building)
    {
        if (!CanBuild(building)) return;

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
