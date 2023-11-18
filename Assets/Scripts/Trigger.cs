using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider))]
public abstract class MouseTrigger : MonoBehaviour
{
    public UnityEvent activationEvent;
    public UnityEvent diactivationEvent;
    public UnityEvent stillActivatedEvent;
    [SerializeField] private bool isActivated;
    public bool isOutline;

    [FormerlySerializedAs("TodoText")] [SerializeField]
    private string todoText;

    public IEnumerable<MeshRenderer> OutlineRenderers => outlines;
    [SerializeField] private List<MeshRenderer> outlines;
    [SerializeField, Range(0, 10)] private float outlineSize;
    public float OutlineSize => outlineSize;
    public string TodoString => todoText;
    private static readonly int Outline = Shader.PropertyToID("_Outline");

    public bool IsActivated => isActivated;


    protected virtual void Awake()
    {
        if (OutlineSize > 0 && outlines.Count > 0)
        {
            StartCoroutine(Main());
        }
    }

    public virtual void Activate()
    {
        activationEvent?.Invoke();
        isActivated = true;
    }

    public virtual void Diactivate()
    {
        diactivationEvent?.Invoke();
        isActivated = false;
    }

    private void Update()
    {
        if (isActivated)
            stillActivatedEvent.Invoke();
        Run();
    }

    private IEnumerator Main()
    {
        for (;;)
        {
            yield return new WaitForSeconds(0.1f);
            if (!enabled) continue;
            if (!gameObject.activeSelf) continue;
            if (isOutline)
            {
                OnOutline();
            }
            else
            {
                OffOutline();
            }
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void OnOutline() => SetOutlineSize(OutlineSize);

    private void OffOutline() => SetOutlineSize(0);

    private void SetOutlineSize(float size)
    {
        /*
        foreach (var mesh in OutlineRenderers)
        {
            foreach (var material in mesh.materials)
            {
                if (!material.HasFloat(Outline)) continue;
                material.SetFloat(Outline, size);
            }
        }*/
    }

    public void ManualOnOutline()
    {
        OnOutline();
        isOutline = true;
    }

    public void ManualOffOutline()
    {
        OffOutline();
        isOutline = false;
    }

    protected virtual void Run()
    {
    }
}