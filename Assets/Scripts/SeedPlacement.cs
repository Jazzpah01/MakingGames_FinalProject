using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPlacement : MonoBehaviour, IBuildingRestrictions
{
    public LayerMask layerMask;
    GameObject holder = null;
    public PlacementOutput CheckPlacement(Ray ray)
    {
        PlacementOutput retval = new PlacementOutput(false);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            SeedGround ground = hit.collider.gameObject.GetComponent<SeedGround>();

            if (ground != null && ground.IsFree(this.gameObject))
            {
                retval = new PlacementOutput(true, 
                    PlacementOptions.OverridePosition, 
                    ground.GetCenter(), 
                    delegate { ground.SetObject(gameObject); });
            }
        }
        return retval;
    }

    private void Start()
    {
        
    }
}