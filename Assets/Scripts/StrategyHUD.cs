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

    public GameObject itemScrollView;

    public GameObject scrollPanel;

    public int offset = -10;

    private void Start()
    {
        strategyController = GameController.instance.strategyController;

        // Setup menu for selecting buildings
        initialItem.GetComponentsInChildren<TextMeshProUGUI>()[0].text = buildings[0].cost.ToString();
        initialItem.GetComponentsInChildren<TextMeshProUGUI>()[1].text = buildings[0].prefab.name;

        initialItem.GetComponentInChildren<Button>().onClick.AddListener(delegate { SelectPrefab(0); });

        int items = 1;

        for (int i = 1; i < buildings.Count; i++)
        {
            GameObject newItem = Instantiate(initialItem);
            newItem.transform.parent = initialItem.transform.parent;
            items++;

            // Change prosition
            RectTransform rectT = newItem.GetComponent<RectTransform>();
            RectTransform oldRectT = initialItem.GetComponent<RectTransform>();
            rectT.localScale = oldRectT.localScale;
            rectT.position = oldRectT.position - 
                (Vector3)(rectT.rect.height * i * new Vector2(0,1) + 
                offset * i * new Vector2(0,1));

            // Change values
            newItem.GetComponentsInChildren<TextMeshProUGUI>()[0].text = buildings[i].cost.ToString();
            newItem.GetComponentsInChildren<TextMeshProUGUI>()[1].text = buildings[i].prefab.name;

            int index = i;

            newItem.GetComponentInChildren<Button>().onClick.AddListener(delegate { SelectPrefab(index); });
        }

        RectTransform panelRect = scrollPanel.GetComponent<RectTransform>();
        panelRect.position = panelRect.position * new Vector2(1,0);
        //RectTransform itemRect = initialItem.GetComponent<RectTransform>();
        //RectTransform panelRect = scrollPanel.GetComponent<RectTransform>();
        //panelRect.localScale = new Vector3(panelRect.localScale.x, itemRect.localScale.y * items + offset * items, panelRect.localScale.z);
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