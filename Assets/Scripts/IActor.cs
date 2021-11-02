using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActor
{
    ActorType type { get; }
    GameObject gameObject { get; }
}

public enum ActorType
{
    Player,
    Enemy,
    Obstacle
}