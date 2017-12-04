using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public int HP = 1000;

    public GameObject DieEffect;
    public Text HPText;

    GameMaster _gms;
    private void Start()
    {
        _gms = FindObjectOfType<GameMaster>();
        HPText.text = HP.ToString();
    }

    public void TakeDamage(int damageToTake)
    {
        HP -= damageToTake;
        HPText.text = HP.ToString();

        if(HP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(DieEffect, transform.position, transform.rotation);
        Destroy(gameObject);

        var missilsLeft = FindObjectsOfType<Missil>();
        foreach (var missil in missilsLeft)
        {
            missil.Explode();
        }

        //Congrats Screen
        _gms.CompleteLevel(3f);
    }

    
	
}
