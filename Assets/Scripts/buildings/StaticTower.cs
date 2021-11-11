using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticTower : MonoBehaviour, IActor
{

    public GameObject peak;
    public GameObject projectile;
    public Image healthbar;
    public List<GameObject> enemies;
    public float distanceToClosestEnemy;
    public GameObject closestEnemy;
    public CollisionObserver detectionCollision;
    public float shootCooldown;
    private float damage;
    public float maxHealth = 100;
    private float currentHealth;
    public float Health { get => currentHealth; 
            set
            {
                currentHealth = value;
                StartCoroutine(SmoothSliderDecrease(currentHealth / maxHealth, healthbar));
                if (currentHealth <= 0f)
                {
                    Die();
                }
            }
        }
    private float speed = 3.0f;
    public float Speed { get => speed; set { speed = value;}}
    public ActorType type => ActorType.Obstacle;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
        
        detectionCollision.Subscribe(Detection_Enter, CollisionObserver.CollisionType.Enter);
        detectionCollision.Subscribe(Detection_Stay, CollisionObserver.CollisionType.Stay);
        detectionCollision.Subscribe(Detection_Exit, CollisionObserver.CollisionType.Exit);

        distanceToClosestEnemy = -1;

        damage = 25;
    }

    // Update is called once per frame
    void Update()
    {
        shootCooldown -= Time.deltaTime;
    }

    void shoot()
    {
        shootCooldown = 1.0f;
        GameObject newProjectile = Instantiate(projectile) as GameObject;
        newProjectile.transform.position = peak.transform.position;
        newProjectile.GetComponent<ProjectileScript>().setDamage(damage);
        newProjectile.GetComponent<ProjectileScript>().setTarget(closestEnemy);
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

        if(shootCooldown < 0 && distanceToClosestEnemy > 0 && closestEnemy != null)
        {
            shoot();
        }
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

        if(enemies.Count == 1)
        {
            closestEnemy = enemies[0];
            distanceToClosestEnemy = Vector3.Distance(enemies[0].transform.position, this.transform.position);
        } else {
            foreach(GameObject enemy in enemies)
            {
                if(enemy == null)
                {
                    enemies.Remove(enemy);
                }

                float distance = Vector3.Distance(enemy.transform.position, this.transform.position);
                
                if(distanceToClosestEnemy == -1)
                {
                    distanceToClosestEnemy = distance;
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

    IEnumerator SmoothSliderDecrease(float amount, Image image)
    {
        if (image != null)
            while (image.fillAmount > amount)
        {
            if(image != null)
            image.fillAmount -= 0.01f;

            yield return new WaitForSeconds(0.000001f);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
