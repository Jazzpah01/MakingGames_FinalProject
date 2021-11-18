using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingDescription : MonoBehaviour
{
    private string newDescriptionTitle;
    private TextMeshProUGUI title;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getTowerTitle(TextMeshProUGUI descriptionTitle)
    {
        newDescriptionTitle = descriptionTitle.GetComponent<TMP_Text>().text;
    }

    public void UpdateTitle(TextMeshProUGUI descriptionTitle)
    {
        descriptionTitle.text = newDescriptionTitle;
    }
}