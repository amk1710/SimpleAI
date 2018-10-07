using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstatiateFood : MonoBehaviour {

    public int InitialFoodQuantity;
    public GameObject food_prefab;

    public float minX, maxX, minY, maxY;
    
    // Use this for initialization
	void Start () {
        //instacia N comidas, nas posições
        for(int i = 0; i < InitialFoodQuantity; i++)
        {
            GameObject.Instantiate(food_prefab, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0.0f), Quaternion.identity);
        }



    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
