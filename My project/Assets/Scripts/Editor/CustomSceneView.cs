using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class CustomSceneView
{
    private static float selectPlaySpeed = 1;

    private static string selectColor = "Yellow";
    private static string noneColor = "Green";

    private static float[] arrSpeeds = { 0.5f, 1f, 2f, 5f, 10f };

    static CustomSceneView()
    {
        // Scene �信 ��ư �߰�
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Handles.BeginGUI();

        float buttonWidth = 80f;
        float buttonHeight = 30f;


        var style1 = new GUIStyle();

        for (int i = 0; i < arrSpeeds.Length; i++)
        {
            // float x = sceneView.position.width * 0.08f - buttonWidth * 0.5f;
            float y = sceneView.position.height - (buttonHeight * (i + 2) - 10);

            var strColor = selectPlaySpeed.Equals(arrSpeeds[i]) ? selectColor : noneColor;
            var buttonText = $"<color={strColor}>x{arrSpeeds[i]}</color>";
            if (GUI.Button(new Rect(20, y, buttonWidth, buttonHeight), buttonText, style1))
            {
                selectPlaySpeed = arrSpeeds[i];
                Time.timeScale = arrSpeeds[i];
            }
        }

        Handles.EndGUI();
    }
}