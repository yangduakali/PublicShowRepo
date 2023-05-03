using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExt;


public static class MeshExt
{
    public static Mesh Ring(int detail, float radius, float thickNess)
    {
        var mesh = new Mesh();
        var vector3List = new List<Vector3>();
        var angle = 360f / detail;
        var vector21 = Vector2.right * radius;
        var vector22 = Vector2Ext.GetVectorFromAngle(angle) * radius;
        vector3List.Add(vector21);
        vector3List.Add(vector21 + vector21.normalized * thickNess);
        vector3List.Add(vector22 + vector22.normalized * thickNess);
        vector3List.Add(vector22);
        var intList = new List<int> {
            0,
            1,
            2,
            0,
            2,
            3
        };
        for (var index = 1; index < detail - 1; ++index)
        {
            var vector23 = Vector2Ext.GetVectorFromAngle(angle * (index + 1)) * radius;
            vector3List.Add(vector23 + vector23.normalized * thickNess);
            vector3List.Add(vector23);
            intList.Add(index * 2 + 1);
            intList.Add(index * 2);
            intList.Add(index * 2 + 2);
            intList.Add(index * 2 + 1);
            intList.Add(index * 2 + 2);
            intList.Add(index * 2 + 3);
        }
        intList.Add(vector3List.Count - 1);
        intList.Add(vector3List.Count - 2);
        intList.Add(1);
        intList.Add(vector3List.Count - 1);
        intList.Add(1);
        intList.Add(0);
        mesh.vertices = vector3List.ToArray();
        mesh.triangles = intList.ToArray();
        return mesh;
    }
    public static void CreateRing(this Mesh mesh, int detail, float radius, float thickNess)
    {
        var vector3List = new List<Vector3>();
        var angle = 360f / detail;
        var vector21 = Vector2.right * radius;
        var vector22 = Vector2Ext.GetVectorFromAngle(angle) * radius;
        vector3List.Add(vector21);
        vector3List.Add(vector21 + vector21.normalized * thickNess);
        vector3List.Add(vector22 + vector22.normalized * thickNess);
        vector3List.Add(vector22);
        var intList = new List<int> {
            0,
            1,
            2,
            0,
            2,
            3
        };
        for (var index = 1; index < detail - 1; ++index)
        {
            var vector23 = Vector2Ext.GetVectorFromAngle(angle * (index + 1)) * radius;
            vector3List.Add(vector23 + vector23.normalized * thickNess);
            vector3List.Add(vector23);
            intList.Add(index * 2 + 1);
            intList.Add(index * 2);
            intList.Add(index * 2 + 2);
            intList.Add(index * 2 + 1);
            intList.Add(index * 2 + 2);
            intList.Add(index * 2 + 3);
        }
        intList.Add(vector3List.Count - 1);
        intList.Add(vector3List.Count - 2);
        intList.Add(1);
        intList.Add(vector3List.Count - 1);
        intList.Add(1);
        intList.Add(0);
        mesh.vertices = vector3List.ToArray();
        mesh.triangles = intList.ToArray();
    }
    public static GameObject CreateQuadMesh(Vector3 origin, Vector2 size, Material material, string name = "Quad")
    {
        var quadMesh1 = CreateQuadMesh(origin, size, material);
        var quadMesh2 = new GameObject(name);
        quadMesh2.AddComponent<MeshFilter>().mesh = quadMesh1;
        quadMesh2.AddComponent<MeshRenderer>().material = material;
        return quadMesh2;
    }
    public static Mesh CreateQuadMesh(Vector3 origin, Vector2 size, Material material)
    {
        var quadMesh = new Mesh();
        Vector3[] vector3Array = new Vector3[4];
        Vector2[] vector2Array = new Vector2[4];
        int[] numArray = new int[6];
        vector3Array[0] = new Vector3(origin.x, origin.y, origin.z);
        vector3Array[1] = new Vector3(origin.x, origin.y + size.y, origin.z);
        vector3Array[2] = new Vector3(origin.x + size.x, origin.y + size.y, origin.z);
        vector3Array[3] = new Vector3(origin.x + size.x, origin.y, origin.z);
        vector2Array[0] = new Vector2(0.0f, 0.0f);
        vector2Array[1] = new Vector2(0.0f, 1f);
        vector2Array[2] = new Vector2(1f, 1f);
        vector2Array[3] = new Vector2(1f, 0.0f);
        numArray[0] = 0;
        numArray[1] = 1;
        numArray[2] = 2;
        numArray[3] = 0;
        numArray[4] = 2;
        numArray[5] = 3;
        quadMesh.vertices = vector3Array;
        quadMesh.uv = vector2Array;
        quadMesh.triangles = numArray;
        return quadMesh;
    }
    public static Mesh GeneratePie(float arcAngle, int detail, float diameter, float thickness)
    {
        List<Vector3> vector3List = new List<Vector3>()
        {
            Vector3.zero.WithOffsetY(thickness)
        };
        for (int index = 0; index < detail + 1; ++index)
        {
            var vector = GetVectorFromAngle(arcAngle / detail * index) * diameter;
            vector3List.Add(vector.WithY(thickness));
            vector3List.Add(vector.WithY(-thickness));
        }
        vector3List.Add(Vector3.zero.WithOffsetY(-thickness));
        List<int> intList = new List<int>();
        for (int index = 0; index < detail; ++index)
        {
            int num = index + 1;
            intList.Add(0);
            intList.Add(num * 2 + 1);
            intList.Add(index * 2 + 1);
            intList.Add(index * 2 + 1);
            intList.Add(num * 2 + 1);
            intList.Add(num * 2 + 2);
            intList.Add(num * 2 + 2);
            intList.Add(index * 2 + 2);
            intList.Add(index * 2 + 1);
            intList.Add(vector3List.Count - 1);
            intList.Add(index * 2 + 2);
            intList.Add(num * 2 + 2);
        }
        intList.Add(0);
        intList.Add(1);
        intList.Add(2);
        intList.Add(0);
        intList.Add(2);
        intList.Add(vector3List.Count - 1);
        intList.Add(0);
        intList.Add(vector3List.Count - 1);
        intList.Add(vector3List.Count - 1 - 1);
        intList.Add(vector3List.Count - 1 - 1);
        intList.Add(vector3List.Count - 1 - 2);
        intList.Add(0);
        return new Mesh()
        {
            vertices = vector3List.ToArray(),
            triangles = intList.ToArray()
        };

        static Vector3 GetVectorFromAngle(float angle) => new (Mathf.Cos(angle * ((float) Math.PI / 180f)), 0.0f, Mathf.Sin(angle * ((float) Math.PI / 180f)));
    }
        public static Material NewUrpSpriteLit() => NewUrpSpriteLit(Color.white);
    public static Material NewUrpSpriteLit(Color color){
        return new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default")) {
            color = color
        };
    }
    public static Mesh Triangle(){
        var mesh = new Mesh();
        mesh.vertices = new[] {
            new Vector3(-10, 0, 0),
            new Vector3(-10, 10, 0),
            new Vector3(10, 0, 0),
        };
        mesh.triangles  = new[] { 0,1,2 };
        
        
        return mesh;
    }
}