using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayer : MonoBehaviour
{
    [SerializeField] PlayerBattle playerBattle;
    [SerializeField] PlayerController playerController;

    
    void Start()
    {
        if (SplashFade.partidaNueva == true)
        {
            
            GameObject.Find("Main Camera").transform.position = new Vector3(-79.5f, 65.5f, 0.0f);
            
            playerBattle.GetFighter().level = 4;

            Vector3 position;
            position.x = -79.5f;
            position.y = 65.5f;
            position.z = 0.0f;
            transform.position = position;
            FindObjectOfType<AudioManager>().Play("MusicaPueblo");
        }
        else
        {
            PlayerData data = SaveSystem.LoadPlayer();

            playerBattle.GetFighter().level = data.level;
            playerBattle.GetFighter().HP = data.health;
            playerBattle.GetFighter().Exp = data.exp;

            Vector3 position;
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];
            transform.position = position;
            

        }
    }
}

