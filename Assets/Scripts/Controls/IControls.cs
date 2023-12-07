using UnityEngine;

namespace Controls
{
    public struct ControlKey<T>
    {
        public string ShowName { get; }
        public string Key { get; }
        public T Input { get; }

        public ControlKey(string name, string currentKey, T action)
        {
            ShowName = name;
            Input = action;
            Key = currentKey;
        }
    }

    public interface IControls
    {
        ControlKey<bool> UseKey { get; }
        ControlKey<bool> UseKeyDown { get; }
        ControlKey<bool> UseKeyUp { get; }
        ControlKey<bool> EscKeyUp { get; }
        ControlKey<bool> ItemUseKey { get; }
        ControlKey<bool> ItemUseKeyDown { get; }
        ControlKey<bool> FlashLightKey { get; }
        ControlKey<bool> JumpKey { get; }
        ControlKey<bool> MouseButtonUp { get; }
        ControlKey<float> MouseScrollWheel { get; }
        float MouseScrollWheelDelay { get; }
        ControlKey<Vector2> WASD { get; }
        ControlKey<Vector2> MousePos { get; }
        ControlKey<Vector2> MouseAxis { get; }
    }
}