using GlobalEnum;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoSingleton<InputManager>
{
    public override bool Initialize()
    {
        return true;
    }

    protected override void Destroy()
    {
    }

    void Update()
    {
        TestPlayerState();

        //터치 했을시
        if (Input.touchCount > 0)
        {
            // Click_Move(Input.touches.FirstOrDefault().position);
        }
        else
        {
            //마우스 클릭시
            if (Input.GetMouseButtonUp(0))
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var hit = Physics2D.Raycast(mousePos, Camera.main.transform.position);

                if(hit.collider is null)
                    return;

                SetClick_Player(hit);
                SetClick_ActionPlate(hit);
            }
        }
    }

    private void SetClick_Player(RaycastHit2D hit)
    {
        var playerChar = hit.collider.GetComponent<PlayerChar>();
        if (playerChar is null)
            return;

        switch (playerChar.CharAction)
        {
            case eCharAction.Move:
            {
                PlayerManager.I.CreateMovePlates(playerChar.transform.position);
                break;
            }
            case eCharAction.UseItem:
                break;
            case eCharAction.Attack:
                break;
            case eCharAction.Magic:
                break;
            case eCharAction.Attack_Special:
                break;
            case eCharAction.Recess:
                break;
            case eCharAction.Management:
                break;
            case eCharAction.System_Option:
                break;

            case eCharAction.None:
            default:
            {
                UIManager.I.GameUI.SetCommander(hit.collider.transform.position);
                break;
            }
        }
    }

    private void SetClick_ActionPlate(RaycastHit2D hit)
    {
        var actionPlate = hit.collider.GetComponent<ActionPlate>();
        if (actionPlate is null)
            return;

        actionPlate.ClickedPlate(hit.collider.transform.position);
    }

    private void TestPlayerState()
    {
        var mainPlayer = PlayerManager.I.PlayerChar;

        var action = eCharAction.None;
        if (Input.GetKeyUp(KeyCode.Alpha1)) mainPlayer.CharAction = (eCharAction.Move);
        else if (Input.GetKeyUp(KeyCode.Alpha2)) mainPlayer.CharAction = (eCharAction.UseItem);
        else if (Input.GetKeyUp(KeyCode.Alpha3)) mainPlayer.CharAction = (eCharAction.Attack);
        else if (Input.GetKeyUp(KeyCode.Alpha4)) mainPlayer.CharAction = (eCharAction.Magic);
        else if (Input.GetKeyUp(KeyCode.Alpha5)) mainPlayer.CharAction = (eCharAction.Attack_Special);
        else if (Input.GetKeyUp(KeyCode.Alpha6)) mainPlayer.CharAction = (eCharAction.Recess);
        else if (Input.GetKeyUp(KeyCode.Alpha7)) mainPlayer.CharAction = (eCharAction.Management);
        else if (Input.GetKeyUp(KeyCode.Alpha8)) mainPlayer.CharAction = (eCharAction.System_Option);
        else if (Input.GetKeyUp(KeyCode.Alpha9)) mainPlayer.CharAction = (eCharAction.Attack);
    }
}