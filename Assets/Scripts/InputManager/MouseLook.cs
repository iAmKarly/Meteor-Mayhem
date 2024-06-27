using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseLook : MonoBehaviour
{
    public Vector3 viewDirection;
    
    [Tooltip("The camera")]
    [SerializeField] private Camera cam;
    [Tooltip("Source of hand data.")]
    [SerializeField] private HandTracker handTracker;
    [Tooltip("The sensitivity of the mouse movement.")]
    [SerializeField] private Vector2 sensitivity;
    [Tooltip("The acceleration of the mouse movement.")]
    [SerializeField] private Vector2 acceleration;
    [Tooltip("The maximum angel where the mouse can go.")]
    [SerializeField] private float maxAngle;
    [Tooltip("Input lag (Prevents an error of no input due to high refresh rate).")]
    [SerializeField] private float inputLagTimer;
    [Tooltip("This is the canvas of the  crosshair")]
    [SerializeField] private Canvas crosshair;

    private Vector2 velocityVertical;
    private Vector2 velocityHorizontal;
    private Vector2 rotationVertical;
    private Vector2 rotationHorizontal;
    private Vector2 lastInputEvent;
    private float inputLagPeriod;

    private GameObject leftHand, rightHand, body;

    // Start is called before the first frame update
    void Start()
    {
        leftHand = GameObject.FindGameObjectWithTag("LeftHandObj").gameObject;
        rightHand = GameObject.FindGameObjectWithTag("RightHandObj").gameObject;
        body = GameObject.FindGameObjectWithTag("Body").gameObject;
        
        OnEnable();
    }

    // Update is called once per frame
    void Update()
    {
        updateCrosshair(handTracker.checkHandRecognized());
        if(!handTracker.checkHandRecognized()){
            Vector2 wantedVelocityVertical = GetInput() * sensitivity;
            Vector2 wantedVelocityHorizontal = GetInput() * sensitivity;

            wantedVelocityVertical.x = 0;
            wantedVelocityHorizontal.y = 0;

            velocityVertical = new Vector2(
                Mathf.MoveTowards(velocityVertical.x, wantedVelocityVertical.x, acceleration.x * Time.deltaTime),
                Mathf.MoveTowards(velocityVertical.y, wantedVelocityVertical.y, acceleration.y * Time.deltaTime)
            );
            velocityHorizontal = new Vector2(
                Mathf.MoveTowards(velocityVertical.x, wantedVelocityVertical.x, acceleration.x * Time.deltaTime),
                Mathf.MoveTowards(velocityVertical.y, wantedVelocityVertical.y, acceleration.y * Time.deltaTime)
            );

            // update look direction
            rotationVertical += wantedVelocityVertical * Time.deltaTime;
            rotationHorizontal += wantedVelocityHorizontal * Time.deltaTime;

            cam.transform.localEulerAngles = new Vector3(rotationVertical.y, 180-rotationVertical.x, 0);
            body.transform.localEulerAngles = new Vector3(rotationHorizontal.y, rotationHorizontal.x, 0);
            leftHand.transform.localEulerAngles = new Vector3(-rotationVertical.y, rotationVertical.x, 0);
            rightHand.transform.localEulerAngles = new Vector3(-rotationVertical.y, rotationVertical.x, 0);

            rotationHorizontal.y = ClampAngle(rotationHorizontal.y);
            rotationHorizontal.x = ClampAngle(rotationHorizontal.x);
            rotationVertical.x = ClampAngle(rotationVertical.x);
            rotationVertical.y = ClampAngle(rotationVertical.y);
            viewDirection = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).direction;
        }
        else{
            cam.transform.localEulerAngles = new Vector3(0, 180, 0);
            body.transform.localEulerAngles = new Vector3(0, 0, 0);

            OnEnable();
        }
    }

    /// <summary>
    /// Retrieve the direction of the mouse.
    /// </summary>
    /// <returns>Returns direction of the mouse.</returns>
    Vector2 GetInput(){
        inputLagTimer += Time.deltaTime;
        Vector2 input = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );

        if((Mathf.Approximately(0,input.x) && Mathf.Approximately(0, input.y)) == false || inputLagTimer >= inputLagPeriod){
            lastInputEvent = input;
            inputLagTimer = 0;
        }

        return input;
    }

    /// <summary>
    /// Returns the angle limitied by a boundary angle
    /// </summary>
    /// <param name="angle">The boundary angle.</param>
    /// <returns>Returns the angle limited by the boundary</returns>
    float ClampAngle(float angle){
        return Mathf.Clamp(angle, -maxAngle, maxAngle);
    }

    /// <summary>
    /// Sets the mouse looks to default.
    /// </summary>
    void OnEnable(){
        velocityHorizontal = Vector2.zero;
        velocityVertical = Vector2.zero;
        inputLagTimer = 0;
        lastInputEvent = Vector2.zero;

        Vector3 euler = transform.localEulerAngles;

        if(euler.x >=180){
            euler.x -= 360;
        }

        euler.x = ClampAngle(euler.x);
        transform.localEulerAngles = euler;
        rotationHorizontal = new Vector2(euler.y, euler.x);
        rotationVertical = new Vector2(euler.y, euler.x);
    }

    /// <summary>
    /// Updates the crosshair if the system recognizes a hand
    /// </summary>
    /// <param name="handRecognized">Is a hand recognized</param>
    /// <returns>Returns true if crosshair is successfully changed</returns>
    bool updateCrosshair(bool handRecognized){
        bool success = false;
        CanvasGroup canvasGroup;
        if (crosshair != null){
            canvasGroup = crosshair.GetComponent<CanvasGroup>();
            success = true;
            if (!handRecognized){
                canvasGroup.alpha = 1;
            }
            else{
                canvasGroup.alpha = 0;
            }
            
        }
        return success;
    }

    /// <summary>
    /// Checks if mouse input means shooting.
    /// </summary>
    /// <returns>Returns True if mouse input means shooting.</returns>
    public bool checkShoot(){
        bool mouseShooting = Input.GetKey(KeyCode.Mouse0);
        return mouseShooting;
    }
}
