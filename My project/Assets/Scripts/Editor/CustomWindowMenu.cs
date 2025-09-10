using UnityEditor;
using UnityEngine;

public class CustomWindowMenu : Editor
{
    private enum eType
    {
        Type_1,
        Type_2,
        Type_3,    
    }
    
    private const string TempMenuPath01 = "Utility/TempMenu/Toggle/toggle_1";
    private const string TempMenuPath02 = "Utility/TempMenu/Toggle/toggle_2";
    private const string TempMenuPath03 = "Utility/TempMenu/Toggle/toggle_3";

    private static eType _type;
    
    [MenuItem(TempMenuPath01, true)]
    private static bool TempMenuToggle01()
    {
        if (_type != eType.Type_1)
        {
            _type = eType.Type_1;
        }
        
        Menu.SetChecked(TempMenuPath01, _type == eType.Type_1);
        return true;
    }
}
