using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EcholocationUpdate : MonoBehaviour
{

    public LineRenderer m_circleRender;
    public CircleCollider2D m_echolocation;

    public bool has_eatenFish;

    public int m_precision;
    // Start is called before the first frame update
    void Start()
    {
        m_circleRender.positionCount = m_precision + 1;
    }


    
    public void drawEcho()
    {
        for (int i = 0; i < m_precision; i++)
        {
            float circumferenceProgress = (float)i / m_precision;

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * this.m_echolocation.radius;
            float y = yScaled * this.m_echolocation.radius;

            Vector3 currentPosition = new Vector3(x, y, 0) + this.transform.position;

            m_circleRender.SetPosition(i, currentPosition);
        }

        m_circleRender.SetPosition(m_precision, m_circleRender.GetPosition(0));
    }

    public void resetEcho()
    {
        for (int i = 0; i < m_precision; i++)
        {
            Vector3 currentPosition = this.transform.position;

            m_circleRender.SetPosition(i, currentPosition);
        }

        m_circleRender.SetPosition(m_precision, m_circleRender.GetPosition(0));
    }
}
