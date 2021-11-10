using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseOver : MonoBehaviour
{
    private UpgradeController upgrade;

    private void Start()
    {
        upgrade = GameObject.FindObjectOfType(typeof(UpgradeController)) as UpgradeController;
    }

    private void OnMouseEnter()
    {
        gameObject.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = upgrade.upgrades[int.Parse(gameObject.name.Replace("btnUpgrades(Clone)", ""))].GetName();
        gameObject.transform.GetChild(2).gameObject.SetActive(true);    
    }

    private void OnMouseExit()
    {
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }
}

