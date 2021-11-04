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

    public List<Collider> Enter => enter;
    public List<Collider> Exit => exit;
    public List<Collider> Stay => stay;

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
        enter.Clear();
        exit.Clear();
    }

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