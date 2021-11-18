using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController: MonoBehaviour, IEnemy
{
    public Transform primaryTarget;
    public HealthBar healthBar;
    public CollisionObserver detectionCollision;
    public CollisionObserver damagerCollision;
    public GameObject deathEffect;

    private Transform secondaryTarget;
    private NavMeshAgent agent;
    private Material material;

    private float time1 = 0, time2 = 0;

    public float maxHealth;
    private float health = 100;

    public ActorType type => ActorType.Enemy;

    public float Speed { get => agent.speed;
        set {
            agent.speed = value;
        }}

    public float MaxHealth => maxHealth;
    public float Health { get => health; 
        set {
            health = value;
            //TODO: implement hiteffects
            //PlayHitEffects();
            healthBar.SetHealthbar(health/maxHealth);
            if (health <= 0)
            {
                Destroy(this.gameObject);
                Instantiate(deathEffect).transform.position = transform.position;
            }
        } 
    }

    public int typeIdentifyer { set; get; }
    public float value { get; set; }
    public EnemyType enemyType { get; set; }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        detectionCollision.Subscribe(Detection_Enter, CollisionObserver.CollisionType.Enter);
        detectionCollision.Subscribe(Detection_Exit, CollisionObserver.CollisionType.Exit);
        material = GetComponent<Material>();
        healthBar.SetHealthImageColour(Color.red);
    }

    private void Update()
    {
        time1 += Time.deltaTime;
        if (secondaryTarget != null)
        {
            FaceTarget(secondaryTarget);
            FollowTarget(secondaryTarget);

            Collider col = secondaryTarget.GetComponent<Collider>();

            IActor target = secondaryTarget.GetComponent<IActor>();

            if ((target.type == ActorType.Obstacle || target.type == ActorType.Player) && damagerCollision.Stay.Contains(col) && time1 >= time2)
            {
                target.Health -= 10;
                time2 = time1 + 1;
            }

        }else if (primaryTarget != null)
        {
            FaceTarget(primaryTarget);
            FollowTarget(primaryTarget);
        }
        else
        {
            StopFollowingTarget();
        }
    }
        IEnumerator PlayHitEffects()
    {
        Material oldMaterial = material;
        material.color = Color.white;
        yield return null;
    }


    public void FollowTarget(Transform target)
    {
        agent.SetDestination(target.position);
        agent.stoppingDistance = 0.8f;
        agent.updateRotation = false;
        agent.isStopped = false;
    }

    public void StopFollowingTarget()
    {
        agent.isStopped = true;
        agent.stoppingDistance = 0f;
        agent.updateRotation = true;
    }

    private void Detection_Enter(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        switch (actor.type)
        {
            case ActorType.Player:
                secondaryTarget = other.transform;
                break;
            case ActorType.Obstacle:
                if (secondaryTarget == null)
                {
                    secondaryTarget = other.transform;
                }
                break;
        }
    }

    private void Detection_Exit(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        switch (actor.type)
        {
            case ActorType.Player:
                secondaryTarget = null;
                break;
            case ActorType.Obstacle:
                secondaryTarget = null;
                break;
        }
    }

    private void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
