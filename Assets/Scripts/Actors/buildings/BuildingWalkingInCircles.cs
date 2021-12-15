using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BuildingWalkingInCircles : MonoBehaviour, IBuildingBehavior
{

    GameManager gameManager;
    Buildable buildable;
    CircleBuildingData data;

    public CollisionObserver detectionCollision;
    public Transform center;
    public Transform grabPoint;

    private float centerDistance;
    private float speed;
    private float timer;
    private Transform grabbedEnemyOldParent, grabbedEnemy;

    private void Awake()
    {
        centerDistance = (transform.position - center.position).magnitude;
    }

    void Start()
    {
        gameManager = GameManager.instance;
        buildable = GetComponent<Buildable>();
        data = (CircleBuildingData)buildable.data;
        speed = GetComponent<Buildable>().Speed;
        detectionCollision.Subscribe(Detection_Enter, CollisionObserver.CollisionType.Enter);
        detectionCollision.Subscribe(Detection_Exit, CollisionObserver.CollisionType.Exit);
    }

    void Update()
    {
        //Grab timer
        timer -= Time.deltaTime;
        if (grabbedEnemy!=null)
        {
            Debug.Log("Timer: " + timer + " Parent: " + grabbedEnemy.parent.name + " agentstopped: " + grabbedEnemy.GetComponent<NavMeshAgent>().isStopped);
        }
        if (gameManager.gameController.state == GameController.GameState.Combat)
        {
            Vector3 centerOut = (transform.position - center.position).normalized;

            transform.Translate(Vector3.forward * (Time.deltaTime * speed));
            transform.position = center.position + centerOut * centerDistance;

            transform.forward = new Vector3(centerOut.z, 0, -centerOut.x);
        }
        //let go of the enemy when the timer expires
        if (timer <= 0 && grabbedEnemy != null)
        {
            LetEmGo();
        }
    }

    private void GrabEm(Transform enemy)
    {
        grabbedEnemy = enemy;
        grabbedEnemy.GetComponent<NavMeshAgent>().isStopped = true;
        //TODO: stop the AI and maybe also animation, depends on impelmentation
        grabbedEnemyOldParent = grabbedEnemy.parent;
        grabbedEnemy.position = grabPoint.position;
        grabbedEnemy.parent = grabPoint;
        timer = data.grabbedTime;
    }

    private void LetEmGo()
    {
        grabbedEnemy.parent = grabbedEnemyOldParent;
        //TODO: start the AI and maybe also animation, depends on impelmentation
        grabbedEnemy.GetComponent<NavMeshAgent>().isStopped = false;
        grabbedEnemy = null;
    }

    private void Detection_Enter(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        if (actor.isActorType(ActorType.Enemy))
        {
            if (grabbedEnemy != null)
            {
                LetEmGo();
            }
            GrabEm(actor.gameObject.transform);

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
