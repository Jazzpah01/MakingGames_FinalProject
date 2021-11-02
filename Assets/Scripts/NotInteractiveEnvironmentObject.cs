using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotInteractiveEnvironmentObject : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objects;

    /// <summary>
    /// 0,1,2 - Bush | 3,4,5 - Grass | 6,7,8 - Rock | 9,10,11,12 - Tree
    /// </summary>

    public void SetObject(int n)
    {
        for(int i = 0; i < objects.Count; i++)
        {
            if (i == n)
            {
                objects[i].SetActive(true);
            }
            else
            {
                objects[i].SetActive(false);
            }
        }
    }
}
