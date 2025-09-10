using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [SerializeField] private TableLoader tableLoader;

    private Dictionary<int, DungeonDailyData> dicDungeonDailyDatas = new Dictionary<int, DungeonDailyData>();
    private Dictionary<int, DungeonDaily1Data> dicDungeonDaily1Datas = new Dictionary<int, DungeonDaily1Data>();
    private Dictionary<int, DungeonDaily2Data> dicDungeonDaily2Datas = new Dictionary<int, DungeonDaily2Data>();
    private Dictionary<int, DungeonDaily3Data> dicDungeonDaily3Datas = new Dictionary<int, DungeonDaily3Data>();
    private Dictionary<int, DungeonDaily4Data> dicDungeonDaily4Datas = new Dictionary<int, DungeonDaily4Data>();

    private void Start()
    {
        tableLoader.LoadTable("Data_Dungeon_Daily",ref dicDungeonDailyDatas);
        tableLoader.LoadTable("Data_Dungeon_Daily1",ref dicDungeonDaily1Datas);
        tableLoader.LoadTable("Data_Dungeon_Daily2",ref dicDungeonDaily2Datas);
        tableLoader.LoadTable("Data_Dungeon_Daily3",ref dicDungeonDaily3Datas);
        tableLoader.LoadTable("Data_Dungeon_Daily4",ref dicDungeonDaily4Datas);
    }
}
