using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScrollData : BaseScrollData
{
    public int TestValue { get; set; }
}

public class TestScroll : MonoBehaviour
{
    [SerializeField] private BaseScrollView scrollView;

    public int createCount = 5;

    private void Start()
    {
        //  юс╫цz
        var list = new List<TestScrollData>();
        for (int i = 0; i < createCount; i++)
        {
            list.Add(new TestScrollData
            {
                Index = i,
                TestValue = i + 1,
            });
        }

        scrollView.InitScroll(list);
    }
}
