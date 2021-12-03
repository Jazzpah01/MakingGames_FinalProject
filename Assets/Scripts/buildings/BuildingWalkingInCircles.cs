using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingWalkingInCircles : MonoBehaviour, IBuildingBehavior
{
    double StartPos;

    public float damage;
    public CollisionObserver detectionCollision;
    public CollisionObserver damagerCollision;

    private float currentHealth;
    private float rotation = 0.7f;
    private float speed;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
        speed = GetComponent<Buildable>().Speed;
        StartPos = transform.position.x;
        detectionCollision.Subscribe(Detection_Enter, CollisionObserver.CollisionType.Enter);
        detectionCollision.Subscribe(Detection_Exit, CollisionObserver.CollisionType.Exit);
    }

    void FixedUpdate()
    {
        if (gameManager.gameController.state == GameController.GameState.Combat)
        {
        transform.Translate(Vector3.forward * (Time.deltaTime * speed));
        transform.Rotate(0.0f, rotation, 0.0f, Space.Self);
        }
    }

    private void Detection_Enter(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        if (actor.isActorType(ActorType.Enemy))
        {
            actor.Health -= damage;
        }
    }

    private void Detection_Exit(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;
    }
}
