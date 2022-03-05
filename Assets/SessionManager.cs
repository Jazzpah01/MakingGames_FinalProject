using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;
    public int playerId;
    public int GameSessionId;

    void Awake()
    {
        instance = this;
        playerId = Random.Range(1, 50000);
        GameSessionId = playerId;
    }
}
