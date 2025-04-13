using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showSpiritBeluga : MonoBehaviour
{

    public GameObject spirit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Echo"))
        {
            this.spirit.SetActive(true);
        }
    }
}
