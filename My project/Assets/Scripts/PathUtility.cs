using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GlobalEnum;
using UnityEngine;

public class PathUtility
{
    // private List<PlanePathNode> openNodeList = new List<PlanePathNode>();  //오픈노드 리스트
    // private List<PlanePathNode> closeNodeList = new List<PlanePathNode>(); //클로즈 노드 리스트
    // private readonly List<PlanePathNode> result = new List<PlanePathNode>();      //결과 벡터
    // private bool bFindGoal;          //목적지 찾은값

    /// <summary>
    /// 알고리즘으로 경로 찾기
    /// </summary>
    /// <param name="startPos">월드 시작 위치</param>
    /// <param name="endPos">월드 종료 위치</param>
    /// <param name="bDiagonal">대각 갈수있는</param>
    /// <returns></returns>
    public List<PlanePathNode> FindPath(Vector3 startPos, Vector3 endPos, bool bDiagonal)
    {
        //위치에 따른 시작노드와 종료 노드를 얻는다.
        var startNode = TilemapManager.I.GetNode_WorldPos(startPos);    //시작 노드
        var endNode = TilemapManager.I.GetNode_WorldPos(endPos);        //목적지 노드

        //유효하지 않는 경로다.
        if (startNode == null || endNode == null)
            return null;

        //경로 계산을 할 필요가 없다.
        if (startNode == endNode)
            return null;

        //갈수 없는 목적지를 선택햇다면 
        if (endNode.isMoveAble == false)
            return null;

        //경로 검색전 리셋 작업
        //모든 리스트 초기화
        List<PlanePathNode> result = new List<PlanePathNode>();
        List<PlanePathNode> openNodeList = new List<PlanePathNode>();
        List<PlanePathNode> closeNodeList = new List<PlanePathNode>();

        //시작 노드의 부모는 반드시 널
        startNode.pParent = null;

        //
        //시작 노드를 클로즈리스트에 넣고 그놈을 현재 검색노드로 설정한다.
        //
        closeNodeList.Add(startNode);
        PlanePathNode curNode = startNode; //현재 검색노드
        //
        //핵심 패스파인딩 loop검색노드를 찾을수 없다면 검색실패
        //보통 이루프가 빠져나가는 경우는 목적지까지 길이 다 막혀 있을때 이다.
        //

        while (curNode != null)
        {
            //루프에서 첫번째
            //현재노드 주변으로 갈수 있는 노드를 오픈노드에 추가한다.
            var bFindGoal = CheckAround(ref openNodeList, ref closeNodeList, curNode, endNode, bDiagonal);

            //위의 첫번째 과정에서 엔드노드를 찾았다면
            if (bFindGoal)
            {
                //백트래킹
                PlanePathNode node = endNode;

                //스택에 목적지부터 result 거꾸로 쌓는다.
                while (node != null)
                {
                    //스택에 추가
                    result.Add(node);
                    node = node.pParent;
                }

                //반대로 뒤집는다.
                result.Reverse();

                return result;
            }

            //루프 두번째
            //오픈노드리스트중에 토탈코스트가 가장 작은놈을 오픈노드리스트에서 빼고 클로즈노드리스트에 넣은후
            //그놈을 검색 노드로 지정
            curNode = GetMinimumNodeFromOpenNode(ref openNodeList, ref closeNodeList);
        }

        return null;
    }


    /// <summary>
    /// 해당 인덱스의 위치가 갈수 있는 노드인지 확인
    /// </summary>
    /// <param name="indexX"></param>
    /// <param name="indexY"></param>
    /// <returns></returns>
    public bool IsMoveAble(int indexX, int indexY)
    {
        //  0부터 만들어진 필드플랜의 갯수 유효한 노드인지를 판단합니다.
        if (0 <= indexX && indexX < TilemapManager.I.CellMaxWidth &&
            0 <= indexY && indexY < TilemapManager.I.CellMaxHeight)
        {
            //  적 배치
            var tileNode = TilemapManager.I.GetNode(indexX, indexY);
            return tileNode.isMoveAble;
        }
        
        return false;
    }


    /// <summary>
    /// 해당 인덱스의 위치에 캐릭터가 서있는지
    /// </summary>
    /// <param name="indexX">x좌표</param>
    /// <param name="indexY">y좌표</param>
    /// <returns>해당위치에 캐릭터가 있는가?</returns>
    public bool IsDontStandChar(int indexX, int indexY)
    {
        //  0부터 만들어진 필드플랜의 갯수 유효한 노드인지를 판단합니다.
        if (0 <= indexX && indexX < TilemapManager.I.CellMaxWidth &&
            0 <= indexY && indexY < TilemapManager.I.CellMaxHeight)
        {
            var node = TilemapManager.I.GetNode(indexX, indexY);
            return TilemapManager.I.IsStandChar(node.centerPos) == false;
        }

        return false;
    }


