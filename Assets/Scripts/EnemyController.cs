using UnityEngine;
using UnityEngine.AI;


public class EnemyController: MonoBehaviour, IActor
{

    public float speed = 1;
    public Transform primaryTarget;
    private Transform secondaryTarget;
    private NavMeshAgent agent;

    private float time1 = 0, time2 = 0;

    private float maxHealth = 100, health = 100;

    public ActorType type => ActorType.Enemy;

    public float Health { get => health; 
        set {
            health = value;
            if (health <= 0)
            {
                Destroy(this.gameObject);
            }
        } 
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        time1 += Time.deltaTime;
        if (secondaryTarget != null)
        {
            FaceTarget(secondaryTarget);
            FollowTarget(secondaryTarget);

            IActor target = secondaryTarget.GetComponent<IActor>();

            if (target.type == ActorType.Obstacle && Vector3.Distance(transform.position, secondaryTarget.position) < 2 && time1 >= time2)
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

    public void FollowTarget(Transform target)
    {
        agent.SetDestination(target.position);
        agent.speed = speed;
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

    private void OnTriggerEnter(Collider other)
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

    private void OnTriggerExit(Collider other)
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
