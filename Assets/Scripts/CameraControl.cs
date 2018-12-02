using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    const float MOVE_SPEED = 15f;

    void Start()
    {
    }

    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * MOVE_SPEED * Time.deltaTime;
        transform.Translate(move, Space.World);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -100f, 100f),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -110f, 110f)
        );
    }
}
