using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class CreateActionPlate : MonoBehaviour
{
    [SerializeField] private ActionPlate movePlateGo;

    private readonly List<ActionPlate> createdList = new List<ActionPlate>();
    private const float PosValue = 1f;

    public bool IsCreatedPlate() => createdList.Count > 0;
    
    public void ClearPlate()
    {
        foreach (var plate in createdList)
        {
            Destroy(plate.gameObject);
        }
    
        createdList.Clear();
    }
    
    public void CreateMovePlate(Vector2 pos, int rangeCount)
    {
        ClearPlate();
        
        //1 = 3
        //2 = 5
        //3 = 7
        //4 = 9

        List<ActionPlate> plateList = new List<ActionPlate>();

        var startNum = -rangeCount;
        var endNum = rangeCount;

        var charNode = TilemapManager.I.GetNode_WorldPos(pos);

        for (int y = startNum; y <= endNum; y++)
        {
            for (int x = startNum; x <= endNum; x++)
            {
                if (x == 0 && y == 0)
                    continue;

                var calc = Mathf.Abs(x) + Mathf.Abs(y);
                if (calc <= rangeCount)
                {
                    var xPos = charNode.centerPos.x + x * PosValue;
                    var yPos = charNode.centerPos.y + y * PosValue;
                    if (TilemapManager.I.IsMove((int)xPos, (int)yPos) == false)
                        continue;

                    var go = Instantiate(movePlateGo.gameObject, this.transform);
                    if (go != null)
                    {
                        var plate = go.GetComponent<ActionPlate>();
                        plate.transform.localPosition = new Vector3(xPos + 0.5f, yPos + 0.5f, 1);
                        plate.transform.localScale = Vector3.one;

                        plate.gameObject.SetActive(true);
                        plate.SetMeshRenderColor();

                        plateList.Add(plate);
                    }
                }
            }
        }

        createdList.AddRange(plateList);
    }

    public void CreateNormalAttack(Vector2 pos, int rangeCount)
    {
        List<ActionPlate> plateList = new List<ActionPlate>();

        var startNum = -rangeCount;
        var endNum = rangeCount;

        var charNode = TilemapManager.I.GetNode_WorldPos(pos);

        for (int y = startNum; y <= endNum; y++)
        {
            for (int x = startNum; x <= endNum; x++)
            {

            }
        }
    }
}
