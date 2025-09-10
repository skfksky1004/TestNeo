using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BehaviorTreeEditor : EditorWindow
{
    Rect window1;
    Rect window2;
    Rect window3;

    float panX = 0;
    float panY = 0;

    private List<Rect> rects = new List<Rect>();

    [MenuItem("Window/Node editor")]
    static void ShowEditor()
    {
        BehaviorTreeEditor editor = EditorWindow.GetWindow<BehaviorTreeEditor>();
        editor.Init();
    }

    private void Init()
    {
        window1 = new Rect(100, 100, 100, 100);
        window2 = new Rect(200, 250, 100, 100);
        window3 = new Rect(0, 250, 100, 100);
    }

    void OnGUI()
    {
        GUI.BeginGroup(new Rect(panX, panY, 100000, 100000));
        DrawNodeCurve(window1, window2);
        DrawNodeCurve(window1, window3);

        BeginWindows();
        window1 = GUI.Window(1, window1, DrawNodeWindow, "Window 1");
        window2 = GUI.Window(2, window2, DrawNodeWindow, "Window 2");
        window3 = GUI.Window(3, window3, DrawNodeWindow, "Window 3");
        EndWindows();

        GUI.EndGroup();

        if (GUI.RepeatButton(new Rect(15, 5, 20, 20), "^"))
        {
            panY += 1;
            Repaint();
        }

        if (GUI.RepeatButton(new Rect(5, 25, 20, 20), "<"))
        {
            panX += 1;
            Repaint();
        }

        if (GUI.RepeatButton(new Rect(25, 25, 20, 20), ">"))
        {
            panX -= 1;
            Repaint();
        }

        if (GUI.RepeatButton(new Rect(15, 45, 20, 20), "v"))
        {
            panY -= 1;
            Repaint();
        }
    }

    void DrawNodeWindow(int id)
    {
        GUI.DragWindow();
    }

    void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height, 0);
        Vector3 endPos = new Vector3(end.x + start.width / 2, end.y, 0);
        Vector3 startTan = start.x + start.width / 2 < end.x
            ? startPos + Vector3.right * 50
            : startPos + Vector3.left * 50;
        Vector3 endTan = start.x + start.width / 2 < end.x
            ? endPos + Vector3.right * 50
            : endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);

        // for (int i = 0; i < 3; i++) // Draw a shadow
        //     Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        // Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
        Handles.DrawLine(startPos, endPos);
    }
}