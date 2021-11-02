using UnityEngine;

public class HealthBar : MonoBehaviour
{

    void Update()
    {
        FaceTarget();
    }
    void FaceTarget()
    {
        transform.rotation = Quaternion.Euler(0, 45, 0);

    }
}
