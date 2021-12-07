using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPlacement : MonoBehaviour, IBuildingRestrictions
{
    public LayerMask layerMask;
    public PlacementOutput CheckPlacement(Ray ray)
    {
        PlacementOutput retval = new PlacementOutput();
        retval.validPlacement = false;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, layerMask))
        {
            SeedGround ground = hit.collider.gameObject.GetComponent<SeedGround>();

            if (ground != null && ground.IsFree())
            {
                Vector3 newPos = ground.GetCenter();
                retval.validPlacement = true;
                retval.position = newPos;
                retval.options = PlacementOptions.OverridePosition;
            }
        } else
        {
            retval.validPlacement = false;
        }

        return retval;
    }
}