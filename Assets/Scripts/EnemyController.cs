using UnityEngine;
using UnityEngine.AI;


public class EnemyController: MonoBehaviour
{

    public float speed = 1;
    public Transform primaryTarget;

    private Transform secondaryTarget;
    private NavMeshAgent agent;

    private float time1 = 0, time2 = 0;

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

            if (secondaryTarget.name.Contains("Barricade") && Vector3.Distance(transform.position, secondaryTarget.position) < 2 && time1 >= time2)
            {
                secondaryTarget.GetComponent<Barricade>().Damage(10);
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
        if (other.name == "Player")
        {
            secondaryTarget = other.transform;
        }
        if (other.name.Contains("Barricade"))
        {
            if (secondaryTarget == null)
            {
                secondaryTarget = other.transform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            secondaryTarget = null;
        }
        if (other.name.Contains("Barricade"))
        {
            secondaryTarget = null;
        }
    }

    private void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
