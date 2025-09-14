//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.Controls;
//using UnityEngine.UI;

//public partial class GamePadController //: InputController
//{
//    private float _zoomValue = 0;
//    private bool ControlMouse { get; set; } = false;

//    private void Update()
//    {
//        if (IsGamepad == false)
//            return;

//        UpdateMoveMouse();
        
//        if (false == CheckBeforeUpdate())
//            return;

//        var chat = false;//ChatManager.instance.IsOpen;
//        var dlgCnt = 0;//UIManager.instance.PostDialogCnt;
//        var sceneCnt = 0;//UIManager.instance.PostSceneCnt;
//        if (chat || dlgCnt + sceneCnt > 0)
//        {
//            w = a = s = d = 0;
//            // UpdateToHotKeyAction(true);
            
//            UpdateUIScrollAction();
//        }
//        else
//        {
//            UpdatePressMove();
//            UpdateRotate();
//            UpdateZoomInOut();

//            // UpdateToHotKeyAction(false);
//        }
//    }

//    protected override bool CheckBeforeUpdate()
//    {
//        if (tutorialKeyboardLock)
//            return false;

//        // if (_hotKeyFunction.IsNullOrEmpty())
//        //     return false;

//        // if (null == PlayerManager.instance.GetMainPlayer())
//        //     return false;

//        // if (null == UIManager.instance.NowScene<InGameHUDScene>(SCENE_VALUE.eSCENE_INGAME))
//        //     return false;

//        // if (GameCamera.instance.IsHideVisibility() && false == CheckTalkingToNpc())
//        //     return false// ;

//        // if (true == IngameTutorialController.Instance.IsPlaying)
//        //     return false;

//        if (false == CheckValueBeforeUpdate())
//            return false;

//        // if (true == CheckTutorialBeforeUpdate())
//        //     return false;

//        // if (true == GuildManager.Instance.CheckGuildBattleObserver()) // 길드전 : 관전자
//        // {
//        //     // ESC / 안드로드이드 BackButton 만 허용
//        //     // if (KeyBoard.Instance.GetKeyUp(KeyCode.Escape))
//        //     //     return true;
//        // 
//        //     return false;
//        // }

//        // 로딩화면 전환시 방향키를 강제로 릴리즈 합니다.
//        // if (UIGlobalHandler.instance.IsLockScreen())
//        // {
//        //     w = a = s = d = 0;
//        //     return false;
//        // }

//        return true;
//    }

//    private void UpdatePressMove()
//    {
//        if (IsGamepad == false)
//            return;

//        if (w == 0 && a == 0 && d == 0 && s == 0)
//        {
//            return;
//        }

//        if (null == MainPlayer)
//        {
//            return;
//        }

//        // if (UserController.CancelActionOnControl(MainPlayer))
//        // {
//        //     return;
//        // }

//        // AutoPlayer.Instance.Button.CheckWait();

