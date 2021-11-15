using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObserver : MonoBehaviour
{
    public delegate void CollisionObservation(Collider other);
    private CollisionObservation enterObservation;
    private CollisionObservation exitObservation;
    private CollisionObservation stayObservation;

    private List<Collider> enter;
    private List<Collider> exit;
    private List<Collider> stay;

    private Collider collider;

    /// <summary>
    /// Get all collisions that entered this frame.
    /// </summary>
    public List<Collider> Enter => enter;
    /// <summary>
    /// Get all collisions that exited this frame.
    /// </summary>
    public List<Collider> Exit => exit;
    /// <summary>
    /// Get all collisions currently colliding this frame.
    /// </summary>
    public List<Collider> Stay => stay;
    /// <summary>
    /// Get the collider that this script is observing.
    /// </summary>
    public Collider Collider => collider;

    public enum CollisionType
    {
        Enter,
        Exit,
        Stay
    }

    private void Awake()
    {
        enter = new List<Collider>();
        exit = new List<Collider>();
        stay = new List<Collider>();
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == transform.parent)
            return;

        enter.Add(other);
        stay.Add(other);
        if (enterObservation != null) enterObservation(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == transform.parent)
            return;

        exit.Add(other);
        stay.Remove(other);
        if (exitObservation != null) exitObservation(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform == transform.parent)
            return;

        if (stayObservation != null) stayObservation(other);
    }

    private void LateUpdate()
    {
        // Clean up the lists containing information on collisions.
        enter.Clear();
        exit.Clear();

        for (int i = 0; i < stay.Count; i++)
        {
            if (stay[i] == null || stay[i].gameObject == null)
            {
                stay.RemoveAt(i);
                i--;
            }
        }
    }

    /// <summary>
    /// Subscribe a function to the observer. When this script observes a collision, it will called the subscribed function.
    /// </summary>
    /// <param name="observerMethod"></param>
    /// <param name="type"></param>
    public void Subscribe(CollisionObservation observerMethod, CollisionType type)
    {
        switch (type)
        {
            case CollisionType.Enter:
                enterObservation += observerMethod;
                break;
            case CollisionType.Exit:
                exitObservation += observerMethod;
                break;
            case CollisionType.Stay:
                stayObservation += observerMethod;
                break;
        }
    }
}