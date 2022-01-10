using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour { 

    public GameObject player;
    private Vector3 position;

   
    void Start() {

        position = transform.position - player.transform.position;
        
    }

 
    void Update() {

        transform.position = player.transform.position + position;
        
    }
}