//   //      if (MainPlayer.Brain.NowState == (int)AIState.eIdle
//   //          || MainPlayer.Brain.NowState == (int)AIState.eRun
//   //          || MainPlayer.Brain.NowState == (int)AIState.eRide
//   //          || MainPlayer.Brain.NowState == (int)AIState.eMutualActionHoldIdle
//   //          || MainPlayer.Brain.NowState == (int)AIState.eMutualActionHoldRun)
//   //      {
//   //          Vector3 movePos;
//   //
//   //          if (w == 1 && a == 0 && d == 0 && s == 0) movePos = Vector3.forward;
//   //          else if (w == 1 && a == 1 && d == 0 && s == 0) movePos = Vector3.forward + Vector3.left;
//   //          else if (w == 1 && a == 0 && d == 1 && s == 0) movePos = Vector3.forward + Vector3.right;
//   //          else if (w == 0 && a == 1 && d == 0 && s == 0) movePos = Vector3.left;
//   //          else if (w == 0 && a == 0 && d == 1 && s == 0) movePos = Vector3.right;
//   //          else if (w == 0 && a == 0 && d == 0 && s == 1) movePos = Vector3.back;
//   //          else if (w == 0 && a == 1 && d == 0 && s == 1) movePos = Vector3.back + Vector3.left;
//   //          else if (w == 0 && a == 0 && d == 1 && s == 1) movePos = Vector3.back + Vector3.right;
//   //          else movePos = Vector3.zero;
//   //
//   //          if (Vector3.zero == movePos)
//   //              return;
//   //
//   //          var cameraTransform = GameCamera.instance.MainCameraTransform;
//   //          var direction = cameraTransform.TransformDirection(movePos);
//   //          direction.y = 0;
//   //          direction = direction.normalized;
//   //
//   //          if(MainPlayer.StatusEffectManager.IsActiveEffect(eSkillEffectType.ConfusionState))
//   //          {
//   //              direction *= -1;
//			// }
//   //
//   //          if (MainPlayer.Riding)
//   //          {
//   //              if (MainPlayer.SummonedPet.Target != null)
//   //              {
//   //                  MainPlayer.SummonedPet.SetTarget(null);
//   //              }
//   //
//   //              if (MainPlayer.StageBrain.CompareNow((int)AIState.eRide) &&
//   //                  MainPlayer.Brain.CompareNow((int)AIState.eGather))
//   //              {
//   //                  MainPlayer.Brain.ChangeState((int) AIState.eRide);
//   //              }
//   //
//   //              MainPlayer.InitGatherTarget(null);
//   //
//   //              var speedFix = Mathf.Min(3f, MainPlayer.SummonedPet.NavAgent.Speed / 5f);
//   //              var currentPosition = MainPlayer.SummonedPet.Transform.position;
//   //              var destination = currentPosition + speedFix * direction;
//   //              var targetPosition = GameNavUtils.GetSampleNearestPosition(destination);
//   //
//   //              if (!GroundInput.CheckMinMovableDistance(currentPosition, targetPosition))
//   //              {
//   //                  return;
//   //              }
//   //
//   //              MainPlayer.SummonedPet.SetMovePosition(targetPosition);
//   //          }
//   //          else
//   //          {
//   //              var target = MainPlayer.Target;
//   //              if (target != null)
//   //              {
//   //                  MainPlayer.SetTarget(null);
//   //                  MainPlayer.ReserveSkillContext.ResetReserve();
//   //              }
//   //
//   //              if (MainPlayer.InputGroundSkillPos != Vector3.zero)
//   //              {
//   //                  MainPlayer.InputGroundSkillPos = Vector3.zero;
//   //                  MainPlayer.ReserveSkillContext.ResetReserve();
//   //              }
//   //
//   //              var speedFix = Mathf.Min(3f, MainPlayer.NavAgent.Speed / 5f);
//   //              var currentPosition = MainPlayer.Transform.position;
//   //              var destination = currentPosition + speedFix * direction;
//   //              var targetPosition = GameNavUtils.GetSampleNearestPosition(destination);
//   //
//   //              if (!GroundInput.CheckMinMovableDistance(currentPosition, targetPosition))
//   //              {
//   //                  return;
//   //              }
//   //
//   //              MainPlayer.SetMovePosition(targetPosition);
//   //          }
//   //      }
//    }

//    private void UpdateRotate()
//    {
//        if (_isPressLeftShoulder)
//            return;

//        // var x = RightAxis.ReadValue().x * 10;
//        // var y = -RightAxis.ReadValue().y * 10;
//        // var delta = new Vector2(x, y);
//        // if (delta != Vector2.zero)
//        // {
//        //     GameCamera.instance.RotateInput(delta);
//        //     _prevRightAxis = delta;
//        // }
//        // if (_prevRightAxis != Vector2.zero)
//        // {
//        //     GameCamera.instance.RotateInput(_prevRightAxis);
//        // }
//    }

//    private void UpdateZoomInOut()
//    {
//        if (_isPressLeftShoulder == false)
//            return;

//        var up = -RightAxis.up.ReadValue();
//        var down = RightAxis.down.ReadValue();
        
//        if (up != 0 || down != 0)
//        {
//            var delta = (up + down) * 0.1f;
//            // GameCamera.instance.ZoomInput(delta);
//            Debug.Log(delta);
//        }
//    }

//    private void UpdateMoveMouse()
//    {
//        if (ControlMouse)
//        {
//            if (_prevLeftAxis != Vector2.zero)
//            {
//                var x = Input.mousePosition.x + _prevLeftAxis.x;
//                var y = Input.mousePosition.y + _prevLeftAxis.y;
//                Mouse.current.WarpCursorPosition(new Vector2(x,y));
//            }
//        }
//    }

