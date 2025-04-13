using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followMom : MonoBehaviour
{
    public GameObject follow;

    private float m_prevPosX;
    public SpriteRenderer m_belugaImage;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, follow.transform.position, 6 * Time.deltaTime);
        transform.up = follow.transform.position - transform.position;
        Quaternion obj = Quaternion.Euler(new Vector3(0, 0, -90));

        transform.rotation = obj * transform.rotation;

        if (transform.position.x >= this.m_prevPosX)
            this.m_belugaImage.flipY = true;
        else
            this.m_belugaImage.flipY = false;

        this.m_prevPosX = transform.position.x;

    }
}
