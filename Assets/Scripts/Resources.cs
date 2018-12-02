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
    const int TOTAL_SACRIFICES = 100;

    int coins = 20;
    public int Coins { get { return coins; } }

    void Start()
	{
        sacrificeBar.localScale = new Vector3((float)sacrifices / TOTAL_SACRIFICES, 1f, 1f);
        coinText.text = "Coin: " + coins;
    }

    public void AddSacrifice()
    {
        sacrifices++;
        sacrificeBar.localScale = new Vector3((float)sacrifices / TOTAL_SACRIFICES, 1f, 1f);
    }

    public void AddCoin(int value)
    {
        coins += value;
        coinText.text = "Coin: " + coins;

        CoinChanged();
    }
}
