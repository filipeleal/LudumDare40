using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missil : MonoBehaviour {

    public float Speed = 700f;
    public float AngularSpeed = 360f;
    public int Damage = 1;

    [Header("Effects")]
    public GameObject ExplosionEffect;
    public ParticleSystem FlamesEffect;

    [Header("Target")]
    public bool UseTaget = true;
    public GameObject Target;

    [Header("Components")]
    public Rigidbody2D Rb;
    public Collider2D Trigger;

    private bool exploded = false;

    private void FixedUpdate()
    {
        if (exploded)
            return;

        Rb.velocity = transform.up * Speed * Time.fixedDeltaTime;
        if(UseTaget)
            FollowTarget();
    }

    void FollowTarget()
    {
        Vector2 dir = (Vector2)Target.transform.position - Rb.position;
        dir.Normalize();

        float angle = -Vector3.Cross(dir, transform.up).z;

        Rb.angularVelocity = angle * AngularSpeed;

    }

    public void Explode()
    {
        exploded = true;
        Instantiate(ExplosionEffect, transform.position, transform.rotation);
        GetComponent<SpriteRenderer>().enabled = false;
        UseTaget = false;
        Rb.velocity = Vector2.zero; 
        FlamesEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        Trigger.enabled = false;
        
        Destroy(gameObject, 2f);
    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (exploded)
            return;
        if (collision.tag == "Obstacle" || collision.tag == "Missil")
        {

            Explode();
        }

        if(collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
            Explode();
        }
    }
}
