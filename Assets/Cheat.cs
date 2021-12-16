using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    private bool cheatActive;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            cheatActive = true;
        }
        if (cheatActive)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                foreach (Enemy e in GetComponentsInChildren<Enemy>())
                {
                    e.Health = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                GameManager.instance.baseController.Health = GameManager.instance.baseController.MaxHealth * 0.1f;
                PlayerManager.instance.playerController.Health = PlayerManager.instance.playerController.MaxHealth * 0.1f;
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                GameManager.instance.baseController.Health = GameManager.instance.baseController.MaxHealth;
                PlayerManager.instance.playerController.Health = PlayerManager.instance.playerController.MaxHealth;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                GameManager.instance.currentResource += 10f;
            }
        }
    }
}
