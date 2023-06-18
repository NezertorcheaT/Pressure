using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureCanvas : MonoBehaviour
{
    [SerializeField, Min(0)] private float pressure;
    [SerializeField, Min(0)] private float pressureDelay;
    [SerializeField, Min(0)] private float ClampAngle;
    [SerializeField] private Transform Arrow;
    [SerializeField] private List<float> pressureQueue;

    public float Pressure
    {
        get => pressure;
        set
        {
            pressure = value;
            pressureQueue.Add(Pressure);
        }
    }

    private IEnumerator Start()
    {
        for (; ; )
        {
            if (pressureQueue.Count != 0)
            {
                for (; pressureQueue.Count != 0;)
                {
                    if (pressureDelay == 0)
                    {
                        Arrow.localEulerAngles = new Vector3(0, 0, pressureQueue[0] * (360f - ClampAngle * 2) + ClampAngle);
                    }
                    else
                    {
                        for (float delay = 0; delay < pressureDelay; delay += Time.deltaTime)
                        {
                            Arrow.localEulerAngles = Vector3.Lerp(Arrow.localEulerAngles, new Vector3(0, 0, pressureQueue[0] * (360f - ClampAngle * 2) + ClampAngle), delay / pressureDelay);
                            yield return new WaitForEndOfFrame();
                        }
                    }
                    pressureQueue.RemoveAt(0);
                }
            }
            else
                yield return new WaitForSeconds(0.1f);
        }
    }
}
