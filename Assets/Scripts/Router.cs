using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Router : MonoBehaviour
{
    public GameObject roadPrefab;
    public Camera cam;
    public Transform routeFromMarker;
    public Transform routeToMarker;

    GameObject road;

    GameObject routeFrom;
    GameObject routeTo;

    public delegate void Finished();
    Finished finished;

    int nodeMask, groundMask;

    void Start()
    {
        nodeMask = LayerMask.GetMask("Node");
        groundMask = LayerMask.GetMask("Ground");
    }

    public void StartRouting(Finished finished)
    {
        enabled = true;

        this.finished = finished;

        road = Instantiate(roadPrefab);
        road.name = "Road";
    }

    void Update()
    {
        RaycastHit hit;
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, nodeMask))
        {
            if (routeFrom == null)
            {
                routeFromMarker.gameObject.SetActive(true);
                routeFromMarker.position = hit.transform.position;
                routeFromMarker.localScale = Vector3.one * (hit.collider as SphereCollider).radius;

                if (Input.GetMouseButtonDown(0))
                {
                    routeFrom = hit.transform.gameObject;
                    road.GetComponent<LineRenderer>().SetPosition(0, new Vector3(hit.transform.position.x, 0.01f, hit.transform.position.z));
                    road.GetComponent<LineRenderer>().SetPosition(1, new Vector3(hit.transform.position.x, 0.01f, hit.transform.position.z));
                }
            }
            else if (hit.transform.gameObject != routeFrom)
            {
                routeToMarker.gameObject.SetActive(true);
                routeToMarker.position = hit.transform.position;
                routeToMarker.localScale = Vector3.one * (hit.collider as SphereCollider).radius;
                road.GetComponent<LineRenderer>().SetPosition(1, new Vector3(hit.transform.position.x, 0.01f, hit.transform.position.z));

                if (Input.GetMouseButtonDown(0))
                {
                    routeTo = hit.transform.gameObject;

                    routeToMarker.gameObject.SetActive(false);
                    routeFromMarker.gameObject.SetActive(false);
                    routeFrom = null;
                    routeTo = null;

                    finished();

                    enabled = false;
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
                    road.GetComponent<LineRenderer>().SetPosition(1, new Vector3(hit.point.x, 0.01f, hit.point.z));
                }
            }
        }
    }
}
