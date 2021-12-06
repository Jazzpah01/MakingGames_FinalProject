using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticTower : MonoBehaviour, IBuildingBehavior
{
    public GameObject peak;
    public List<GameObject> enemies;
    public float distanceToClosestEnemy;
    public GameObject closestEnemy;
    public CollisionObserver detectionCollision;

    private Buildable buildable;
    private StaticTowerData data;

    private float timer;

    // Start is called before the first frame update
    void Awake()
    {
        buildable = GetComponent<Buildable>();

        data = (StaticTowerData)buildable.data;

        timer = 0;

        enemies = new List<GameObject>();
        
        detectionCollision.Subscribe(Detection_Enter, CollisionObserver.CollisionType.Enter);
        detectionCollision.Subscribe(Detection_Stay, CollisionObserver.CollisionType.Stay);
        detectionCollision.Subscribe(Detection_Exit, CollisionObserver.CollisionType.Exit);

        distanceToClosestEnemy = -1;

        GetComponentInChildren<Projector>().orthographicSize = data.range * 2 * 10 / 15;

        
    }

    private void Start()
    {
        ((SphereCollider)detectionCollision.Collider).radius = data.range;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    void shoot()
    {
        timer = 0.0f;
        GameObject newProjectile = Instantiate(data.projectilePrefab) as GameObject;
        newProjectile.transform.position = peak.transform.position;
        newProjectile.GetComponent<ProjectileScript>().setValue(data.damage, data.projectileSpeed);
        newProjectile.GetComponent<ProjectileScript>().setTarget(closestEnemy);
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

        if(timer > data.cooldown && distanceToClosestEnemy > 0 && closestEnemy != null)
        {
            shoot();
        }
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

    private void checkNearByEnemeis()
    {
        distanceToClosestEnemy = -1;
        closestEnemy = null;

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
