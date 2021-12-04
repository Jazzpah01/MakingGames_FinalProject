using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpiderData : ActorData
{
    [Header("Area Spider")]
    float speed;
    [Tooltip("The enemy speed will be multiplied with slow. Ex: 'slow = 0.5' means it halfes enemy speed.")]
    [Range(0.1f, 1)]float slow;
    float webRadius;
}