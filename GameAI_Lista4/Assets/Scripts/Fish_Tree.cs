using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//condição em que pode estar cada nó ação
public enum Condition { Success, Running, Failed }

[RequireComponent(typeof(Food))]
[RequireComponent(typeof(Rigidbody2D))]
public class Fish_Tree : MonoBehaviour {

    public float MovementSpeed;
    public float MaxRotationAngle;
    public int QtdWalkFrames;

    private Food _food;
    private Rigidbody2D _rb;
    private Renderer _renderer;
    
    //usa corotinas pra fazer implementar ações na árvore
    //cada nó é um array de metodos que retornam uma condição

    private Func<Condition>[] ChaseFood;
    private List<Func<Condition>> Roam;

    private GameObject _foundFood = null;

    // Use this for initialization
	void Start () {
        _food = GetComponent<Food>();
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();

        ChaseFood = new Func<Condition>[] { CheckFoodDetected, ChaseAction };
        Roam = new List<Func<Condition>>();
        for(int i = 0; i < QtdWalkFrames; i++)
        {
            Roam.Add(Walk);
        }
        Roam.Add(Turn);

        StartCoroutine("BehaviourTree");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator BehaviourTree()
    {
        //nó raiz, sempre executa na ordem
        while (true)
        {
            //nó seletor, executa na ordem enquanto condição der true
            //Chase Food
            for (int i = 0; i < ChaseFood.Length; i++)
            {
                Condition c = ChaseFood[i]();
                if(c == Condition.Success)
                {

                    yield return null;
                    Debug.Log("continue1");
                    continue;
                }
                else if(c == Condition.Running)
                {
                    yield return null;
                    i--;
                }
                else if(c == Condition.Failed)
                {
                    break;
                }
            }

            //nó seletor, executa na ordem enquanto condição der true
            //Roam
            for (int i = 0; i < Roam.Count; i++)
            {
                Condition c = Roam[i]();
                if (c == Condition.Success)
                {
                    yield return null;
                    continue;
                }
                else if (c == Condition.Running)
                {
                    yield return null;
                    i--;
                }
                else if (c == Condition.Failed)
                {
                    break;
                }
            }
            
        }
    }

    private Condition CheckFoodDetected()
    {
        if (_foundFood != null)
        {
            Debug.LogWarning("check food success");
            return Condition.Success;
        }
        else return Condition.Failed;
    }

    private Condition ChaseAction()
    {
        if (_foundFood == null) return Condition.Failed;
        else
        {
            //vira para apontar ao chased

            //pos final menos inicial
            Vector3 point = _foundFood.transform.position - this.transform.position;
            //angulo em graus
            float angle = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);

            //anda em direção ao chased
            _rb.AddRelativeForce(Vector2.right * MovementSpeed);

            return Condition.Running;
        }
        

        
        
    }

    private Condition Walk()
    {
        //anda pra frente
        _rb.AddRelativeForce(Vector2.right * MovementSpeed);
        return Condition.Success;
    }

    private Condition Turn()
    {
        if (!_renderer.isVisible)
        {
            //pos final menos inicial
            Vector3 point = Vector3.zero - this.transform.position;
            //angulo em graus
            float angle = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        }
        else
        {
            transform.Rotate(0.0f, 0.0f, UnityEngine.Random.Range(-MaxRotationAngle, MaxRotationAngle));
        }
        return Condition.Success;
    }

    private Condition Idle()
    {
        return Condition.Success;
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //necessário pq mesmo quando não enabled essa função é chamada
        if (!enabled) return;

        Food colfood = collision.gameObject.GetComponent<Food>();
        if (colfood != null && _foundFood == null && colfood.FoodValue < _food.FoodValue)
        {
            _foundFood = colfood.gameObject;
        }

        
    }

    //se saiu de range para de chasear
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == _foundFood)
        {
            _foundFood = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Food colFood = collision.gameObject.GetComponent<Food>();

        //se colidido é comida e meu tamanho é maior que o da comida:
        if (colFood != null && _food.FoodValue > colFood.FoodValue)
        {
            //come comida
            _food.FoodValue += colFood.FoodValue;
            GameObject.Destroy(colFood.gameObject);
            _foundFood = null;
        }
    }




}