//    /// <summary>
//    /// 취소
//    /// </summary>
//    private void CancelContinue()
//    {
//        //  esc 기능에서 최우선으로 채팅관련 UI 체크
//        // if (ChatManager.instance.IsOpen ||                                  //  채팅
//        //     ChatManager.instance.ChatRootUI.IsOpen_IntegratedSlotDialog)    //  채팅 감정표현
//        // {
//        //     ChatManager.instance.CloseChat();
//        //     return;
//        // }

//        // var currentScene = SceneManager.Instance.CurrentSceneType;
//        // Debug.Log(currentScene.ToString());
//        // switch (currentScene)
//        // {
//        //     case SceneType.Village:
//        //     case SceneType.Field:
//        //         if (Billboard.IsFullScreen())
//        //         {
//        //             Billboard.instance.CloseFullScreen();
//        //             return;
//        //         }
//        //         InVillageProcess();
//        //         break;
//        //
//        //     // case SceneType.Login:
//        //     // case SceneType.SelectCharacter:
//        //     // case SceneType.SelectCharacter_sunset:
//        //     //     InLoginProcess();
//        //     //     break;
//        //
//        //     case SceneType.GroupPvP:
//        //     case SceneType.Dungeon:
//        //     {
//        //         if (GameUser.Instance.CompareMapType(
//        //                 eMapType.BattleNPC,
//        //                 eMapType.Valhalla,
//        //                 eMapType.GuildBattle_GigantesWar,
//        //                 eMapType.GuildBattle_Valhalla,
//        //                 eMapType.Daily,
//        //                 eMapType.Endless,
//        //                 eMapType.GuildDungeon,
//        //                 eMapType.Raid,
//        //                 eMapType.Single,
//        //                 eMapType.FronteraDefense,
//        //                 eMapType.Constellation))
//        //             InSingleDungeonProcess();
//        //         else
//        //             InVillageProcess();
//        //
//        //         break;
//        //     }
//        //
//        //     // case SceneType.Tutorial:
//        //     //     InTutorialProcess();
//        //     //     break;
//        //     default:
//        //         break;
//        // }
//    }
    
//    /// <summary>
//    /// 펫타기 & 내리기
//    /// </summary>
//    private void Riding()
//    {
//        if (CheckLockContentThanLockDialog(eContentOpenType.Pet))
//            return;

//        if (InMainHUD == null)
//            return;

//        InMainHUD.SetRide();
//    }

//    /// <summary>
//    /// 달리기 & 걷기
//    /// </summary>
//    private void RunOrWalk()
//    {
//        MainPlayer.SetToggleWalk();
//    }
    
//    /// <summary>
//    /// 공격 & 대화
//    /// </summary>
//    public void AttackOrTalk()
//    {
//        if (ControlMouse)
//        { 
//            return;
//        }
        
//        // 몬스터 공격 또는 대화
//        if (InMainHUD == null)
//            return;

//        if (GameUser.Instance.isTown)
//        {
//            var dlg = UIManager.instance.GetActiveDialog();
//            if (dlg is QuestTalkDialog questTalk)
//            {
//                questTalk.OnNextTalk();
//            }
//            else if (dlg is NPCTalkDialog npcTalk)
//            {
//                npcTalk.OnNextTalk();
//            }
//            else
//            {
//                InMainHUD.UpTalkBtn();
//            }
//        }
//        else
//        {
//            InMainHUD.UpAttactBtn();
//        }
//    }

//    /// <summary>
//    /// 타겟 변경
//    /// </summary>
//    private void ChangeTarget()
//    {   
//        if (InMainHUD == null)
//            return;

//        InMainHUD.UpChangeTargetBtn();
//    }
    

//    /// <summary>
//    /// 마우스 좌표의 클릭 기능 구현
//    /// </summary>
//    private void RunAction()
//    {
//        if (ControlMouse == false)
//            return;
        
//        // 마우스 위치에서 클릭 이벤트를 발생시킵니다.
//        var pointerData = new PointerEventData(EventSystem.current)
//        {
//            position = Mouse.current.position.value
//        };

