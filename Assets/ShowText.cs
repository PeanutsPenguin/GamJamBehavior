using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowText : MonoBehaviour
{

    public SpriteRenderer m_text;
    public bool m_showText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_text.color.a < 100 && m_showText)
        {
            double alpha = m_text.color.a + (2 * Time.deltaTime);


            m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, (float)alpha);
            
        }
    }
}
