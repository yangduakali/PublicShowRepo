using System;

namespace UnityExt.attributes{
    [AttributeUsage(AttributeTargets.Method)]
    public class RightToolbarButtonAttribute : Attribute
    {
        public readonly string Name;
        public readonly int Order;

        public RightToolbarButtonAttribute(string name = "", int order = 0)
        {
            Name = name;
            Order = order;
        }
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class LeftToolbarButtonAttribute : Attribute
    {
        public readonly string Name;
        public readonly int Order;

        public LeftToolbarButtonAttribute(string name = "", int order = 0)
        {
            Name = name;
            Order = order;
        }
    }
    [AttributeUsage(AttributeTargets.Class)]
    public class ClassIconAttribute : Attribute
    {
        public ClassIconAttribute(string icon){
            Icon = icon;
        }
        public readonly string Icon;
    }
}