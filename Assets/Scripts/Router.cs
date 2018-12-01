using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Router : MonoBehaviour
{
    public Control control;
    public GameObject roadPrefab;
    public Camera cam;
    public Transform routeFromMarker;
    public Transform routeToMarker;

    GameObject road;
    const float ROAD_HEIGHT = 0.07f;

    RouteHandler routeFrom;

    int nodeMask, groundMask;

    void Start()
    {
        nodeMask = LayerMask.GetMask("Node");
        groundMask = LayerMask.GetMask("Ground");
    }

    public void StartRouting()
    {
        enabled = true;

        road = Instantiate(roadPrefab);
        road.name = "Road";
    }

    void StopRouting()
    {
        routeFrom = null;
        routeToMarker.gameObject.SetActive(false);
        routeFromMarker.gameObject.SetActive(false);

        control.StartIdle();

        enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (road != null) Destroy(road);
            StopRouting();
            return;
        }

        RaycastHit hit;
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, nodeMask))
        {
            var routeHandler = hit.transform.GetComponent<RouteHandler>();
            if (routeFrom == null)
            {
                if (!routeHandler.HasAvailableRoutes()) return;

                routeFromMarker.gameObject.SetActive(true);
                routeFromMarker.position = hit.transform.position;
                routeFromMarker.localScale = Vector3.one * (hit.collider as SphereCollider).radius;

                if (Input.GetMouseButtonDown(0))
                {
                    routeFrom = routeHandler;
                    road.GetComponent<LineRenderer>().SetPosition(0, new Vector3(hit.transform.position.x, ROAD_HEIGHT, hit.transform.position.z));
                    road.GetComponent<LineRenderer>().SetPosition(1, new Vector3(hit.transform.position.x, ROAD_HEIGHT, hit.transform.position.z));
                }
            }
            else if (routeHandler != routeFrom && routeFrom.CanRouteTo((routeHandler as MonoBehaviour).gameObject))
            {
                routeToMarker.gameObject.SetActive(true);
                routeToMarker.position = hit.transform.position;
                routeToMarker.localScale = Vector3.one * (hit.collider as SphereCollider).radius;
                road.GetComponent<LineRenderer>().SetPosition(1, new Vector3(hit.transform.position.x, ROAD_HEIGHT, hit.transform.position.z));

                if (Input.GetMouseButtonDown(0))
                {
                    routeFrom.AddRouteTo((routeHandler as MonoBehaviour).gameObject);
                    routeHandler.AddRouteFrom((routeFrom as MonoBehaviour).gameObject);

                    StopRouting();
                }
            }
        }
        else
        {
            if (routeFrom == null)
            {
                routeFromMarker.gameObject.SetActive(false);
            }
            else
            {
                routeToMarker.gameObject.SetActive(false);

                if (Physics.Raycast(ray, out hit, float.PositiveInfinity, groundMask))
                {
                    road.GetComponent<LineRenderer>().SetPosition(1, new Vector3(hit.point.x, ROAD_HEIGHT, hit.point.z));
                }
            }
        }
    }
}
