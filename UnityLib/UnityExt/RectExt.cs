using UnityEngine;

namespace UnityExt;

public static class RectExt{


    public static Rect WithX(this Rect rect, float x){
        rect.x = x;
        return rect;
    }
    public static Rect WithOffsetX(this Rect rect, float offset){
        rect.x += offset;
        return rect;
    }
    public static Rect WithY(this Rect rect, float y){
        rect.y = y;
        return rect;
    }
    public static Rect WithOffsetY(this Rect rect, float offset){
        rect.y += offset;
        return rect;
    }
    
    public static Rect WithWidth(this Rect rect, float width){
        rect.width = width;
        return rect;
    }
    public static Rect WithOffsetWidth(this Rect rect, float offset){
        rect.width += offset;
        return rect;
    }
    public static Rect WithHeight(this Rect rect, float height){
        rect.height = height;
        return rect;
    }
    public static Rect WithOffsetHeight(this Rect rect, float offset){
        rect.height += offset;
        return rect;
    }
}