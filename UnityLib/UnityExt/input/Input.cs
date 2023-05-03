using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace UnityExt.input;

public class Input{
    public static bool Enable { get; set; } = true;

    public static Vector2 AxisRaw => !Enable ? Vector2.zero : new Vector2(UnityEngine.Input.GetAxisRaw("Horizontal"), UnityEngine.Input.GetAxisRaw("Vertical"));

    public static Vector2 Axis => !Enable ? Vector2.zero : new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));

    public static float HorizontalAxis => !Enable ? 0.0f : UnityEngine.Input.GetAxis("Horizontal");

    public static float VerticalAxis => !Enable ? 0.0f : UnityEngine.Input.GetAxis("Vertical");

    public static float HorizontalRawAxis => !Enable ? 0.0f : UnityEngine.Input.GetAxisRaw("Horizontal");

    public static float VerticalRawAxis => !Enable ? 0.0f : UnityEngine.Input.GetAxisRaw("Vertical");

    public static bool LeftMouse => Enable && UnityEngine.Input.GetMouseButton(0);

    public static bool LeftMouseDown => Enable && UnityEngine.Input.GetMouseButtonDown(0);

    public static bool LeftMouseUp => Enable && UnityEngine.Input.GetMouseButtonUp(0);

    public static bool RightMouse => Enable && UnityEngine.Input.GetMouseButton(1);

    public static bool RightMouseDown => Enable && UnityEngine.Input.GetMouseButtonDown(1);

    public static bool RightMouseUp => Enable && UnityEngine.Input.GetMouseButtonUp(1);

    public static Vector3 MouseWorldPosition => Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

    public static Vector3 MouseScreenPosition => UnityEngine.Input.mousePosition;
    
}