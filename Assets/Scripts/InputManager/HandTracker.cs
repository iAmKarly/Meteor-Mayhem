using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTracker : MonoBehaviour{
    public Vector3 firstHandOrigin, secondHandOrigin, firstHandDirection, secondHandDirection;

    [Tooltip("The source of landmark data.")]
    [SerializeField] private UDPReceive udpReceive;
    [Tooltip("The material for the landmark points.")]
    [SerializeField] private Material handPointMaterial;
    [Tooltip("The material for the line between landmark points.")]
    [SerializeField] private Material handLineMaterial;
    [Tooltip("The material for the line where the hand is aiming at.")]
    [SerializeField] private Material handRayMaterial;

    private GameObject firstHandRay, secondHandRay;
    private GameObject[] secondHand, secondHandLines, firstHand, firstHandLines;
    private int firstHandFingerUp, secondHandFingerUp = 0;
    private bool handRecognized = false;
    private bool bothHandRecognized = false;
    private int secondHandConfirmation = 0;
    private GameObject body;
    private Vector3 firstVelocity, secondVelocity;

    // Start is called before the first frame update
    void Start(){   
        body = GameObject.FindGameObjectWithTag("Body").gameObject;

        createHandPoints(ref firstHand);
        createHandLines(ref firstHandLines);
        createHandRay(ref firstHandRay);

        createHandPoints(ref secondHand);
        createHandLines(ref secondHandLines);
        createHandRay(ref secondHandRay);
    }

    // Update is called once per frame
    void Update(){
        // Extract data
        string data = udpReceive.data;

        // Check for hand
        if (data.Length > 50){
            handRecognized = true;
            // Pre process data
            data = data.Remove(0,1);
            data = data.Remove(data.Length-1,1);
            string[] points = data.Split(',');            
            if (firstHand[0] == null){
                resetHand(ref firstHand, ref firstHandLines, ref firstHandRay);
                createHandPoints(ref firstHand);
                createHandLines(ref firstHandLines);
                createHandRay(ref firstHandRay);
            }
            else{
                firstHandFingerUp = int.Parse(points[63]);
                inputHandPoints(points, ref firstHand, true);
                inputHandLines(ref firstHandLines, firstHand);
                inputHandDirection(ref firstHandDirection, firstHand, ref firstVelocity);
                inputHandRay(ref firstHandRay, firstHandOrigin, firstHandDirection, 100);    
                calculateOrigin(ref firstHandOrigin, firstHand);
            }     

            // Check for second hand
            if(points.Length > 64){
                secondHandConfirmation++;
                if(secondHandConfirmation > 5){
                    bothHandRecognized = true;
                    if (secondHand[0] == null){
                        resetHand(ref secondHand, ref secondHandLines, ref secondHandRay);
                        createHandPoints(ref secondHand);
                        createHandLines(ref secondHandLines);
                        createHandRay(ref secondHandRay);
                    }
                    else{
                        secondHandFingerUp = int.Parse(points[127]);
                        inputHandPoints(points, ref secondHand, false);
                        inputHandLines(ref secondHandLines, secondHand);
                        inputHandDirection(ref secondHandDirection, secondHand, ref secondVelocity);
                        inputHandRay(ref secondHandRay, secondHandOrigin, secondHandDirection, 100);
                        calculateOrigin(ref secondHandOrigin, secondHand);
                    }
                }
            }
            else{
                // Reset second hand
                bothHandRecognized = false;
                secondHandConfirmation = 0;
                resetHand(ref secondHand, ref secondHandLines, ref secondHandRay);
                secondHandFingerUp = 0;
                secondHandOrigin = Vector3.zero;
            }
        }
        else{
            // Reset first hand landmarks
            handRecognized = false;
            bothHandRecognized = false;
            secondHandConfirmation = 0;
            resetHand(ref firstHand, ref firstHandLines, ref firstHandRay);
            firstHandFingerUp = 0;
            secondHandFingerUp = 0;
            firstHandOrigin = Vector3.zero;
        }
        if(!handRecognized){
            resetHand(ref firstHand, ref firstHandLines, ref firstHandRay);
            resetHand(ref secondHand, ref secondHandLines, ref secondHandRay);
            firstHandOrigin = Vector3.zero;
            secondHandOrigin = Vector3.zero;
        }
    }

    /// <summary>
    /// Create points to represent the hands landmarks
    /// </summary>
    /// <param name="hand">A list that will store the sphere objects</param>
    /// <returns>Returns True if all points are successfully created</returns>
    bool createHandPoints(ref GameObject[] hand){
        bool success = true;
        hand = new GameObject[21];
        for (int i = 0; i < 21; i++){
            hand[i] = new GameObject("HandPoints");
            hand[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            hand[i].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            hand[i].transform.position = new Vector3(body.transform.localPosition.x , body.transform.localPosition.y, -1000);
            hand[i].tag = "Player";
            MeshRenderer meshRenderer = hand[i].GetComponent<MeshRenderer>();
            if (meshRenderer != null){
                meshRenderer.material = handPointMaterial;
            }
            if (hand[i] == null){
                success = false;
            }
        }
        return success;
    }

    /// <summary>
    /// Set position of hand landmarks
    /// </summary>
    /// <param name="points">A list of landmark positions.</param>
    /// <param name="hand">A list of sphere objects that represents the landmark.</param>
    /// <param name="first">If true means it is the first hand else it is the second hand.</param>
    /// <returns>Returns True if all points are successfully inputted</returns>
    bool inputHandPoints(string[] points, ref GameObject[] hand, bool first){
        bool success = true;
        int displacement1 = 21;
        int displacement2 = 1;

        if (first){
            displacement1 = 0;
            displacement2 = 0;
        }

        for(int i=0; i<21; i++){
            float x = float.Parse(points[(i+displacement1)*3 + displacement2]) /100 + body.transform.localPosition.x - 6.5f;
            float y = float.Parse(points[(i+displacement1)*3 + 1 + displacement2])/100;
            float z = float.Parse(points[(i+displacement1)*3 + 2 + displacement2])/100;

            // Assign landmarks to ingame points
            hand[i].transform.localPosition = new Vector3(x,y,z);

            if (hand[i].transform == null){
                success = false;
            }
        }
        return success;
    }

    /// <summary>
    /// Calculate perpendicular direction of hand
    /// </summary>
    /// <param name="handDirection">A list that will store directional data.</param>
    /// <param name="hand">A list of landmark to calculate the perpendicular direction.</param>
    /// <param name="velocity">A reference to store the current velocity, modified by the function.</param>
    /// <returns>Returns True if the direction is succesfully inputted.</returns>
    bool inputHandDirection(ref Vector3 handDirection, GameObject[] hand, ref Vector3 velocity){
        bool success = true;
        // Extract Vector from coordinates
        Vector3 pointA = hand[0].transform.localPosition;
        Vector3 pointB = hand[8].transform.localPosition;
        Vector3 pointC = hand[20].transform.localPosition;
        // Calculate the direction from the origin to the destination
        Vector3 directionAB = (pointB - pointA).normalized;
        Vector3 directionAC = (pointC - pointA).normalized;

        // Calculate a perpendicular direction
        Vector3 newHandDirection = Vector3.Cross(directionAB, directionAC).normalized;
        
        if (newHandDirection.z > 0 ){
            newHandDirection = -newHandDirection;
        }

        if (newHandDirection == null){
            success = false;
        }

        handDirection = Vector3.SmoothDamp(handDirection, newHandDirection, ref velocity, 0.2f).normalized;

        return success;
    }

    /// <summary>
    /// Create lines to represent the lines between the landmarks.
    /// </summary>
    /// <param name="handLines">A list that will store the line objects</param>
    /// <returns>Returns True if all lines are successfully created</returns>
    bool createHandLines(ref GameObject[] handLines){
        bool success = true;
        handLines = new GameObject[21];
        for (int i = 0; i < 21; i++){
            handLines[i] = new GameObject("HandLines");
            LineRenderer lineRenderer = handLines[i].AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.SetPosition(0, new Vector3(0,0,-1000));
            lineRenderer.SetPosition(1, new Vector3(0,0,-1000));
            handLines[i].tag = "Player";
            lineRenderer.material = handLineMaterial;
            if (lineRenderer == null){
                success = false;
            }
        }
        return success;
    }

    /// <summary>
    /// Set line between the hand landmarks
    /// </summary>
    /// <param name="hand">A list of sphere objects that represents the landmark.</param>
    /// <param name="handLines">A list of line objects that represents the lines between landmarks.</param>
    /// <returns>Returns True if all lines are successfully inputted</returns>
    bool inputHandLines(ref GameObject[] handLines, GameObject[] hand){
        bool success = true;
        for (int i = 0; i < 21; i++){
            LineRenderer lineRenderer = handLines[i].GetComponent<LineRenderer>();
            if (i == 4){
                lineRenderer.SetPosition(0,hand[0].transform.position);
                lineRenderer.SetPosition(1,hand[5].transform.position);
            }
            else if (i == 8){
                lineRenderer.SetPosition(0,hand[5].transform.position);
                lineRenderer.SetPosition(1,hand[9].transform.position);
            }
            else if (i == 12){
                lineRenderer.SetPosition(0,hand[9].transform.position);
                lineRenderer.SetPosition(1,hand[13].transform.position);
            }
            else if (i == 16){
                lineRenderer.SetPosition(0,hand[13].transform.position);
                lineRenderer.SetPosition(1,hand[17].transform.position);
            }
            else if (i == 20){
                lineRenderer.SetPosition(0,hand[0].transform.position);
                lineRenderer.SetPosition(1,hand[17].transform.position);
            }
            else{
                lineRenderer.SetPosition(0,hand[i].transform.position);
                lineRenderer.SetPosition(1,hand[i+1].transform.position);
            }
            if (lineRenderer == null){
                success = false;
            }
        }
        return success;
    }

    /// <summary>
    /// Create the ray to represent the aim direction of the hand
    /// </summary>
    /// <param name="handRay">A game object that represents the ray.</param>
    /// <returns>Returns True if ray is successfully created</returns>
    bool createHandRay(ref GameObject handRay){
        bool success = true;
        handRay = new GameObject("HandDirection");
        LineRenderer lineRenderer = handRay.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, new Vector3(0,0,-1000));
        lineRenderer.SetPosition(1, new Vector3(0,0,-1000));
        handRay.tag = "Player";
        lineRenderer.material = handRayMaterial;
        if (lineRenderer== null){
            success = false;
        }
        return success;
    }

    /// <summary>
    /// Sets the positions for a hand ray using an origin and a direction.
    /// </summary>
    /// <param name="handRay">The GameObject with the LineRenderer component.</param>
    /// <param name="handOrigin">The starting point of the ray.</param>
    /// <param name="handDirection">The direction of the ray.</param>
    /// <param name="length">The length of the ray.</param>
    /// <returns>Returns true if the LineRenderer is successfully set.</returns>
    bool inputHandRay(ref GameObject handRay, Vector3 handOrigin, Vector3 handDirection, float length){
        bool success = true;
        LineRenderer lineRenderer = handRay.GetComponent<LineRenderer>();
        Vector3 endPosition = handOrigin + handDirection.normalized * length;
        lineRenderer.SetPosition(0, handOrigin);
        lineRenderer.SetPosition(1, endPosition);
        if (lineRenderer == null){
            success = false;
        }
        return success;
    }

    /// <summary>
    /// Reset all the hand data.
    /// </summary>
    /// <param name="handRay">A game object that represents the ray.</param>
    /// <param name="hand">A list that store represents the hand landmarks</param>
    /// <param name="handLines">A list of line objects that represents the lines between landmarks.</param>
    /// <returns>Returns True if all data are successfully reseted</returns>
    bool resetHand(ref GameObject[] hand, ref GameObject[] handLines, ref GameObject handRay){
        bool success = true;
        for(int i=0; i<21; i++){
            Destroy(hand[i]);
            Destroy(handLines[i]);

            if(hand[i] == null && handLines[i] == null){
                success = false;
            }
        }
        Destroy(handRay);
        if (handRay == null){
            success = false;
        }
        return success;
    }

    /// <summary>
    /// Calculate the middle / origin point of a hand
    /// </summary>
    /// <param name="originPoint">A game object that represents the ray.</param>
    /// <param name="hand">A list of sphere objects that represents the landmark.</param>
    /// <returns>Returns True if origin point succesfully calculated</returns>
    bool calculateOrigin(ref Vector3 originPoint, GameObject[] hand){
        bool success = true;
        Vector3 pointA = hand[0].transform.localPosition;
        Vector3 pointB = hand[5].transform.localPosition;
        Vector3 pointC = hand[17].transform.localPosition;

        Vector3 center = pointA + pointB + pointC;
        center.x = center.x/3;
        center.y = center.y/3;

        originPoint = center;

        return success;
    }

    /// <summary>
    /// Checks if the hand gesture means shooting
    /// </summary>
    /// <param name="first">If true means it is the first hand else it is the second hand.</param>
    /// <returns>Returns True if the hand gesture means shooting.</returns>
    public bool checkShoot(bool first){
        bool shoot = false;
        if(first){
            shoot = firstHandFingerUp > 3;
        }
        else{
            shoot = secondHandFingerUp > 3;
        }
        return shoot;
    }

    /// <summary>
    /// Returns if the hand is recognized
    /// </summary>
    /// <returns>Returns True if the hand is recognized.</returns>
    public bool checkHandRecognized(){
        return handRecognized;
    }

    /// <summary>
    /// Returns if the hand is recognized
    /// </summary>
    /// <returns>Returns True if the hand is recognized.</returns>
    public bool checkBothHandRecognized(){
        return bothHandRecognized;
    }

    /// <summary>
    /// Return where the player is dodging.
    /// </summary>
    /// <returns>Returns the direction of dodge.</returns>
    public string dodging(){
        string dodgeDirection = "NotDodging";
        if(!checkBothHandRecognized()){
            return dodgeDirection;
        }
        if(firstHandOrigin.x != 0 && secondHandOrigin.x != 0){
            if((firstHandOrigin.x - body.transform.localPosition.x < -3 )&& (secondHandOrigin.x - body.transform.localPosition.x < -3)){
                dodgeDirection = "Right";
            }
            if((firstHandOrigin.x - body.transform.localPosition.x > 3) && (secondHandOrigin.x - body.transform.localPosition.x > 3)){
                dodgeDirection = "Left";
            }
        }
        return dodgeDirection;
    }
}