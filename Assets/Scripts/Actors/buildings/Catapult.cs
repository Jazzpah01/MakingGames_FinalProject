using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Catapult : MonoBehaviour, IBuildingBehavior
{
    public GameObject peak;
    public GameObject projectile;
    public List<GameObject> enemies;
    public float distanceToClosestEnemy;
    public GameObject closestEnemy;
    public CollisionObserver detectionCollision;
    Animator animator;

    public float shootCooldown;
    private float timer;

    private Buildable buildable;
    private CatapultData data;

    // Start is called before the first frame update
    void Start()
    {
        buildable = GetComponent<Buildable>();
        data = (CatapultData)buildable.data;

        shootCooldown = data.cooldown;

        timer = 0;

        enemies = new List<GameObject>();
        
        detectionCollision.Subscribe(Detection_Enter, CollisionObserver.CollisionType.Enter);
        detectionCollision.Subscribe(Detection_Stay, CollisionObserver.CollisionType.Stay);
        detectionCollision.Subscribe(Detection_Exit, CollisionObserver.CollisionType.Exit);

        animator = transform.GetChild(3).GetChild(0).GetComponent<Animator>();

        distanceToClosestEnemy = -1;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(closestEnemy != null)
        {
            this.transform.GetChild(3).transform.LookAt(closestEnemy.transform, Vector3.up);
        }
    }

    public void shoot()
    {
        timer = shootCooldown;
        GameObject newProjectile = Instantiate(projectile) as GameObject;
        newProjectile.transform.position = peak.transform.position;
        newProjectile.GetComponent<CatapultProjectileScript>().setValues(data.damage, data.projectileSpeed);
        newProjectile.GetComponent<CatapultProjectileScript>().setTarget(closestEnemy);
    }

    public void setValues(float d, float cooldown)
    {
        shootCooldown = cooldown;
    }

    private void Detection_Enter(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        if (actor.isActorType(ActorType.Enemy))
        {
            enemies.Add(actor.gameObject);
        }
    }
    private void Detection_Stay(Collider other)
    {
        checkNearByEnemeis();
    }
    
    private void Detection_Exit(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        if (actor.isActorType(ActorType.Enemy))
        {
            enemies.Remove(actor.gameObject);
        }
    }

    public List<GameObject> getEnemies()
    {
        return enemies;
    }

    private void checkNearByEnemeis()
    {
        distanceToClosestEnemy = -1;
        closestEnemy = null;
        if(enemies.Count > 0 )
        {   
            animator.SetTrigger("Shooting"); 
        }
        if(enemies.Count == 1 && enemies[0] != null)
        {
            closestEnemy = enemies[0];
            distanceToClosestEnemy = Vector3.Distance(enemies[0].transform.position, this.transform.position);
        } else {
            foreach(GameObject enemy in enemies)
            {
                if(enemy == null)
                {
                    enemies.Remove(enemy);
                    break;
                }

                float distance = Vector3.Distance(enemy.transform.position, this.transform.position);
                
                if(distanceToClosestEnemy == -1)
                {
                    distanceToClosestEnemy = distance;
                    closestEnemy = enemy;
                } else {
                    if(distanceToClosestEnemy >= distance)
                    {
                        distanceToClosestEnemy = distance;
                        closestEnemy = enemy;
                    }
                }
            }
        }
    }
}