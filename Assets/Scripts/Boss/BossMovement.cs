using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BossState
{
    Idle, Chasing, Attacking, Shuriken,Died
}

public class BossMovement : MonoBehaviour

{
    [SerializeField]
    private HealthBar m_HealthBarPlayer;
    [SerializeField]
    private GameObject m_ShurikenPrefab;
    [SerializeField]
    private Transform m_ShurikenSpawnPoint;
    [SerializeField]
    private GameObject m_WinPanel;
    [SerializeField]
    private GameObject m_PlayerObj;
    [SerializeField]
    private float m_ChaseRange = 3f;
    [SerializeField]
    private float m_ShootRange = 8f;
    [SerializeField]
    private float m_RaycastDistance = 5f; 
    [SerializeField]
    private float m_AttackDistance = 0.5f; 
    [SerializeField]
    private float m_Speed = 4f;
    [SerializeField]    
    private Transform m_RaycastGenerator; 
    private EnemyState m_State = EnemyState.Idle; // Estado inicial
    private Animator m_SpriteAnimator; 
    private Transform m_Player = null; 

    private bool m_IsTalking;

    private bool m_AttackExecuted = false;
    private bool m_ShootExecuted = false; 

    private bool m_Died = false;

    private void Awake() 
    {
        m_SpriteAnimator = transform.Find("Sprite").GetComponent<Animator>();
    }

    private void Update() 
    {
        switch (m_State)
        {
            case EnemyState.Idle:
                OnIdle();
                CheckForPlayer(); // Verificar si el jugador está en rango para cambiar al estado Chasing
                break;
                
            case EnemyState.Chasing:
                OnChase();
                break;

            case EnemyState.Attacking:
                OnAttack();
                break;
            case EnemyState.Shuriken:
                OnShoot();
                break;
                case EnemyState.Died:
                OnDied();
                break;
        }
    }

