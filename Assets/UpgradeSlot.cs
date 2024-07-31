using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    private PlayerController player;
    private TextMeshProUGUI tmp;
    private Image img;


    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        img = transform.GetChild(1).GetComponent<Image>();
        gameObject.SetActive(false);
    }

    PowerUp option;
    public void InitOption(PowerUp option)
    {
        this.option = option;
        img.sprite = option.image;
        tmp.text = option.name + "\n-\n" + option.description.ToString();
    }

    public void ChooseThis()
    { 
        player.ChooseOption(option);
    }
}
