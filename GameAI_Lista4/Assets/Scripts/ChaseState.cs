using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Fish))]
public class ChaseState : MonoBehaviour {

    public GameObject _chased;
    private Fish _fish;
    private Rigidbody2D _rb;

    void Awake()
    {
        _fish = this.gameObject.GetComponent<Fish>();

        _rb = this.gameObject.GetComponent<Rigidbody2D>();

        _chased = GameObject.Find("Food");

        this.enabled = false;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //transições
        if(_chased == null)
        {
            _fish.ChangeState(this.gameObject.GetComponent<RoamState>());
        }
       
	}

    private void FixedUpdate()
    {
        //vira para apontar ao chased
        
        //pos final menos inicial
        Vector3 point = _chased.transform.position - this.transform.position;
        //angulo em graus
        float angle = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);

        //anda em direção ao chased
        _rb.AddRelativeForce(Vector2.right * _fish.MovementSpeed);

    }

    //se saiu de range para de chasear
    private void OnCollisionExit2D(Collision2D collision)
    {
        Food food = collision.gameObject.GetComponent<Food>();

        //se colidido é comida,
        if (food != null)
        {
            _chased = null;
        }
    }
}
