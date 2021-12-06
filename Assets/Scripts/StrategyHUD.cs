using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StrategyHUD : MonoBehaviour
{
    GameManager gameManager;

    public float offset = -10;
    public float contentChunkSize = 400;
    public float itemSelectedScale;

    [Header("References")]
    public GameObject initialItemPrefab;
    public GameObject contentGO;
    public Scrollbar scrollbar;
    public float scrollStart, scrollEnd;
    public GameObject defendButton;
    public GameObject menuButton;
    public float itemScaleOnToggle = 1;
    public BuildingDescription description;
    public GameObject pauseMenu;

    private List<GameObject> itemList;
    private InteractableUI toggled;
    private BuildingController strategyController;
    private BuildingList buildings;
    private float resource;
    private RectTransform contentRT;

    private void Start()
    {
        contentRT = contentGO.GetComponent<RectTransform>();
        itemList = new List<GameObject>();
        gameManager = GameManager.instance;
        strategyController = gameManager.buildingController;
        buildings = gameManager.buildingTypes;
        pauseMenu.SetActive(false);


        // Setup buttons
        defendButton.GetComponentInChildren<InteractableUI>().OnClicked += delegate { DefendButton(); };
        menuButton.GetComponentInChildren<InteractableUI>().OnClicked += delegate { PauseButton(); };
        for (int i = 0; i < buildings.Count; i++)
        {
            //instantiate the item
            GameObject newItem = Instantiate(initialItemPrefab, contentGO.transform);

            //set the building type
            newItem.GetComponent<UIBuildingItem>().Type = buildings[i];

            //set the position in the list
            RectTransform RT = newItem.GetComponent<RectTransform>();
            Vector3 v = RT.position;
            //RT.position = v + new Vector3(0, i * (RT.rect.height + offset) * -1, 0);

            //set the button effects
            int index = i;
            InteractableUI IUI = newItem.GetComponentInChildren<InteractableUI>();
            IUI.OnClicked += delegate { ButtonPressed(index); };
            IUI.OnEnter += delegate { HighlightButton(index); };
            IUI.OnExit += delegate { UnHighlightButton(); };

            //add the item to itemList
            itemList.Add(newItem);
        }
    }


    private void Update()
    {
        if (!gameManager.buildingController.isBuilding && toggled != null)
        {
            SetToggled(false);
        }
        UpdateAlfa();

        Vector3 v = contentRT.transform.position;
        //TODO: find the best values here
        float scrollSize = 90*itemList.Count;
        contentRT.transform.position = new Vector3(v.x,(scrollSize + (scrollSize*scrollbar.value)*-1) + 500, v.z);
    }
    
    public void UpdateAlfa()
    {
        if (gameManager.currentResource != resource)
        {
            resource = gameManager.currentResource;
            foreach (GameObject GO in itemList)
            {
                float cost = GO.GetComponentInChildren<UIBuildingItem>().Type.cost;

                if (gameManager.currentResource < cost)
                {
                    GO.GetComponentInChildren<InteractableUI>().Interactable = false;
                }
                else
                {
                    GO.GetComponentInChildren<InteractableUI>().Interactable = true;
                }
            }
        }
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
            SetToggled(false);
        }

        toggled = itemList[index].GetComponentInChildren<InteractableUI>();
        SetToggled(true);
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
        if (toggled != null)
        {
            SetToggled(false);
        }
        GameController.instance.GoToBattle();
    }
    public void PauseButton()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    public void SetToggled(bool b)
    {
        if (toggled == null)
            return;

        if (toggled.Toggled && !b)
        {
            toggled.mainGameObject.transform.localScale -= new Vector3(itemScaleOnToggle, itemScaleOnToggle, itemScaleOnToggle);
            toggled.Toggled = b;
            toggled = null;
        } else if (!toggled.Toggled && b)
        {
            toggled.mainGameObject.transform.localScale += new Vector3(itemScaleOnToggle, itemScaleOnToggle, itemScaleOnToggle);
            toggled.Toggled = b;
        } else
        {
            toggled.Toggled = b;
        }
    }
}