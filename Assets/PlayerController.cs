using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Components
    public Camera cam;
    public Rigidbody2D m_playerRigidbody;
    public EcholocationUpdate m_echoObject;
    public PlayerScriptableObject m_playerValues;        ///Scriptable Object containing all of the player Values
    public SpriteRenderer m_belugaImage;
    public RandomDirection m_fish;


    public GameObject m_Child;

    private bool m_isInGround;
    private bool m_moove;
    private bool m_growing;

    private float m_currentSpeed;
    private float m_currentEchoTime;

    ///Input Vector
    private Vector2 m_joyPos;


    //Camera
    private Vector3 m_offset;
    public float m_camSpeed;
    public float m_camYoffset;
    public float m_camYspeed;
    private Vector3 m_camYVelocity;

    ///Movement
    private float m_prevAngle;
    private float m_movementAngle;
    Quaternion m_targetRotation;
    public GameObject m_objdirection;                     ///Small Arrows that shows the player direction
    private float prevPosX;


    private float m_exitGroundSpeed;


    private float m_beginningTimer;
    // Start is called before the first frame update
    void Start()
    {
        ///Reset the camera offset
        this.m_offset = new Vector3(0, 0, -1);
        this.m_currentSpeed = 1;
        this.m_echoObject.m_echolocation.radius = 0;
        //this.m_prevAngle = -90;
        //this.m_movementAngle = -90;
    }

    // Update is called once per frame
    void Update()
    {
        this.JoystickHandler();

        float XaxisJoy = Input.GetAxis("TriggerRight");

        if (XaxisJoy > 0 && !this.m_growing)
        {
            this.m_growing = true;
            this.m_echoObject.transform.position = this.transform.position;
        }




        if (Input.GetButton("Fire1")) 
        {
            if (this.m_fish.m_rendering) 
            {
                float dist = Vector3.Distance(this.transform.position, this.m_fish.transform.position);

                if (dist < 15)
                {
                    this.m_fish.gameObject.SetActive(false);
                    this.m_echoObject.has_eatenFish = true;
                }
            }
        }



        this.UpdateCoolDown();

        if (this.m_growing)
            ThrowEcho();
        else
            this.m_echoObject.resetEcho();
    }

    void UpdateCoolDown()
    {
        if(m_growing && this.m_currentEchoTime < this.m_playerValues.maxEchoTime)
            this.m_currentEchoTime += Time.deltaTime;
        else if(this.m_currentEchoTime >= this.m_playerValues.maxEchoTime)
        {
            this.m_growing=false;
            this.m_currentEchoTime = 0;
            this.m_echoObject.m_echolocation.radius = 0;
        }

        if (m_beginningTimer < 5)
            this.m_beginningTimer += Time.deltaTime;
    }

    void ThrowEcho()
    {
        this.m_echoObject.drawEcho();
        this.m_echoObject.m_echolocation.radius += this.m_playerValues.echoGrowing * Time.deltaTime;
    }

    void UpdateCamera()
    {
        ///If we're in the ground or hooked, make the camera slowly follow us 
        if (this.m_isInGround)
        { 
            Vector3 targetPosition = this.transform.position + this.m_offset;
            targetPosition.x = this.cam.transform.position.x;
            this.cam.transform.position = Vector3.Lerp(this.cam.transform.position, targetPosition, this.m_camSpeed * Time.deltaTime);
        }

        ///if we're out of the ground and going up, up the camera a bit to still see where we going
        else if (!this.m_isInGround && this.m_playerRigidbody.velocity.y > 0)
        {
            Vector3 targetPosition = new Vector3(this.cam.transform.position.x, this.transform.position.y + this.m_camYoffset, -1);
            this.cam.transform.position = Vector3.SmoothDamp(this.cam.transform.position, targetPosition, ref this.m_camYVelocity, this.m_camYspeed);
        }

        ///if we're out of the ground and going down, down the camera a bit to still see where we going
        else if (!this.m_isInGround && this.m_playerRigidbody.velocity.y < 0)
        {
            Vector3 targetPosition = new Vector3(this.cam.transform.position.x, this.transform.position.y - this.m_camYoffset, -1);
            this.cam.transform.position = Vector3.SmoothDamp(this.cam.transform.position, targetPosition, ref this.m_camYVelocity, this.m_camYspeed);

        }

        if(this.cam.transform.position.y <= 5)
            this.cam.transform.position = new Vector3(this.cam.transform.position.x, 5, this.cam.transform.position.z);

        if (this.cam.transform.position.y >= 256)
            this.cam.transform.position = new Vector3(this.cam.transform.position.x, 256, this.cam.transform.position.z);
    }

    ///Input handler for the joystick
    private void JoystickHandler()
    {
        ///Get the left Joystick x and y position and make it a vec2
        float XaxisJoy = Input.GetAxis("Vertical");
        float YAxisJoy = Input.GetAxis("Horizontal");
        this.m_joyPos = new Vector2(XaxisJoy, YAxisJoy);

        if (m_beginningTimer < 5)
        {
            this.m_joyPos.x = 0;
            this.m_joyPos.y = -1;
        }

            ///Move an arrow to see we're we going
            Vector3 direction = new Vector3(this.transform.position.x + this.m_joyPos.y, this.transform.position.y + this.m_joyPos.x, 0);
        this.m_objdirection.transform.position = direction;

        ///Calculate angle between my direction and the arrow direction and rotate to it
        this.m_movementAngle = (Mathf.Atan2(direction.y - transform.position.y, direction.x - transform.position.x) * Mathf.Rad2Deg);

        if(m_joyPos.x <= 0.10f &&  m_joyPos.y <= 0.10f && m_joyPos.x >= -0.10f && m_joyPos.y >= -0.10f) 
        {
            this.m_movementAngle = this.m_prevAngle;
            this.m_moove = false;
        }
        else 
        {
            this.m_prevAngle = this.m_movementAngle;
            m_moove = true;
        }


        this.m_targetRotation = Quaternion.Euler(new Vector3(0, 0, this.m_movementAngle + 90));
    }

    private void FixedUpdate()
    {
        ///Rotate the player to the arrow direction
        transform.rotation = Quaternion.RotateTowards(transform.rotation, this.m_targetRotation, this.m_playerValues.rotationSpeed);

        ///Call the fixed update function in ground if we're in ground
        if (this.m_isInGround)
            this.FixedUpdateInGroundBeluga();

        ///Call the fixed update function out of ground if we're out of ground
        else if (!this.m_isInGround)
            this.FixedUpdateOutOfGroundBeluga();

        ///Call the camera update function
        this.UpdateCamera();
    }

    ///Fixed update when we're in ground
    private void FixedUpdateInGroundBeluga()
    {
        if (!m_moove) 
        {
            this.m_currentSpeed *= this.m_playerValues.waterDecceleration;

            if (this.m_currentSpeed < 0.25f)
                this.m_currentSpeed = 0;
        }
        else if (this.m_currentSpeed < this.m_playerValues.Maxspeed)  
        {
            if (this.m_currentSpeed == 0)
                this.m_currentSpeed = 0.75f;

            this.m_currentSpeed *= this.m_playerValues.waterAcceleration;
        }

        if (Input.GetButton("Fire2"))
        {
            this.m_playerRigidbody.velocity = transform.TransformDirection(Vector2.up).normalized * this.m_playerValues.Maxspeed;
            //this.m_belugaImage.flipY = true;

            if (transform.position.x >= prevPosX)
                this.m_belugaImage.flipX = true;
            else
                this.m_belugaImage.flipX = false;
        }
        else
        {
            this.m_playerRigidbody.velocity = transform.TransformDirection(Vector2.down).normalized * this.m_currentSpeed;
            //this.m_belugaImage.flipY = false;


            if (transform.position.x >= prevPosX)
                this.m_belugaImage.flipX = false;
            else
                this.m_belugaImage.flipX = true;
        }



        prevPosX = transform.position.x;
    }

    ///Fixed update when we're out of ground
    private void FixedUpdateOutOfGroundBeluga()
    {
        ///If we're in the air and going down, accelerate the player
        if (this.m_playerRigidbody.velocity.y < 0)
            this.m_currentSpeed *= this.m_playerValues.airAcceleration;

        ///If we're in the air and going up, decelerate the player
        else if (this.m_playerRigidbody.velocity.y > 0)
        {
            this.m_currentSpeed *= this.m_playerValues.airAcceleration;
            this.m_exitGroundSpeed = this.m_currentSpeed;
        }

        ///If we're in the air and not hooked, add force to the rb to move depending on the joystick values
        if (this.m_joyPos.y != 0)
        {
            Vector2 forceAdded = new Vector2(this.m_joyPos.y * this.m_playerValues.AirSpeed, 0);
            this.m_playerRigidbody.AddForce(forceAdded);
        }
    }



    ///Function called everytime we enter a triggered object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ///If it's a ground
        if (collision.CompareTag("Ground"))
        {
            this.m_isInGround = true;
            this.m_playerRigidbody.gravityScale = 0;
        }

        ///If it's a ground
        if (collision.CompareTag("DiedBlugaText"))
        {
            collision.gameObject.ConvertTo<ShowText>().m_showText = true;
        }

        ///If it's a ground
        if (collision.CompareTag("LastText"))
        {
            if(this.m_Child.gameObject.activeInHierarchy)
                collision.gameObject.ConvertTo<ShowText>().m_showText = true;
        }

    }

    ///Function called everytime we exit a triigered object
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            this.m_isInGround = false;
            this.m_playerRigidbody.gravityScale = m_playerValues.gravity;
        }
    }

    ///Function called everytime we stay in a triggered object
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            ///If we're not hook and not in ground, set that we're actually in ground
            if (!this.m_isInGround)
                this.m_isInGround = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            collision.gameObject.GetComponent<deactivate>().m_playerTouching = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            collision.gameObject.GetComponent<deactivate>().m_playerTouching = false;
            collision.gameObject.GetComponent<deactivate>().m_endedCoroutine = false;
        }
    }
}