//        // Raycast를 통해 UI 버튼을 찾습니다.
//        var results = new List<RaycastResult>();
//        EventSystem.current.RaycastAll(pointerData, results);

//        // if (results.Count > 0)
//        // {
//        //     var targetGo = results.FirstOrDefault().gameObject;
//        //     if (targetGo.TryGetComponent<Button>(out var btn))
//        //     {
//        //         btn?.onClick.Invoke();
//        //     }
//        //     else if (targetGo.TryGetComponent<Toggle>(out var tgl))
//        //     {
//        //         tgl?.onValueChanged.Invoke(true);
//        //     }
//        //     else if (targetGo.TryGetComponent<LongPressButton>(out var longBtn))
//        //     {
//        //         longBtn?.OnClick.Invoke();
//        //     }
//        // }

//        if (UIManager.instance.GetActiveScene() is ReNew2SelectCharacter selectCharScene &&
//            selectCharScene.CheckRay(pointerData.position))
//        {
//            return;
//        }

//        foreach (var result in results)
//        {
//            var targetGo = result.gameObject;
//            if (targetGo == null)
//                continue;
            
//            if (targetGo.GetComponent<Selectable>() != null ||
//                targetGo.GetComponent<IPointerClickHandler>() != null)
//            {
//                if (targetGo.GetComponent<InputField>())
//                {
//                    if (PlatformUtil.IsSteam())
//                    {
//                        var inputMode = Steamworks.EFloatingGamepadTextInputMode.k_EFloatingGamepadTextInputModeModeSingleLine;
//                        Steamworks.SteamUtils.ShowFloatingGamepadTextInput(inputMode, 0, 0, 500, 200);
//                        return;
//                    }
//                }
                
//                ExecuteEvents.Execute(targetGo, pointerData, ExecuteEvents.pointerClickHandler);
//                break;
//            }
//            else if (targetGo.GetComponent<IPointerDownHandler>() != null)
//            {
//                ExecuteEvents.Execute(targetGo, pointerData, ExecuteEvents.pointerDownHandler);
//                break;
//            }
//            else if (targetGo.GetComponent<IPointerUpHandler>() != null)
//            {
//                ExecuteEvents.Execute(targetGo, pointerData, ExecuteEvents.pointerUpHandler);
//                break;
//            }
//            else if (targetGo.GetComponent<IPointerExitHandler>() != null)
//            {
//                ExecuteEvents.Execute(targetGo, pointerData, ExecuteEvents.pointerExitHandler);
//                break;
//            }
//        }
//    }

//    private void UpdateUIScrollAction()
//    {
//        if (ControlMouse == false)
//            return;

//        if (_raycastResults == null ||
//            _raycastResults.Count == 0)
//            return;

//        if (_prevRightAxis == Vector2.zero)
//            return;
        
//        foreach (var result in _raycastResults)
//        {
//            var isScrollHandler = result.gameObject.GetComponent<IScrollHandler>();
//            if (isScrollHandler != null)
//            {
//                var x = _prevRightAxis.x > 1 ? 1 : _prevRightAxis.x;
//                x = x < -1 ? -1 : x;

//                var y = _prevRightAxis.y > 1 ? 1 : _prevRightAxis.y;
//                y = y < -1 ? -1 : y;
                
//                var scroll = result.gameObject.GetComponent<ScrollRect>();
//                if (scroll != null)
//                {
//                    if (scroll.vertical)
//                    {
//                        if (Mathf.Approximately(y, 0.1f) && scroll.verticalNormalizedPosition >= 1)
//                            return;
                        
//                        if (Mathf.Approximately(y, -0.1f) && scroll.verticalNormalizedPosition <= 0)
//                            return;
                        
//                        scroll.verticalNormalizedPosition += (y * Time.deltaTime);
//                    }
//                    else
//                    {
//                        if (Mathf.Approximately(x, 0.1f) && scroll.horizontalNormalizedPosition >= 1)
//                            return;

//                        if (Mathf.Approximately(x, -0.1f) && scroll.horizontalNormalizedPosition <= 0)
//                            return;

//                        scroll.horizontalNormalizedPosition += (-x * Time.deltaTime);
//                    }
//                }

//                var loopScroll = result.gameObject.GetComponent<LoopScrollRect>();
//                if (loopScroll != null)
//                {
//                    if (loopScroll.vertical)
//                    {
//                        if (Mathf.Approximately(y, 0.1f) && loopScroll.verticalNormalizedPosition >= 1)
//                            return;
                        
