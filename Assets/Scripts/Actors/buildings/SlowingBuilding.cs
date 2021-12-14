using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowingBuilding : MonoBehaviour, IBuildingBehavior
{
    public float walkDistance = 13;
    float walktimer = 5;
    float gastimer = 0.2f;
    float yRotation;
    Vector3 gasPosition;

    public GameObject slowGas;
    public Animator animator;
    public GameObject model;

    private GameManager gm;

    private Buildable buildable;
    private LineSlowerData data;

    void Start()
    {
        buildable = GetComponent<Buildable>();
        data = (LineSlowerData)buildable.data;

        yRotation = model.transform.rotation.y;
        gm = GameManager.instance;

        walktimer = walkDistance / data.speed;
    }

    void FixedUpdate()
    {
        gasPosition = new Vector3(model.transform.position.x, model.transform.position.y - 0.5f, model.transform.position.z);

        gastimer -= Time.deltaTime;
        if (walktimer > 0 && gm.gameController.state == GameController.GameState.Combat)
        {
            animator.SetTrigger("Walking");
            walktimer -= Time.deltaTime;
            model.transform.Translate(Vector3.forward * (Time.deltaTime * data.speed));
        } else if (gm.gameController.state == GameController.GameState.Combat) {
            TurnAround();
        }

        if(gastimer < 0 && gm.gameController.state == GameController.GameState.Combat)
        {
            GameObject go = Instantiate(slowGas, gasPosition, Quaternion.identity);
            go.GetComponent<SlowGas>().setValues(data.speedModifyer, data.slowObjectLifeTime);
            gastimer = data.slowObjectEveryXSecond;
        }
    }

    void TurnAround(){

        model.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
        yRotation = model.transform.rotation.y;

        walktimer = walkDistance / data.speed;
    }
}
