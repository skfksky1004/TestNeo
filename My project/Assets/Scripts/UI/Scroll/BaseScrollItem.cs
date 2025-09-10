using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ScrollIndex
{
    public long Index { get; set; }
}

public class BaseScrollData : ScrollIndex
{
    public long Index { get; set; }
}

public class BaseScrollItem : MonoBehaviour
{
    [SerializeField] private Text itemText;

    public Vector2 ItemSize => Rect.sizeDelta;

    public RectTransform Rect => (RectTransform)gameObject.transform;

    private BaseScrollData scrollData;
    public long ItemIndex => scrollData.Index;

    private void Awake()
    {
        Rect.sizeDelta = ItemSize;
    }

    public virtual void UpdateItem(BaseScrollData data)
    {
        scrollData = data;

        // itemText.text = ItemIndex.ToString();
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