//                        if (Mathf.Approximately(y, -0.1f) && loopScroll.verticalNormalizedPosition <= 0)
//                            return;
                        
//                        loopScroll.verticalNormalizedPosition += (y * Time.deltaTime);
//                    }
//                    else
//                    {
//                        if (Mathf.Approximately(x, 0.1f) && loopScroll.horizontalNormalizedPosition >= 1)
//                            return;

//                        if (Mathf.Approximately(x, -0.1f) && loopScroll.horizontalNormalizedPosition <= 0)
//                            return;

//                        loopScroll.horizontalNormalizedPosition += (-x * Time.deltaTime);
//                    }
//                }
                
//                var loopScrollBase = result.gameObject.GetComponent<LoopScrollRectBase>();
//                if (loopScrollBase != null)
//                {
//                    if (loopScrollBase.vertical)
//                    {
//                        if (Mathf.Approximately(_prevRightAxis.y, 0.1f) && loopScrollBase.verticalNormalizedPosition >= 1)
//                            return;
                        
//                        if (Mathf.Approximately(_prevRightAxis.y, -0.1f) && loopScrollBase.verticalNormalizedPosition <= 0)
//                            return;
                        
//                        loopScrollBase.verticalNormalizedPosition += (_prevRightAxis.y * Time.deltaTime);
//                    }
//                    else
//                    {
//                        if (Mathf.Approximately(_prevRightAxis.x, 0.1f) && loopScrollBase.horizontalNormalizedPosition >= 1)
//                            return;

//                        if (Mathf.Approximately(_prevRightAxis.x, -0.1f) && loopScrollBase.horizontalNormalizedPosition <= 0)
//                            return;

//                        loopScrollBase.horizontalNormalizedPosition += (-_prevRightAxis.x * Time.deltaTime);
//                    }
//                }
//            }
//        }
//    }
    
//    // public static void RumbleAction()
//    // {
//    //     StartCoroutine(StartRumble());
//    // }
    
//    public static IEnumerator<WaitForSeconds> StartRumble()
//    {
//        Gamepad.current.SetMotorSpeeds(0.1f, 0.5f);
//        yield return new WaitForSeconds(0.2f);
//        Gamepad.current.PauseHaptics();
//    }
//}

//public partial class GamePadController
//{   
//    private enum eGamePadKeyType
//    {
//        None,
        
//        Left_Shoulder,
//        Left_Trigger,
//        Left_Stick,
//        Left_Arrow_Up,
//        Left_Arrow_Down,
//        Left_Arrow_Left,
//        Left_Arrow_Right,
//        Press_Left_Stick,
        
//        Right_Shoulder,
//        Right_Trigger,
//        Right_Stick,
//        Press_Button_x,
//        Press_Button_y,
//        Press_Button_a,
//        Press_Button_b,
//        Press_Right_Stick,
        
//        Option_Select,
//        Option_Start,
        
//        Max,
//    }
    
//    private GamepadInputActions _gamePadActions;

//    /// <summary> Click과 Press의 기준 </summary>
//    private const float LimitClickDuration = 0.3f;
    
//    /// <summary> 버튼이 눌려졌는지와 얼마만큼의 시간 눌려졌는지 확인 하는 용도 </summary>
//    private readonly Dictionary<string, float> _dicPressButtons = new Dictionary<string, float>();
    
//    /// <summary> 버튼 타입에 대한 기능 셋팅용 </summary>
//    private readonly Dictionary<eGamePadKeyType, Action> _dicButtonActions = new Dictionary<eGamePadKeyType, Action>();

//    public static bool IsGamepad => Gamepad.current != null;

//    private List<RaycastResult> _raycastResults;
    
//    private void Awake()
//    {
//        if (_gamePadActions == null)
//        {
//            _gamePadActions = new GamepadInputActions();
//            _gamePadActions.Left.SetCallbacks(this);
//            _gamePadActions.Right.SetCallbacks(this);
//            _gamePadActions.Center.SetCallbacks(this);
//        }
//    }

//    private void OnEnable()
//    {
//        _gamePadActions.Enable();
//    }

