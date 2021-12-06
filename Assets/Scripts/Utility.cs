using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static bool IsDestroyed(this object component)
    {
        return !(component as UnityEngine.Object);
    }

    public static IEnumerator DelayedAbility(float delay, Action function)
    {
        yield return new WaitForSeconds(delay);

        function();
    }

    public static bool IsType(this IActor actor, ActorType other)
    {
        if (other == ActorType.None)
            throw new Exception("Cannot check for type None.");

        if (actor.actorType == ActorType.None)
            throw new Exception("Actor has no type no type.");

        return ((actor.actorType & other) != ActorType.None);
    }

    public static bool HasOption(this IActor actor, ActorType other)
    {
        if (other == ActorType.None)
            throw new Exception("Cannot check for type None.");

        if (actor.actorType == ActorType.None)
            throw new Exception("Actor has no type no type.");

        return ((actor.actorType & other) != ActorType.None);
    }

    // Untested
    //public static bool HasFlag(this Enum t, Enum other)
    //{
    //    Int32 v1 = Convert.ToInt32(t);
    //    Int32 v2 = Convert.ToInt32(other);
    //
    //    if (v2 == 0)
    //        throw new Exception("0 cannot be a flag.");
    //
    //    if (v1 == 0)
    //        return false;
    //
    //    return ((Convert.ToInt32(t) & Convert.ToInt32(other)) != 0);
    //}
}