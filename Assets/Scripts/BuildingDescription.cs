using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingDescription : MonoBehaviour
{
    private string newDescriptionTitle;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void getTowerTitle(TextMeshProUGUI descriptionTitle)
    //{
    //    newDescriptionTitle = descriptionTitle.GetComponent<TMP_Text>().text;
    //}
    //
    //public void UpdateTitle(TextMeshProUGUI descriptionTitle)
    //{
    //    descriptionTitle.text = newDescriptionTitle;
    //}

    public void UpdateItemDescription(BuildingType type)
    {
        title.text = type.name;
        description.text = type.description;
    }
}
