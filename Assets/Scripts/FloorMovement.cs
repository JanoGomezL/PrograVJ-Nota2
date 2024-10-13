using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement m_PlayerMovement;

    [SerializeField]
    private Transform m_Sprite;

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.collider.CompareTag("Player"))
        {
            m_PlayerMovement.DeactivateJump();
        }
    }

    private void Update()
    {
        if (m_Sprite != null)
        {
            Vector3 spritePosition = m_Sprite.position;
            spritePosition.x = transform.position.x;
            m_Sprite.position = spritePosition;
        }
    }
}
