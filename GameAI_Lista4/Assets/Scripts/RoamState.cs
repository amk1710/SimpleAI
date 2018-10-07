using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Fish))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Food))]
public class RoamState : MonoBehaviour
{

    //state roam. Anda por aí e muda de estado se encontra comida

    private Fish _fish;
    private Food _food;
    private Food _foundFood;
    private ChaseState _cs;
    private Rigidbody2D _rb;
    private Renderer _renderer;

    public float MaxRotationAngle;
    public float ChangeDirectionTimer;
    private float t;

    private void Awake()
    {
        _fish = gameObject.GetComponent<Fish>();
        _cs = gameObject.GetComponent<ChaseState>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _renderer = gameObject.GetComponent<Renderer>();
        _food = gameObject.GetComponent<Food>();

        this.enabled = false;
    }

    // Use this for initialization
    void Start()
    {

    }

    //quando começa o estado
    public void OnEnable()
    {
        _foundFood = null;
        t = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //behaviour
        //anda por aí, procurando por comida(pela função OnTriggerEnter2D)
                
        //se saiu da visão da camêra, aponta para o centro:

        if(!_renderer.isVisible)
        {
            //pos final menos inicial
            Vector3 point = Vector3.zero - this.transform.position;
            //angulo em graus
            float angle = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        }
        else
        {
            t += Time.deltaTime;
            if (t > ChangeDirectionTimer)
            {
                t = 0.0f;
                transform.Rotate(0.0f, 0.0f, Random.Range(-MaxRotationAngle, MaxRotationAngle));
            }
        }


        //transição
        
        if (_foundFood != null)
        {
            //se achou comida menor que si mesmo, persegue
            if(_food.FoodValue > _foundFood.FoodValue)
            {
                _cs._chased = _foundFood.gameObject;
                _fish.ChangeState(_cs);
            }
            else if(_food.FoodValue < _foundFood.FoodValue)
            {
                //to do
            }
            
        }
    }

    private void FixedUpdate()
    {
        //anda por aí
        _rb.AddRelativeForce(Vector2.right * _fish.MovementSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //necessário pq mesmo quando não enabled essa função é chamada
        if (!enabled) return;

        Food food = collision.gameObject.GetComponent<Food>();
        if (food != null)
        {
            Debug.Log("Found food");
            _foundFood = food;
        }
    }
}
