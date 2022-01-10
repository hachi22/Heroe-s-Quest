using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy}

public class BattleSystem : MonoBehaviour
{

    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] GameObject panelTransitions;
    [SerializeField] Animator transition;
    [SerializeField] GameObject finalBoss;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentMove;

    PlayerBattle playerBattle;
    Fighter enemyFighter;
    PlayerBattle bossBattle;

    [SerializeField] PlayerController playerController;
    BossController bossController;

    bool isBossBattle = false;

    public void StartBattle(PlayerBattle playerBattle, Fighter enemyFighter)
    {
        this.playerBattle = playerBattle;
        this.enemyFighter = enemyFighter;

        StartCoroutine(SetupBattle());

        FindObjectOfType<AudioManager>().Play("BattleTheme");
        FindObjectOfType<AudioManager>().Stop("MusicaMapamundi");
        FindObjectOfType<AudioManager>().Stop("MusicaBosque");
        FindObjectOfType<AudioManager>().Stop("MusicaCueva");
    }

    public void StartBossBattle(PlayerBattle playerBattle, PlayerBattle bossBattle)
    {
        this.playerBattle = playerBattle;
        this.bossBattle = bossBattle;

        isBossBattle = true;

        playerController = playerBattle.GetComponent<PlayerController>();
        bossController = bossBattle.GetComponent<BossController>();

        StartCoroutine(SetupBattle());

        FindObjectOfType<AudioManager>().Stop("BattleTheme");
        FindObjectOfType<AudioManager>().Stop("MusicaMapamundi");
        FindObjectOfType<AudioManager>().Stop("MusicaBosque");
        FindObjectOfType<AudioManager>().Stop("MusicaCueva");
    }

    public IEnumerator SetupBattle()
    {
        if (!isBossBattle)
        {
            playerUnit.Setup(playerBattle.GetFighter());
            enemyUnit.Setup(enemyFighter);

            dialogBox.SetMoveNames(playerUnit.Fighter.Moves);
            playerHud.SetData(playerUnit.Fighter);
            enemyHud.SetData(enemyUnit.Fighter);


            yield return dialogBox.TypeDialog($"A {enemyUnit.Fighter.Base.Name} appeared.");
        }
        else
        {
            playerUnit.Setup(playerBattle.GetFighter());
            enemyUnit.Setup(bossBattle.GetFighter());

            dialogBox.SetMoveNames(playerUnit.Fighter.Moves);
            playerHud.SetData(playerUnit.Fighter);
            enemyHud.SetData(enemyUnit.Fighter);


            yield return dialogBox.TypeDialog($"{enemyUnit.Fighter.Base.Name} challenges you.");
        }

        
        

        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        state = BattleState.Busy;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
        state = BattleState.PlayerAction;
    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PerformPlayerMove()//player ataca y enemigo recibe daño
    {
        state = BattleState.Busy;

        var move = playerUnit.Fighter.Moves[currentMove];
        yield return dialogBox.TypeDialog($"{playerUnit.Fighter.Base.Name} used {move.Base.Name}");
        if (!move.Base.IsMagick)
        {
            FindObjectOfType<AudioManager>().Play("AtaqueFisico");
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("AtaqueMagico");
        }
            

        playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        enemyUnit.PlayHitAnimation();

        var damageDetails = enemyUnit.Fighter.TakeDamage(move, playerUnit.Fighter);
        yield return enemyHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            FindObjectOfType<AudioManager>().Play("DeathEffect");
            enemyUnit.PlayFaintAnimation();
            yield return dialogBox.TypeDialog($"{enemyUnit.Fighter.Base.Name} Fainted");

            int expYield = enemyUnit.Fighter.Base.ExpYield;
            int enemyLevel = enemyUnit.Fighter.Level;

            int expGain = (expYield * enemyLevel) / 4; // formula para ganar exp
            playerUnit.Fighter.Exp += expGain;

            yield return dialogBox.TypeDialog($"{playerUnit.Fighter.Base.Name} gained {expGain} exp");
            
            yield return playerUnit.Hud.SetExpSmooth();
           

           if (playerUnit.Fighter.CheckForLevelUp())
            {
                playerUnit.Hud.SetLevel();
                yield return dialogBox.TypeDialog($"{playerUnit.Fighter.Base.Name} level up!");
                yield return playerUnit.Hud.SetExpSmooth(true);
            }

            yield return new WaitForSeconds(1f);
            FindObjectOfType<AudioManager>().Stop("BattleTheme");

            if(!finalBoss.activeSelf == false)
            {
                OnBattleOver(true); // Batalla ganada
            }
            else
                SceneManager.LoadScene("End");


        }
        else
        { 
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;


        var move = enemyUnit.Fighter.GetRandomMove();
        move.PP--;
        yield return dialogBox.TypeDialog($"{enemyUnit.Fighter.Base.Name} used {move.Base.Name}");

        if (!move.Base.IsMagick)
        {
            FindObjectOfType<AudioManager>().Play("AtaqueEnemigo");
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("AtaqueMagico");
        }

        enemyUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        playerUnit.PlayHitAnimation();

        var damageDetails = playerUnit.Fighter.TakeDamage(move, enemyUnit.Fighter);
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Fighter.Base.Name} Fainted");
            playerUnit.PlayFaintAnimation();

            
            FindObjectOfType<AudioManager>().Stop("BattleTheme");
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("GameOverScreen"); //GameOver

        }
        else
        {
            PlayerAction();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
            yield return dialogBox.TypeDialog("A critical hit!");

        if (damageDetails.TypeEffectiveness > 1f)
            yield return dialogBox.TypeDialog("It's super effective");

        else if (damageDetails.TypeEffectiveness < 1f)
            yield return dialogBox.TypeDialog("It's not very effective");
    }

    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FindObjectOfType<AudioManager>().Play("SonidoElegir");
            ++currentAction;
        }
            
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FindObjectOfType<AudioManager>().Play("SonidoElegir");
            --currentAction;
        }
           
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            FindObjectOfType<AudioManager>().Play("SonidoElegir");
            currentAction += 2;
        }
            
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            FindObjectOfType<AudioManager>().Play("SonidoElegir");
            currentAction -= 2;
        }
          
        currentAction = Mathf.Clamp(currentAction, 0, 3);

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                FindObjectOfType<AudioManager>().Play("SonidoElegir");
                Attack();
            }
            else if (currentAction == 1)
            {
                //Fight
                FindObjectOfType<AudioManager>().Play("SonidoElegir");
                PlayerMove();
            }
            else if (currentAction == 2)
            {
                FindObjectOfType<AudioManager>().Play("SonidoElegir");
                StartCoroutine(UsePotion());
            }
            else if (currentAction == 3)
            {
                //Escape
                FindObjectOfType<AudioManager>().Play("SonidoElegir");
                StartCoroutine(Escape());
            }
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < playerUnit.Fighter.Moves.Count - 1)
            {
                FindObjectOfType<AudioManager>().Play("SonidoElegir");
                currentMove++;
            }

                
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0)
            {
                FindObjectOfType<AudioManager>().Play("SonidoElegir");
                currentMove--;
            }

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < playerUnit.Fighter.Moves.Count - 2)
            {
                FindObjectOfType<AudioManager>().Play("SonidoElegir");
                currentMove += 2;
            }

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
            {
                FindObjectOfType<AudioManager>().Play("SonidoElegir");
                currentMove -= 2;
            }
        }

        currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Fighter.Moves.Count - 1);

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Fighter.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            FindObjectOfType<AudioManager>().Play("SonidoElegir");
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            PlayerAction();
        }

    }

    void Attack()
    {
        playerUnit.Fighter.Moves[currentMove] = playerUnit.Fighter.Moves[0];
        StartCoroutine(PerformPlayerMove());
    }

    IEnumerator Escape()
    {
        state = BattleState.Busy;
        
        if (isBossBattle)
        {
            yield return dialogBox.TypeDialog($"You cant run from boss battles");                //Para las boss battles o batallas de historia
            state = BattleState.PlayerAction; 
            yield break;
        }
        

        if (UnityEngine.Random.Range(1, 101) <= 60)// 60% de huir
        {
            
            yield return dialogBox.TypeDialog($"You run away!");
            
            OnBattleOver(true);
            FindObjectOfType<AudioManager>().Stop("BattleTheme");
        }

        else
        {
            state = BattleState.Busy;
            yield return dialogBox.TypeDialog($"You failed to escape!");
            state = BattleState.EnemyMove;
            StartCoroutine(EnemyMove());
        }

    }

    IEnumerator Transition()
    {
        panelTransitions.SetActive(true);
        transition.SetTrigger("end");
        yield return new WaitForSeconds(1.5f);
        panelTransitions.SetActive(false);
    }

    IEnumerator UsePotion()
    {
        if (playerController.potion != 0)
        {
            state = BattleState.Busy;
            yield return dialogBox.TypeDialog($"You used 1 potion!");
            playerUnit.Fighter.HP = playerUnit.Fighter.MaxHp;
            if (playerUnit.Fighter.HP >= playerUnit.Fighter.MaxHp)
            {
                playerUnit.Fighter.HP = playerUnit.Fighter.MaxHp;
            }
             yield return playerHud.UpdateHP();
            if (playerController.potion > 0)
                playerController.potion--;
            yield return dialogBox.TypeDialog($"You have {playerController.potion} potions left");
            state = BattleState.EnemyMove;
            StartCoroutine(EnemyMove());

        }
        else
        {
            state = BattleState.Busy;
            yield return dialogBox.TypeDialog($"You don't have any potions");
            state = BattleState.PlayerAction;
        }
            


    }

}

