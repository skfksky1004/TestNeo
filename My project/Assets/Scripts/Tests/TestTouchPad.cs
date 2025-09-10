using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestTouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Image imgBack;
    [SerializeField] private Image imgFront;

    private bool _isPress = false;
    private Vector3 _moveVec = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPress = true;
        
        // Debug.Log($"{_isPress}");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _moveVec = Vector3.zero;
        _isPress = false;
        
        // Debug.Log($"{_isPress}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(imgBack.rectTransform, 
                eventData.position,
                eventData.pressEventCamera, 
                out var localPoint))
        {
            _moveVec = localPoint;
            Debug.Log($"{_moveVec}");
        }
    }
}
