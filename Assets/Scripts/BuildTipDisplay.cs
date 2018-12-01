using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildTipDisplay : MonoBehaviour
{
    public TMP_SpriteAsset sprite;
    public TextMeshProUGUI tipText;
    public Text costText;

    void Start()
    {
        Hide();
    }

    public void Display(Building tip)
    {
        gameObject.SetActive(true);
        var emoji = char.ConvertFromUtf32(sprite.spriteInfoList.Find(sprite => sprite.name == tip.displayName).unicode).ToString();
        var accepts = String.Join(", ", tip.accepts.Select(type => type.ToString()).ToArray());
        var connects = String.Join(", ", tip.connects.Select(type => type.ToString()).ToArray());

        tipText.text = string.Format(
@"<size=36>{0} {1}</size>
Accepts: {2}
Connects: {3}

{4}", emoji, tip.displayName, accepts, connects, tip.description);

        costText.text = tip.cost.ToString();

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