    /// <summary>
    /// 현재 노드 중심으로 갈수있는 노드를 오픈리스트에 추가
    /// </summary>
    /// <param name="openList">검색한 오픈리스트</param>
    /// <param name="closeList">검색한 클로즈리스트</param>
    /// <param name="curNode">현재노드</param>
    /// <param name="endNode">끝 노드</param>
    /// <param name="bDiagonal">대각 검색</param>
    /// <param name="bCheckStandChar">캐릭터가 서있는가?</param>
    private bool CheckAround(ref List<PlanePathNode> openList, ref List<PlanePathNode> closeList,
        PlanePathNode curNode, PlanePathNode endNode,
        bool bDiagonal, bool bCheckStandChar = false)
    {
        bool isGoal = false;

        var isUp = IsMoveAble(curNode.indexX, curNode.indexY + 1);
        var isDown = IsMoveAble(curNode.indexX, curNode.indexY - 1);
        var isRight = IsMoveAble(curNode.indexX + 1, curNode.indexY);
        var isLeft = IsMoveAble(curNode.indexX - 1, curNode.indexY);

        if (bCheckStandChar)
        {
            isUp = isUp && IsDontStandChar(curNode.indexX, curNode.indexY + 1);
            isDown = isDown && IsDontStandChar(curNode.indexX, curNode.indexY - 1);
            isRight = isRight && IsDontStandChar(curNode.indexX + 1, curNode.indexY);
            isLeft = isLeft && IsDontStandChar(curNode.indexX - 1, curNode.indexY);
        }

        if (isUp)
            isGoal = AddOpenList(ref openList,ref closeList, curNode.indexX, curNode.indexY + 1, curNode, endNode);

        if (isDown)
            isGoal = AddOpenList(ref openList,ref closeList, curNode.indexX, curNode.indexY - 1, curNode, endNode);

        if (isRight)
            isGoal = AddOpenList(ref openList,ref closeList, curNode.indexX + 1, curNode.indexY, curNode, endNode);

        if (isLeft)
            isGoal = AddOpenList(ref openList,ref closeList, curNode.indexX - 1, curNode.indexY, curNode, endNode);

        //  대각 검색
        if (bDiagonal)
        {
            var isRightUp = IsMoveAble(curNode.indexX + 1, curNode.indexY + 1);     //  오른쪽 위
            var isRightDown = IsMoveAble(curNode.indexX + 1, curNode.indexY - 1);   //  오른쪽 아래
            var isLeftUp = IsMoveAble(curNode.indexX - 1, curNode.indexY + 1);      //  왼쪽 위
            var isLeftDown = IsMoveAble(curNode.indexX - 1, curNode.indexY - 1);    //  왼쪽아래

            if (bCheckStandChar)
            {
                isRightUp = isRightUp && IsDontStandChar(curNode.indexX + 1, curNode.indexY + 1);
                isRightDown = isRightDown && IsDontStandChar(curNode.indexX + 1, curNode.indexY - 1);
                isLeftUp = isLeftUp && IsDontStandChar(curNode.indexX - 1, curNode.indexY + 1);
                isLeftDown = isLeftDown && IsDontStandChar(curNode.indexX - 1, curNode.indexY - 1);
            }

            if (isRightUp)
                isGoal = AddOpenList(ref openList,ref closeList, curNode.indexX + 1, curNode.indexY + 1, curNode, endNode);

            if (isRightDown)
                isGoal = AddOpenList(ref openList,ref closeList, curNode.indexX + 1, curNode.indexY - 1, curNode, endNode);

            if (isLeftUp)
                isGoal = AddOpenList(ref openList,ref closeList, curNode.indexX - 1, curNode.indexY + 1, curNode, endNode);

            if (isLeftDown)
                isGoal = AddOpenList(ref openList,ref closeList, curNode.indexX - 1, curNode.indexY - 1, curNode, endNode);
        }

        return isGoal;
    }

    /// <summary>
    /// 해당 인덱스의 노드 오픈리스트 추가
    /// </summary>
    /// <param name="openList">검색한 오픈리스트</param>
    /// <param name="closeList">검색한 클로즈리스트</param>
    /// <param name="indexX">인덱스X</param>
    /// <param name="indexY">인덱스Y</param>
    /// <param name="parent">누구로 부터왓니?</param>
    /// <param name="endNode">목표 위치</param>
    private bool AddOpenList(ref List<PlanePathNode> openList, ref List<PlanePathNode> closeList,
        int indexX, int indexY,
        PlanePathNode parent, PlanePathNode endNode)
    {
        bool bFindGoal = false;
        PlanePathNode node = TilemapManager.I.GetNode(indexX, indexY);

        if (closeList.Contains(node))
            return false;

        if (openList.Contains(node))
        {
            int nowStartCost = parent.costFromStart + 1;
            int prevStartCost = node.costFromStart;
            if (nowStartCost < prevStartCost)
            {
                node.costFromStart = nowStartCost;
                node.pParent = parent;
                node.costTotal = node.costFromStart + node.costToGoal;
            }
        }
        else
        {
            node.pParent = parent;
            node.costFromStart = parent.costFromStart + 1;
            node.costToGoal = GetCostToGoal(node, endNode);
            node.costTotal = node.costFromStart + node.costToGoal;

            openList.Add(node);

            if (node == endNode)
                bFindGoal = true;
        }

        return bFindGoal;
    }

    private int GetCostToGoal(PlanePathNode node, PlanePathNode endNode)
    {
        return Mathf.Abs(node.indexX - endNode.indexX) + Mathf.Abs(node.indexY - endNode.indexY);
    }

    private PlanePathNode GetMinimumNodeFromOpenNode(ref List<PlanePathNode> openNodes, ref List<PlanePathNode> closeNodes)
    {
        if (openNodes.Count == 0)
        {
            return null;
        }

        var minimumNode = openNodes[0];
        for (int i = 1; i < openNodes.Count; i++)
        {
            if (openNodes[i].costTotal < minimumNode.costTotal)
                minimumNode = openNodes[i];
        }

        openNodes.Remove(minimumNode);
        closeNodes.Add(minimumNode);

        return minimumNode;
    }
}

