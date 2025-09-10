using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TestEditorWindow : EditorWindow
{
    // EditorWindow 생성 메뉴 등록
    [MenuItem("Window/TestView")]
    public static void ShowWindow()
    {
        var window = GetWindow(typeof(TestEditorWindow));
        window.ShowPopup();
    }

    private PreviewRenderUtility _previewRenderUtility;
    private Texture _outputTexture;

    private void OnEnable()
    {
        if (_previewRenderUtility != null)
            _previewRenderUtility.Cleanup();

        _previewRenderUtility = new PreviewRenderUtility();

        System.GC.SuppressFinalize(_previewRenderUtility);

        var cam = _previewRenderUtility.camera;
        cam.fieldOfView = 30f;
        cam.nearClipPlane = 0.3f;
        cam.farClipPlane = 1000f;

        cam.transform.position = new Vector3(0, 10, -10);
        cam.transform.LookAt(Vector3.zero);

        var targetObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        targetObj.AddComponent<TestMove>();

        _outputTexture = CreatePreviewTexture(targetObj);
        DestroyImmediate(targetObj);
    }

    private RenderTexture CreatePreviewTexture(GameObject go)
    {
        _previewRenderUtility.BeginPreview(new Rect(0, 0, 300, 300), GUIStyle.none);

        _previewRenderUtility.lights.FirstOrDefault().transform.localEulerAngles = new Vector3(30, 30, 0);
        _previewRenderUtility.lights.FirstOrDefault().intensity = 2f;
        _previewRenderUtility.AddSingleGO(go);
        _previewRenderUtility.Render(true);

        return (RenderTexture)_previewRenderUtility.EndPreview();
    }

    private void OnDisable() {
        _previewRenderUtility.Cleanup();
    }

    private void OnGUI()
    {
        if (_previewRenderUtility != null || _outputTexture != null)
            GUI.DrawTexture(new Rect(0,0, 300, 300), _outputTexture);
    }
}
