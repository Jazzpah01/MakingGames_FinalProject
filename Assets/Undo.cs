using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undo : MonoBehaviour
{
    public GameObject button;
    GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        if (gameManager.waveBuildingList.Count > 0)
        {
            button.SetActive(true);
        }
        else
        {
            button.SetActive(false);
        }
    }
    public void UndoBuilding()
    {
        gameManager.buildingController.Undo();
    }
}
