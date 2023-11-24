using UnityEngine;

namespace Controls
{
    [CreateAssetMenu(fileName = "PC Controls", menuName = "Controls/Desktop", order = 1)]
    public class PCControls : ScriptableObject, IControls
    {
        [SerializeField] private KeyCode useKey;
        [SerializeField] private KeyCode itemUseKey;
        [SerializeField] private KeyCode jumpKey;
        [SerializeField] private KeyCode flashLightKey;

        bool IControls.UseKeyDown => Input.GetKeyDown(useKey);
        bool IControls.UseKeyUp => Input.GetKeyUp(useKey);
        bool IControls.UseKey => Input.GetKey(useKey);
        bool IControls.EscKeyUp => Input.GetKey(KeyCode.Escape);
        bool IControls.MouseButtonUp => Input.GetMouseButtonUp(0);
        bool IControls.ItemUseKeyDown => Input.GetKeyDown(itemUseKey);
        bool IControls.ItemUseKey => Input.GetKey(itemUseKey);
        bool IControls.FlashLightKey => Input.GetKeyUp(flashLightKey);
        bool IControls.JumpKey => Input.GetKey(jumpKey);

        float IControls.MouseScrollWheel => Input.GetAxis("Mouse ScrollWheel");

        Vector2 IControls.WASD => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 IControls.MousePos => Input.mousePosition;
        Vector2 IControls.MouseAxis => new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    }
}