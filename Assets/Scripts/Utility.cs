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
}