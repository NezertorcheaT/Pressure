using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PressureCanvas : MonoBehaviour
{
    [SerializeField, Min(0)] private float pressure;
    [SerializeField, Min(0)] private float pressureDelay;

    [FormerlySerializedAs("ClampAngle")] [SerializeField, Min(0)]
    private float clampAngle;

    [FormerlySerializedAs("Arrow")] [SerializeField]
    private Transform arrow;

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
        for (;;)
        {
            if (pressureQueue.Count != 0)
            {
                for (; pressureQueue.Count != 0;)
                {
                    if (pressureDelay == 0)
                    {
                        arrow.localEulerAngles =
                            new Vector3(0, 0, pressureQueue[0] * (360f - clampAngle * 2) + clampAngle);
                    }
                    else
                    {
                        for (float delay = 0; delay < pressureDelay; delay += Time.deltaTime)
                        {
                            arrow.localEulerAngles = Vector3.Lerp(arrow.localEulerAngles,
                                new Vector3(0, 0, pressureQueue[0] * (360f - clampAngle * 2) + clampAngle),
                                delay / pressureDelay);
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