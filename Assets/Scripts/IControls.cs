using UnityEngine;

public interface IControls
{
    bool UseKey { get; }
    bool UseKeyDown { get; }
    bool UseKeyUp { get; }
    bool EscKeyUp { get; }
    bool ItemUseKey { get; }
    bool ItemUseKeyDown { get; }
    bool FlashLightKey { get; }
    bool JumpKey { get; }
    bool MouseButtonUp { get; }
    float MouseScrollWheel { get; }
    Vector2 WASD { get; }
    Vector2 MousePos { get; }
    Vector2 MouseAxis { get; }
}