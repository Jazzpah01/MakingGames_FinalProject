using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach(Enemy e in GetComponentsInChildren<Enemy>())
            {
                e.Health = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameManager.instance.gameController.baseController.Health = GameManager.instance.gameController.baseController.MaxHealth * 0.2f;
            PlayerManager.instance.playerController.Health = PlayerManager.instance.playerController.MaxHealth * 0.2f;
        }
    }
}
