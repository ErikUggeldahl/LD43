using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnchorOscillator : MonoBehaviour
{
    RectTransform rect;
    Vector3 origin;

	void Start()
	{
        rect = GetComponent<RectTransform>();
        origin = rect.position;
	}
	
	void Update()
	{
        rect.position = origin + rect.up * Mathf.Sin(Time.time * Mathf.PI) * 10f;
	}
}
