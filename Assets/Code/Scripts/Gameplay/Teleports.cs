using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleports : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] PlayerController playercontroller;
    [SerializeField] GameObject arenaBattleMap;
    [SerializeField] GameObject arenaBattleCave;
    [SerializeField] GameObject arenaBattleForest;
    [SerializeField] Animator transition;
    [SerializeField] GameObject panelTransitions;


    IEnumerator OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "TeleportPueblo")
        {

            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(-8.5f, -3.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
            FindObjectOfType<AudioManager>().Stop("MusicaMapamundi");
            FindObjectOfType<AudioManager>().Play("MusicaPueblo");
        }

        if (collider.gameObject.tag == "TeleportCueva")
        {
            yield return StartCoroutine(Transition());

            arenaBattleCave.SetActive(true);
            player.transform.position = new Vector3(-43.5f, 49.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
            FindObjectOfType<AudioManager>().Stop("MusicaMapamundi");
            FindObjectOfType<AudioManager>().Play("MusicaCueva");
        }

        if (collider.gameObject.tag == "TeleportMapamundiCueva")
        {

            yield return StartCoroutine(Transition());

            arenaBattleMap.SetActive(true);
            player.transform.position = new Vector3(-106.5f, 6.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
            FindObjectOfType<AudioManager>().Stop("MusicaCueva");
            FindObjectOfType<AudioManager>().Play("MusicaMapamundi");
        }

        if (collider.gameObject.tag == "TeleportCueva2")
        {
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(-8.5f, 70.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
        }

        if (collider.gameObject.tag == "TeleportVolverCueva2")
        {
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(-46.5f, 59.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
        }

        if (collider.gameObject.tag == "TeleportCueva3")
        {
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(-44.5f, 76.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
        }

        if (collider.gameObject.tag == "TeleportVolverCueva3")
        {
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(-17.5f, 104.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
        }

        if (collider.gameObject.tag == "TeleportBosque")
        {
            yield return StartCoroutine(Transition());

            arenaBattleForest.SetActive(true);
            player.transform.position = new Vector3(56.5f, 69.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
            FindObjectOfType<AudioManager>().Stop("MusicaMapamundi");
            FindObjectOfType<AudioManager>().Play("MusicaBosque");
        }

        if (collider.gameObject.tag == "TeleportMapamundiBosque")
        {

            yield return StartCoroutine(Transition());

            arenaBattleMap.SetActive(true);
            player.transform.position = new Vector3(-106.5f, 2.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
            FindObjectOfType<AudioManager>().Stop("MusicaBosque");
            FindObjectOfType<AudioManager>().Play("MusicaMapamundi");

        }

        if (collider.gameObject.tag == "TeleportBoss")
        {
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(66.5f, 137.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
        }

        if (collider.gameObject.tag == "TeleportVolverBoss")
        {
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(66.5f, 113.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
        }

        if (collider.gameObject.tag == "TeleportMapamundiPueblo")
        {
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(-117.5f, 0.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
            FindObjectOfType<AudioManager>().Stop("MusicaPueblo");
            FindObjectOfType<AudioManager>().Play("MusicaMapamundi");
        }

        if (collider.gameObject.tag == "TeleportVolverCasa")
        {
            FindObjectOfType<AudioManager>().Play("SonidoPuerta");
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(3.5f, 5.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
        }

        if (collider.gameObject.tag == "TeleportVolverCasa2")
        {
            FindObjectOfType<AudioManager>().Play("SonidoPuerta");
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(-23.5f, 14.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
        }

        if (collider.gameObject.tag == "TeleportVolverCasa3")
        {
            FindObjectOfType<AudioManager>().Play("SonidoPuerta");
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(-35.5f, 24.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
        }

        if (collider.gameObject.tag == "TeleportVolverCasaArmas")
        {
            FindObjectOfType<AudioManager>().Play("SonidoPuerta");
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(3.5f, 23.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
        }

        if (collider.gameObject.tag == "TeleportVolverCasaObjetos")
        {
            FindObjectOfType<AudioManager>().Play("SonidoPuerta");
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(13.5f, 23.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
        }

        if (collider.gameObject.tag == "TeleportCasa")
        {
            FindObjectOfType<AudioManager>().Play("SonidoPuerta");
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(-86.5f, 33.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
        }

        if (collider.gameObject.tag == "TeleportCasa2")
        {
            FindObjectOfType<AudioManager>().Play("SonidoPuerta");
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(-86.5f, 58.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
        }

        if (collider.gameObject.tag == "TeleportCasa3")
        {
            FindObjectOfType<AudioManager>().Play("SonidoPuerta");
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(-86.5f, 82.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
        }

        if (collider.gameObject.tag == "TeleportCasaArmas")
        {
            FindObjectOfType<AudioManager>().Play("SonidoPuerta");
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(-122.5f, 33.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
        }

        if (collider.gameObject.tag == "TeleportCasaObjetos")
        {
            FindObjectOfType<AudioManager>().Play("SonidoPuerta");
            yield return StartCoroutine(Transition());

            player.transform.position = new Vector3(-122.5f, 49.5f, 0f);
            ResetearPlayer();

            panelTransitions.SetActive(false);
        }
    }

    

    public void ResetearPlayer()
    {
        player.SetActive(false);
        player.SetActive(true);
        playercontroller.canMove = true;
    }

    public IEnumerator Transition()
    {
        panelTransitions.SetActive(true);
        transition.SetTrigger("end");
        playercontroller.canMove = false;
        yield return new WaitForSeconds(1.5f);
    }
}
