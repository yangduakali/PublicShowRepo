using UnityEngine;
using Input = UnityExt.input.Input;
namespace UnityExt;

public static class Rigidbody2DExt{
    public static void LookToMouse2D(this Rigidbody2D rb, ref float currentRotationVelocity, float smoothRotation = 0.1f){
        var mousePosition = (Vector2)Input.MouseWorldPosition;
        var dirToTarget = rb.position - mousePosition;
        var a = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg + 90;
        var targetRotation = Mathf.SmoothDampAngle(rb.rotation, a, ref currentRotationVelocity,
            smoothRotation);
        rb.rotation = targetRotation;
    }
}