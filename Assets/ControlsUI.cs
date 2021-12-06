using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUI : MonoBehaviour
{
    public void BackButton()
    {
        GetComponentInParent<HUD>().ControlsBackButton();
    }
}
