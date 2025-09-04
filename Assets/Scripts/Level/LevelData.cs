using System;
using System.Collections.Generic;

[Serializable]
public class LevelData
{
    public int Rows = 8;
    public int Columns = 4;

    public List<LevelEnemyData> EnemyDataList = new List<LevelEnemyData>();

    public List<LevelDefenceItemData> DefenceItemsDataList = new List<LevelDefenceItemData>();

    public LevelData(int rows, int columns)
    {
        this.Rows = rows;
        this.Columns = columns;
    }
}
