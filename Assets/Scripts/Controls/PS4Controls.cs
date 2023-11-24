using UnityEngine;

namespace Controls
{
    [CreateAssetMenu(fileName = "PS4 Controls", menuName = "Controls/PS4", order = 1)]
    public class PS4Controls : ScriptableObject, IControls
    {
        bool IControls.UseKeyDown => Input.GetButtonDown("Joystick R2");
        bool IControls.UseKeyUp => Input.GetButtonUp("Joystick R2");
        bool IControls.UseKey => Input.GetButton("Joystick R2");
        bool IControls.EscKeyUp => Input.GetButtonDown("Joystick Options") || Input.GetButtonDown("Joystick PS");
        bool IControls.MouseButtonUp => Input.GetButtonDown("Joystick X");
        bool IControls.ItemUseKeyDown => Input.GetButtonDown("Joystick L2");
        bool IControls.ItemUseKey => Input.GetButton("Joystick R2");
        bool IControls.FlashLightKey => Input.GetButtonUp("Joystick Circle");
        bool IControls.JumpKey => Input.GetButton("Joystick X");

        float IControls.MouseScrollWheel => Input.GetAxis("Mouse ScrollWheel");

        Vector2 IControls.WASD =>
            new Vector2(Input.GetAxisRaw("Joystick LeftStickX"), Input.GetAxisRaw("Joystick LeftStickY"));

        Vector2 IControls.MousePos
        {
            get
            {
                _mousePos += _mouseAxis;
                Debug.Log(_mouseAxis);
                return _mousePos;
            }
        }

        Vector2 IControls.MouseAxis => _mouseAxis;

        private Vector2 _mousePos = Vector2.zero;

        private Vector2 _mouseAxis =>
            new Vector2(Input.GetAxisRaw("Joystick RightStickX"), Input.GetAxisRaw("Joystick RightStickY"));
    }
}