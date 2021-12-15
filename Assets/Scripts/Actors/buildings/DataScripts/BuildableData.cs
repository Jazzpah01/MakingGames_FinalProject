using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildableData : ActorData
{
    [Header("Building Data")]
    public ActorType actorType;
    public LayerMask containmentLayermask;
    public string description = "";
    public Sprite icon;
    public Sprite descriptionItemImage;
}