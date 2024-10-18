using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenMovement : MonoBehaviour
{
    [SerializeField]
    private HealthBar s_HealthBarPlayer;


    
    [SerializeField]
    private float speed = 5f; // Velocidad del shuriken
    private Vector2 direction; // Dirección del movimiento del shuriken

    void Start()
    {
        s_HealthBarPlayer = GameObject.FindWithTag("HealthBarPlayer").GetComponent<HealthBar>();
        Destroy(gameObject, 10f);
    }
    public void SetDirection(Vector2 dir)
    {
        Debug.Log(dir);
        direction = dir;
    }

    private void Update()
    {
        if(direction == new Vector2(1,0))
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        // Mover el shuriken en la dirección asignada
        transform.Translate(direction * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Manejar colisiones, por ejemplo, con el jugador
        if (collision.CompareTag("PlayerHitBox"))
        {
            Debug.Log("daño player");
            s_HealthBarPlayer.TakeShotDamage();
        }

        // Destruir el shuriken al colisionar
        Destroy(gameObject);
    }
}

