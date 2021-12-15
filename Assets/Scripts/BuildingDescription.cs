using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingDescription : MonoBehaviour
{
    private string newDescriptionTitle;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image descriptionItemImage;
    public void UpdateItemDescription(BuildingType type)
    {
        title.text = type.data.name;
        description.text = type.data.description;
        descriptionItemImage.sprite = type.data.descriptionItemImage;
    }
}
