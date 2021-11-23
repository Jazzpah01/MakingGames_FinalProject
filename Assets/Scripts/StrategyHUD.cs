using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StrategyHUD : MonoBehaviour
{
    [HideInInspector] public BuildingController strategyController;

    public BuildingList buildings;
    public float offset = -10;

    private List<GameObject> itemList = new List<GameObject>();

    GameManager gameManager;

    [Header("References")]
    public GameObject initialItem;
    public GameObject scrollPanel;


    private void Start()
    {
        gameManager = GameManager.instance;
        strategyController = gameManager.buildingController;

        // Setup menu for selecting buildings
        //print(initialItem);
        //print(buildings[0]);

        SetElementValues(initialItem, buildings[0]);

        initialItem.GetComponentInChildren<Button>().onClick.AddListener(delegate { SelectPrefab(0); });
        itemList.Add(initialItem);
        int items = 1;

        for (int i = 1; i < buildings.Count; i++)
        {
            GameObject newItem = Instantiate(initialItem);
            itemList.Add(newItem);
            newItem.transform.parent = initialItem.transform.parent;
            items++;

            SetElementPosition(newItem, i);
            SetElementValues(newItem, buildings[i]);

            int index = i;

            newItem.GetComponentInChildren<Button>().onClick.AddListener(delegate { SelectPrefab(index); });
        }

        RectTransform panelRect = scrollPanel.GetComponent<RectTransform>();
        panelRect.position = panelRect.position * new Vector2(1, 0);
    }
    public void UpdateAlfa()
    {
        float alfa = 1;
        int i = 0;
        foreach (GameObject GO in itemList)
        {
            Image[] images = new Image[0];
            images = GO.GetComponentsInChildren<Image>();

            if (gameManager.resource < buildings[i].cost)
            {
                alfa = 0.2f;
            }
            else
            {
                alfa = 1;
            }
            foreach (Image image in images)
            {
                if (!image.name.Equals("Button"))
                {
                    Color tempColor = image.color;
                    tempColor.a = alfa;
                    image.color = tempColor;
                }
            }
            i++;
        }
    }

    private void SetElementPosition(GameObject item, int indexOfItem)
    {
        RectTransform rectT = item.GetComponent<RectTransform>();
        RectTransform oldRectT = initialItem.GetComponent<RectTransform>();
        rectT.localScale = oldRectT.localScale;
        rectT.position = oldRectT.position -
            rectT.rect.height * rectT.lossyScale.y * indexOfItem * new Vector3(0, 1, 0) -
            rectT.rect.height * rectT.lossyScale.y * indexOfItem * offset * new Vector3(0, 1, 0);
    }

    private void SetElementValues(GameObject item, BuildingType type)
    {
        UIBuildingItem UIItem = item.GetComponent<UIBuildingItem>();

        UIItem.Type = type;
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