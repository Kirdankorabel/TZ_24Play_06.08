using System;
using System.Collections;
using UnityEngine;

public class PositionYCalculator : MonoBehaviour
{
    public float positionY;
    public float targetPositionY;
    public event Action<bool> OnFalled;

    public void StartCalculating(float targetPosition, float delayTime = 0.15f)
    {
        targetPositionY = targetPosition;
        StartCoroutine(FallCorutine(targetPosition, delayTime));
    }

    private IEnumerator FallCorutine(float targetPositionY, float delayTime)
    {
        OnFalled?.Invoke(true);
        yield return new WaitForSeconds(delayTime);
        positionY = transform.position.y;
        float startTime = Time.time;
        while(positionY >= targetPositionY)
        {
            positionY += Physics.gravity.y * 3 * (Time.time - startTime) * Time.deltaTime;
            yield return null;
        }
        positionY = targetPositionY;
        OnFalled?.Invoke(false);
        yield break;
    }
}
