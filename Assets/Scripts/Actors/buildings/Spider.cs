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

    public List<IActor> enemies = new List<IActor>();
    
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
        if (actor.IsDestroyed())
            return;

        if (actor.isActorType(ActorType.Enemy))
        {

            enemies.Add(actor);
            actor.damageModifyer *= data.damageModifyer;
            actor.speedModifyer *= data.speedModifyer;
        }
    }

    private void Detection_Exit(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor.IsDestroyed())
            return;

        if (actor.isActorType(ActorType.Enemy))
        {
            actor.damageModifyer /= data.damageModifyer;
            actor.speedModifyer  /= data.speedModifyer;
            enemies.Remove(actor);
        }
    }

    private void Detection_Stay(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if(actor.IsDestroyed())
            return;

        if (actor.isActorType(ActorType.Enemy))
        {
            if (actor.IsDestroyed())
            {
                enemies.Remove(actor);
            }
            if (timer > data.attackCooldown)
            {
                actor.Health -= data.damage;
                timer = 0;
            }
        }
    }

    private void moveToEnemy()
    {
        while (enemies.Count > 0 && enemies[0].IsDestroyed())
        {
            enemies.Remove(enemies[0]);
        }

        if (enemies.Count > 0)
        {
            animator.SetTrigger("Walking"); 
            model.transform.LookAt(new Vector3(enemies[0].gameObject.transform.position.x, model.transform.position.y, enemies[0].gameObject.transform.position.z), Vector3.up);
            model.transform.position = Vector3.MoveTowards(model.transform.position, new Vector3(enemies[0].gameObject.transform.position.x, model.transform.position.y, enemies[0].gameObject.transform.position.z), data.speed * Time.deltaTime);

            if (timer > data.attackCooldown)
            {
                enemies[0].Health -= data.damage;
                timer = 0;
            }
        } else if (model.transform.position != this.transform.position) {
            animator.SetTrigger("Walking"); 
            model.transform.LookAt(this.transform.position);
            model.transform.position = Vector3.MoveTowards(model.transform.position, this.transform.position, data.speed * Time.fixedDeltaTime);
        }
    }
}
