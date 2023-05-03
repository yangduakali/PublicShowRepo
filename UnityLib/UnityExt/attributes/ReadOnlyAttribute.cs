using System;

namespace UnityExt.attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ReadOnlyAttribute : Attribute
{
}