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
    public Transform roads;

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
                int randomIndex = Random.Range(0, notFullyRouted.Count);
                var selected = notFullyRouted[randomIndex];
                var selectedTransform = (selected as MonoBehaviour).transform;
                foreach (Transform building in nodes)
                {
                    if (selectedTransform == building) continue;

                    if (selected.CanRouteTo(building.gameObject))
                    {
                        Route(selectedTransform, building);
                        if (!selected.HasAvailableRoutes() || Random.value < (selected as MonoBehaviour).GetComponent<Building>().removeRouteChance)
                        {
                            notFullyRouted.RemoveAt(randomIndex);
                        }

                        break;
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

    GameObject Build(GameObject toBuild, Vector3 location)
    {
        var buildingObj = Instantiate(toBuild, location, Quaternion.identity);
        buildingObj.transform.parent = nodes;
        var building = buildingObj.GetComponent<Building>();
        buildingObj.name = building.displayName;
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
