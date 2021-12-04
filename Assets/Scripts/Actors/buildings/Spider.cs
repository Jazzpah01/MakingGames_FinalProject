using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour, IBuildingBehavior
{
    public GameObject model;
    public GameObject web;
    public CollisionObserver detectionCollision;
    public CollisionObserver damageCollision;
    public Animator animator;

    public List<GameObject> enemies;
    
    private float timer;

    private Buildable buildable;
    private AreaSpiderData data;

    // Start is called before the first frame update
    void Start()
    {
        buildable = GetComponent<Buildable>();
        data = (AreaSpiderData)buildable.data;

        detectionCollision.Subscribe(Detection_Enter, CollisionObserver.CollisionType.Enter);
        detectionCollision.Subscribe(Detection_Exit, CollisionObserver.CollisionType.Exit);
        damageCollision.Subscribe(Detection_Stay, CollisionObserver.CollisionType.Stay);
    }

    // Update is called once per frame
    void Update()
    {
        moveToEnemy();
        timer += Time.deltaTime;
    }

    private void Detection_Enter(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        if (actor.isActorType(ActorType.Enemy))
        {

            enemies.Add(actor.gameObject);
            actor.Speed = actor.Speed / speedReduction;
            actor.gameObject.GetComponent<IActor>().damageReduction = actor.gameObject.GetComponent<IActor>().damageReduction - damageReduction;
        }
    }

    private void Detection_Exit(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;


        if (actor.isActorType(ActorType.Enemy))
        {
            actor.Speed = actor.Speed * speedReduction;
            actor.gameObject.GetComponent<IActor>().damageReduction = actor.gameObject.GetComponent<IActor>().damageReduction + damageReduction;
            enemies.Remove(actor.gameObject);
        }
    }

    private void Detection_Stay(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if(actor == null)
            return;

        if (actor.isActorType(ActorType.Enemy))
        {
            if (timer > attackCooldown)
            {
                actor.Health = actor.Health - damage;
                timer = 0;
            }
            if (actor.Health == 0)
            {
                enemies.Remove(actor.gameObject);
            }
        }
    }

    private void moveToEnemy()
    {
        if(enemies.Count > 0 && enemies[0] != null)
        {
            animator.SetTrigger("Walking"); 
            model.transform.LookAt(new Vector3(enemies[0].transform.position.x, model.transform.position.y, enemies[0].transform.position.z), Vector3.up);
            model.transform.position = Vector3.MoveTowards(model.transform.position, new Vector3(enemies[0].transform.position.x, model.transform.position.y, enemies[0].transform.position.z), speed * Time.fixedDeltaTime);
        } else if (model.transform.position != this.transform.position) {
            animator.SetTrigger("Walking"); 
            model.transform.LookAt(this.transform.position);
            model.transform.position = Vector3.MoveTowards(model.transform.position, this.transform.position, speed * Time.fixedDeltaTime);
        }
    }
}