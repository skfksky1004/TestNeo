using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPanel : BasePanel
{
    public override void CreatedPanel()
    {
    }

    public override void ShowPanel()
    {
    }

    public override void HidePanel()
    {
    }

    public override bool IsProcessEscape()
    {
        return true;
    }

    public override string GetPanelName() => "�׽�Ʈ�� �г�";

    public override UIType GetPanelType() => UIType.TestPanel;
}
