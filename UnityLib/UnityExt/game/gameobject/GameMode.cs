using UnityEngine;

namespace UnityExt.game.gameobject;

[CreateAssetMenu(fileName = "Game Mode", menuName = "Game/Game Mode")]
public class GameMode : ScriptableObject{
    [SerializeField] internal Pawn defaultPawn;
    [SerializeField] internal Character defaultCharacter;
    [SerializeField] internal CharacterController controller;
}