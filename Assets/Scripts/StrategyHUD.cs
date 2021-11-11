using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StrategyHUD : MonoBehaviour
{
    [HideInInspector]public StrategyController strategyController;
    public GameObject initialItem;

    public BuildingList buildings;

    private void Start()
    {
        strategyController = GameController.instance.strategyController;

        // Setup menu for selecting buildings
        initialItem.GetComponentsInChildren<TextMeshProUGUI>()[0].text = buildings[0].cost.ToString();
        initialItem.GetComponentsInChildren<TextMeshProUGUI>()[1].text = buildings[0].prefab.name;

        initialItem.GetComponentInChildren<Button>().onClick.AddListener(delegate { SelectPrefab(0); });

        for (int i = 1; i < buildings.Count; i++)
        {
            GameObject newItem = Instantiate(initialItem);
            newItem.transform.parent = initialItem.transform.parent;
            RectTransform rectT = newItem.GetComponent<RectTransform>();
            newItem.transform.position = initialItem.transform.position - (Vector3)(rectT.rect.height * i * new Vector2(0,1));
            newItem.GetComponentsInChildren<TextMeshProUGUI>()[0].text = buildings[i].cost.ToString();
            newItem.GetComponentsInChildren<TextMeshProUGUI>()[1].text = buildings[i].prefab.name;

            int index = i;

            newItem.GetComponentInChildren<Button>().onClick.AddListener(delegate { SelectPrefab(index); });
        }
    }

    /// <summary>
    /// Will set strategy controller's prefab
    /// </summary>
    /// <param name="index"></param>
    public void SelectPrefab(int index)
    {
        strategyController.SelectPrefab(buildings[index]);
    }
}