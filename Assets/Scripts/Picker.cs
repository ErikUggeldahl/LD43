using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picker : MonoBehaviour
{
    public GameObject nodes;
    public Camera cam;
    public Transform blueprint;

    GameObject building = null;
    float pickingRadius = 0.0f;

    Action<GameObject> finished;

    int groundMask;

    void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
    }

    public void StartPicking(GameObject building, Action<GameObject> finished)
    {
        enabled = true;

        this.building = building;
        this.finished = finished;

        building.transform.parent = blueprint;
        building.transform.localPosition = Vector3.zero;

        pickingRadius = building.GetComponent<SphereCollider>().radius;

        blueprint.GetChild(0).localScale = new Vector3(pickingRadius, pickingRadius, pickingRadius);
    }

    public void StopPicking()
    {
        Destroy(this.building);
        this.building = null;
        blueprint.gameObject.SetActive(false);
        enabled = false;
    }

    void FinishPicking()
    {
        finished(this.building);
        this.building = null;
        blueprint.gameObject.SetActive(false);
        enabled = false;
    }

    void Update()
    {
        var colliders = nodes.GetComponentsInChildren<SphereCollider>();

        RaycastHit hit;
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, groundMask))
        {
            foreach (var collider in colliders)
            {
                if (Vector3.Distance(collider.transform.position, hit.point) < collider.radius + pickingRadius) return;
            }
            blueprint.gameObject.SetActive(true);
            blueprint.position = hit.point;
        }

        if (Input.GetMouseButtonDown(0))
        {
            building.transform.parent = nodes.transform;
            FinishPicking();
        }
    }
}
