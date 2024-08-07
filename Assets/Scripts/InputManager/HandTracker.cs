using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using UnityEngine.Rendering;

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
    [Tooltip("Hand Object Applied.")]
    [SerializeField] private bool handsObjExist;
    [Tooltip("Left Hand.")]
    [SerializeField] private GameObject leftHand;
    [Tooltip("Right Hand.")]
    [SerializeField] private GameObject rightHand;

    [Tooltip("Manager type.\n0 = None \n1 = Game \n2 = Main Menu \n3 = Level \n4 = Leaderboard \n5 = Tutorial")]
    [SerializeField] private int managerType;
    [Tooltip("The game manager.")]
    [SerializeField] private GameManager gameManager;
    [Tooltip("The menu manager.")]
    [SerializeField] private MainMenuSceneManager mainMenuSceneManager;
    [Tooltip("The level manager.")]
    [SerializeField] private LevelSceneManager levelSceneManager;
    [Tooltip("The level manager.")]
    [SerializeField] private TutorialSceneManager tutorialSceneManager;
    [Tooltip("The confirmation quit app object.")]
    [SerializeField] private GameObject confirmationQuitApp;
    [Tooltip("The pause app object.")]
    [SerializeField] private GameObject pauseApp;
    [Tooltip("The game over app object.")]
    [SerializeField] private GameObject gameOverApp;
    private GameObject firstHandRay, secondHandRay;
    private GameObject[] secondHand, secondHandLines, firstHand, firstHandLines;
    private int firstHandFingerUp, secondHandFingerUp = 0;
    private string firstHandGesture, secondHandGesture = "";
    private bool handRecognized = false;
    private bool bothHandRecognized = false;
    private int firstHandConfirmation, secondHandConfirmation = 0;
    private GameObject body;
    private Vector3 firstVelocity, secondVelocity;
    private string firstType, secondType;
    private int gestureConfirmationStart, gestureConfirmationQuit, gestureConfirmationLogin, gestureConfirmationLB, gestureConfirmationSetting, gestureConfirmationBeginner, gestureConfirmationIntermediate, gestureConfirmationAdvanced, gestureConfirmationTutorial, gestureConfirmationQuitConfirm, gestureConfirmationQuitCancel, gestureConfirmationMainMenu, gestureConfirmationResume, gestureConfirmationRestart, gestureConfirmationNext, gestureConfirmationPrevious, gestureConfirmationExit = 0;
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
        if (udpReceive == null)
        {
            GameObject udpReceiverObject = GameObject.FindGameObjectWithTag("UDPreceiver");
            if (udpReceiverObject != null)
            {
                udpReceive = udpReceiverObject.GetComponent<UDPReceive>();
                if (udpReceive == null)
                {
                    Debug.LogError("UDPReceive component not found on the GameObject with tag 'UDPreceiver'.");
                }
            }
            else
            {
                Debug.LogError("No GameObject with tag 'UDPreceiver' found in the scene.");
            }
        }
        // Extract data
        string data = udpReceive.data;
        if(firstType == "Left"){
            secondType = "Right";
        }
        else{
            secondType = "Left";
        }
        // Check for hand
        if (data.Length > 50){
            handRecognized = true;
            // Pre process data
            data = data.Remove(0,1);
            data = data.Remove(data.Length-1,1);
            string[] points = data.Split(','); 
            firstHandConfirmation++;
            if(firstHandConfirmation > 0){           
                if (firstHand[0] == null){
                    resetHand(ref firstHand, ref firstHandLines, ref firstHandRay);
                    createHandPoints(ref firstHand);
                    createHandLines(ref firstHandLines);
                    createHandRay(ref firstHandRay);
                }
                else{
                    inputHandPoints(points, ref firstHand, true);
                    inputHandLines(ref firstHandLines, firstHand);
                    inputHandDirection(ref firstHandDirection, firstHand, ref firstVelocity);
                    inputHandRay(ref firstHandRay, firstHandOrigin, firstHandDirection, 100);    
                }     
                firstHandGesture = points[63].Substring(2, points[63].Length - 3);;
                countFingerUp(ref firstHandFingerUp, firstHandGesture);
                firstType =  points[64].Substring(2, points[64].Length - 3);
                calculateOrigin(ref firstHandOrigin, firstHand);
                if(handsObjExist){
                    disableHandObj(firstType);
                }
            }

            // Check for second hand
            if(points.Length > 65){
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
                        inputHandPoints(points, ref secondHand, false);
                        inputHandLines(ref secondHandLines, secondHand);
                        inputHandDirection(ref secondHandDirection, secondHand, ref secondVelocity);
                        inputHandRay(ref secondHandRay, secondHandOrigin, secondHandDirection, 100);
                        
                    }
                    secondHandGesture = points[128].Substring(2, points[128].Length - 3);;
                    countFingerUp(ref secondHandFingerUp, secondHandGesture);
                    secondType = points[129].Substring(2, points[129].Length - 3);
                    calculateOrigin(ref secondHandOrigin, secondHand);
                    if(handsObjExist){
                        disableHandObj(secondType);
                    }
                }
            }
            else{
                // Reset second hand
                bothHandRecognized = false;
                secondHandConfirmation = 0;
                
                resetHand(ref secondHand, ref secondHandLines, ref secondHandRay);
                secondHandGesture = "";
                secondHandFingerUp = 0;
                secondHandOrigin = Vector3.zero;

                if(handsObjExist){
                    enableHandObj(secondType);
                }
            }
            checkGesture();
        }
        else{
            // Reset first hand landmarks
            handRecognized = false;
            bothHandRecognized = false;
            secondHandConfirmation = 0;
            firstHandConfirmation = 0;
            resetHand(ref firstHand, ref firstHandLines, ref firstHandRay);
            firstHandGesture = "";
            secondHandGesture = "";
            firstHandFingerUp = 0;
            secondHandFingerUp = 0;
            firstHandOrigin = Vector3.zero;

            if(handsObjExist){
                enableHandObj(firstType);
            }
        }
        if(!handRecognized){
            resetHand(ref firstHand, ref firstHandLines, ref firstHandRay);
            resetHand(ref secondHand, ref secondHandLines, ref secondHandRay);
            firstHandOrigin = Vector3.zero;
            secondHandOrigin = Vector3.zero;
            if(handsObjExist){
                enableHandObj("Left");
                enableHandObj("Right");
            }
        }
    }

    /// <summary>
    /// Create points to represent the hands landmarks
    /// </summary>
    /// <param name="hand">A list that will store the sphere objects</param>
    /// <returns>Returns True if all points are successfully created</returns>
    bool createHandPoints(ref GameObject[] hand){
        bool success = true;
        if (hand == null || hand.Length <= 21) {
            hand = new GameObject[21];
        }
        for (int i = 0; i < 21; i++){
            if (hand[i] != null) {
                Destroy(hand[i]);
            }
            hand[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            hand[i].name = "HandPoint" + i;
            if(gameOverApp != null && gameOverApp.activeInHierarchy || (pauseApp != null && pauseApp.activeInHierarchy)){
                hand[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            else{
                hand[i].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
            hand[i].transform.position = new Vector3(body.transform.localPosition.x , body.transform.localPosition.y, -1000);
            hand[i].tag = "Player";
            MeshRenderer meshRenderer = hand[i].GetComponent<MeshRenderer>();
            if (meshRenderer != null){
                meshRenderer.material = handPointMaterial;
                meshRenderer.sortingOrder = 1;
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
        int displacement2 = 2;

        if (first){
            displacement1 = 0;
            displacement2 = 0;
        }

        for(int i=0; i<21; i++){
            float x,y,z;
            if(gameOverApp != null && gameOverApp.activeInHierarchy || (pauseApp != null && pauseApp.activeInHierarchy)){
                x = (float.Parse(points[(i+displacement1)*3 + displacement2]) /100  + body.transform.localPosition.x ) * 0.5f ;
                y = (float.Parse(points[(i+displacement1)*3 + 1 + displacement2] )/100) * 0.5f + 3;
                z = (float.Parse(points[(i+displacement1)*3 + 2 + displacement2])/100) + 2;
            }
            else{
                x = float.Parse(points[(i+displacement1)*3 + displacement2]) /100 + body.transform.localPosition.x - 6.5f;
                y = float.Parse(points[(i+displacement1)*3 + 1 + displacement2])/100;
                z = float.Parse(points[(i+displacement1)*3 + 2 + displacement2])/100;
            }
            

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
        Vector3 pointB = hand[5].transform.localPosition;
        Vector3 pointC = hand[17].transform.localPosition;
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

        handDirection = Vector3.SmoothDamp(handDirection, newHandDirection, ref velocity, 0.1f).normalized;

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
            if(gameOverApp != null && gameOverApp.activeInHierarchy || (pauseApp != null && pauseApp.activeInHierarchy)){
                lineRenderer.startWidth = 0.05f;
                lineRenderer.endWidth = 0.05f;
            }
            else{
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.1f;
            }
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
        if(gameOverApp != null && gameOverApp.activeInHierarchy || (pauseApp != null && pauseApp.activeInHierarchy) || (managerType != 1)){
            lineRenderer.startWidth = 0;
            lineRenderer.endWidth = 0;
        }
        else{
            lineRenderer.startWidth = 0.01f;
            lineRenderer.endWidth = 0.01f;
        }
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
            if(hand[i] != null && handLines[i] != null)
            {
                Destroy(hand[i]);
                Destroy(handLines[i]);
            }
            if(hand[i] == null && handLines[i] == null){
                success = false;
            }
        }
        if(handRay != null){
            Destroy(handRay);
        }
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
            shoot = firstHandFingerUp == 5;
        }
        else{
            shoot = secondHandFingerUp == 5;
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

    /// <summary>
    /// Makes the hand object invisible
    /// </summary>
    /// <param name="type">The type of hand</param>
    public void disableHandObj(string type){
        if (type == "Left")
        {
            Component [] renderers = leftHand.GetComponentsInChildren(typeof(Renderer));
            foreach(Renderer renderer in renderers) {
                renderer.enabled = false;
            }
        }
        if (type == "Right") 
        {
            Component [] renderers = rightHand.GetComponentsInChildren(typeof(Renderer));
            foreach(Renderer renderer in renderers) {
                renderer.enabled = false;
            }
        }
    }

    /// <summary>
    /// Makes the hand object visible
    /// </summary>
    /// <param name="type">The type of hand</param>
    public void enableHandObj(string type){
        if (type == "Left")
        {
            Component [] renderers = leftHand.GetComponentsInChildren(typeof(Renderer));
            foreach(Renderer renderer in renderers) {
                renderer.enabled = true;
            }
        }
        if (type == "Right") 
        {
            Component [] renderers = rightHand.GetComponentsInChildren(typeof(Renderer));
            foreach(Renderer renderer in renderers) {
                renderer.enabled = true;
            }
        }
    }

    /// <summary>
    /// Get the type of hand
    /// </summary>
    /// <param name="first">The hand object's order.</param>
    /// <returns>Returns the hand objects type.</returns>
    public string checkHandType(bool first){
        string type = "";
        if(first){
            type = firstType;
        }
        else{
            type = secondType;
        }
        return type;
    }

    /// <summary>
    /// Get the finger up of a hand
    /// </summary>
    /// <param name="fingerUp">The hand object's order.</param>
    /// <param name="handGesture">The hand object's order.</param>
    public void countFingerUp(ref int fingerUp, string handGesture){
        bool isNumeric = int.TryParse(handGesture, out int fingers);
        if (isNumeric)
        {
            fingerUp = fingers;
        }
        else
        {
            fingerUp = 0;
        }
    }

    /// <summary>
    /// Activate a button based on hand gesture and the scene
    /// </summary>
    public void checkGesture(){
        switch(managerType)
        {
            case 1:
                if(gameManager != null)
                {
                    if ((firstHandFingerUp == 1 || secondHandFingerUp == 1)){
                        if(pauseApp != null && pauseApp.activeInHierarchy && gestureConfirmationResume>500){
                            gameManager.resumeGame();
                            gestureConfirmationQuitConfirm = 0;
                            gestureConfirmationRestart = 0;
                        }
                        gestureConfirmationResume++;
                        if(confirmationQuitApp != null && confirmationQuitApp.activeInHierarchy && gestureConfirmationQuitConfirm>500){
                            gameManager.QuitConfirmation();
                            gestureConfirmationResume = 0;
                            gestureConfirmationRestart = 0;
                        }
                        gestureConfirmationQuitConfirm++;
                        if(gameOverApp != null && gameOverApp.activeInHierarchy && gestureConfirmationRestart>500){
                            gameManager.restartGame();
                            gestureConfirmationResume = 0;
                            gestureConfirmationQuitConfirm = 0;
                        }
                        gestureConfirmationRestart++;
                    }
                    else if ((firstHandFingerUp == 2 || secondHandFingerUp == 2)){
                        if(((pauseApp != null && pauseApp.activeInHierarchy) || (gameOverApp != null && gameOverApp.activeInHierarchy)) && gestureConfirmationMainMenu>500){
                            gameManager.openMainMenuConfirmation();
                            gestureConfirmationQuitCancel = 0;
                        }
                        gestureConfirmationMainMenu++;
                        if(confirmationQuitApp != null &&  confirmationQuitApp.activeInHierarchy && gestureConfirmationQuitCancel>500){
                            gameManager.QuitCancel();
                            gestureConfirmationMainMenu = 0;
                        }
                        gestureConfirmationQuitCancel++;
                    }
                    else{
                        gestureConfirmationQuitCancel = 0;
                        gestureConfirmationQuitConfirm = 0;
                        gestureConfirmationResume = 0;
                        gestureConfirmationMainMenu = 0;
                    }
                }
                break;
            case 2:
                if(mainMenuSceneManager != null)
                {
                    if ((firstHandFingerUp == 1 || secondHandFingerUp == 1)){
                        if(confirmationQuitApp != null && !confirmationQuitApp.activeInHierarchy && gestureConfirmationStart>500){
                            mainMenuSceneManager.ChangeToChooseLevel();
                            gestureConfirmationQuitConfirm = 0;
                        }
                        if(confirmationQuitApp != null && confirmationQuitApp.activeInHierarchy && gestureConfirmationQuitConfirm>500){
                            mainMenuSceneManager.QuitConfirmation();
                            gestureConfirmationStart = 0;
                        }
                        gestureConfirmationStart++;
                        gestureConfirmationQuitConfirm++;
                    }
                    else if ((firstHandFingerUp == 2 || secondHandFingerUp == 2)){
                        if(confirmationQuitApp != null && !confirmationQuitApp.activeInHierarchy && gestureConfirmationLogin>500){
                            mainMenuSceneManager.OpenRegister();
                            gestureConfirmationQuitCancel = 0;
                        }
                        if(confirmationQuitApp != null &&  confirmationQuitApp.activeInHierarchy && gestureConfirmationQuitCancel>500){
                            mainMenuSceneManager.QuitCancel();
                            gestureConfirmationLogin = 0;
                        }
                        gestureConfirmationLogin++;
                        gestureConfirmationQuitCancel++;
                    }
                    else if ((firstHandFingerUp == 3 || secondHandFingerUp == 3)){
                        if(gestureConfirmationLB>500){
                            mainMenuSceneManager.OpenLeaderboard();
                        }
                        gestureConfirmationLB++;
                    }
                    else if ((firstHandFingerUp == 4 || secondHandFingerUp == 4)){
                        if(gestureConfirmationSetting>500){
                            mainMenuSceneManager.OpenSettings();
                        }
                        gestureConfirmationSetting++;
                    }
                    else if ((firstHandFingerUp == 5 || secondHandFingerUp == 5)){
                        if(gestureConfirmationQuit>500){
                            mainMenuSceneManager.QuitApplicationConfirmation();
                            print("quit");
                        }
                        gestureConfirmationQuit++;
                    }
                    else{
                        gestureConfirmationStart = 0;
                        gestureConfirmationLogin = 0;
                        gestureConfirmationLB = 0;
                        gestureConfirmationSetting = 0;
                        gestureConfirmationQuit = 0;
                        gestureConfirmationQuitCancel = 0;
                        gestureConfirmationQuitConfirm = 0;
                    }
                }
                break;
            case 3:
                if(levelSceneManager != null){
                    if ((firstHandFingerUp == 1 || secondHandFingerUp == 1)){
                        if(gestureConfirmationBeginner>500){
                            levelSceneManager.StartGameBeginner();
                        }
                        gestureConfirmationBeginner++;
                    }
                    else if ((firstHandFingerUp == 2 || secondHandFingerUp == 2)){
                        if(gestureConfirmationIntermediate>500){
                            levelSceneManager.StartGameIntermediate();
                        }
                        gestureConfirmationIntermediate++;
                    }
                    else if ((firstHandFingerUp == 3 || secondHandFingerUp == 3)){
                        if(gestureConfirmationAdvanced>500){
                            levelSceneManager.StartGameAdvanced();
                        }
                        gestureConfirmationAdvanced++;
                    }
                    else if ((firstHandFingerUp == 4 || secondHandFingerUp == 4)){
                        if(gestureConfirmationTutorial>500){
                            levelSceneManager.OpenTutorial();
                        }
                        gestureConfirmationTutorial++;
                    }
                    else{
                        gestureConfirmationTutorial = 0;
                        gestureConfirmationBeginner = 0;
                        gestureConfirmationIntermediate = 0;
                        gestureConfirmationAdvanced = 0;
                    }
                }
                break;
            case 4:
                if ((firstHandFingerUp == 1 || secondHandFingerUp == 1)){
                    if(gestureConfirmationMainMenu>500){
                        MainMenuSceneManager.OpenHome();
                    }
                    gestureConfirmationMainMenu++;
                }
                else{
                    gestureConfirmationMainMenu = 0;
                }
                break;
            case 5:
                if ((firstHandFingerUp == 1 || secondHandFingerUp == 1)){
                        if(gestureConfirmationNext>500){
                            tutorialSceneManager.Next();
                            gestureConfirmationNext = 0;
                        }
                        gestureConfirmationNext++;
                    }
                    else if ((firstHandFingerUp == 2 || secondHandFingerUp == 2)){
                        if(gestureConfirmationPrevious>500){
                            tutorialSceneManager.Previous();
                            gestureConfirmationPrevious = 0;
                        }
                        gestureConfirmationPrevious++;
                    }
                    else if ((firstHandFingerUp == 3 || secondHandFingerUp == 3)){
                        if(gestureConfirmationExit>500){
                            tutorialSceneManager.ChangeToChooseLevel();
                        }
                        gestureConfirmationExit++;
                    }
                    else{
                        gestureConfirmationExit = 0;
                        gestureConfirmationNext = 0;
                        gestureConfirmationPrevious = 0;
                    }
                break;
        }
        
    }

    /// <summary>
    /// Checks if the hand gesture means shooting a rocket
    /// </summary>
    /// <param name="first">If true means it is the first hand else it is the second hand.</param>
    /// <returns>Returns True if the hand gesture means shooting a rocket.</returns>
    public bool checkShootRocket(bool first){
        bool shoot = false;
        if(first){
            shoot = firstHandGesture == "bomb";
        }
        else{
            shoot = secondHandGesture == "bomb";
        }
        return shoot;
    }

    /// <summary>
    /// Checks if the hand gesture means shooting a laser
    /// </summary>
    /// <param name="first">If true means it is the first hand else it is the second hand.</param>
    /// <returns>Returns True if the hand gesture means shooting a laser.</returns>
    public bool checkShootLaser(bool first){
        bool shoot = false;
        if(first){
            shoot = firstHandGesture == "laser";
        }
        else{
            shoot = secondHandGesture == "laser";
        }
        return shoot;
    }
}