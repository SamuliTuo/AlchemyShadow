using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class UpgradeSlot : MonoBehaviour
{
    private PlayerController player;
    private TextMeshProUGUI tmp;


    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    PowerUp option;
    public void InitOption(PowerUp option)
    {
        this.option = option;
        tmp.text = option.name + "\n-\n" + option.description.ToString();
    }

    public void ChooseThis()
    { 
        player.ChooseOption(option);
    }
}
