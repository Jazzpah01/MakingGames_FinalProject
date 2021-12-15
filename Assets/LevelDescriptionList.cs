using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Assets/LevelDescription")]
public class LevelDescriptionList : ScriptableObject
{
    public List<DescriptionData> descriptions;
}