    private void CheckForPlayer()
{
    // Ajusta la separación en el eje X o Y para los raycasts
    float raycastOffset = 0.5f; // Ajusta este valor para controlar la separación

    // Posición modificada para el raycast hacia la izquierda
    Vector2 leftRaycastOrigin = new Vector2(m_RaycastGenerator.position.x- raycastOffset , m_RaycastGenerator.position.y);

    // Posición modificada para el raycast hacia la derecha
    Vector2 rightRaycastOrigin = new Vector2(m_RaycastGenerator.position.x+ raycastOffset , m_RaycastGenerator.position.y );

    // Lanza el Raycast a la izquierda
    var hitLeft = Physics2D.Raycast(
        leftRaycastOrigin,
        Vector2.left,
        m_RaycastDistance,
        LayerMask.GetMask("Hitbox")
    );

    // Lanza el Raycast a la derecha
    var hitRight = Physics2D.Raycast(
        rightRaycastOrigin,
        Vector2.right,
        m_RaycastDistance,
        LayerMask.GetMask("Hitbox")
    );

    // Si colisiona con el jugador en cualquiera de los lados
    if (hitLeft.collider != null || hitRight.collider != null)
    {
        m_Player = (hitLeft.collider != null) ? hitLeft.collider.transform : hitRight.collider.transform;
        float distanceToPlayer = Vector3.Distance(m_Player.position, transform.position);

        // Determina la orientación del sprite
        if (hitRight.collider != null)
        {
            m_SpriteAnimator.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (hitLeft.collider != null)
        {
            m_SpriteAnimator.GetComponent<SpriteRenderer>().flipX = false;
        }

        // Cambiar al estado de persecución si la distancia es menor que el rango de Raycast
        if (distanceToPlayer < m_ChaseRange)
        {
            m_State = EnemyState.Chasing;
        }
        else if (distanceToPlayer > m_ChaseRange && distanceToPlayer < m_ShootRange)
        {
            m_State = EnemyState.Shuriken;
        }
    }
}


    private void OnIdle()
    {
        m_SpriteAnimator.SetTrigger("StopWalkBoss");
    }

    private void OnChase()
    {
        if (m_Player == null)
        {
            m_State = EnemyState.Idle; // Volver a Idle si no hay jugador
            return;
        }

        // Calcular la distancia al jugador
        float distanceToPlayer = Vector3.Distance(m_Player.position, transform.position);

        // Cambiar a Idle si el jugador se aleja más de 5 unidades
        if (distanceToPlayer > m_ChaseRange)
        {
            m_State = EnemyState.Idle;
            return;
        }

        // Cambiar al estado de ataque si está dentro del rango de ataque
        if (distanceToPlayer <= m_AttackDistance)
        {
            m_State = EnemyState.Attacking;
            return;
        }

        // Perseguir al jugador
        Vector3 dir = (m_Player.position - transform.position).normalized;
        transform.position += m_Speed * Time.deltaTime * dir;
        m_SpriteAnimator.SetTrigger("StartWalkBoss");
    }

    private void OnAttack()
    {
        // Si el ataque no se ha ejecutado, iniciar el ataque
        if (!m_AttackExecuted)
        {
            StartCoroutine(MeleeAttack());
        }
        m_State = EnemyState.Idle;
    }

    IEnumerator MeleeAttack()
    {
        m_AttackExecuted = true;
        m_SpriteAnimator.SetTrigger("StartSlashBoss");
        yield return new WaitForSeconds(0.3f);
        m_HealthBarPlayer.TakeMeleeDamage();
        yield return new WaitForSeconds(0.2f);
        m_SpriteAnimator.SetTrigger("StopSlashBoss");
        yield return new WaitForSeconds(3.5f);
        m_AttackExecuted = false;
        
    }

    private void OnShoot()
    {
        // Si el ataque no se ha ejecutado, iniciar el ataque
        if (!m_ShootExecuted)
        {
            StartCoroutine(ShootAttack());
        }
        m_State = EnemyState.Idle;
    }
    

    IEnumerator ShootAttack()
    {
        m_ShootExecuted = true;
        m_SpriteAnimator.SetTrigger("StartShootBoss");
        yield return new WaitForSeconds(0.3f);
        GameObject shuriken = Instantiate(m_ShurikenPrefab, m_ShurikenSpawnPoint.position, Quaternion.identity);

        float direction = m_SpriteAnimator.GetComponent<SpriteRenderer>().flipX ? 1f : -1f;
        shuriken.GetComponent<ShurikenMovement>().SetDirection(new Vector2(direction, 0));
        yield return new WaitForSeconds(0.2f);
        m_SpriteAnimator.SetTrigger("StopShootBoss");
        yield return new WaitForSeconds(1.5f);
        m_ShootExecuted = false;
        
    }

    private void OnDied()
    {
        if(!m_Died)
        {
            StartCoroutine(DiedAnim());
        }
        
    }

    public void TriggerDied()
    {
        m_State = EnemyState.Died;
    }

    IEnumerator DiedAnim()
    {
        m_Died = true;
        m_SpriteAnimator.SetTrigger("StartDiedBoss");
        yield return new WaitForSeconds(0.5f);
        m_WinPanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
        Destroy(m_PlayerObj);
        
      
    }


    private void OnDrawGizmos()
    {
        float raycastOffset = 0.5f; 
        Vector2 leftRaycastOrigin = new Vector2(m_RaycastGenerator.position.x- raycastOffset , m_RaycastGenerator.position.y);
        Vector2 rightRaycastOrigin = new Vector2(m_RaycastGenerator.position.x+ raycastOffset , m_RaycastGenerator.position.y );
        Gizmos.color = Color.green;
        Gizmos.DrawRay(leftRaycastOrigin, Vector2.left * m_RaycastDistance);
        Gizmos.DrawRay(rightRaycastOrigin, Vector2.right * m_RaycastDistance);
    }


    public void Talk()
    {
        if (!m_IsTalking)
        {
            m_SpriteAnimator.SetTrigger("StartTalkBoss");
            m_IsTalking = true;
        }
        else
        {
            m_SpriteAnimator.SetTrigger("StopTalkBoss");
            m_IsTalking = false;
        }
    }
}
