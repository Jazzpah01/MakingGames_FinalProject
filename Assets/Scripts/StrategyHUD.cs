using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StrategyHUD : MonoBehaviour
{
    public StrategyController strategyController;
    public GameObject initialItem;

    public BuildingList buildings;

    private void Start()
    {
        initialItem.GetComponentsInChildren<TextMeshProUGUI>()[0].text = buildings[0].cost.ToString();
        initialItem.GetComponentsInChildren<TextMeshProUGUI>()[1].text = buildings[0].prefab.name;

        for (int i = 1; i < buildings.Count; i++)
        {
            GameObject newItem = Instantiate(initialItem);
            RectTransform rectT = newItem.GetComponent<RectTransform>();
            rectT.anchoredPosition = rectT.rect.height * i * new Vector2(0,1) + rectT.rect.position;
            newItem.GetComponentsInChildren<TextMeshProUGUI>()[0].text = buildings[i].cost.ToString();
            newItem.GetComponentsInChildren<TextMeshProUGUI>()[1].text = buildings[i].prefab.name;
        }
    }

    public void SelectPrefab(int index)
    {
        strategyController.SelectPrefab(buildings[index]);
    }
}