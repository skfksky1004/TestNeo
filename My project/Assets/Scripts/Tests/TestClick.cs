using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0,DateTimeKind.Utc);
        Debug.Log((int)timeSpan.TotalSeconds);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonUp(0))
        // {
        //     var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     if (Physics.Raycast(ray, out var hit))
        //     {
        //         if (hit.collider?.gameObject == this.gameObject)
        //         {
        //             // Debug.Log("클");
        //             UIManager.I.GameUI.SetCommander(hit.collider.transform.position);
        //         }
        //     }
        // }
    }
}
