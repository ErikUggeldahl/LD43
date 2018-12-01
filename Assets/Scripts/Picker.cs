using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picker : MonoBehaviour
{
    public Control control;
    public GameObject nodes;
    public Camera cam;
    public Transform blueprint;

    GameObject building = null;
    float pickingRadius = 0.0f;

    int groundMask;

    void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
    }

    public void StartPicking(GameObject building)
    {
        enabled = true;

        this.building = building;
        building.transform.parent = blueprint;
        building.transform.localPosition = Vector3.zero;

        pickingRadius = building.GetComponent<SphereCollider>().radius;

        blueprint.GetChild(0).localScale = new Vector3(pickingRadius, pickingRadius, pickingRadius);
    }

    public void StopPicking()
    { 
        building = null;
        blueprint.gameObject.SetActive(false);

        control.StartIdle();

        enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(building);
            StopPicking();
            return;
        }

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
            StopPicking();
        }
    }
}
