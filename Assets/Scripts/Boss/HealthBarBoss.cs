using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con UI

public class HealthBarBoss : MonoBehaviour
{
    [SerializeField]
    private Image m_HealthBarFill; // Cambiar de Transform a Image para trabajar con la UI

    [SerializeField]
    private BossMovement m_Enemy;
    private float m_CurrentHealth = 1f;

    private Vector2 m_OriginalSize; // Guardar el tamaño original de la barra de salud

    private void Start()
    {
        // Guardar el tamaño original de la barra de salud
        m_OriginalSize = m_HealthBarFill.rectTransform.sizeDelta;
        UpdateHealthBar();
    }

    public void TakeMeleeDamage()
    {
        m_CurrentHealth -= 0.25f;
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

        // Ajustar el tamaño del RectTransform en función de la salud actual
        m_HealthBarFill.rectTransform.sizeDelta = new Vector2(m_OriginalSize.x * m_CurrentHealth, m_OriginalSize.y);

        // Cambiar el color de la barra según la salud
        if (m_CurrentHealth > 0.75f)
        {
            m_HealthBarFill.color = Color.green;
        }
        else if (m_CurrentHealth > 0.5f)
        {
            m_HealthBarFill.color = Color.yellow;
        }
        else if (m_CurrentHealth > 0.25f)
        {
            m_HealthBarFill.color = new Color(1f, 0.65f, 0f); // Anaranjado
        }
        else if (m_CurrentHealth <= 0f)
        {
            m_Enemy.TriggerDied(); // Llama a la función de muerte cuando la salud llega a 0
        }
        else
        {
            m_HealthBarFill.color = Color.red;
        }
    }
}