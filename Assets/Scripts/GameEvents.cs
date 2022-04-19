using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public delegate void GameEventOfType<T>(T data);
    public delegate void GameEvent();

    public static GameEventOfType<int> LevelChanged = null; // Index of new level
    public static GameEventOfType<(IActor, IActor, float)> DamageDealt = null; // Source, target, amount
    public static GameEventOfType<IActor> ActorKilled; // Actor died
    public static GameEventOfType<IActor> ActorDestroyed; // Actor destroyed
    public static GameEventOfType<(Buildable, bool)> BuildablePlacement; // Buildable placed, Is Success
    public static GameEvent GameClosed;
    public static GameEventOfType<Buildable> UndoPlacement; // Buildable that has been undone
    public static GameEventOfType<int> WaveFinished; // Wave that is finished
    public static GameEvent GameCompleted; // Call when game completed
}