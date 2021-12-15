using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBuildingItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI cost;
    public Image icon;

    private BuildingType type;
    public BuildingType Type { get => type;
        set
        {
            type = value;

            name.text = type.data.name;
            cost.text = type.cost.ToString();
            icon.sprite = type.data.icon;
        }
    }

}