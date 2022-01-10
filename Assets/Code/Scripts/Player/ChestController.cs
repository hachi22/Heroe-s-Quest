using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject chest;
    public void Interact()
    {
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        playerController.potion++;
        chest.SetActive(false);
    }
}
