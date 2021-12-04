using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StrategyHUD : MonoBehaviour
{
    [HideInInspector] public BuildingController strategyController;

    private BuildingList buildings;
    public float offset = -10;
    public float contentChunkSize = 400;
    public float buildableSelectedScale;

    private List<GameObject> itemList = new List<GameObject>();
    private InteractableUI toggled;

    GameManager gameManager;

    [Header("References")]
    public GameObject initialItemPrefab;
    public GameObject scrollPanel;
    public GameObject defendButton;
    public BuildingDescription description;

    private GameObject initialItem;

    private void Start()
    {
        initialItem = initialItemPrefab;
        gameManager = GameManager.instance;
        strategyController = gameManager.buildingController;
        buildings = gameManager.buildingTypes;

        // Setup buttons
        defendButton.GetComponentInChildren<InteractableUI>().OnClicked += delegate { DefendButton(); };

        //
        //Instantiate

        // Setup menu for selecting buildings
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
        panelRect.sizeDelta = new Vector2(0, contentChunkSize * items);
    }
    public void UpdateAlfa()
    {
        float alfa = 1;
        int i = 0;
        foreach (GameObject GO in itemList)
        {
            float cost = GO.GetComponentInChildren<UIBuildingItem>().Type.cost;

            if (gameManager.resource < cost)
            {
                GO.GetComponentInChildren<InteractableUI>().Interactable = false;
            } else
            {
                GO.GetComponentInChildren<InteractableUI>().Interactable = true;
            }
        }
    }

    private void Update()
    {
        if (!gameManager.buildingController.isBuilding && toggled != null)
        {
            toggled.mainGameObject.transform.localScale -= new Vector3(buildableSelectedScale, buildableSelectedScale, buildableSelectedScale);
            toggled.Toggled = false;
            toggled = null;
        }
        UpdateAlfa();
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

        if (toggled != null)
        {
            toggled.Toggled = false;
            toggled.mainGameObject.transform.localScale -= new Vector3(buildableSelectedScale, buildableSelectedScale, buildableSelectedScale);

        }

        toggled = itemList[index].GetComponentInChildren<InteractableUI>();
        toggled.mainGameObject.transform.localScale += new Vector3(buildableSelectedScale, buildableSelectedScale, buildableSelectedScale);
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

    public void DefendButton()
    {
        toggled.mainGameObject.transform.localScale -= new Vector3(buildableSelectedScale, buildableSelectedScale, buildableSelectedScale);
        GameController.instance.GoToBattle();
    }
}