using UnityEngine;

[CreateAssetMenu(fileName = "PC Controls", menuName = "ScriptableObjects/PC Controls", order = 1)]
public class PCControls : ScriptableObject, IControls
{
    [SerializeField] private KeyCode useKey;
    bool IControls.UseKeyDown => Input.GetKeyDown(useKey);
    bool IControls.UseKeyUp => Input.GetKeyUp(useKey);
    bool IControls.UseKey => Input.GetKey(useKey);
    bool IControls.EscKeyUp => Input.GetKey(KeyCode.Escape);
    bool IControls.MouseButtonUp => Input.GetMouseButtonUp(0);

    [SerializeField] private KeyCode itemUseKey;
    bool IControls.ItemUseKeyDown => Input.GetKeyDown(itemUseKey);
    bool IControls.ItemUseKey => Input.GetKey(itemUseKey);

    [SerializeField] private KeyCode flashLightKey;
    bool IControls.FlashLightKey => Input.GetKeyUp(flashLightKey);

    [SerializeField] private KeyCode jumpKey;
    bool IControls.JumpKey => Input.GetKey(jumpKey);

    float IControls.MouseScrollWheel => Input.GetAxis("Mouse ScrollWheel");

    Vector2 IControls.WASD => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    Vector2 IControls.MousePos => Input.mousePosition;
    Vector2 IControls.MouseAxis => new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
}