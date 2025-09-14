//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public partial class KeymapManager //: Singleton<KeymapManager>
//{
//    private readonly Dictionary<int, Data_KeyMap> _keyMapData = new Dictionary<int, Data_KeyMap>();
    
//    private readonly List<KeyCode> _allCombineKey = new List<KeyCode>();

//    public List<Data_KeyMap> KeyMapList => _keyMapData.Values.ToList();

//    public void OnCreated()
//    {
//        CheckLoadKeyMap();
//        InitCombineKey();
//    }
    
//    public void InitCombineKey()
//    {
//         var arrKeys = Enum.GetValues(typeof(KeyCode));
//         var it = arrKeys.GetEnumerator();
//         while (it.MoveNext())
//         {
//              if (it.Current == null)
//                   continue;
    
//              if (it.Current.ToString().Contains("Left") ||
//                  it.Current.ToString().Contains("Right"))
//              {
//                   _allCombineKey.Add(Enum.Parse<KeyCode>(it.Current.ToString()));
//              }
//         }
//    }

//    /// <summary>
//    /// 플랫폼 키맵 리스트가 따로 저장되어있으면 교체
//    /// </summary>
//    private void CheckLoadKeyMap()
//    {
//        var list = GetKeyMapList();
//        foreach (var data in list)
//        {
//            var prefsKey = GetPrefsKey(data);
//            if (PlayerPrefs.HasKey(prefsKey))
//            {
//                var arrKeyValue = PlayerPrefs.GetString(prefsKey).Split('_');
//                var copyData = new Data_KeyMap(data)
//                {
//                    CombineKey = arrKeyValue[1],
//                    MainKey = arrKeyValue[0],
//                };
                
//                _keyMapData.Add(data.ID, copyData);
//            }
//            else
//            {
//                var copyData = new Data_KeyMap(data);
//                _keyMapData.Add(data.ID, copyData);
//            }
//        }
//    }

//    /// <summary>
//    /// 각 플랫폼의 키맵 데이터 확인
//    /// </summary>
//    /// <returns></returns>
//    private List<Data_KeyMap> GetKeyMapList()
//    {
//        var list = new List<Data_KeyMap>();
//        //if (Application.platform == RuntimePlatform.Android)
//        //{
//        //    list = KeyMapTableManager.I.GetPlatformKeys($"{PlatformUtil.ePlatformType.Android}").ToList();
//        //}
//        //else if (Application.platform == RuntimePlatform.WindowsPlayer ||
//        //         Application.platform == RuntimePlatform.WindowsEditor)
//        //{
//        //    // var joystick = InputSystem.GetDevice<Joystick>();
//        //    // if (joystick != null)
//        //    // {
//        //    //     list = TableManager.I.KeyMapTable.DicKeyMapDatas.Values
//        //    //         .Where(x=>x.InputType == "Gamepad")
//        //    //         .ToList();
//        //    //
//        //    // }
//        //    // else
//        //    {
//        //        list = KeyMapTableManager.I.GetPlatformKeys($"{PlatformUtil.ePlatformType.PC}").ToList();
//        //    }
//        //}

//        return list;
//    }

//    public void SetKeyMap(int id, KeyCode mainKey, string combineKey)
//    {
//        if (_keyMapData.TryGetValue(id, out var data))
//        {
//            data.MainKey = mainKey.ToString();
//            data.CombineKey = combineKey;

//            _keyMapData[id] = data;
//        }
//    }
    
//    public KeyCode GetMainKey(string mainKey)
//    {   
//        var strKey = mainKey.ToUpper();
//        if (Enum.TryParse<KeyCode>(strKey, out var result))
//        {
//            return result;
//        }
        
//        return KeyCode.None;
//    }
    
//    /// <summary>
//    /// 키맵 데이터의 콤바인 키의 Left,Right 키 입력 여부 확인
//    /// </summary>
//    /// <param name="data"></param>
//    /// <returns></returns>
//    public int GetCombineKey(Data_KeyMap data)
//    {
//        var combineKeys = _allCombineKey.FindAll(x => x.ToString().Contains(data.CombineKey));
//        foreach (var key in combineKeys)
//        {
//            if (Input.GetKey(key))
//                return (int)key;
//        }

