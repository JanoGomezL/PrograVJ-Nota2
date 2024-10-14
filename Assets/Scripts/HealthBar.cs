using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Transform m_HealthBarFill;
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
        else
        {
            renderer.material.color = Color.red;
        }
    }
}
