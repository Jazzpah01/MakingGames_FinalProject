using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActor: IDamageable
{
    ActorType type { get; }
    GameObject gameObject { get; }
    float Speed {get; set;}
    bool enabled { get; set; }
    bool blockDamage { get; set; }
    float damageReduction { get; set; }
}

public enum ActorType
{
    Player,
    Enemy,
    Obstacle,
    Crops
}