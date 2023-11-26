using UnityEngine;

namespace Controls
{
    [CreateAssetMenu(fileName = "PS4 Controls", menuName = "Controls/PS4", order = 1)]
    public class PS4Controls : ScriptableObject, IControls
    {
        ControlKey<bool> IControls.UseKeyDown =>
            new ControlKey<bool>("Interact", "R2", Input.GetButtonDown("Joystick R2"));

        ControlKey<bool> IControls.UseKeyUp =>
            new ControlKey<bool>("Interact", "R2", Input.GetButtonUp("Joystick R2"));

        ControlKey<bool> IControls.UseKey => new ControlKey<bool>("Interact", "R2", Input.GetButton("Joystick R2"));

        ControlKey<bool> IControls.EscKeyUp =>
            new ControlKey<bool>("Exit Screen", "Options/PS",
                Input.GetButtonDown("Joystick Options") || Input.GetButtonDown("Joystick PS"));

        ControlKey<bool> IControls.MouseButtonUp =>
            new ControlKey<bool>("Enter Screen", "X", Input.GetButtonDown("Joystick X"));

        ControlKey<bool> IControls.ItemUseKeyDown =>
            new ControlKey<bool>("Item using", "L2", Input.GetButtonDown("Joystick L2"));

        ControlKey<bool> IControls.ItemUseKey =>
            new ControlKey<bool>("Item using", "L2", Input.GetButton("Joystick L2"));

        ControlKey<bool> IControls.FlashLightKey =>
            new ControlKey<bool>("Flashlight", "O", Input.GetButtonUp("Joystick Circle"));

        ControlKey<bool> IControls.JumpKey =>
            new ControlKey<bool>("Jump", "R1", Input.GetButton("Joystick R1"));

        ControlKey<float> IControls.MouseScrollWheel => new ControlKey<float>("Item Changing", "Dpad Up/Down",
            Input.GetAxis("Joystick DPadY") / 10f);

        float IControls.MouseScrollWheelDelay => 0.1f;

        ControlKey<Vector2> IControls.WASD => new ControlKey<Vector2>("Movement", "Left Stick",
            new Vector2(Input.GetAxisRaw("Joystick LeftStickX"), Input.GetAxisRaw("Joystick LeftStickY")));

        ControlKey<Vector2> IControls.MousePos =>
            new ControlKey<Vector2>("Mouse Position", "Right Stick", MousePos);

        private Vector2 MousePos
        {
            get
            {
                _mousePos += _mouseAxis;

                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    _mousePos = new Vector2(Screen.width / 2f, Screen.height / 2f);
                    return _mousePos;
                }

                _mousePos = new Vector2(
                    Mathf.Clamp(_mousePos.x, 0, Screen.width),
                    Mathf.Clamp(_mousePos.y, 0, Screen.height)
                );
                return _mousePos;
            }
        }

        private Vector2 _mousePos = new Vector2(Screen.width / 2f, Screen.height / 2f);

        ControlKey<Vector2> IControls.MouseAxis =>
            new ControlKey<Vector2>("Mouse Velocity", "Right Stick", _mouseAxis);

        private Vector2 _mouseAxis =>
            new Vector2(Input.GetAxisRaw("Joystick RightStickX"), Input.GetAxisRaw("Joystick RightStickY"));
    }
}