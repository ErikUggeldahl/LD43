using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCControl : MonoBehaviour
{
    public Resources resources;
    public AllBuildings buildings;
    public Transform spawnLocations;

    public Transform nodes;
    public Transform[] opponentNodes;
    public Transform roads;

    public Control playerControl;

    public int playerNumber;

    const float DECISION_DOWNTIME = 1.5f;
    float Downtime { get { return DECISION_DOWNTIME / (DebugControl.Instance.decisionsAtSpeed ? DebugControl.Instance.speedMultiplier : 1f); } }

    bool acting = true;
    bool building = true;

    List<RouteHandler> notFullyRouted = new List<RouteHandler>();

    void Start()
    {
        StartCoroutine(Act());
    }

    IEnumerator Act()
    {
        while (!playerControl.TutorialComplete)
        {
            yield return new WaitForSeconds(2f);
        }

        var farm = Build(buildings.farm, NextBuildingLocation(2));
        yield return new WaitForSeconds(Downtime);
        var market = Build(buildings.market, NextBuildingLocation(2));
        yield return new WaitForSeconds(Downtime);
        Route(farm.transform, market.transform);
        yield return new WaitForSeconds(Downtime);
        var city = nodes.GetChild(0);
        Route(market.transform, city);

        Building nextBuild = buildings.WeightedRandom().GetComponent<Building>();

        while (acting)
        {

            yield return new WaitForSeconds(Downtime);

            if (resources.Coins >= Router.ROAD_COST && notFullyRouted.Count > 0)
            {
                // Attempt only one route per iteration
                int randomIndex = Random.Range(0, notFullyRouted.Count);
                var selected = notFullyRouted[randomIndex];
                var selectedTransform = (selected as MonoBehaviour).transform;
                var routeTo = selectedTransform.GetComponent<Building>().type == Building.Type.ThievesDen ?
                    RandomNode(selected, opponentNodes[RandomOpponent()]) :
                    RandomNode(selected, nodes);

                if (routeTo)
                {
                    Route(selectedTransform, routeTo);

                    if (!selected.HasAvailableRoutes() || Random.value < (selected as MonoBehaviour).GetComponent<Building>().removeRouteChance)
                    {
                        notFullyRouted.RemoveAt(randomIndex);
                    }
                }
            }

            if (building && resources.Coins >= nextBuild.cost)
            {
                Build(nextBuild.gameObject, NextBuildingLocation());
                nextBuild = buildings.WeightedRandom().GetComponent<Building>();
            }
        }
    }

    Vector3 NextBuildingLocation(int range = 4)
    {
        var childIndex = Random.Range(0, Mathf.Min(4, spawnLocations.childCount));
        var location = spawnLocations.GetChild(childIndex).position;
        Destroy(spawnLocations.GetChild(childIndex).gameObject);

        if (spawnLocations.childCount == 1)
        {
            building = false;
        }

        return location;
    }

    Transform RandomNode(RouteHandler routeHandler, Transform nodes)
    {
        if (routeHandler == null) Debug.Log("Passed a null handler");
        if (nodes.childCount == 0) return null;

        var possible = nodes
            .Cast<Transform>()
            .Where(building => { if (building == null) Debug.Log("Building is null"); return building != transform && routeHandler.CanRouteTo(building.gameObject); })
            .ToList();
        if (possible.Count == 0) return null;

        return possible[Random.Range(0, possible.Count)];
    }

    int RandomOpponent()
    {
        return Random.Range(0, opponentNodes.Length);
    }

    GameObject Build(GameObject toBuild, Vector3 location)
    {
        var buildingObj = Instantiate(toBuild, location, Quaternion.identity);
        buildingObj.transform.parent = nodes;
        var building = buildingObj.GetComponent<Building>();
        buildingObj.name = building.displayName;
        building.owner = playerNumber;
        resources.AddCoin(-building.cost);

        var routeHandler = building.GetComponent<RouteHandler>();
        if (routeHandler.HasAvailableRoutes())
        {
            notFullyRouted.Add(routeHandler);
        }
        return buildingObj;
    }

    void Route(Transform from, Transform to)
    {
        var road = Instantiate(buildings.road);
        road.name = "Road";
        road.transform.parent = roads;
        resources.AddCoin(-Router.ROAD_COST);

        road.GetComponent<LineRenderer>().SetPositions(new Vector3[]
        {
            new Vector3(from.transform.position.x, Router.ROAD_HEIGHT, from.transform.position.z),
            new Vector3(to.transform.position.x, Router.ROAD_HEIGHT, to.transform.position.z),
        });

        from.GetComponent<RouteHandler>().AddRouteTo(to.gameObject);
        to.GetComponent<RouteHandler>().AddRouteFrom(from.gameObject);
    }
}
