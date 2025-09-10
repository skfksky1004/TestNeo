using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEx : MonoBehaviour
{
    public bool bGrayScale = false;
    public RectTransform Rect => (RectTransform)transform;
    public bool Interactable => _button?.interactable ?? true;



    private Button _button;
    public Button Button
    {
        get
        {
            if (_button is null)
            {
                _button = GetComponent<Button>();
            }

            return _button;
        }
    }


    private Image _targetImage;
    public Image TargetImage
    {
        get
        {
            if (_targetImage is null)
            {
                _targetImage = _button.targetGraphic.GetComponent<Image>();
                if (_targetImage is null)
                {
                    _targetImage = GetComponent<Image>();
                }
            }

            return _targetImage;
        }
    }


    private Text _textComponent;
    public string Text
    {
        get
        {
            if (_textComponent is null)
                _textComponent = GetComponentInChildren<Text>();

            return _textComponent.text;
        }
        set
        {
            if (_textComponent is null)
                _textComponent = GetComponentInChildren<Text>();

            _textComponent.text = value;
        }
    }

    private Action _onClick = null;
    public Action OnClick
    {
        set => _onClick = value;
    }

    private void OnEnable()
    {
        Button.onClick.AddListener(() =>
        {
            _onClick?.Invoke();
        });

        if (bGrayScale)
        {
            // var graphics = GetComponentsInChildren<MaskableGraphic>();
            // foreach (var graphic in graphics)
            // {
            //     var material = new Material(graphic.material);
            //     material.color = Interactable
            //         ? Color.gray
            //         : Color.white;
            //     graphic.material = material;
            // }
        }
    }

    private void OnDisable()
    {
        Button.onClick.RemoveListener(() =>
        {
            _onClick?.Invoke();
        });
    }
}
