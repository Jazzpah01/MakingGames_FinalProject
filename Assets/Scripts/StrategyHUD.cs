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
    private InteractableUI toggled;

    GameManager gameManager;

    [Header("References")]
    public GameObject initialItem;
    public GameObject scrollPanel;
    public BuildingDescription description;


    private void Start()
    {
        gameManager = GameManager.instance;
        strategyController = gameManager.buildingController;

        // Setup menu for selecting buildings
        //print(initialItem);
        //print(buildings[0]);

        SetElementValues(initialItem, buildings[0]);

        initialItem.GetComponentInChildren<InteractableUI>().OnClicked += delegate { ButtonPressed(0); };
        initialItem.GetComponentInChildren<InteractableUI>().OnEnter += delegate { HighlightButton(0); };
        initialItem.GetComponentInChildren<InteractableUI>().OnExit += delegate { UnHighlightButton(); };

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

            newItem.GetComponentInChildren<InteractableUI>().OnClicked += delegate { ButtonPressed(index); };
            newItem.GetComponentInChildren<InteractableUI>().OnEnter += delegate { HighlightButton(index); };
            newItem.GetComponentInChildren<InteractableUI>().OnExit += delegate { UnHighlightButton(); };
        }

        RectTransform panelRect = scrollPanel.GetComponent<RectTransform>();
        panelRect.position = panelRect.position * new Vector2(1, 0);
    }
    public void UpdateAlfa()
    {
        //float alfa = 1;
        //int i = 0;
        //foreach (GameObject GO in itemList)
        //{
        //    Image[] images = new Image[0];
        //    images = GO.GetComponentsInChildren<Image>();
        //
        //    if (gameManager.resource < buildings[i].cost)
        //    {
        //        alfa = 0.2f;
        //    }
        //    else
        //    {
        //        alfa = 1;
        //    }
        //    foreach (Image image in images)
        //    {
        //        if (!image.name.Equals("Button"))
        //        {
        //            Color tempColor = image.color;
        //            tempColor.a = alfa;
        //            image.color = tempColor;
        //        }
        //    }
        //    i++;
        //}
    }

    private void Update()
    {
        //if (GameManager.instance.buildingController == null && toggled != null)
        //{
        //    toggled.Toggled = false;
        //    toggled = null;
        //}

        //foreach (GameObject go in itemList)
        //{
        //
        //}
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
    public void ButtonPressed(int index)
    {
        strategyController.SelectPrefab(buildings[index]);

        toggled = itemList[index].GetComponentInChildren<InteractableUI>();
        toggled.Toggled = true;
    }

    public void HighlightButton(int index)
    {
        description.UpdateItemDescription(buildings[index]);
        description.gameObject.SetActive(true);
    }

    public void UnHighlightButton()
    {
        description.gameObject.SetActive(false);
    }
}