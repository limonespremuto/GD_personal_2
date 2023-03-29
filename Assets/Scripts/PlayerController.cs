using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    public float maxSpeed;
    
    [SerializeField]
    Rigidbody2D RB;
    [SerializeField]
    float playerDrag;
    [SerializeField]
    float acceleration;
    [HideInInspector]
    public Vector2 knockBackVector = Vector3.zero;
    
    [SerializeField]
    WeaponScript weaponScript;

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Camera playerCamera;


    // Start is called before the first frame update
    void Start()
    {
        if (playerDrag <= 0f)
        {
            Debug.Log("Player drag needs to be above 0");
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowards(Input.mousePosition);
        Shoot();
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

        Vector2 newVelocityVector = Vector2.zero;

        newVelocityVector.x = Input.GetAxisRaw("Horizontal");
        newVelocityVector.y = Input.GetAxisRaw("Vertical");
        //newVelocityVector = Vector2.ClampMagnitude( newVelocityVector * moveSpeed, moveSpeed);
        
        float accelerationForce = acceleration * (maxSpeed - RB.velocity.magnitude) / maxSpeed;
        accelerationForce = Mathf.Max(accelerationForce, -playerDrag -1f);

        //Debug.Log(accelerationForce);
        RB.velocity += Vector2.ClampMagnitude(newVelocityVector, 1f) * (maxSpeed * (playerDrag + accelerationForce) * Time.fixedDeltaTime);

        RB.velocity = RB.velocity / (1f + (playerDrag * Time.fixedDeltaTime));
        if (RB.velocity.magnitude <= 0.02f)
        {
            RB.velocity = Vector2.zero;
        }

        //Debug.Log(RB.velocity.magnitude);
    
    }

}
