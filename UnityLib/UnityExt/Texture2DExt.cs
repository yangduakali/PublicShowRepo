using UnityEngine;

namespace UnityExt;

public static class Texture2DExt{
    public static Texture2D Create( int width, int height, Color col, float alpha = 1 ){
        col.a = alpha;
        var pix = new Color[width * height];
        for( int i = 0; i < pix.Length; ++i ) {
            pix[ i ] = col;
        }
        var result = new Texture2D( width, height );
        result.SetPixels( pix );
        result.Apply();
        return result;
    }
}