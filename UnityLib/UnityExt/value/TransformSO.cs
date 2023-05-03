using UnityEngine;

namespace UnityExt.value;

[CreateAssetMenu(fileName = "so_TransformOf_", menuName = "Value SO/Transform")]
public class TransformSO : ValueTSO<Transform>
{
    public static implicit operator Transform(TransformSO transformSO) => transformSO.Value;
}