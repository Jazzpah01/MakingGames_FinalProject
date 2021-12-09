using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class PlayerMotor : MonoBehaviour
{
    NavMeshAgent agent;
    PlayerManager playerManager;
    PlayerController controller;

    private PlayerData data;

    public Animator animator;

    [HideInInspector]
    Camera cam;

    public float isoAngle = -45;
    public float maxVelocityChange = 10.0f;

    private LayerMask movementMask;
    private LayerMask actorMask;
    private float directionX, directionZ;
    private bool dashing = false;
    private bool attacking = false;
    private float dashTimer;
    private float distance = 999999;

    [System.NonSerialized] public bool isMoving = false;
    [System.NonSerialized] public bool blockMoving = false;



    void Start()
    {
        playerManager = PlayerManager.instance;
        controller = playerManager.playerController;
        data = controller.data;
        agent = GetComponent<NavMeshAgent>();
        cam = playerManager.cam;
        movementMask = controller.movementMask;
        actorMask = controller.actorMask;
    }

    private void Update()
    {
        //dash cooldown
        dashTimer -= Time.deltaTime;

        isMoving = false;

        //reset direction variables so we return to standing still
        directionX = 0;
        directionZ = 0;

        //dash or move
        if (!dashing && !attacking && !GameManager.instance.hud.isLevelDescriptionActive)
        {
            if (Input.GetKeyDown(KeyCode.Space) && 0 >= dashTimer)
            {
                animator.SetTrigger("dash");
                //reset dash cooldown
                dashTimer = data.dashCooldown;
                //dash
                Dash(data.dashSpeed, data.dashLength);
            }
            else if (!blockMoving)
            {
                //read input keys
                MovementKeyInput();
                //move the player
                Move();
            }
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
        agent.updateRotation = true;

        Vector3 targetVelocity = new Vector3(directionX, 0, directionZ).normalized;
        targetVelocity *= data.speed;
        targetVelocity.y = 0;

        Quaternion rotation = Quaternion.Euler(0, isoAngle, 0);
        Matrix4x4 rotaMatrix = Matrix4x4.Rotate(rotation);

        agent.velocity = rotaMatrix.MultiplyPoint3x4(targetVelocity);

        if (targetVelocity.magnitude > 0)
        {
            isMoving = true;
        }
    }

    //attempt to dash, return false if dash fails
    public bool Dash(float dashSpeed, float DashLength)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, movementMask))
        {
            if (!dashing)
            {
                dashing = true;
                Coroutine c = StartCoroutine(DashE(hit.point, dashSpeed, DashLength));
                return true;
            }
            else
            {
                Debug.Log("Trying to dash while dash is already in progress");
                return false;
            }
        }
        else
        {
            Debug.Log("Raycast on dash failed");
            return false;
        }
    }

    private IEnumerator DashE(Vector3 dashPoint, float dashSpeed, float dashLength)
    {
        // turn the player to look at the point
        TurnTowardsTarget(dashPoint);
        // find the vector
        Vector3 moveTo = ((dashPoint - transform.position).normalized) * dashLength;
        // find the point
        Vector3 position = (moveTo + transform.position);

        float remainingDistance = dashLength;

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
            agent.velocity = moveTo * Mathf.Min(d, dashSpeed);

            yield return new WaitForSeconds(0.01f);
        }
        dashing = false;
    }

    public void TurnTowardsTarget(Vector3 target)
    {
        agent.updateRotation = false;
        transform.rotation = Quaternion.LookRotation(new Vector3(target.x, 0, target.z) - transform.position);
    }
}
