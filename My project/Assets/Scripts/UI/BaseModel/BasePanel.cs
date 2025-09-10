using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    protected RectTransform RectTransform => (RectTransform)transform;

    private void Start()
    {
        if (canvasGroup is null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    protected void SetPanelAlpha(float value)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = value;
        }
    }

    public void SetActive(bool bActive, GameObject targetGo = null)
    {
        if (targetGo == null)
            targetGo = this.gameObject;

        if (targetGo != null)
        {
            targetGo.SetActive(bActive);
        }
    }

    /// <summary>
    /// �г� ������ ����
    /// </summary>
    public abstract void CreatedPanel();
    /// <summary>
    /// �г� Ȱ��ȭ�� ����
    /// </summary>
    public abstract void ShowPanel();
    /// <summary>
    /// �г� ��Ȱ��ȭ�� ����
    /// </summary>
    public abstract void HidePanel();

    /// <summary>
    /// ���ư ó��
    /// </summary>
    /// <returns></returns>
    public abstract bool IsProcessEscape();

    /// <summary>
    /// žUI�� ������ �г��� �̸�
    /// </summary>
    /// <returns></returns>
    public abstract string GetPanelName();
    /// <summary>
    /// �г��� Ÿ��
    /// </summary>
    /// <returns></returns>
    public abstract UIType GetPanelType();
}
