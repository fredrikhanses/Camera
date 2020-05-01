using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLook : MonoBehaviour
{
    public Transform player;

    float threshhold = 0.8f;

    private void OnDrawGizmos()
    {
        Vector3 posTrigger = transform.position;
        Vector3 posPlayer = player.position;
        Vector3 playerLookDir = player.forward;

        Vector3 playerToTrigger = posTrigger - posPlayer;
        Vector3 playerToTriggerDir = playerToTrigger.normalized;

        float lookTowardness = Vector3.Dot(playerLookDir, playerToTriggerDir);

        bool lookingAt = lookTowardness > threshhold;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(posPlayer, posPlayer + playerLookDir);


        Gizmos.color = lookingAt ? Color.green : Color.red;
        Gizmos.DrawLine(posPlayer, posPlayer + playerToTriggerDir);
        Gizmos.color = Color.white;
    }
}
