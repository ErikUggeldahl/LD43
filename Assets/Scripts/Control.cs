using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Control : MonoBehaviour
{
    public Resources resources;
    public AllBuildings buildings;
    public Picker picker;
    public Router router;

    public Transform nodes;

    public GameObject uiAction;
    public GameObject uiBuild;

    public TextMeshProUGUI tutorialText;
    public GameObject tutorialBarArrow;
    public GameObject tutorialOpponentBarArrow;
    public TutorialBarOscillator tutorialBar;
    public TutorialBarOscillator tutorialOpponentBar;
    public Transform tutorialCamera;

    enum State
    {
        Idle,
        BuildSelect,
        Picking,
        Routing,
    }
    State state;

    enum TutorialStage
    {
        Intro,
        BuildFarm,
        BuildMarket,
        Route,
        Done,
    }
    TutorialStage stage = TutorialStage.Intro;
    public bool TutorialComplete { get { return stage == TutorialStage.Done; } }
    Coroutine tutorial;

    void Start()
    {
        resources.CoinChanged += UpdateActionButtonInteraction;
        resources.CoinChanged += UpdateBuildButtonInteraction;

        foreach (var button in uiBuild.GetComponentsInChildren<Button>())
        {
            button.onClick.AddListener(() => StartPicking(button.GetComponent<TooltipHover>().represent));
        }

        StartIdle();
        tutorial = StartCoroutine(StartTutorial());
    }

    void Update()
    {
        if (!TutorialComplete && Input.GetKeyDown(KeyCode.Escape))
        {
            StopTutorial();
        }
        if (state == State.Idle)
        {
            if (Input.GetKeyDown(KeyCode.B) && (TutorialComplete || stage == TutorialStage.BuildFarm || stage == TutorialStage.BuildMarket))
            {
                StartBuilding();
            }
            else if (Input.GetKeyDown(KeyCode.R) && (TutorialComplete || stage == TutorialStage.Route))
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
            else if (Input.GetKeyDown(KeyCode.F) && (TutorialComplete || stage == TutorialStage.BuildFarm))
            {
                StartPicking(buildings.farm);
            }
            else if (Input.GetKeyDown(KeyCode.M) && (TutorialComplete || stage == TutorialStage.BuildMarket))
            {
                StartPicking(buildings.market);
            }
            else if (Input.GetKeyDown(KeyCode.G) && TutorialComplete)
            {
                StartPicking(buildings.grainSilo);
            }
            else if (Input.GetKeyDown(KeyCode.T) && TutorialComplete)
            {
                StartPicking(buildings.thievesDen);
            }
        }
    }

    public void StartIdle()
    {
        state = State.Idle;
        uiAction.SetActive(true);
        uiBuild.SetActive(false);

        UpdateActionButtonInteraction();
    }

    public void StartBuilding()
    {
        state = State.BuildSelect;
        uiAction.SetActive(false);
        uiBuild.SetActive(true);

        UpdateBuildButtonInteraction();
    }

    void UpdateActionButtonInteraction()
    {
        if (stage == TutorialStage.Intro)
        {
            foreach (var button in uiAction.GetComponentsInChildren<Button>())
            {
                button.interactable = false;
            }
        }
        else if (stage == TutorialStage.BuildFarm || stage == TutorialStage.BuildMarket)
        {
            uiAction.transform.Find("BuildButton").GetComponent<Button>().interactable = true;
            uiAction.transform.Find("RouteButton").GetComponent<Button>().interactable = false;
        }
        else if (stage == TutorialStage.Route)
        {
            uiAction.transform.Find("BuildButton").GetComponent<Button>().interactable = false;
            uiAction.transform.Find("RouteButton").GetComponent<Button>().interactable = true;
        }
        else
        {
            uiAction.transform.Find("BuildButton").GetComponent<Button>().interactable = true;
            uiAction.transform.Find("RouteButton").GetComponent<Button>().interactable = CanRoute();
        }
    }

    void UpdateBuildButtonInteraction()
    {
        if (stage == TutorialStage.BuildFarm)
        {
            foreach (var button in uiBuild.GetComponentsInChildren<Button>())
            {
                button.interactable = button.name == "FarmButton";
            }
        }
        else if (stage == TutorialStage.BuildMarket)
        {
            foreach (var button in uiBuild.GetComponentsInChildren<Button>())
            {
                button.interactable = button.name == "MarketButton";
            }
        }
        else
        {
            foreach (var button in uiBuild.GetComponentsInChildren<Button>())
            {
                button.interactable = CanBuild(button.GetComponent<TooltipHover>().represent);
            }
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

    bool CanRoute()
    {
        return DebugControl.Instance.unlimitedCoin ? true : resources.Coins >= Router.ROAD_COST;
    }

    public void StartRouting()
    {
        if (!CanRoute()) return;

        state = State.Routing;
        uiAction.SetActive(false);

        router.StartRouting();
    }

    IEnumerator StartTutorial()
    {
        tutorialText.gameObject.SetActive(false);
        tutorialBarArrow.SetActive(false);
        tutorialOpponentBarArrow.SetActive(false);
        tutorialBar.enabled = false;
        tutorialOpponentBar.enabled = false;

        yield return Wait(2f);

        tutorialText.text = FormatTutorialText("I AM ZEUS, ALMIGHTY.\nTHIS I DECREE:\nSACRIFICE FOR MY GLORY.");

        tutorialText.gameObject.SetActive(true);

        yield return Wait(6f);

        tutorialText.text = FormatTutorialText("<size=\"14\"><i>PRESS ESCAPE TO SKIP AND BEGIN HONOURING ME IMMEDIATELY.</i></size>");

        yield return Wait(6f);

        tutorialBarArrow.SetActive(true);
        tutorialBar.enabled = true;

        tutorialText.text = FormatTutorialText("SEE HERE THE DISCRETE MEASURMENT OF MY PLEASURE.");

        yield return Wait(6f);

        tutorialText.text = FormatTutorialText("GLORIFY ME BY BRINGING SHEEP TO THE CITY FOR SACRIFICE.");

        yield return Wait(6f);

        tutorialBar.enabled = false;
        tutorialBarArrow.SetActive(false);
        tutorialOpponentBar.enabled = true;
        tutorialOpponentBarArrow.SetActive(true);

        tutorialText.text = FormatTutorialText("MY RIVALS IN THE PANTHEON HAVE SIMILAR DESIGNS.");

        yield return Wait(6f);

        tutorialText.text = FormatTutorialText("YOU WILL OUTDO THEM OR YOUR LANDS ARE SMOTE.");

        yield return Wait(6f);

        Vector3 originalCamPosition = tutorialCamera.position;

        tutorialText.text = FormatTutorialText("SURVEY THE LAND WITH\nW, A, S, OR D.");

        while (tutorialCamera.position == originalCamPosition)
        {
            yield return new WaitForSeconds(0.5f);
        }

        tutorialOpponentBar.enabled = false;
        tutorialOpponentBarArrow.SetActive(false);

        tutorialText.text = FormatTutorialText("IT IS YOURS TO REAP.\nBEGIN BY BUILDING ME A FARM ADJACENT TO MY CITY.");

        stage = TutorialStage.BuildFarm;
        UpdateActionButtonInteraction();
        UpdateBuildButtonInteraction();

        while (nodes.GetComponentInChildren<Farm>() == null)
        {
            yield return new WaitForSeconds(0.5f);
        }

        stage = TutorialStage.BuildMarket;
        UpdateActionButtonInteraction();
        UpdateBuildButtonInteraction();

        tutorialText.text = FormatTutorialText("ACCEPTABLE.\nNOW BUILD A MARKET NEARBY.");

        while (nodes.GetComponentInChildren<Market>() == null)
        {
            yield return new WaitForSeconds(0.5f);
        }

        int startingSacrifices = resources.Sacrifices;

        stage = TutorialStage.Route;
        UpdateActionButtonInteraction();
        UpdateBuildButtonInteraction();

        tutorialText.text = FormatTutorialText("THIS WILL DO.\nNOW BUILD A ROAD FROM THE FARM TO MY CITY.");

        while (resources.Sacrifices == startingSacrifices)
        {
            yield return new WaitForSeconds(0.5f);
        }

        int finalCoins = resources.Coins - 2 * Router.ROAD_COST;

        tutorialText.text = FormatTutorialText("THIS PLEASES ME.\nNOW ROUTE THE FARM TO THE MARKET AND THENCE TO THE CITY.");

        while (resources.Coins > finalCoins)
        {
            yield return new WaitForSeconds(0.5f);
        }

        tutorialText.text = FormatTutorialText("YOU WILL USE THIS COIN TO FURTHER MY DESIRE.");

        yield return Wait(6f);

        tutorialText.text = FormatTutorialText("I LEAVE YOU NOW. SERVE ME WELL.");

        yield return Wait(6f);

        StopTutorial();
    }

    void StopTutorial()
    {
        tutorialText.gameObject.SetActive(false);
        tutorialBarArrow.SetActive(false);
        tutorialOpponentBarArrow.SetActive(false);
        tutorialBar.enabled = false;
        tutorialOpponentBar.enabled = false;
        StopCoroutine(tutorial);
        stage = TutorialStage.Done;
        UpdateActionButtonInteraction();
        UpdateBuildButtonInteraction();
    }

    WaitForSeconds Wait(float time)
    {
        return new WaitForSeconds(time / DebugControl.Instance.tutorialSpeed);
    }

    string FormatTutorialText(string message)
    {
        return string.Format("🏠\n{0}\n🏠", message);
    }
}
