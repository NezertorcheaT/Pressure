using UnityEngine;

public class MouseIteractor : MonoBehaviour
{
    [SerializeField] private KeyCode iteractionKey;
    [SerializeField] private LayerMask iteractionLayer;
    [SerializeField] private bool isWork;
    [SerializeField] private bool showTips = true;
    [SerializeField, Min(0.1f)] private float distance;
    [SerializeField] private CursorText cursorText;

    private MouseTrigger trigger;
    private NothingMouseTrigger Ntrigger;

    public bool IsWork
    {
        get => isWork;
        set => isWork = value;
    }

    private void Update()
    {
        cursorText.ClearText();
        if (IsWork)
        {
            if (showTips)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray, distance, iteractionLayer);
                foreach (var hit in hits)
                {
                    trigger = hit.collider.gameObject.GetComponent<MouseTrigger>();
                    if (trigger != null)
                    {
                        cursorText.SetText(trigger.todoString);
                        break;
                    }
                }
            }

            if (Input.GetKeyDown(iteractionKey))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray, distance, iteractionLayer);
                foreach (var hit in hits)
                {
                    trigger = hit.collider.gameObject.GetComponent<MouseTrigger>();
                    if (trigger != null)
                    {
                        trigger.Activate();
                        break;
                    }
                }
            }
            if (Input.GetKeyUp(iteractionKey))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray, distance, iteractionLayer);
                foreach (var hit in hits)
                {
                    trigger = hit.collider.gameObject.GetComponent<MouseTrigger>();
                    if (trigger != null)
                    {
                        trigger.Diactivate();
                        trigger = null;
                        break;
                    }
                }
            }

            if (!Input.GetKeyDown(iteractionKey) &&
                !Input.GetKeyUp(iteractionKey) &&
                Input.GetKey(iteractionKey))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * distance, Color.yellow);
                if (trigger != null)
                {
                    RaycastHit[] hits = Physics.RaycastAll(ray, distance, iteractionLayer);
                    if (hits.Length == 0)
                    {
                        trigger?.Diactivate();
                        trigger = null;
                    }
                    foreach (var hit in hits)
                    {
                        Ntrigger = hit.collider.gameObject.GetComponent<NothingMouseTrigger>();
                        if (Ntrigger != null)
                        {
                            Ntrigger.Activate();
                            Ntrigger.TwoSideTrigger.CurrentSide = Ntrigger.side;
                            Ntrigger = null;
                            break;
                        }
                    }
                }
            }
        }
    }
}
