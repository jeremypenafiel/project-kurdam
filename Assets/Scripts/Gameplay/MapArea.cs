using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<AswangEncounterRecord> wildAswangs;

    private void Start()
    {
        int totalChance = 0;
        foreach (var record in wildAswangs)
        {
            record.chanceLower = totalChance;
            record.chanceUpper = totalChance + record.chancePercentage;

            totalChance = totalChance + record.chancePercentage;
        }
    }
    public Aswang GetRandomWildAswang()
    {
        int randVal = Random.Range(1, 101);
        var aswangRecord = wildAswangs.First(a => randVal >= a.chanceLower && randVal <= a.chanceUpper);    
        var levelRange = aswangRecord.levelRange;
        int level = levelRange.y == 0 ? levelRange.x : Random.Range(levelRange.x, levelRange.y + 1);

        var wildAswang = new Aswang(aswangRecord.aswangData, level);


        wildAswang.Init();
        return wildAswang;

     
    }
}

[System.Serializable]
public class AswangEncounterRecord
{
    [FormerlySerializedAs("aswangBase")] public AswangData aswangData;
    public Vector2Int levelRange;
    public int chancePercentage;

    public int chanceLower {  get; set; }
    public int chanceUpper {  get; set; }
}
