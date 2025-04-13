using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForeground : MonoBehaviour
{

    public Camera cam;

    private float minCam;
    private float MaxCam;

    private float minForeGround;
    private float maxForeGround;
    // Start is called before the first frame update
    void Start()
    {
        minCam = 5;
        MaxCam = 256;

        minForeGround = 125;
        maxForeGround = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float resultPos = minForeGround + (((cam.transform.position.y - minCam) * (maxForeGround - minForeGround)) / MaxCam - minCam);

        this.transform.position = new Vector3(this.transform.position.x, resultPos, this.transform.position.z);
    }
}
