using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    private BoxCollider2D Groundcollider;
    void Start()
    {
        this.Groundcollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        ///Reset the isTrigger of the collider every time the player not tuching the ground
        int layerIgnore = 1 << 6;
        layerIgnore = ~layerIgnore;

        //int icebergIgnore = 1 << 7;
        //icebergIgnore = ~icebergIgnore;


        if (!this.Groundcollider.IsTouchingLayers(layerIgnore))
        {
            this.Groundcollider.isTrigger = true;
        }
    }

}
