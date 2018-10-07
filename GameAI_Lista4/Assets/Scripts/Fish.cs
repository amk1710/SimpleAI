using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//todo peixe também é comida(para outros peixes)
[RequireComponent(typeof(Food))]
[RequireComponent(typeof(Collider2D))]
public class Fish : MonoBehaviour {

    private Food _myFood;
    public float MovementSpeed;

    public MonoBehaviour _currentState;

    public void ChangeState(MonoBehaviour state)
    {
        _currentState.enabled = false;
        state.enabled = true;
        _currentState = state;
    }
    
    // Use this for initialization
	void Start () {

        _myFood = this.GetComponent<Food>();

        //enable primeiro estado
        _currentState.enabled = true;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Food food = collision.gameObject.GetComponent<Food>();
        
        //se colidido é comida e meu tamanho é maior que o da comida:
        if (food != null && _myFood.FoodValue > food.FoodValue)
        {
            //come comida
            _myFood.FoodValue += food.FoodValue;
            GameObject.Destroy(food.gameObject);
        }
    }

}
