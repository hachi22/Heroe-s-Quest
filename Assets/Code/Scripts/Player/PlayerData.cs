using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    public int level;
    public int health;
    public int exp;
    public float[] position;

    public PlayerData (PlayerController player, PlayerBattle playerBattle)
    {
        level = playerBattle.GetFighter().level;
        health = playerBattle.GetFighter().HP;
        exp = playerBattle.GetFighter().Exp;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
