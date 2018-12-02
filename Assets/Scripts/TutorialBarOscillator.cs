using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBarOscillator : MonoBehaviour
{
    RectTransform rect;

    void Awake()
	{
        rect = GetComponent<RectTransform>();
    }

    void OnDisable()
    {
        rect.localScale = new Vector3(0f, 1f, 1f);
    }

    void Update()
	{
        rect.localScale = new Vector3((Mathf.Sin(Time.time * Mathf.PI / 2f) + 1) / 2f, 1f, 1f);
    }
}