//    private void Start()
//    {
//        _dicButtonActions.Add(eGamePadKeyType.Left_Shoulder, null);
//        _dicButtonActions.Add(eGamePadKeyType.Press_Left_Stick, Riding);
        
//        _dicButtonActions.Add(eGamePadKeyType.Right_Shoulder, ChangeTarget);
//        _dicButtonActions.Add(eGamePadKeyType.Press_Right_Stick, RunOrWalk);
//        _dicButtonActions.Add(eGamePadKeyType.Press_Button_x, AttackOrTalk);
//        // _dicButtonActions.Add(eGamePadKeyType.Press_Button_y, RumbleAction);
//        _dicButtonActions.Add(eGamePadKeyType.Press_Button_a, RunAction);
//        _dicButtonActions.Add(eGamePadKeyType.Press_Button_b, CancelContinue);
//    }

//    private void OnDisable()
//    {
//        _gamePadActions.Disable();
//    }

//    private void OnDestroy()
//    {
//        _gamePadActions.Dispose();
//    }

//    /// <summary>
//    /// Context 상태에 대한 기능 구현
//    /// </summary>
//    /// <param name="context"></param>
//    /// <returns></returns>
//    private bool RefreshContext(InputAction.CallbackContext context)
//    {
//        if (context.started)
//        {
//            _dicPressButtons.TryAdd(context.action.name, Time.realtimeSinceStartup);
//        }
//        else if (context.performed)
//        {
//            return _dicPressButtons.ContainsKey(context.action.name);
//        }
//        else if (context.canceled)
//        {
//            var savedTime = _dicPressButtons.GetValueOrDefault(context.action.name, 0);
//            var curTime = Time.realtimeSinceStartup;
//            if (curTime - savedTime < LimitClickDuration)
//            {
//                var keyType = Enum.TryParse<eGamePadKeyType>(context.action.name, out var result) 
//                    ? result
//                    : eGamePadKeyType.None;

//                if (keyType != eGamePadKeyType.None)
//                {
//                    if (_dicButtonActions.TryGetValue(keyType, out var action))
//                    {
//                        action?.Invoke();
//                    }
//                }
//            }

//            _dicPressButtons.Remove(context.action.name);
//        }

//        return false;
//    }
//}

///// <summary>
///// 왼쪽 버튼과 스틱 모음
///// </summary>
//public partial class GamePadController : GamepadInputActions.ILeftActions
//{   
//    /// <summary>
//    /// 왼쪽 스틱 값
//    /// </summary>
//    private Vector2 _prevLeftAxis = Vector2.zero;
    
//    /// <summary>
//    /// 왼쪽 범퍼 버튼
//    /// </summary>
//    private bool _isPressLeftShoulder = false;
//    private bool _isPressLeftTrigger = false;
    
//    private StickControl LeftAxis => Gamepad.current.leftStick;
    
//    public void OnLeft_Shoulder(InputAction.CallbackContext context)
//    {
//        _isPressLeftShoulder = RefreshContext(context);
//    }

//    public void OnLeft_Trigger(InputAction.CallbackContext context)
//    {
//        _isPressLeftTrigger = RefreshContext(context);
//        if (_isPressLeftTrigger)
//        {
//            var results = Util.Instance.PointIsOverUiObject(Mouse.current.position.value);
//            if (results?.Count > 0)
//            {
//                _raycastResults = results;
//            }
//        }
//        else
//        {
//            _raycastResults = null;
//        }
//    }

//    public void OnLeft_Stick(InputAction.CallbackContext context)
//    {
//        var speedValue = _isPressRightTrigger ? 20 : 10;
//        var isMoveStick = RefreshContext(context);
//        var x = isMoveStick ? LeftAxis.ReadValue().x * speedValue : 0;
//        var y = isMoveStick ? LeftAxis.ReadValue().y * speedValue : 0;

//        if (ControlMouse)
//        {
//            _prevLeftAxis = new Vector2(x, y);
//        }
//        else
//        {
//            w = y >= 0.5f ? 1 : 0;
//            s = y <= -0.5f ? 1 : 0;
//            a = x <= -0.5f ? 1 : 0;
//            d = x >= 0.5f ? 1 : 0;
//        }
//    }

//    public void OnLeft_Arrow_Up(InputAction.CallbackContext context)
//    {
//        RefreshContext(context);
//    }

