using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityExt;

public static class Vector2Ext{
    public static float Volume(this Vector2 vector) => vector.x * vector.y;
    public static int VolumeInt(this Vector2 vector) => (int) vector.x * (int) vector.y;
    public static Vector2 GetVectorFromAngle(float angle){
      return new Vector2(Mathf.Cos(angle * ((float)Math.PI / 180f)), Mathf.Sin(angle * (Mathf.PI / 180f)))
        .normalized;
    }
    public static float SignedAngle(this Vector2 vector){
      return Vector2.SignedAngle(Vector2.right, vector);
    }
    public static float Angle(this Vector2 vector){
      return Vector2.Angle(Vector2.right, vector);
    }
    public static float Angle360(this Vector2 vector){
      var a = vector.SignedAngle();
      if (a < 0) {
        a += 360;
      }
      return a;
    }
    public static Vector2 GetRandomPosCorner(Vector2[] corner, float offset = 0.0f)
    {
      float x;
      float y;
      switch (Random.Range(0, 4))
      {
        case 0:
          x = Random.Range(corner[1].x - offset, corner[2].x + offset);
          y = corner[2].y + offset;
          break;
        case 1:
          x = corner[2].x + offset;
          y = Random.Range(corner[3].y - offset, corner[2].y + offset);
          break;
        case 2:
          x = Random.Range(corner[0].x - offset, corner[3].x + offset);
          y = corner[3].y - offset;
          break;
        case 3:
          x = corner[0].x - offset;
          y = Random.Range(corner[0].y - offset, corner[1].y + offset);
          break;
        default:
          return Vector2.zero;
      }
      return new Vector2(x, y);
    }
    public static Vector2 GetRandomPosCorner(float offset = 0.0f)
    {
      Vector2[] corner = new Vector2[4];
      GetCornerWorld(corner);
      return GetRandomPosCorner(corner, offset);
    }
    public static void GetCornerWorld(Vector2[] corner, float expanse = 0.0f){
      GetCornerWorld(corner, Camera.main, expanse);
    }
    public static void GetCornerWorld(Vector2[] corner, Camera cam, float expanse = 0.0f)
    {
      corner[0] = new Vector2(cam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).x - expanse, cam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).y - expanse);
      corner[1] = new Vector2(cam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).x - expanse, cam.ViewportToWorldPoint(new Vector3(0.0f, 1f, 0.0f)).y + expanse);
      corner[2] = new Vector2(cam.ViewportToWorldPoint(new Vector3(1f, 0.0f, 0.0f)).x + expanse, cam.ViewportToWorldPoint(new Vector3(0.0f, 1f, 0.0f)).y + expanse);
      corner[3] = new Vector2(cam.ViewportToWorldPoint(new Vector3(1f, 0.0f, 0.0f)).x + expanse, cam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).y - expanse);
    }
    public static Vector2 GetRandomPositionInsideCamera()
    {
      Vector2[] corner = new Vector2[4];
      GetCornerWorld(corner);
      return new Vector2(Random.Range(corner[0].x, corner[3].x), Random.Range(corner[0].y, corner[1].y));
    }
    public static Vector2 WitOffsetX(this Vector2 vector, float x)
    {
      vector.x += x;
      return vector;
    }
    public static Vector2 WitOffsetY(this Vector2 vector, float y)
    {
      vector.y += y;
      return vector;
    }
    public static Vector2 WitOffset(this Vector2 vector, float x, float y)
    {
      vector.x += x;
      vector.y += y;
      return vector;
    }
    public static Vector2 WithX(this Vector2 vector, float x)
    {
      vector.x = x;
      return vector;
    }
    public static Vector2 WithY(this Vector2 vector, float y)
    {
      vector.y = y;
      return vector;
    }

    public static Matrix4x4 ToMatrix(this Vector2 v){
      var mat = Matrix4x4.identity;
      mat.SetTRS(v,mat.rotation,mat.lossyScale);
      return mat;
    }
}