using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenPanel : BasePanel
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

    public override string GetPanelName()
    {
        return "�κ��丮";
    }

    public override UIType GetPanelType() => UIType.InventoryPanel;

}
