using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image healthbarWhite, healthbarRed;
    public float smoothDelay = 0.01f;
    public float smoothAmount = 0.02f;
    public TextMeshProUGUI textBox;
    public Image heathImage;

    Coroutine routineIncrease, routineDecrease;

    private Camera cam;

    private void Start()
    {
        cam = PlayerManager.instance.cam;
    }
    void Update()
    {
        FaceTarget(cam.transform);
    }
    //face the healthbars in the direction of the camera each frame.
    private void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 500f);
    }
    //set the healthbar image fill amount between 0 and 1
    public void SetHealthbar(float amount)
    {
        //The animation is setting the red bar to the actual amount of health
        //with a white bar behind the red bar slowly decreasing towards the red bar
        //as the black background is revealed as the final background of the bar
        healthbarRed.fillAmount = amount;
        //if we are setting a new healthbar, all threads handling this healthbar should stop to avoid raceconditions
        StopTheseCoroutines();

        //handles increase or decrease in health
        if (amount >= healthbarWhite.fillAmount)
        {
            smoothAmount *= Mathf.Pow(1 + (healthbarRed.fillAmount - healthbarWhite.fillAmount), 8);
            routineIncrease = StartCoroutine(SmoothSliderIncrease(amount));
        }
        else
        {
            //remove the white image faster if more health is removed at once
            smoothAmount *= Mathf.Pow(1+(healthbarWhite.fillAmount - healthbarRed.fillAmount),8);
            routineDecrease = StartCoroutine(SmoothSliderDecrease(amount));
        }
    }
    private IEnumerator SmoothSliderDecrease(float amount)
    {
        while (healthbarWhite.fillAmount > amount)
        {
            healthbarWhite.fillAmount -= smoothAmount;
            yield return new WaitForSeconds(smoothDelay);
        }
    }
    private IEnumerator SmoothSliderIncrease(float amount)
    {
        while (healthbarWhite.fillAmount < amount)
        {
            healthbarWhite.fillAmount += smoothAmount;
            yield return new WaitForSeconds(smoothDelay);
        }
    }

    //stop all threads if the object is destroyed to prevent null-pointers
    private void OnDestroy()
    {
        StopTheseCoroutines();
    }

    private void StopTheseCoroutines()
    {
        if (routineIncrease != null)
        {
            StopCoroutine(routineIncrease);
        }
        if (routineDecrease != null)
        {
            StopCoroutine(routineDecrease);
        }
    }
    public void SetHealthImageColour(Color colour)
    {
        heathImage.color = colour;
    }
}
