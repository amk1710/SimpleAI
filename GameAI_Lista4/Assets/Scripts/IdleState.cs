using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Fish))]
public class IdleState : MonoBehaviour {

    //state idle. Não faz nada.

    private Fish _fish;

    private void Awake()
    {
        _fish = this.gameObject.GetComponent<Fish>();
        this.enabled = false;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
