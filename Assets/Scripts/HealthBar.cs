using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthbar;
    public float smoothDelay = 0.01f;
    public float smoothAmount = 0.02f;

    Coroutine routineIncrease, routineDecrease;


    void Update()
    {
        FaceTarget(PlayerManager.instance.camera.transform);
    }
    //face the healthbars in the direction of the camera each frame.
    private void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    //set the healthbar image fill amount between 0 and 1
    public void SetHealthbar(float amount)
    {
        if (routineIncrease != null)
        {
            StopCoroutine(routineIncrease);
        }
        if (routineDecrease != null)
        {
            StopCoroutine(routineDecrease);
        }
        if (amount >= healthbar.fillAmount)
        {
            routineIncrease = StartCoroutine(SmoothSliderIncrease(amount));
        }
        else
        {
            routineDecrease = StartCoroutine(SmoothSliderDecrease(amount));
        }
    }
    //make it look smooth
    private IEnumerator SmoothSliderDecrease(float amount)
    {
        while (healthbar.fillAmount > amount)
        {
            healthbar.fillAmount -= smoothAmount;
            yield return new WaitForSeconds(smoothDelay);
        }
    }
    private IEnumerator SmoothSliderIncrease(float amount)
    {
        while (healthbar.fillAmount < amount)
        {
            healthbar.fillAmount += smoothAmount;
            yield return new WaitForSeconds(smoothDelay);
        }
    }
}
