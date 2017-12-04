using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    private GameMaster _gms;

    public GameObject CollectibleEffect;

	void Start () {
        _gms = FindObjectOfType<GameMaster>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Missil")
        
            Collect();
        
    }

    void Collect()
    {
        Instantiate(CollectibleEffect, transform.position, transform.rotation);

        _gms.CollectiblesCollected++;

        Destroy(gameObject);
    }
}
