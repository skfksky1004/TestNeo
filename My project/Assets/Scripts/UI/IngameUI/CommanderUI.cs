using System;
using System.Collections;
using System.Collections.Generic;
using GlobalEnum;
using UnityEngine;
using UnityEngine.Serialization;

public class CommanderUI : MonoBehaviour
{
    [SerializeField] private ButtonEx btnMove;
    [SerializeField] private ButtonEx btnAttack;
    [SerializeField] private ButtonEx btnRest;
    [FormerlySerializedAs("btnSystemOption")] [SerializeField] private ButtonEx btnOption;

    public RectTransform RectTf => (RectTransform)transform;

    public void Awake()
    {
        btnMove.OnClick = OnClick_Move;
        btnMove.Text = "이동";

        btnAttack.OnClick = OnClick_Attack;
        btnAttack.Text = "공격";

        btnRest.OnClick = OnClick_Rest;
        btnRest.Text = "휴식";

        btnOption.OnClick = OnClick_Option;
        btnOption.Text = "옵션";
    }

    private void OnClick_Option()
    {
        // PlayerManager.I.PlayerChar.CharAction = eCharAction.System_Option;
    }

    private void OnClick_Rest()
    {
        PlayerManager.I.PlayerChar.CharAction = eCharAction.Recess;
    }

    private void OnClick_Attack()
    {
        PlayerManager.I.PlayerChar.CharAction = eCharAction.Attack;
    }

    private void OnClick_Move()
    {
        PlayerManager.I.PlayerChar.CharAction = eCharAction.Move;
        PlayerManager.I.CreateMovePlates(PlayerManager.I.PlayerChar.transform.position);

        gameObject.SetActive(false);
    }
}
