using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour,Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject boss;
    [SerializeField] GameObject wall;

    public void Interact()
    {
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () =>
        {
            GameController.Instance.StartBossBattle(this);
            boss.SetActive(false);
            wall.SetActive(false);

        }));
    }

}
