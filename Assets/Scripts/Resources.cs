using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Resources : MonoBehaviour
{
    public RectTransform sacrificeBar;
    public Text coinText;

    public GameObject resultPanel;
    public TextMeshProUGUI resultText;

    public event Action CoinChanged;

    public int owner;

    bool gameFinished = false;

    int sacrifices = 0;
    public int Sacrifices { get { return sacrifices; } }
    const int TOTAL_SACRIFICES = 100;

    int coins = 30;
    public int Coins { get { return coins; } }

    void Start()
	{
        resultPanel.SetActive(false);

        sacrificeBar.localScale = new Vector3(Mathf.Min((float)sacrifices / TOTAL_SACRIFICES, 1f), 1f, 1f);
        if (coinText) coinText.text = coins.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            resultPanel.SetActive(false);
        }
    }

    public void AddSacrifice(int value)
    {
        sacrifices += value;
        sacrificeBar.localScale = new Vector3(Mathf.Min((float)sacrifices / TOTAL_SACRIFICES, 1f), 1f, 1f);

        if (sacrifices >= TOTAL_SACRIFICES && !gameFinished)
        {
            gameFinished = true;
            resultPanel.SetActive(true);
            if (owner != 0)
            {
                resultText.text = "YOU LOSE";
            }
        }
    }

    public void AddCoin(int value)
    {
        coins += value;
        if(coinText) coinText.text = coins.ToString();

        if (CoinChanged != null) CoinChanged();
    }
}
