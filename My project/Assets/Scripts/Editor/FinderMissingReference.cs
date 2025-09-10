using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Utility;

public class FinderMissingReference : EditorWindow
{
    public class FindGoInfo
    {
        public readonly int No;
        public readonly GameObject Go;

        public FindGoInfo(int no, GameObject go)
        {
            this.No = no;
            this.Go = go;
        }
    }

    private readonly List<FindGoInfo> findList = new List<FindGoInfo>();
    private Vector2 _scrollPos = Vector2.zero;

    [MenuItem("Utility/Tools/FinderMissingReference &m")]
    public static void OpenWindow()
    {
        var window = GetWindow<FinderMissingReference>(true, "FinderMissingReferences");
        window.minSize = window.maxSize = new Vector2(400f, 600f);
        window.Show();
    }

    private void OnGUI()
    {
        Display();
    }

    private void Display()
    {
        EditorGUILayout.BeginVertical();
        {
            ReSearch();

            //  �˻��� UISprite�� ǥ��
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            if (findList.Count != 0)
            {
                //  ������ ����
                int viewCount = 40;
                //  �� Į���� ����
                int culumnHeight = 20;
                //  ù Į���� �ε���
                int firstIndex = (int)_scrollPos.y / culumnHeight;
                firstIndex = Mathf.Clamp(firstIndex, 0, Mathf.Max(0, (findList.Count -1)));

                //  ���� ����
                GUILayout.Space(firstIndex * culumnHeight);

                //  �������� Į����
                int count = Mathf.Min(findList.Count, firstIndex + viewCount);
                for (int i = firstIndex; i < count; i++)
                {
                    HorizontalColumn(findList[i]);
                }

                //  �Ʒ��� ����
                GUILayout.Space(Mathf.Max(0, (findList.Count) - firstIndex - viewCount) * culumnHeight);
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }

    private void ReSearch()
    {
        if (GUILayout.Button("SearchComponent"))
        {
            findList.Clear();

            var arrGo = FindObjectsOfType<GameObject>();
            foreach (var go in arrGo)
            {
                var so = new SerializedObject(go);
                var it = so.GetIterator();
                while (it.Next(true))
                {
                    if (it.propertyType != SerializedPropertyType.ObjectReference)
                        continue;

                    if (it.objectReferenceValue == null && it.objectReferenceInstanceIDValue != 0)
                    {
                        findList.Add(new FindGoInfo(findList.Count + 1, go));
                    }
                }
            }
        }
    }


    private void HorizontalColumn(FindGoInfo info)
    {
        EditorGUILayout.BeginHorizontal();
        {
            //  �ѹ�
            GUIStyle style = new GUIStyle();
            style.wordWrap = true;
            style.alignment = TextAnchor.MiddleRight;
            EditorGUILayout.LabelField($" <color=#ffffff>{info.No}</color> ", style,
                GUILayout.Width(40),
                GUILayout.ExpandHeight(false));

            //  ��������Ʈ ������ ���� ������Ʈ\
            var arrOptions = new[] { GUILayout.MaxWidth(400), GUILayout.ExpandHeight(false) };
            EditorGUILayout.ObjectField(info.Go, typeof(GameObject), false, arrOptions);
        }
        EditorGUILayout.EndHorizontal();
    }
}
