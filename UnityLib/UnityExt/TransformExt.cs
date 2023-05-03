using UnityEngine;
using Input = UnityExt.input.Input;
namespace UnityExt;

public static class TransformExt
{
    public static void ResetLocal(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
    }
    public static void SetParentAndResetLocal(this Transform transform, Transform to)
    {
        transform.SetParent(to);
        transform.ResetLocal();
    }
    public static Vector3 DirectionTo(this Transform from, Vector3 to){
        return from.position.DirectionTo(to);
    }
    public static Vector3 DirectionTo(this Transform from, Transform to) => from.position.DirectionTo(to.position);
    public static void SmoothLookInput(this Transform transform, Vector2 input, float smoothRotation = 0.03f){
        var angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        float currentRotationVelocity = 0;
        var targetRot = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, angle, ref currentRotationVelocity, smoothRotation);
        transform.rotation = Quaternion.Euler(0,targetRot,0);
    }
    public static void SmoothLookInput(this Transform transform, Vector2 input, Camera camera, float smoothRotation = 0.03f){
        var angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + camera.transform.rotation.eulerAngles.y;
        float currentRotationVelocity = 0;
        var targetRot = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, angle, ref currentRotationVelocity, smoothRotation);
        transform.rotation = Quaternion.Euler(0,targetRot,0);
    }
    public static void SmoothLookInput2D(this Transform transform, Vector2 input, float smoothRotation = 0.03f){
        var angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg - 90;
        float currentRotationVelocity = 0;
        var targetRot = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.z, angle, ref currentRotationVelocity, smoothRotation);
        transform.rotation = Quaternion.Euler(0,0,targetRot);
    }
    public static void SmoothLookInput2D(this Transform transform, Vector2 input, Camera camera, float smoothRotation = 0.03f){
        var angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg + camera.transform.rotation.eulerAngles.z - 90;
        float currentRotationVelocity = 0;
        var targetRot = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.z, angle, ref currentRotationVelocity, smoothRotation);
        transform.rotation = Quaternion.Euler(0,0,targetRot);
    }
    public static void SetPositionAndScaleAtTwoPoint(this Transform transform, Vector3 origin, Vector3 to)
    {
        var vector31 = new Vector3(Mathf.Min(origin.x, to.x), Mathf.Min(origin.y, to.y));
        var vector32 = new Vector3(Mathf.Max(origin.x, to.x), Mathf.Max(origin.y, to.y));
        transform.position = vector31;
        transform.localScale = vector32 - vector31;
    }
    public static void SetXZPosition(this Transform transform, Vector2 value){
        transform.position = new Vector3(value.x,transform.position.y, value.y);
    }
    public static void AddXZPosition(this Transform transform, Vector2 value){
        var position = transform.position;
        position += new Vector3(value.x, 0, value.y);
        transform.position = position;
    }
    public static float NormalizeTorqueTo(this Transform transform, Vector2 target){
        var dirToTarget = ( target - (Vector2)transform.position).normalized;
        float dotUp = Vector2.Dot(transform.up, dirToTarget);
        float dotRight = Vector2.Dot(transform.right, dirToTarget);
        float m;
        if (dotUp > 0  && dotRight > 0 || dotUp < 0 && dotRight > 0) {
            m = 1;
        }
        else {
            m = -1;
        }
        return  m; 
    }
    public static Vector2 DirToMouse2D(this Transform transform){
        return (Input.MouseWorldPosition - transform.position).normalized;
    }
    public static void LookToMouse2D(this Transform transform, float smoothRotation = 0.03f){
        transform.SmoothLookInput2D(transform.DirToMouse2D(),smoothRotation);
    }
    public static void LookToMouse2D(this Transform transform,  Camera camera, float smoothRotation = 0.03f){
        transform.SmoothLookInput2D(transform.DirToMouse2D(),camera, smoothRotation);
    }
}