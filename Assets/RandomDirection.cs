using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomDirection : MonoBehaviour
{
    public GameObject player;

    public float m_switchDirectionTime;
    public float m_speed;
    private float m_timer;
    private Vector3 m_direction;
    public Rigidbody2D m_fishRigidbody;
    public BoxCollider2D m_fishCollider;

    private float prevPosX;

    public SpriteRenderer m_fishRender;

    public float m_renderDuration;
    private float m_renderTime;

    public bool m_rendering;

    // Start is called before the first frame update
    void Start()
    {
        this.m_rendering = false;
        m_fishRender.enabled = false;
        m_direction = new Vector3(Random.Range(-50.0f, 50.0f), Random.Range(170.0f, 236.0f), 0);
    }

    // Update is called once per frame
    void Update()
    {
        this.m_timer += Time.deltaTime;
        this.m_renderTime += Time.deltaTime;

        if(this.m_timer < this.m_switchDirectionTime) 
        {
            transform.up = this.m_direction - transform.position;

            if(transform.position.x >= m_direction.x - 5.0f && transform.position.x <= m_direction.x + 5.0f) 
            {
                if (transform.position.y >= m_direction.y - 5.0f && transform.position.y <= m_direction.y + 5.0f)
                {
                    m_direction = new Vector3(Random.Range(-50.0f, 50.0f), Random.Range(170.0f, 236.0f), 0);
                }
            }

            ///Set the velocity of the player to go forward depending on the speed we calculate
            this.m_fishRigidbody.velocity = transform.TransformDirection(Vector2.up).normalized * this.m_speed;
        }
        else if(this.m_timer > this.m_switchDirectionTime)
        {
            this.m_timer = 0;
            m_direction = new Vector3(Random.Range(-50.0f, 50.0f), Random.Range(170.0f, 236.0f), 0);
        }

        if(this.m_renderTime > this.m_renderDuration)
        {
            int layerIgnore = 1 << 6;
            layerIgnore = ~layerIgnore;

            this.m_fishRender.enabled = false;
            this.m_rendering = false;

            Physics2D.IgnoreCollision(this.m_fishCollider,  player.GetComponent<Collider2D>(), true);
        }

        if (transform.position.x >= prevPosX)
            this.m_fishRender.flipX = true;
        else
            this.m_fishRender.flipX = false;

        this.prevPosX = this.transform.position.x; 
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Echo") && !this.m_rendering)
        {
            m_fishRender.enabled = true;
            this.m_rendering = true;
            this.m_renderTime = 0;

            Physics2D.IgnoreCollision(this.m_fishCollider, player.GetComponent<Collider2D>(), false);
        }
    }
}
