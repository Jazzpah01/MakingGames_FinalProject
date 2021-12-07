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
        title.text = type.name;
        description.text = type.description;
        descriptionItemImage.sprite = type.descriptionItemImage;
}
}
