using System;
using UnityEngine;

public class PlayerTriggerController : MonoBehaviour
{
    public event Action OnObstacleTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "obstacle")
            OnObstacleTriggered?.Invoke();
    }
}