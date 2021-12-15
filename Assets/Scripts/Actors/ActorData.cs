using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName ="Assets/Actors/Actor")]
public class ActorData : ScriptableObject
{
    [Header("Standard Values")]
    public GameObject actorPrefab;
    public string name = "";
    public float maxHealth;
    public float speed;
}