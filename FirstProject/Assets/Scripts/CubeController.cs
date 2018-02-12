using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class CubeController : MonoBehaviour {
    public float jumpVar = 45000.0f;
    public float moveVar = 10f;
    public float boostSpd = 1.5f;
    public float dashDuration = 0.3f;
    public float airTime = 0.5f;
    public float leniencyTime = 0.7f;

    private bool directionFace = true;
    private bool onFloor = true;
    private bool isDashing = false;
    private int actionNum = -1;
    private float actionTimestamp = 0;
    private GameObject equip1;
    private GameObject equip2;
    private List<GameObject> defaultLoadout;
    public Rigidbody rb;

    void Start() {
        defaultLoadout = GameObject.Find("Equipment").GetComponent<EquipSystem>().equipSet;
        equip1 = defaultLoadout[0];
        equip2 = defaultLoadout[1];
    }
    // Update is called once per frame
    void Update () {
        var InputDevice = InputManager.ActiveDevice;
    /*
    *----------------------------------------------------------------------------------------------------------
    *                                          CODE FOR MOVEMENT
    *----------------------------------------------------------------------------------------------------------
    */
        //map for simple Movement
        if (InputManager.ActiveDevice.DPadLeft.IsPressed)
        {
            transform.Translate(Vector3.back * moveVar * Time.deltaTime * InputDevice.DPadLeft);
            directionFace = false;
        }
        else if(InputManager.ActiveDevice.DPadRight.IsPressed)
        {
            transform.Translate(Vector3.forward * moveVar * Time.deltaTime * InputDevice.DPadRight);
            directionFace = true;
        }

        //map for jumping
        if (onFloor == true && InputManager.ActiveDevice.Action2.WasPressed)
        {
            onFloor = false;
            rb.AddForce(Vector3.up * jumpVar * Time.deltaTime* InputDevice.Action2);
        }

        //map for dashing
        if (onFloor == true && InputManager.ActiveDevice.LeftBumper.WasPressed && isDashing == false)
        {
            StartCoroutine(dash(dashDuration));
        }

    /*
    *----------------------------------------------------------------------------------------------------------
    *                                    CODE FOR ATTACKING / EQUIPS
    *----------------------------------------------------------------------------------------------------------
    */

        //mapping for X button/primary attack
        if (InputManager.ActiveDevice.Action1.WasPressed)
        {
            //check if successive taps happened.
            if (Time.time - actionTimestamp < leniencyTime)
            {
                actionNum += 1;
                Debug.Log("Attack " + actionNum + " came out!");
                //attack funct goes here
            }
            else {
                //reset the combo if timing is missed
                actionNum = 0;
                Debug.Log("Attack 0 came out!");
                //attack funct goes here
            }
            actionTimestamp = Time.time;
        }

        /*
        *----------------------------------------------------------------------------------------------------------
        *                                               ANIMATIONS
        *----------------------------------------------------------------------------------------------------------
        */
        if (!InputManager.ActiveDevice.AnyButton.IsPressed) {
            GetComponent<Animation>().Play("Idle");
        }

        /*
        *----------------------------------------------------------------------------------------------------------
        *                                    CODE FOR COLLISIONS/INTERACTIONS
        *----------------------------------------------------------------------------------------------------------
        */



        //will only change to true if the player is on the ground.
        onFloor = IsOnFloor();
    }
   
    /*
    *----------------------------------------------------------------------------------------------------------
    *                                             FUNCTIONS
    *----------------------------------------------------------------------------------------------------------
    */

    //updates that happen after actions
    void LateUpdate()
    {
        //to keep the camera on the player without bouncing around. need to think ahead about multi-leveled stages
        Camera.main.transform.position = new Vector3 (transform.position.x+15, 5, transform.position.z);
    }

    //function that checks if the cube is on the ground or touching the wall
    bool IsOnFloor()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 1f);
    }

    //dash function for the coroutine
    IEnumerator dash(float dashDur) {
        float time = 0; //store the time this coroutine is operating
        isDashing = true; 

        while (dashDur > time) //we call this loop every frame while our custom boostDuration is a higher value than the "time" variable in this coroutine
        {
            time += Time.deltaTime; //Increase our "time" variable by the amount of time that it has been since the last update
            if (directionFace == true)
            {
                transform.Translate(Vector3.forward * moveVar * Time.deltaTime * boostSpd);
            }
            else
            {
                transform.Translate(Vector3.back * moveVar * Time.deltaTime * boostSpd);
            }

            if (onFloor == false && airTime != 0)
            {
                dashDur += airTime;
                airTime = 0;
            }
            yield return 0; //go to next frame
        }
        airTime = 0.5f;
        isDashing = false; //set back to true so that we can boost again.
    }
}
