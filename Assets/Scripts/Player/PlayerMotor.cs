using UnityEngine;
using UnityEngine.AI;


public class PlayerMotor : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    PlayerManager playerManager;

    public float isoAngle = -45;
    public float maxVelocityChange = 10.0f;

    private float directionX, directionZ;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerManager = PlayerManager.instance;
    }

    private void FixedUpdate()
    {
        //click to move
        if (target != null)
        {
            agent.SetDestination(target.position);
            FaceTarget();
        }
        //keyboard movement
        if (playerManager.keyboardControl)
        {
            directionX = 0;
            directionZ = 0;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                directionZ = -1;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                directionZ = 1;
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                directionX = -1;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                directionX = 1;
            }

            Vector3 targetVelocity = new Vector3(directionX, 0, directionZ).normalized;
            targetVelocity *= agent.speed;


            Vector3 velocityChange = targetVelocity;
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;

            Quaternion rotation = Quaternion.Euler(0, isoAngle, 0);
            Matrix4x4 rotaMatrix = Matrix4x4.Rotate(rotation);

            agent.velocity = rotaMatrix.MultiplyPoint3x4(velocityChange);
        }
    }

    public void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
    }

    public void StopFollowingTarget()
    {
        agent.stoppingDistance = 0f;
        agent.updateRotation = true;
        target = null;
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
