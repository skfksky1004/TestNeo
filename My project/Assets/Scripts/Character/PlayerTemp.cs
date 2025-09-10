// using System;
// using UnityEngine;
// using UnityEngine.InputSystem;
//
// public class PlayerTemp : MonoBehaviour
// {
//     private PlayerInput playerInput;
//
//     private void Awake()
//     {
//         if (playerInput is null)
//             playerInput = GetComponent<PlayerInput>();
//
//         var maps = playerInput.actions.actionMaps;
//     }
//
//     private void OnEnable()
//     {
//     }
//
//     public void OnMove(InputValue value)
//     {
//         var vec2 = value.Get<Vector2>();
//         if (vec2 != Vector2.zero)
//         {
//             var movedir = new Vector2(vec2.x, vec2.y);
//             Debug.Log(movedir);
//         }
//     }
// }