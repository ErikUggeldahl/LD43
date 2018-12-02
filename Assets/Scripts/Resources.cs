using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{
    public RectTransform sacrificeBar;
    public Text coinText;

    public event Action CoinChanged;

    int sacrifices = 0;
    public int Sacrifices { get { return sacrifices; } }
    const int TOTAL_SACRIFICES = 100;

    int coins = 30;
    public int Coins { get { return coins; } }

    void Start()
	{
        sacrificeBar.localScale = new Vector3(Mathf.Min((float)sacrifices / TOTAL_SACRIFICES, 1f), 1f, 1f);
        if (coinText) coinText.text = "Coin: " + coins;
    }

    public void AddSacrifice(int value)
    {
        sacrifices += value;
        sacrificeBar.localScale = new Vector3(Mathf.Min((float)sacrifices / TOTAL_SACRIFICES, 1f), 1f, 1f);
    }

    public void AddCoin(int value)
    {
        coins += value;
        if(coinText) coinText.text = "Coin: " + coins;

        if (CoinChanged != null) CoinChanged();
    }
}
