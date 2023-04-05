using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IHealth
{
    [SerializeField]
    private ScritableObjectHealth cStats;
    [HideInInspector]
    public float maxHealt;
    [SerializeField]
    private bool healtOvverride = false;
    public float healt;
    public float resistance;

    public float maxSpeed;
    
    [SerializeField]
    Rigidbody2D rB;
    [SerializeField, Tooltip("shoulnt be zero in that case it will break, the higher the snappier stopping is")]
    float playerDrag;
    [SerializeField, Tooltip("Does not fight whit Drag only dictates how fast its going accelerate")]
    float acceleration;
    //used for the interface
    private Vector2 knockBackVector = Vector3.zero;
    
    
    private Vector2 moveVector = Vector2.zero;
    
    [SerializeField]
    WeaponScript weaponScript;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Camera playerCamera;
    [SerializeField, Tooltip("Initial zoom level on play")]
    int zoomLevel = 0;
    [SerializeField, Tooltip("how much zoom for each level, expressed in half width of the camera")]
    float[] zoomLevels;


    // Start is called before the first frame update
    void Start()
    {
        if (playerDrag <= 0f)
        {
            Debug.Log("Player drag needs to be above 0");
        }

        playerCamera.orthographicSize = zoomLevels[zoomLevel];

        maxHealt = cStats.maxHealth;
        resistance = cStats.resistance;
        if (!healtOvverride)
        {
            healt = maxHealt;
        }

        
        //Debug.Log(zoomLevel + " is the zoom level, and " + zoomLevels[zoomLevel] + " is the new zoom");
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowards(Input.mousePosition);
        Shoot();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            zoomLevel++;
            if (zoomLevel > zoomLevels.Length - 1)
                zoomLevel = 0;

            playerCamera.orthographicSize = zoomLevels[zoomLevel];
            //Debug.Log(zoomLevel + " is the zoom level, and " + zoomLevels[zoomLevel] + " is the new zoom");
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            weaponScript.Shoot(false);
        }
    }

    void RotateTowards(Vector2 aimVector2)
    {

        aimVector2.x -= Screen.width /2;
        aimVector2.y -= Screen.height / 2;

        //Vector3 aimPoint = playerCamera.ScreenToWorldPoint(aimVector2);
        //aimPoint.z = playerTransform.position.z;
        //aimPoint = aimPoint.normalized;
        
        //PlayerTransform.LookAt(new Vector3(aimPoint.x, aimPoint.y, PlayerTransform.position.z), Vector3.back);
        playerTransform.up = new Vector3(aimVector2.x,aimVector2.y, 0f);
        //Debug.Log(aimVector2);
    }

    void Movement()
    {
        //internalizing vector
        moveVector = rB.velocity;
        Vector2 newVelocityVector = Vector2.zero;

        newVelocityVector.x = Input.GetAxisRaw("Horizontal");
        newVelocityVector.y = Input.GetAxisRaw("Vertical");
        //newVelocityVector = Vector2.ClampMagnitude( newVelocityVector * moveSpeed, moveSpeed);
        
        float accelerationForce = acceleration * (maxSpeed - rB.velocity.magnitude) / maxSpeed;
        accelerationForce = Mathf.Max(accelerationForce, -playerDrag -1f);

        //Debug.Log(accelerationForce);
        moveVector += Vector2.ClampMagnitude(newVelocityVector, 1f) * (maxSpeed * (playerDrag + accelerationForce) * Time.fixedDeltaTime);
        moveVector += knockBackVector;

        //drag calculations
        knockBackVector = knockBackVector / (1f + (playerDrag * Time.fixedDeltaTime));
        moveVector = moveVector / (1f + (playerDrag * Time.fixedDeltaTime));

        //Stopping the vectors if speed is too low
        if (knockBackVector.magnitude <= 0.02f)
        {
            knockBackVector = Vector2.zero;
        }
        if (moveVector.magnitude <= 0.02f)
        {
            rB.velocity = Vector2.zero;
        }

        //velocity applied only once per fixedupdate
        rB.velocity = moveVector;
        
        //Debug.Log(RB.velocity.magnitude);
    
    }
    public void TakeDamage(float damage)
    {
        healt -= damage * (1f - resistance);
    }

}
