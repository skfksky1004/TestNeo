using System;
using System.Collections;
using System.Collections.Generic;
using GlobalEnum;
using UnityEngine;

public class PlayerChar : BaseCharObject
{
    [SerializeField] public Transform CameraFollowPos;

    private Coroutine _moving = null;

    public bool IsMainPlayer { get; } = true;

    public Vector2 GetNodePos
    {
        get
        {
            return TilemapManager.I.GetNode_WorldPos(transform.position)?.centerPos ?? Vector2.zero;
        }
    }

    public eCharAction CharAction {
        get => charStatus.CharAction;
        set => charStatus.CharAction = value;
    }

    public int CharMoveCount
    {
        get => charStatus.GetStatus.MoveCount;
        set => charStatus.GetStatus.MoveCount = value;
    }

    public override void Attack(PlanePathNode node)
    {
        throw new NotImplementedException();
    }

    public override void Magic()
    {
        throw new NotImplementedException();
    }

    public override void Move(List<PlanePathNode> nodes = null)
    {
        if (_moving != null)
        {
            StopCoroutine(_moving);
            _moving = null;
        }

        _moving = StartCoroutine(OnStartMove(nodes));

        charStatus.GetStatus.actPoint -= 5;
        CharAction = eCharAction.None;
    }

    public override void Recess()
    {
    }

    public override void HitDamage(int damage)
    {
        throw new NotImplementedException();
    }

    private IEnumerator OnStartMove(List<PlanePathNode> nodes)
    {
        CharPath.ResistNodeList(nodes);

        while (CharPath.MoveListLength > 0)
        {
            var node = CharPath.CunNode();

            SetPosition(node.centerPos);
            
            yield return new WaitForSeconds(0.05f);
        }
    }
}
