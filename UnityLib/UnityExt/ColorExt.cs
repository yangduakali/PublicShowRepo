
using UnityEngine;

namespace UnityExt;

public static class ColorExt{
    
    public static Color UnityDarkGrey(){
        return new Color(0.1960784f,0.1960784f,0.1960784f,1);
    }
    public static Color UnityWindow(){
        return new Color(0.2196079f,0.2196079f,0.2196079f,1);
    }
    
    public static Color UnityDarkGrey2(){
        return new Color(0.1660784f,0.1660784f,0.1660784f,1);
    }
    
    public static Color Transparent(){
        return new Color(0,0,0,0);
    }

    public static Color UnityTextColor(){
        return new Color(0.5019608f, 0.5019608f, 0.5019608f, 0.5f);
    }

    public static Color RandomColor(){
        return new Color(Random.Range((float)0, 1), Random.Range((float)0, 1), Random.Range((float)0, 1), 1);
    }
}