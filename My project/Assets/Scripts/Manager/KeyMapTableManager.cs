using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum eContentsOpenType
{
    Escape = 1,
    
    W = 10,
    A,
    S,
    D,
    
    QucikSlot1,
    QuickSlot2,
    QuickSlot3,
    QuickSlot4,
    QuickSlot5,
    LeftPotion,
    RightPotion,
    ChangeSlotNextPage,
    ChangeSlotPrevPage,
    
    TargetAction = 31,
    RunAndWork,
    ChangeTarget,
    ChatAndFocus,
    ChangeChatTab,
    ChatEmotion,
    ChangeKeymap,
    MainMenu,
    ContentsMenu,
    Event,
    MailBox,
    Option,
    PetRide,
    AutoPlay,
    CashShop,
    GachaShop,
    MiniMap,
    
    CharacterInfo = 51,
    MercenaryInfo,
    PetInfo,
    Inventory,
    Skill,
    Party,
    Friend,
    Guild, 
    PersonalShop,
    AdventureNote,
    Collection,
    Album,
    Ranking,
    Mission,
    MaterialMercenary,
    MaterialScroll,
    Enchant,
    Smelt,
    CreateFood,
    CreateEquip,
    CreateAlchemy,
    CreateConsume,
    CreateCard,
    ComposeCard,
    ChangeFace,
    ChangeHair,
    FieldBoss,
    
    MainQuest,
    AutoQuest,
    PartyDungeon,
    DailyDungeon,
    EndlessDungeon,
}

public class Data_KeyMap
{
    public enum ePressType
    {
        None,
        Down,
        Press,
        Up,
    }
    
    public int ID;
    public string InputType;
    public string CombineKey;
    public string MainKey;
    public int StringKey;
    public int ContentsOpenType;
    public bool PossibleChange;
    public int PressNum;
    public List<int> HidingMapType;

    public string PrefabsKey => $"Keymap_{ID}";
    public bool IsUseCombineKey => string.IsNullOrEmpty(CombineKey) == false;
    public ePressType PressType => (ePressType)PressNum;

    public KeyCode MainKeyCode()
    {
        return Enum.TryParse<KeyCode>(MainKey, out var result)
            ? result
            : KeyCode.None;
    }

    public KeyCode CombineKeyCode()
    {
        return (KeyCode)KeymapManager.Instance.GetCombineKey(CombineKey);
    }

     public (KeyCode KeyCodeSub, KeyCode KeyCodeMain) CombinationKey
     {
         get => (CombineKeyCode(), MainKeyCode());
         
         set
         {
             CombineKey = value.Item1.ToString();
             MainKey = value.Item2.ToString();
         }
     }

    public Data_KeyMap()
    {
        
    }
    
    public Data_KeyMap(Data_KeyMap copyData)
    {
        ID = copyData.ID;
        InputType = copyData.InputType;
        CombineKey = copyData.CombineKey;
        MainKey = copyData.MainKey;
        ContentsOpenType = copyData.ContentsOpenType;
        PossibleChange = copyData.PossibleChange;
        PressNum = copyData.PressNum;
        StringKey = copyData.StringKey;
        HidingMapType = copyData.HidingMapType;
    }
    
    public Data_KeyMap(int ID, int StringKey, string CombineKey, string MainKey, int ContentsOpenType, List<int> HidingMapType)
    {
        this.ID = ID;
        this.CombineKey = CombineKey;
        this.MainKey = MainKey;
        this.ContentsOpenType = ContentsOpenType;
        this.StringKey = StringKey;
        this.HidingMapType = HidingMapType;
    }
}

public class KeyMapTableManager : Singleton<KeyMapTableManager>, ITableLoader
{
    private readonly Dictionary<string, Dictionary<int, Data_KeyMap>> _dicPlatformKeyMap = 
        new Dictionary<string, Dictionary<int, Data_KeyMap>>();
    
    /// 데이터를 로드한다
    public void LoadData()
    {
        _dicPlatformKeyMap.Clear();

        var loader = TableLoader.instance;
        using var reader = new CsvReader(loader.GetNewTableData("Data_KeyMap"));
        reader.EachReadLineAll(LoadTablePreset);
    }

    private void LoadTablePreset(CsvReader reader)
    {
        var table = new Data_KeyMap()
        {
            ID = reader.GetTabInt(),
            InputType = reader.GetTabString(),
            CombineKey = reader.GetTabString(),
            MainKey = reader.GetTabString(),
            StringKey = reader.GetTabInt(),
            ContentsOpenType = reader.GetTabInt(),
            PossibleChange = reader.GetTabBoolean(),
            PressNum = reader.GetTabInt(), 
            HidingMapType = reader.GetTabArray<int>(),
        };

        if (_dicPlatformKeyMap.TryGetValue(table.InputType, out var dictionary) == false)
        {
            dictionary = new Dictionary<int, Data_KeyMap>();
            _dicPlatformKeyMap.Add(table.InputType, dictionary);
        }

        dictionary[table.ID] = table;
    }

    public List<Data_KeyMap> GetTable()
    {
        var list = new List<Data_KeyMap>();
        foreach (var platformMap in _dicPlatformKeyMap)
        {
            list.AddRange(platformMap.Value.Values);
        }
        
        return list.Count > 0 ? list : null;
    }

    public List<Data_KeyMap> GetPlatformKeys(string platformType)
    {
        return _dicPlatformKeyMap.TryGetValue(platformType, out var dic)
            ? dic.Values.ToList()
            : null;
    }

    public Data_KeyMap GetData(int id)
    {
        // var index = dataKeyMapList.FindIndex(x => x.Number == number);
        // return index > -1 
        //     ? dataKeyMapList[index] 
        //     : null;
        
        foreach (var platformMap in _dicPlatformKeyMap)
        {
            foreach (var keyMap in platformMap.Value)
            {
                if (keyMap.Value.ID == id)
                {
                    return keyMap.Value;
                }
            }
        }

        return null;
    }
    
    public Data_KeyMap GetData(KeyCode key)
    {
        foreach (var platformMap in _dicPlatformKeyMap)
        {
            foreach (var keyMap in platformMap.Value)
            {
                if (keyMap.Value.MainKeyCode() == key)
                {
                    return keyMap.Value;
                }
            }
        }

        return null;
    }
}

// public class Data_KeyMap
// {
//     public int Number;
//     public int descriptionNum;
//     public KeyCode subKey;
//     public KeyCode mainKey;
//     public int openContentKey;
//     public HashSet<int> hidingMapType;
//     
//     public Data_KeyMap(int num, int descriptionNum, KeyCode subKey, KeyCode key, int openContentKey, HashSet<int> mapType)
//     {
//         this.Number = num;
//         this.descriptionNum = descriptionNum;
//         this.subKey = subKey;
//         this.mainKey = key;
//         this.openContentKey = openContentKey;
//         this.hidingMapType = mapType;
//     }
//
//     public (KeyCode KeyCodeSub, KeyCode KeyCodeMain) CombinationKey
//     {
//         get => (subKey, mainKey);
//         
//         set
//         {
//             subKey = value.Item1;
//             mainKey = value.Item2;
//         }
//     }
//
//     public void AddHidingMapType(int mapType)
//     {
//         hidingMapType?.Add(mapType);
//     }
// }