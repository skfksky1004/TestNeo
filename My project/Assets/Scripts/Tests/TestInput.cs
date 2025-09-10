// using System;
// using Unity.Mathematics;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using UnityEngine.InputSystem.Controls;
//
// public class TestInput : MonoBehaviour, PlatformInputActions.IPlayerActions, PlatformInputActions.IAllActions
// {
//     private PlatformInputActions platformInputActions;
//
//     [SerializeField] private GameObject targetGo;
//     [SerializeField] private Camera camera;
//     private Vector2 prevMousePos = Vector2.zero;
//
//     public bool _isFront = false;
//     public bool _isBack = false;
//     public bool _isLeft = false;
//     public bool _isRight = false;
//
//     public bool _isRunOrWalk = false;
//     public Vector3 _LookMove = Vector3.zero;
//
//     private void Awake()
//     {
//         // if (playerInput is null)
//         //     playerInput = GetComponent<PlayerInput>();
//
//         if (platformInputActions is null)
//         {
//             platformInputActions = new PlatformInputActions();
//             platformInputActions.Player.SetCallbacks(this);
//         }
//     }
//
//     private void OnEnable()
//     {
//         platformInputActions?.Enable();
//     }
//
//     private void OnDisable()
//     {
//         platformInputActions?.Disable();
//     }
//
//     private void Update()
//     {
//         Update_Look();
//     }
//
//     public void OnMove(InputAction.CallbackContext context)
//     {
//         var vec2 = context.ReadValue<Vector2>();
//         var moveDir = new Vector2(vec2.x, vec2.y);
//
//         _isFront = moveDir.y >= 0.5f;
//         _isBack = moveDir.y <= -0.5f;
//         _isLeft = moveDir.x <= -0.5f;
//         _isRight = moveDir.x >= 0.5f;
//     }
//
//     public void OnLook(InputAction.CallbackContext context)
//     {
//         if (context.started)
//         {
//             prevMousePos = GetLookMove();
//         }
//         else if(context.canceled)
//         {
//             prevMousePos = Vector2.zero;
//         }
//     }
//
//     public void OnRunAndWalk(InputAction.CallbackContext context)
//     {
//         if (!context.canceled)
//             return;
//
//         _isRunOrWalk = !_isRunOrWalk;
//     }
//
//     private void Update_Look()
//     {
//         if(prevMousePos == Vector2.zero)
//             return;
//
//         _LookMove = GetLookMove();
//     }
//
//     private Vector2 GetLookMove()
//     {
//         Vector2 result;
//         if (Gamepad.current != null)
//         {
//             result.x = Gamepad.current.rightStick.x.ReadValue();
//             result.y = Gamepad.current.rightStick.y.ReadValue();
//
//             if (result != Vector2.zero)
//                 return result;
//         }
//
//         result.x = Mouse.current.position.x.ReadValue();
//         result.y = Mouse.current.position.y.ReadValue();
//         result = camera.ScreenToWorldPoint(result);
//         result -= prevMousePos;
//
//         return result.normalized;
//     }
//
//     public void OnEscape(InputAction.CallbackContext context)
//     {
//         if (!context.canceled)
//             return;
//     }
// }
