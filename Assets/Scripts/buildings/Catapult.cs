using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Catapult : MonoBehaviour
{
    public GameObject peak;
    public GameObject projectile;
    public List<GameObject> enemies;
    public float distanceToClosestEnemy;
    public GameObject closestEnemy;
    public CollisionObserver detectionCollision;
    Animator animator;

    public float shootCooldown;
    public float damage = 25;
    public float maxHealth = 100;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;

        enemies = new List<GameObject>();
        
        detectionCollision.Subscribe(Detection_Enter, CollisionObserver.CollisionType.Enter);
        detectionCollision.Subscribe(Detection_Stay, CollisionObserver.CollisionType.Stay);
        detectionCollision.Subscribe(Detection_Exit, CollisionObserver.CollisionType.Exit);

        
        animator = transform.GetChild(3).GetComponent<Animator>();

        distanceToClosestEnemy = -1;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    public void shoot()
    {            
        this.transform.GetChild(3).transform.LookAt(closestEnemy.transform, Vector3.up);

        timer = 0.0f;
        GameObject newProjectile = Instantiate(projectile) as GameObject;
        newProjectile.transform.position = peak.transform.position;
        newProjectile.GetComponent<CatapultProjectileScript>().setDamage(damage);
        newProjectile.GetComponent<CatapultProjectileScript>().setTarget(closestEnemy);
    }

    public void setDamage(float d)
    {
        damage = d;
    }

    private void Detection_Enter(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        switch (actor.type)
        {
            case ActorType.Enemy:
                enemies.Add(actor.gameObject);
                break;
        }
    }
    private void Detection_Stay(Collider other)
    {
        checkNearByEnemeis();

       /* if(timer > shootCooldown && distanceToClosestEnemy > 0 && closestEnemy != null)
        {
            //shoot();
        }*/
    }
    private void Detection_Exit(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        switch (actor.type)
        {
            case ActorType.Enemy:
                enemies.Remove(actor.gameObject);
                break;
        }
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
