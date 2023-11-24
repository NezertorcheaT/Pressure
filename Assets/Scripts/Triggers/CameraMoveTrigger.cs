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
    private bool camState;

    private void Start()
    {
        activationEvent.AddListener(Move);
    }

    private void Move()
    {
        camState = cam.IsWork;
        cam.IsWork = false;
        onMoveStart.Invoke();
        if (moveDelay == 0)
        {
            cam.transform.position = target.position;
            cam.transform.rotation = target.rotation;
            cam.IsWork = camState;
            onMoveEnd.Invoke();
            return;
        }

        StartCoroutine(DoMove());
    }
    private IEnumerator DoMove()
    {
        yield return null;

        if (target.position == cam.transform.position && target.rotation == cam.transform.rotation)
        {
            onMoveEnd.Invoke();
            cam.IsWork = camState;
            yield break;
        }

        for (float i = 0; i < moveDelay; i += Time.deltaTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            cam.transform.position = Vector3.Lerp(cam.transform.position, target.position,
                Mathf.Clamp(i, 0, moveDelay) / moveDelay);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, target.rotation,
                Mathf.Clamp(i, 0, moveDelay) / moveDelay);
        }

        cam.IsWork = camState;
        onMoveEnd.Invoke();
    }
}