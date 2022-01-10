using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] LayerMask midgroundSolid;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask battleArenaMap;
    [SerializeField] LayerMask battleArenaCave;
    [SerializeField] LayerMask battleArenaForest;
    [SerializeField] LayerMask bossFight;
    [SerializeField] GameObject tileMapMap;
    [SerializeField] GameObject tileMapCave;
    [SerializeField] GameObject tileMapForest;
    [SerializeField] GameObject backgroundMap;
    [SerializeField] GameObject backgroundForest;
    [SerializeField] GameObject backgroundCave;
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject midBoss;

    public int potion;

    public int Potion
    {
        get
        {
            return potion;
        }
    }


    public event Action OnEncountered;
    public event Action<Collider2D> OnBattleBoss;

    [SerializeField] PlayerBattle playerBattle;
    private bool isMoving;
    private Vector2 input;

    private Animator animator;

    [SerializeField] public bool canMove;

    private void Awake()
    {

        animator = GetComponent<Animator>();

        canMove = true;

        CheckMusic();
    }


    public void HandleUpdate()
    {
        if (!canMove)
        {
            isMoving = false;
            return;
        }

        if (!isMoving)
        {
            Zones();
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");  //inputs para moverse

            if (input.x != 0) input.y = 0; // quitar movimiento diagonal

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);//animaciones idle
                var targetPos = transform.position;//posicion del jugador ahora mismo
                targetPos.x += input.x;
                targetPos.y += input.y;//posicion del jugador ahora mismo pero mas el input que le des(el input siempre es 1(derecha) o -1(izquierda)

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos)); // Inicia el IEnumerator
                }

            }
        }

        animator.SetBool("isMoving", isMoving); //controlar animaciones de caminar

        if (Input.GetKeyDown(KeyCode.Z))
            Interact();
            CheckIfBoss();
            
        if (Input.GetKeyDown(KeyCode.P))
        {
            SavePlayer();
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("movex"), animator.GetFloat("moveY"));
        var interactPosition = transform.position + facingDir; //Interactua segun la posicion donde este mirando

        var collider = Physics2D.OverlapCircle(interactPosition, 0.3f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    IEnumerator Move(Vector3 targetPos)//IEnumerator sirve para hacer algo en un periodo de tiempo (en este caso mover al jugador de una tile a otra)
    {
        isMoving = true;// comienza a moverse

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) //Esto es para controlar que la distancia antes de llegar al siguiente tile sea muy pequeña, Epsilon es un numero muy pequeño es solo un ejemplo. Básicamente hace el movimiento smooth
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;// cuando queda muy poca distancia basicamente hace un tp al siguiente tile, pero como hay muy poca distancia no se ve

        isMoving = false;//se para

        CheckForEncounters();
      
    }

    private bool IsWalkable(Vector3 targetPos)//mira si se puede caminar por encima de la tile
    {
        if (Physics2D.OverlapCircle(targetPos, 0.3f, midgroundSolid | interactableLayer) != null)
        {
            
            return false;
        }
        return true;
    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.3f, battleArenaMap | battleArenaCave | battleArenaForest) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 15)
            {
                animator.SetBool("isMoving", false);
                OnEncountered();
            }
        }
    }

    private void CheckIfBoss()
    {
        
        var interactPosition = transform.position; //Interactua segun la posicion donde este mirando

        var collider = Physics2D.OverlapCircle(interactPosition, 0.3f, interactableLayer);
        if (collider != null)
        {
            animator.SetBool("isMoving", false);
            OnBattleBoss?.Invoke(collider);
        }
    }

    public void Zones()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.3f, battleArenaMap) != null)
        {
            tileMapMap.SetActive(true);
            tileMapCave.SetActive(false);
            tileMapForest.SetActive(false);

            backgroundMap.SetActive(true);
            backgroundForest.SetActive(false);
            backgroundCave.SetActive(false);

            

        } else if (Physics2D.OverlapCircle(transform.position, 0.3f, battleArenaCave) != null)
        {
            tileMapMap.SetActive(false);
            tileMapCave.SetActive(true);
            tileMapForest.SetActive(false);

            backgroundMap.SetActive(false);
            backgroundForest.SetActive(false);
            backgroundCave.SetActive(true);

           
        }
        else if (Physics2D.OverlapCircle(transform.position, 0.3f, battleArenaForest) != null)
        {
            
            tileMapMap.SetActive(false);
            tileMapCave.SetActive(false);
            tileMapForest.SetActive(true);

            backgroundMap.SetActive(false);
            backgroundForest.SetActive(true);
            backgroundCave.SetActive(false);

           
        }
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this, playerBattle);
    }

   

    public void CheckMusic()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.3f, battleArenaMap) != null)
        {
            FindObjectOfType<AudioManager>().Play("MusicaMapamundi");

        }
        else if (Physics2D.OverlapCircle(transform.position, 0.3f, battleArenaCave) != null)
        {
            FindObjectOfType<AudioManager>().Play("MusicaCueva");
        }
        else if (Physics2D.OverlapCircle(transform.position, 0.3f, battleArenaForest) != null)
        {
            FindObjectOfType<AudioManager>().Play("MusicaBosque");
        }
    }
           
}


