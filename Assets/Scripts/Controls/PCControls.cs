using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace Controls
{
    [CreateAssetMenu(fileName = "PC Controls", menuName = "Controls/Desktop", order = 1)]
    public class PCControls : ScriptableObject, IControls
    {
        [SerializeField] private KeyCode useKey;
        [SerializeField] private KeyCode itemUseKey;
        [SerializeField] private KeyCode jumpKey;
        [SerializeField] private KeyCode flashLightKey;

        public ControlKey<bool> UseKeyDown =>
            new ControlKey<bool>("Interact", useKey.ToString(), Input.GetKeyDown(useKey));

        public ControlKey<bool> UseKeyUp =>
            new ControlKey<bool>("Interact", useKey.ToString(), Input.GetKeyUp(useKey));

        public ControlKey<bool> UseKey => new ControlKey<bool>("Interact", useKey.ToString(), Input.GetKey(useKey));

        public ControlKey<bool> EscKeyUp =>
            new ControlKey<bool>("Exit Screen", "Escape", Input.GetKey(KeyCode.Escape));

        public ControlKey<bool> MouseButtonUp =>
            new ControlKey<bool>("Enter Screen", "Mouse Button 0", Input.GetMouseButtonUp(0));

        public ControlKey<bool> ItemUseKeyDown =>
            new ControlKey<bool>("Item using", itemUseKey.ToString(), Input.GetKeyDown(itemUseKey));

        public ControlKey<bool> ItemUseKey =>
            new ControlKey<bool>("Item using", itemUseKey.ToString(), Input.GetKey(itemUseKey));

        public ControlKey<bool> FlashLightKey =>
            new ControlKey<bool>("Flashlight", flashLightKey.ToString(), Input.GetKeyUp(flashLightKey));

        public ControlKey<bool> JumpKey => new ControlKey<bool>("Jump", jumpKey.ToString(), Input.GetKey(jumpKey));

        public ControlKey<float> MouseScrollWheel => new ControlKey<float>("Item Changing", "Mouse Scroll Wheel",
            Input.GetAxis("Mouse ScrollWheel"));

        float IControls.MouseScrollWheelDelay => 0;

        public ControlKey<Vector2> WASD => new ControlKey<Vector2>("Movement", "W A S D",
            new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

        public ControlKey<Vector2> MousePos =>
            new ControlKey<Vector2>("Mouse Position", "Mouse Position", Input.mousePosition);

        public ControlKey<Vector2> MouseAxis => new ControlKey<Vector2>("Mouse Velocity", "Mouse Position",
            new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")));
    }
}