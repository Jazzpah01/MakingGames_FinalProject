using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StrategyHUD : MonoBehaviour
{
    GameManager gameManager;
    GameController gameController;

    public float itemScaleOnToggle;

    [Header("References")]
    public GameObject initialItemPrefab;
    public GameObject contentGO;
    public Scrollbar scrollbar;
    public float scrollStart, scrollEnd;
    public GameObject defendButton;
    public GameObject menuButton;
    public GameObject scrollUpButton;
    public GameObject scrollDownButton;
    public BuildingDescription description;
    public TextMeshProUGUI waveCounter;

    private List<GameObject> itemList;
    private InteractableUI toggled;
    private BuildingController strategyController;
    private BuildingList buildings;
    private float resource;
    private RectTransform contentRT;
    private float itemScrollSize;

    private void Start()
    {
        contentRT = contentGO.GetComponent<RectTransform>();
        itemList = new List<GameObject>();
        gameManager = GameManager.instance;
        gameController = GameController.instance;
        strategyController = gameManager.buildingController;
        buildings = gameManager.buildingTypes;


        // Setup buttons
        defendButton.GetComponentInChildren<InteractableUI>().OnClicked += delegate { DefendButton(); };

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
        //total height of the instantiated list
        itemScrollSize = (initialItemPrefab.GetComponent<RectTransform>().rect.height + contentGO.GetComponent<VerticalLayoutGroup>().spacing) * itemList.Count;
    }

    private void Update()
    {
        if (!gameManager.buildingController.isBuilding && toggled != null)
        {
            SetToggled(false);
        }
        UpdateAlfa();

        if (GetComponentInParent<HUD>().mouseOver)
        {
           Scroll(Input.GetAxis("Mouse ScrollWheel"));
        }
        Vector3 v = contentRT.transform.position;

        //first constant adjusts the length of the list, higher = more whitespace at the bottom
        //second constant adjusts the start position, lower = more whitespace at the top
        float y = (itemScrollSize + (itemScrollSize * scrollbar.value) * -1) * 0.5f + Screen.height * 0.45f;
        contentRT.transform.position = new Vector3(v.x, y, v.z);

        
        //wave text update
        if((gameController.currentWave + 1) > FindObjectOfType<SpawnController>().waves.Count)
        {
            waveCounter.text = "Level finished";
        } else {
            waveCounter.text = "Wave " + (gameController.currentWave + 1).ToString() + " of " + FindObjectOfType<SpawnController>().waves.Count.ToString();
        }
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

        SetToggled(false);
        
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

    public void MenuButton()
    {
        GetComponentInParent<HUD>().IngameMenuButton();
    }

    public void SetToggled(bool b)
    {
        if (toggled == null)
            return;

        if (!b)
        {
            toggled.mainGameObject.transform.localScale /= itemScaleOnToggle;
            toggled.Toggled = b;
            toggled = null;
        }
        else
        {
            toggled.mainGameObject.transform.localScale *= itemScaleOnToggle;
            toggled.Toggled = b;
        }
    }
    public void ScrollUp()
    {
        Scroll(0.33f);
    }
    public void ScrollDown()
    {
        Scroll(-0.33f);
    }

    public void Scroll(float value)
    {
        scrollbar.value += value;
        if (scrollbar.value < 0)
        {
            scrollbar.value = 0;
        }
        if (scrollbar.value > 1)
        {
            scrollbar.value = 1;
        }
    }
}