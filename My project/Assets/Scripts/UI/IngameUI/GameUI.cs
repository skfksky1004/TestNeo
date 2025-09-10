using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private CommanderUI commanderUI;

    public void Awake()
    {
        commanderUI.gameObject.SetActive(false);
    }

    public void SetCommander(Vector3 worldPos)
    {
        var isActive = commanderUI.gameObject.activeSelf;
        if (isActive)
        {
            commanderUI.gameObject.SetActive(false);
        }
        else
        {
            //  commanderRect의 렉트 앵커가 min,max가 모두 0이여야한다.
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPos);
            // float uiPosX = screenPoint.x - (UIManager.I.CanvasScale.x * 0.5f);
            // float uiPosY = screenPoint.y - (UIManager.I.CanvasScale.y * 0.5f);
            // commanderUI.RectTf.anchoredPosition = new Vector2(uiPosX, uiPosY);

            screenPoint.x *= UIManager.I.CanvasScale.x / (float)Camera.main.pixelWidth;
            screenPoint.y *= UIManager.I.CanvasScale.y / (float)Camera.main.pixelHeight;

            // set it
            commanderUI.RectTf.anchoredPosition = screenPoint - UIManager.I.CanvasScale / 2f;

            commanderUI.gameObject.SetActive(true);
        }
    }

}
