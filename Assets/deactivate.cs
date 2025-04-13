using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;

public class deactivate : MonoBehaviour
{
    public PlayerController player;
    public SpriteRenderer m_sprite;
    public BoxCollider2D m_boxCollider;
    public Rigidbody2D m_Rigidbody2D;
    public bool m_activated;
    public bool m_playerTouching;
    public bool m_endedCoroutine;
    private float timer;

    private void Start()
    {
        timer = 4;
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer < 2)
        {
            if (m_playerTouching && m_endedCoroutine)
            {
                StartCoroutine(this.deactivateBlock());
            }
        }
        else 
            m_endedCoroutine = false;

    }

    public IEnumerator deactivateBlock() 
    {
        this.m_sprite.enabled = false;
        this.m_boxCollider.enabled = false;
        this.m_activated = false;
        this.m_Rigidbody2D.simulated = false;
        this.m_endedCoroutine = false;

        yield return new WaitForSeconds(5);

        this.m_sprite.enabled = true;
        this.m_boxCollider.enabled = true;
        this.m_activated = true;
        this.m_Rigidbody2D.simulated = true;
        this.m_endedCoroutine = true;
        m_playerTouching = false;
        timer = 0;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Echo"))
        {
            if (this.m_activated)
            {
                if(collision.gameObject.ConvertTo<EcholocationUpdate>().has_eatenFish)
                    StartCoroutine(this.deactivateBlock());
            }
                
        }
    }
}
