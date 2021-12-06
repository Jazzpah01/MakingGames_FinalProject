using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    public void BackButton()
    {
        GetComponentInParent<HUD>().SettingsBackButton();
    }
}
