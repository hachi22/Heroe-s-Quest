using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Fighter> enemyFighters;

    public Fighter GetRandomenemyFighters()
    {
        var enemyFighter = enemyFighters[ Random.Range(0, enemyFighters.Count)];
        enemyFighter.Init();
        return enemyFighter;
    }
}