//    public void OnLeft_Arrow_Down(InputAction.CallbackContext context)
//    {
//        RefreshContext(context);
//    }

//    public void OnLeft_Arrow_Left(InputAction.CallbackContext context)
//    {
//        RefreshContext(context);
//    }

//    public void OnLeft_Arrow_Right(InputAction.CallbackContext context)
//    {
//        RefreshContext(context);
//    }

//    public void OnPress_Left_Stick(InputAction.CallbackContext context)
//    {
//        RefreshContext(context);
//    }
//}

///// <summary>
///// 오른쪽 버튼과 스틱 모음
///// </summary>
//public partial class GamePadController : GamepadInputActions.IRightActions
//{
//    /// <summary>
//    /// 오른쪽 스틱 값
//    /// </summary>
//    private Vector2 _prevRightAxis = Vector2.zero;

//    private StickControl RightAxis => Gamepad.current.rightStick;
    
//    private bool _isPressRightShoulder = false;
//    private bool _isPressRightTrigger = false;
    
    
//    public void OnRight_Shoulder(InputAction.CallbackContext context)
//    {
//        _isPressRightShoulder = RefreshContext(context);
//    }

//    public void OnRight_Trigger(InputAction.CallbackContext context)
//    {   
//        _isPressRightTrigger = RefreshContext(context);
//        // if (_isPressRightTrigger)
//        // {
//        //     // 마우스 위치에서 클릭 이벤트를 발생시킵니다.
//        //     var results = Util.Instance.PointIsOverUiObject(Mouse.current.position.value);
//        //     if (results?.Count > 0)
//        //     {
//        //         _raycastResults = results;
//        //         _scrollUpDown = -0.5f;
//        //     }
//        // }
//        // else
//        // {
//        //     _raycastResults = null;
//        //     _scrollUpDown = 0f;
//        //     
//        // }
//    }

//    public void OnRight_Stick(InputAction.CallbackContext context)
//    {
//        var isPress = RefreshContext(context);
//        var x = isPress ? RightAxis.ReadValue().x * 10 : 0;
//        var y = isPress ? RightAxis.ReadValue().y * 10 : 0;

//        if (isPress)
//        {
//            _prevRightAxis = new Vector2(x, y);
            
//            if (ControlMouse)
//            {
//                // 마우스 위치에서 클릭 이벤트를 발생시킵니다.
//                var results = Util.Instance.PointIsOverUiObject(Mouse.current.position.value);
//                if (results?.Count > 0)
//                {
//                    _raycastResults = results;
//                }
//            }
//            else
//            {
//                GroundInput.instance.isOperationCamera = true;
//            }
//        }
//        else
//        {
//            _prevRightAxis = Vector2.zero;

//            if (ControlMouse)
//            {
//                _raycastResults = null;
//            }
//            else
//            {
//                GroundInput.instance.isOperationCamera = false;
//            }
//        }
//    }

//    public void OnPress_Button_x(InputAction.CallbackContext context)
//    {
//        RefreshContext(context);
//    }

//    public void OnPress_Button_y(InputAction.CallbackContext context)
//    {
//        RefreshContext(context);
//    }

//    public void OnPress_Button_a(InputAction.CallbackContext context)
//    {
//        RefreshContext(context);
//    }

//    public void OnPress_Button_b(InputAction.CallbackContext context)
//    {
//        RefreshContext(context);
//    }

//    public void OnRight_Buttons(InputAction.CallbackContext context)
//    {
//        RefreshContext(context);
//    }

//    public void OnPress_Right_Stick(InputAction.CallbackContext context)
//    {
//        RefreshContext(context);
//    }
//}

///// <summary>
///// 가운데 버튼 모음
///// </summary>
//public partial class GamePadController : GamepadInputActions.ICenterActions
//{
//    public void OnOption_Select(InputAction.CallbackContext context)
//    {
//        RefreshContext(context);
//    }

//    public void OnOption_Start(InputAction.CallbackContext context)
//    {
//        var isStart = RefreshContext(context);
//        if (isStart)
//        {
//            var mouseState = !ControlMouse;
//            ControlMouse = mouseState;
//            UnityEngine.Cursor.visible = mouseState;
//        }
//    }
//}
