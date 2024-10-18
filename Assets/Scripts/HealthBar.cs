using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Transform m_HealthBarFill;

    [SerializeField]
    private EnemyMovement m_Enemy;
    [SerializeField]
    private PlayerMovement m_Player;
    private float m_CurrentHealth = 1f;

    private Vector3 m_OriginalScale;

    

    private void Start()
    {
        m_OriginalScale = m_HealthBarFill.localScale;
        UpdateHealthBar();
    }

    public void TakeMeleeDamage()
    {
        m_CurrentHealth -= 0.2f;
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, 0f, 1f);
        UpdateHealthBar();
    }

    public void TakeShotDamage()
    {
        m_CurrentHealth -= 0.1f;
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, 0f, 1f);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {   
    const float epsilon = 0.0001f; // Define una pequeña tolerancia para el punto flotante

    if (m_CurrentHealth < epsilon)
    {
        m_CurrentHealth = 0f; // Ajusta la salud a 0 si es muy cercana a 0
    }

    m_HealthBarFill.localScale = new Vector3(m_OriginalScale.x * m_CurrentHealth, m_OriginalScale.y, m_OriginalScale.z);

    Renderer renderer = m_HealthBarFill.GetComponent<Renderer>();
    if (m_CurrentHealth > 0.75f)
    {
        renderer.material.color = Color.green;
    }
    else if (m_CurrentHealth > 0.5f)
    {
        renderer.material.color = Color.yellow;
    }
    else if (m_CurrentHealth > 0.25f)
    {
        renderer.material.color = new Color(1f, 0.65f, 0f); // Anaranjado
    }
    else if (m_CurrentHealth <= 0f)
    {
        if(gameObject.name != "BarraVidaPlayer")
        {
            m_Enemy.TriggerDied();
        }
        else 
        {
            SceneManager.LoadScene("MainScene");
        }
        
    }
    else
    {
        renderer.material.color = Color.red;
    }
    }

}
