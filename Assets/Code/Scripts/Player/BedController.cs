using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] BattleUnit HP;
    
    public void Interact()
    {
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
     
        HP.Fighter.HP = HP.Fighter.MaxHp;
                  
    }
}
