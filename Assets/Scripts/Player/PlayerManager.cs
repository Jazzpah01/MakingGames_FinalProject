using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public GameObject player;
    public Camera cam;
    public PlayerController playerController;

    void Awake()
    {
        instance = this;
    }
}