//        return -1;
//    }
    
//    public int GetCombineKey(string combineKey)
//    {
//        if (string.IsNullOrEmpty(combineKey))
//            return (int)KeyCode.None; 
        
//        var strKey = char.ToUpper(combineKey[0]) + combineKey.Substring(1).ToLower();
//        if (strKey.Equals("None"))
//            return (int)KeyCode.None;
        
//        var combineKeys = _allCombineKey.FindAll(x => x.ToString().Contains(strKey));
//        foreach (var key in combineKeys)
//        {
//            if (Input.GetKey(key))
//                return (int)key;
//        }
        
//        return -1;
//    }

//    public KeyCode GetIsPressMainKey()
//    {
//        var list = Enum.GetValues(typeof(KeyCode));
//        foreach (var key in list)
//        {
//            if (_allCombineKey.Contains((KeyCode)key))
//                continue;
            
//            if (Input.GetKeyDown((KeyCode)key))
//                return (KeyCode)key;
//        }

//        return KeyCode.None;
//    }
    
//    public KeyCode GetIsPressCombineKey()
//    {
//        foreach (var key in _allCombineKey)
//        {
//            if (Input.GetKey(key))
//                return key;
//        }

//        return KeyCode.None;
//    }
    
//    public bool CombineKeyDown(Data_KeyMap data)
//    {
//        if (data.PressType != Data_KeyMap.ePressType.Down)
//            return false;

//        if (data.IsUseCombineKey == false)
//            return Input.GetKeyDown(data.MainKeyCode());

//        return GetCombineKey(data) > 0 && Input.GetKeyDown(data.MainKeyCode());
//    }

//    public bool CombineKeyUp(Data_KeyMap data)
//    {
//        // if (data.PressType != KeyMapData.ePressType.Up)
//        //     return false;

//        if (data.IsUseCombineKey == false)
//            return Input.GetKeyUp(data.MainKeyCode());

//        return GetCombineKey(data) > 0 && Input.GetKeyUp(data.MainKeyCode());
//    }

//    public bool CombineKeyPress(Data_KeyMap data)
//    {
//        if (data.PressType != Data_KeyMap.ePressType.Press)
//            return false;

//        if (data.IsUseCombineKey == false)
//            return Input.GetKey(data.MainKeyCode());

//        return GetCombineKey(data) > 0 && Input.GetKey(data.MainKeyCode());
//    }
//}

//public partial class KeymapManager
//{
//    public void SaveAllKeyMap()
//    {
//        var list = KeyMapList;
//        foreach (var data in list)
//        {
//            //var tableData = KeyMapTableManager.I.GetData(data.ID);
//            //if (tableData == null)
//            //    continue;

//            //if (tableData.MainKeyCode() != data.MainKeyCode() ||
//            //    tableData.CombineKey != data.CombineKey)
//            //{
//            //    var key = GetPrefsKey(data);
//            //    var value = GetPrefsValue(data);
//            //    PlayerPrefs.SetString(key, value);
//            //}
//        }
//    }

//    public void ClearSavedKeyMap()
//    {
//        foreach (var data in KeyMapList)
//        {
//            var key = GetPrefsKey(data);
//            if (PlayerPrefs.HasKey(key))
//            {
//                PlayerPrefs.DeleteKey(key);
//            }
//        }

//        _keyMapData.Clear();
//        CheckLoadKeyMap();
//    }
    
//    public string GetPrefsKey(Data_KeyMap data)
//    {
//        return $"Keymap_{data.InputType}_{data.ID}";
//    }

//    public string GetPrefsValue(Data_KeyMap data)
//    {
//        var mainKey = data.MainKeyCode();
//        var combineKey = data.CombineKey;//char.ToUpper(data.CombineKey[0]) + data.CombineKey.Substring(1).ToLower();
//        return $"{mainKey}_{combineKey}";
//    }
//}