using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float MaxHealth { get; }
    float Health { get; set; }
}
