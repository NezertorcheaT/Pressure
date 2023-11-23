using UnityEngine;
using Zenject;

public class MouseIteractor : MonoBehaviour
{
    [SerializeField] private KeyCode iteractionKey;
    [SerializeField] private LayerMask iteractionLayer;
    [SerializeField] private bool isWork;
    [SerializeField] private bool showTips = true;
    [SerializeField, Min(0.1f)] private float distance;
    [SerializeField] private CursorText cursorText;

    private MouseTrigger _trigger;
    private NothingMouseTrigger _ntrigger;
    private static readonly int Outline = Shader.PropertyToID("_Outline");

    public bool IsWork
    {
        get => isWork;
        set => isWork = value;
    }

    private FirstPerson pl;

    [Inject]
    private void Construct(FirstPerson pl) => this.pl = pl;

    private void Update()
    {
        cursorText.ClearText();
        if (IsWork)
        {
            if (showTips)
            {
                var ray = pl.CursorLocked
                    ? Camera.main.ScreenPointToRay(Input.mousePosition)
                    : Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

                var hits = Physics.RaycastAll(ray, distance, iteractionLayer);
                if (_trigger)
                    _trigger.isOutline = false;
                _trigger = null;
                foreach (var hit in hits)
                {
                    _trigger = hit.collider.gameObject.GetComponent<MouseTrigger>();

                    if (_trigger == null) continue;
                    if (!_trigger.isActiveAndEnabled) continue;

                    cursorText.SetText(_trigger.TodoString);

                    //if (_trigger.OutlineSize == 0) continue;
                    //if (_trigger.isOutline) continue;
                    //_trigger.isOutline = true;

                    break;
                }
            }

            if (Input.GetKeyDown(iteractionKey))
            {
                var ray = pl.CursorLocked
                    ? Camera.main.ScreenPointToRay(Input.mousePosition)
                    : Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

                var hits = Physics.RaycastAll(ray, distance, iteractionLayer);
                _trigger = null;
                foreach (var hit in hits)
                {
                    _trigger = hit.collider.gameObject.GetComponent<MouseTrigger>();

                    if (_trigger == null) continue;
                    if (!_trigger.isActiveAndEnabled) continue;

                    _trigger.Activate();
                    break;
                }
            }

            if (Input.GetKeyUp(iteractionKey))
            {
                var ray = pl.CursorLocked
                    ? Camera.main.ScreenPointToRay(Input.mousePosition)
                    : Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

                var hits = Physics.RaycastAll(ray, distance, iteractionLayer);
                _trigger = null;
                foreach (var hit in hits)
                {
                    _trigger = hit.collider.gameObject.GetComponent<MouseTrigger>();

                    if (_trigger == null) continue;
                    if (!_trigger.isActiveAndEnabled) continue;

                    _trigger.Diactivate();
                    _trigger = null;
                    break;
                }
            }

            if (!Input.GetKeyDown(iteractionKey) &&
                !Input.GetKeyUp(iteractionKey) &&
                Input.GetKey(iteractionKey))
            {
                var ray = pl.CursorLocked
                    ? Camera.main.ScreenPointToRay(Input.mousePosition)
                    : Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

                Debug.DrawRay(ray.origin, ray.direction * distance, Color.yellow);

                var hits = Physics.RaycastAll(ray, distance, iteractionLayer);
                if (hits.Length == 0 && _trigger != null)
                {
                    _trigger?.Diactivate();
                    _trigger.isOutline = false;
                    _trigger = null;
                }

                foreach (var hit in hits)
                {
                    _ntrigger = hit.collider.gameObject.GetComponent<NothingMouseTrigger>();

                    if (_ntrigger == null) continue;
                    if (!_ntrigger.isActiveAndEnabled) continue;

                    _ntrigger.Activate();
                    _ntrigger.TwoSideTrigger.currentSide = _ntrigger.side;
                    _ntrigger = null;
                    break;
                }
            }
        }
        else
        {
            _ntrigger = null;
            _trigger = null;
        }
    }
}