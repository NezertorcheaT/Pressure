using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CameraMoveTrigger : MouseTrigger
{
    [SerializeField] private Transform target;
    [SerializeField] private MouseIteractor cam;
    [SerializeField, Min(0)] private float moveDelay;
    public UnityEvent onMoveStart;
    public UnityEvent onMoveEnd;

    private void Start()
    {
        activationEvent.AddListener(() => StartCoroutine(Move()));

        onMoveStart.AddListener(() => cam.IsWork = false);
        onMoveEnd.AddListener(() => cam.IsWork = true);
    }

    private IEnumerator Move()
    {
        yield return null;
        onMoveStart.Invoke();

        if (target.position == cam.transform.position && target.rotation == cam.transform.rotation)
        {
            onMoveEnd.Invoke();
            yield break;
        }

        if (moveDelay == 0)
        {
            cam.transform.position = target.position;
            cam.transform.rotation = target.rotation;
            onMoveEnd.Invoke();
            yield break;
        }

        for (float i = 0; i < moveDelay; i += Time.deltaTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            cam.transform.position = Vector3.Lerp(cam.transform.position, target.position, i / moveDelay);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, target.rotation, i / moveDelay);
        }

        onMoveEnd.Invoke();
    }
}