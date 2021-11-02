using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float maxVelocityChange = 10.0f;
    public LayerMask movementMask;
    public Interactable focus;

    Camera cam;
    PlayerMotor motor;
    PlayerManager playerManager;
    NavMeshAgent navMeshAgent;

    private Vector3 velocity;
    private float directionX,directionY;


    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
        playerManager = PlayerManager.instance;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        //left click
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                if (!playerManager.keyboardControl)
                {
                    motor.MoveToPoint(hit.point);
                }
                RemoveFocus();
            }
        }
        //right click
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    SetFocus(interactable);
                }

            }
        }
        //keyboard movement
        if (playerManager.keyboardControl)
        {
            Vector3 targetVelocity = new Vector3(directionX, directionY, 0);
            //targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= navMeshAgent.speed;

            velocity = navMeshAgent.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            navMeshAgent.velocity = velocityChange;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                directionX = -1;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                directionX = 1;
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                directionY = 1;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                directionY = -1;
            }
            else
            {
                directionX = 0;
                directionY = 0;
            }
        }
    }

    private void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
            {
                focus.OnDefocused();
            }
            focus = newFocus;
            motor.FollowTarget(newFocus);
        }
        newFocus.OnFocused(transform);
    }

    private void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }
        focus = null;
        motor.StopFollowingTarget();
    }

}
