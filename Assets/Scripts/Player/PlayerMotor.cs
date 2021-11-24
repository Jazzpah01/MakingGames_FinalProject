using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class PlayerMotor : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    PlayerManager playerManager;

    [HideInInspector]
    Camera cam;

    public float isoAngle = -45;
    public float maxVelocityChange = 10.0f;
    public float dashCooldown;
    public float dashLength;
    public float dashSpeed;
    public float attackDashLength;

    private LayerMask movementMask;
    private LayerMask actorMask;
    private float directionX, directionZ;
    private bool dashing = false;
    private bool attacking = false;
    private float dashTimer;
    private float distance = 999999;


    void Start()
    {
        playerManager = PlayerManager.instance;
        agent = GetComponent<NavMeshAgent>();
        cam = playerManager.cam;
        movementMask = GetComponent<PlayerController>().movementMask;
        actorMask = GetComponent<PlayerController>().actorMask;
        attackDashLength *= 0.001f;
    }

    private void FixedUpdate()
    {
        dashTimer -= Time.fixedDeltaTime;

        directionX = 0;
        directionZ = 0;

        if (Input.GetKey(KeyCode.Space) && 0 >= dashTimer && !dashing && !attacking)
        {
            dashTimer += dashCooldown;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, movementMask))
            {
                Dash(hit.point, dashSpeed, dashLength);
            }

        }
        else if (!dashing && !attacking)
        {
            MovementKeyInput();
            Move();
        }
    }

    private void MovementKeyInput()
    {
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
    }
    private void Move()
    {
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
    public void Dash(Vector3 dashPoint, float dashMultiplier, float length)
    {
        dashing = true;
        StartCoroutine(DashE(dashPoint, dashSpeed, dashLength));
    }
    private IEnumerator DashE(Vector3 dashPoint, float dashMultiplier, float length)
    {
        transform.rotation = Quaternion.LookRotation(dashPoint);
        Vector3 moveTo = ((dashPoint - transform.position).normalized) * length;
        Vector3 position = (moveTo + transform.position);
        float d = Vector3.Distance(position, transform.position);
        distance = d;
        while (d > 1)
        {
            d = Vector3.Distance(position, transform.position);
            if (d > distance)
            {
                break;
            }
            distance = d;
            agent.velocity = moveTo * dashMultiplier;

            yield return new WaitForSeconds(0.01f);
        }
        dashing = false;
    }

    //private void Dash()
    //{
    //    Vector3 hitpoint = Vector3.zero;
    //    if (!dashing)
    //    {
    //        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;
    //        if (Physics.Raycast(ray, out hit, 1000, movementMask))
    //        {
    //            hitpoint = new Vector3(hit.point.x, 0, hit.point.z);
    //            dashing = true;
    //        }
    //        else
    //        {
    //            Debug.Log("Mouse not on movementmask");
    //        }

    //    }
    //    if (hitpoint != Vector3.zero)
    //    {
    //        transform.rotation = Quaternion.LookRotation(hitpoint);
    //        Vector3 direction = (hitpoint - transform.position).normalized;
    //        direction *= dashSpeed;
    //        agent.velocity = direction;
    //    }
    //    else
    //    {
    //        Debug.Log("somehting went wrong here");
    //    }

    //    if (0 >= dashTimer - dashCooldown + dashLength)
    //    {
    //        dashing = false;
    //    }
    //}
    //public void AttackDash()
    //{
    //    attackDashLength -= Time.fixedDeltaTime;
    //    Vector3 hitpoint = Vector3.zero;
    //    if (!attacking)
    //    {
    //        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;
    //        if (Physics.Raycast(ray, out hit, 1000, actorMask))
    //        {
    //            hitpoint = new Vector3(hit.point.x, 0, hit.point.z);
    //            attacking = true;
    //        }
    //    }
    //    if (hitpoint != Vector3.zero)
    //    {
    //        transform.rotation = Quaternion.LookRotation(hitpoint);
    //        Vector3 direction = (hitpoint - transform.position).normalized;
    //        direction *= dashSpeed;
    //        agent.velocity = direction;
    //    }
    //    else
    //    {
    //        Debug.Log("Something is wrong here");
    //    }
    //    if (0 >= attackDashLength)
    //    {
    //        attacking = false;
    //    }
    //}
}
