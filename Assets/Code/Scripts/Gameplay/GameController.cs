using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle, Dialog, Cutscene }

public class GameController : MonoBehaviour //Es para dar el control o en la batalla o en el mundo princupal, no en las dos a la vez
{

    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] MapArea battleArenaMap;
    [SerializeField] GameObject panelTransitions;
    [SerializeField] Animator transition;


    GameState state;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        battleArenaMap = GetComponent<MapArea>();
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;


        playerController.OnBattleBoss += (Collider2D bossCollider) =>
        {
          var boss = bossCollider.GetComponent<BossController>();
            if (boss != null)
            {
                state = GameState.Cutscene;
                boss.Interact();
                
            }
            
        };

        DialogManager.Instance.onShowDialog += () =>
        {
            state = GameState.Dialog;
        };

        DialogManager.Instance.onCloseDialog += () =>
        {
            if (state == GameState.Dialog)
            state = GameState.FreeRoam;
        };
    }

    void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerBattle = playerController.GetComponent<PlayerBattle>();
        var enemyFighter = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomenemyFighters();

        battleSystem.StartBattle(playerBattle, enemyFighter);

        
    }

    public void StartBossBattle(BossController boss)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerBattle = playerController.GetComponent<PlayerBattle>();
        var bossBattle = boss.GetComponent<PlayerBattle>();

        
        FindObjectOfType<AudioManager>().Play("BossTheme");
        battleSystem.StartBossBattle(playerBattle, bossBattle);
        
    }

    void EndBattle(bool won)
    {
        
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
        playerController.CheckMusic();
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
           playerController.HandleUpdate();
        }      
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }

    }

   
}

