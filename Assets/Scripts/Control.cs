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
    public GameObject grainSilo;

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

        foreach (var button in uiBuild.GetComponentsInChildren<Button>())
        {
            button.onClick.AddListener(() => StartPicking(button.GetComponent<TooltipHover>().represent));
        }

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
            else if (Input.GetKeyDown(KeyCode.F))
            {
                StartPicking(farm);
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                StartPicking(market);
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                StartPicking(grainSilo);
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
        return DebugControl.Instance.unlimitedCoin ? true : building.GetComponent<Building>().cost <= resources.Coins;
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
