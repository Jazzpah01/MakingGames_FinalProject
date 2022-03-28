using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public IActor parent;
    public GameObject Target;   
    public CollisionObserver detectionCollision;
    public CollisionObserver damagerCollision;

    private float damage;
    private float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        detectionCollision.Subscribe(Detection_Enter, CollisionObserver.CollisionType.Enter);
        detectionCollision.Subscribe(Detection_Exit, CollisionObserver.CollisionType.Exit);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Target.gameObject == null)
        {
            Destroy(this.gameObject);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, speed * Time.fixedDeltaTime);
        }
    }

    private void Detection_Enter(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        if(actor.actorType == ActorType.Enemy && actor.gameObject == Target.gameObject)
        {
            actor.Health -= damage;
            if (GameEvents.DamageDealt != null)
            {
                GameEvents.DamageDealt((parent, actor, damage));
            }
            Destroy(this.gameObject);
        }
    }

    private void Detection_Exit(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;
    }

    public void setValue(IActor parent, float damage, float projectileSpeed)
    {
        this.parent = parent;
        this.damage = damage;
        this.speed = projectileSpeed;
    }
    public void setTarget(GameObject t)
    {
        Target = t;
    }
}
