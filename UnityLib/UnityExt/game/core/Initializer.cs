using UnityEngine;
using UnityExt.attributes;
using UnityExt.game.gameobject;
using CharacterController = UnityExt.game.gameobject.CharacterController;
namespace UnityExt.game.core;

internal class Initializer : MonoBehaviour{
    [InlineScriptableObject]
    [SerializeField] private GameMode gameMode;
    private CharacterController _controller;
    private void Awake(){
        if (gameMode.controller != null) {
            _controller = Instantiate(gameMode.controller);
        }
        if (gameMode.defaultPawn != null) {
            var pawn =  Instantiate(gameMode.defaultPawn);
            if (pawn.Controller == null) pawn.Controller = _controller;
        }
        if (gameMode.defaultCharacter != null) {
             var character =  Instantiate(gameMode.defaultCharacter);
             if (character.Controller == null) character.Controller = _controller;
        }
    }
}