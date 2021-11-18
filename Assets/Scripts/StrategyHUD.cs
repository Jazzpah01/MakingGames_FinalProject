using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StrategyHUD : MonoBehaviour
{
    [HideInInspector] public StrategyController strategyController;
    public GameObject initialItem;

    public BuildingList buildings;

    public GameObject itemScrollView;

    public GameObject scrollPanel;

    public float offset = -10;

    private List<GameObject> itemList = new List<GameObject>();

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        strategyController = GameController.instance.strategyController;

        // Setup menu for selecting buildings
        initialItem.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Cost: " + buildings[0].cost.ToString();
        initialItem.GetComponentsInChildren<TextMeshProUGUI>()[1].text = buildings[0].prefab.name;

        initialItem.GetComponentInChildren<Button>().onClick.AddListener(delegate { SelectPrefab(0); });
        itemList.Add(initialItem);
        int items = 1;

        for (int i = 1; i < buildings.Count; i++)
        {
            GameObject newItem = Instantiate(initialItem);
            itemList.Add(newItem);
            newItem.transform.parent = initialItem.transform.parent;
            items++;

            // Change prosition
            RectTransform rectT = newItem.GetComponent<RectTransform>();
            RectTransform oldRectT = initialItem.GetComponent<RectTransform>();
            rectT.localScale = oldRectT.localScale;
//<<<<<<< HEAD
            rectT.position = oldRectT.position - 
                (Vector3)(rectT.rect.height * i * new Vector2(0,1) +
                (int)(rectT.rect.height * offset) * i * new Vector2(0,1));
//=======
//            rectT.position = oldRectT.position -
//                (Vector3)(rectT.rect.height * i * new Vector2(0, 1) +
//                offset * i * new Vector2(0, 1));
//>>>>>>> 944b7e7847ccee2d8cf8747b73e79f1a05c7fe25

            // Change values
            newItem.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Cost: " + buildings[i].cost.ToString();
            newItem.GetComponentsInChildren<TextMeshProUGUI>()[1].text = buildings[i].prefab.name;

            int index = i;

            newItem.GetComponentInChildren<Button>().onClick.AddListener(delegate { SelectPrefab(index); });
        }

        RectTransform panelRect = scrollPanel.GetComponent<RectTransform>();
        panelRect.position = panelRect.position * new Vector2(1, 0);
        //RectTransform itemRect = initialItem.GetComponent<RectTransform>();
        //RectTransform panelRect = scrollPanel.GetComponent<RectTransform>();
        //panelRect.localScale = new Vector3(panelRect.localScale.x, itemRect.localScale.y * items + offset * items, panelRect.localScale.z);
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
    /// <summary>
    /// Will set strategy controller's prefab
    /// </summary>
    /// <param name="index"></param>
    public void SelectPrefab(int index)
    {
        strategyController.SelectPrefab(buildings[index]);
    }
}