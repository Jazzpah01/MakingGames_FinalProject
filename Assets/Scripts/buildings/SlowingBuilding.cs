using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowingBuilding : MonoBehaviour
{

    float walktimer = 5;
    float gastimer = 0.2f;
    float yRotation;
    Vector3 gasPosition;

    public GameObject slowGas;

    private GameManager gm;
    private float speed;

    void Start()
    {
        speed = GetComponent<Buildable>().Speed;
        yRotation = transform.rotation.y;
        gm = GameManager.instance;
    }

    void FixedUpdate()
    {
        gasPosition = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);

        gastimer -= Time.deltaTime;
        if (walktimer > 0 && gm.inBattle)
        {
            walktimer -= Time.deltaTime;
            transform.Translate(Vector3.forward * (Time.deltaTime * speed));
        } else if (gm.inBattle) {
            TurnAround();
        }

        if(gastimer < 0 && gm.inBattle)
        {
            Instantiate(slowGas, gasPosition, Quaternion.identity);
            gastimer = 0.2f;
        }
    }

    void TurnAround(){

        transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
        yRotation = transform.rotation.y;
        walktimer = 5;
    }
}
