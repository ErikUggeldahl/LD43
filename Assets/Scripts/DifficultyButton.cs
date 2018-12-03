using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DifficultyButton : MonoBehaviour, IPointerDownHandler
{
    public Difficulty.Setting setting;

    public void OnPointerDown(PointerEventData eventData)
    {
        Difficulty.setting = setting;
        SceneManager.LoadScene("SampleScene");
    }
}
