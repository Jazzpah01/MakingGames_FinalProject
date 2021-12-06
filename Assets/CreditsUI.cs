using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    public void BackButton()
    {
        GetComponentInParent<HUD>().CreditsBackButton();
    }
}
