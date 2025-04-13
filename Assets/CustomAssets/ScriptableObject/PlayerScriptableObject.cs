using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObject/PlayerScriptableObject", order = 1)]
public class PlayerScriptableObject : ScriptableObject
{
    [Tooltip("This is the speed of the player on start")][Range(0f, 10f)] public float Maxspeed;

    [Tooltip("This is the speed of the player on start")][Range(0f, 1f)] public float gravity;

    [Tooltip("This is the rotation speed of the player when he's in drill mode")][Range(0.5f, 10f)] public float rotationSpeed;

    [Tooltip("This is how much player speed player gain while he's falling")][Range(1f, 1.1f)] public float airAcceleration;

    [Tooltip("This is how much speed the player has out of ground")][Range(0f, 10f)] public float AirSpeed;

    [Tooltip("This is how much player speed player loose when stop moving in water")][Range(0.9f, 1.0f)] public float waterDecceleration;

    [Tooltip("This is how much player speed player gain when moving in water")][Range(1, 1.1f)] public float waterAcceleration;

    [Tooltip("This is how much the echolocation grows every frame")][Range(0, 100.0f)] public float echoGrowing;

    [Tooltip("This is how much the echolocation grows every frame")][Range(0, 100.0f)] public float maxEchoTime;

    [Tooltip("This is how much the echolocation grows every frame")][Range(0, 10.0f)] public float minEchoTime;

}
