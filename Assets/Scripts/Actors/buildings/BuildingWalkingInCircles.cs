using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingWalkingInCircles : MonoBehaviour, IBuildingBehavior
{
    double StartPos;

    private float damage;
    public CollisionObserver detectionCollision;
    public CollisionObserver damagerCollision;

    public Transform center;
    private float centerDistance;

    private float currentHealth;
    private float rotation = 40;
    private float speed;

    GameManager gameManager;

    private Buildable buildable;
    private CircleBuildingData data;

    private void Awake()
    {
        centerDistance = (transform.position - center.position).magnitude;
    }

    void Start()
    {
        buildable = GetComponent<Buildable>();
        data = (CircleBuildingData)buildable.data;

        gameManager = GameManager.instance;
        speed = GetComponent<Buildable>().Speed;
        StartPos = transform.position.x;
        detectionCollision.Subscribe(Detection_Enter, CollisionObserver.CollisionType.Enter);
        detectionCollision.Subscribe(Detection_Exit, CollisionObserver.CollisionType.Exit);
    }

    void Update()
    {
        if (gameManager.gameController.state == GameController.GameState.Combat)
        {
            transform.Translate(Vector3.forward * (Time.deltaTime * speed));
            transform.Rotate(0.0f, rotation * Time.deltaTime, 0.0f, Space.Self);
        }

        transform.position = center.position + (transform.position - center.position).normalized * centerDistance;
    }

    private void Detection_Enter(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        if (actor.isActorType(ActorType.Enemy))
        {
            actor.Health -= data.damage;
        }
    }

    private void Detection_Exit(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;
    }
}
