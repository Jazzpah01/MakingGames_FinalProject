using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActor: IDamageable
{
    ActorType actorType { get; }
    bool isActorType(ActorType type);
    GameObject gameObject { get; }
    float Speed {get; set;}
    bool enabled { get; set; }
    bool blockDamage { get; set; }
    float damageReduction { get; set; }
}

[System.Flags]
public enum ActorType
{
    None = 0,
    Player = 1,
    Enemy = 2,
    Obstacle = 4,
    Wall = 8,
    Crops = 16,
    Tower = 32,
}