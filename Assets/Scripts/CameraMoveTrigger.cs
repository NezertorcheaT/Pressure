using System.Collections;
using UnityEngine;

public class CameraMoveTrigger : MouseTrigger
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform cam;
    [SerializeField] private float moveDelay;

    private void Start()
    {
        activationEvent.AddListener(() => StartCoroutine(Move()));
    }
    private IEnumerator Move()
    {
        yield return null;
        if (target.position == cam.position && target.rotation == cam.rotation) yield break;

        for (float i = 0; i < moveDelay; i += Time.deltaTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            cam.position = Vector3.Lerp(cam.position, target.position, i / moveDelay);
            cam.rotation = Quaternion.Lerp(cam.rotation, target.rotation, i / moveDelay);
        }
    }
}
