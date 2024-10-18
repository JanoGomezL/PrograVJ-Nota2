using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossHitbox : MonoBehaviour
{
    [SerializeField]
    private Animator m_EnemyAnimator;

    [SerializeField]
    private HealthBarBoss m_HealthBar;

    public void Hit()
    {
        var light = m_EnemyAnimator.gameObject.transform.Find("Light");
        light.gameObject.SetActive(true);
        m_EnemyAnimator.SetTrigger("StartHitBoss");
        m_HealthBar.TakeMeleeDamage();
    }
}
