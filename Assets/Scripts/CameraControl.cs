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
    }
}
