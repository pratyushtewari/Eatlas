using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGlobe : MonoBehaviour
{
    public Rigidbody rb;
    // Amount of the degrees the globle rotate per frame
    public float angleRate = 5f;
    private float angleRateOrig = 5f;
    // If the mouse is being dragged then this determines 
    // how much to rotate the globe with the length of the drag
    public float dragSpeedFactor = 830;
    // Multipler to determine amount of angle to rotate with the speed of the swipe
    public float rotationMultiplier = 50.0f;
    private float startTime;
    private Vector3 startPos;
    private float dragForce = 0f;

    // This is the coroutine holder for the rotation swipe
    private IEnumerator coroutine;
    bool CR_Running = false;
    // 1 for left to right and -1 for right to left.
    float direction = 1;

    // Update is called once per frame
    void Update()
    {
        // Rotate this at angleRate per second
       transform.Rotate(direction * Vector3.down * angleRate / 100 ); 

       // Make the skybox rotate with the globe slower than the globe
       RenderSettings.skybox.SetFloat("_Rotation",  direction * angleRate*Time.time/1000);
    }

    void OnMouseDrag() {
        float rotAroundY = Input.GetAxis("Mouse X")*dragSpeedFactor*Mathf.Deg2Rad;
        transform.Rotate(Vector3.down, rotAroundY);
        // Make the skybox rotate with the globe slower than the globe - NOT WORKING
        // RenderSettings.skybox.SetFloat("_Rotation",  rotY*1000);
        
    }
    
    void OnMouseDown() {
        // Stop the coroutine if running
        if (CR_Running && coroutine != null) {
            StopCoroutine(coroutine);
            angleRate = 0;
        }
        angleRate = 0;
        startTime = Time.time;
        startPos = Input.mousePosition;
        startPos.z = transform.position.z - Camera.main.transform.position.z;
        startPos = Camera.main.ScreenToWorldPoint(startPos);
    }
    
    void OnMouseUp() {
        // Stop the coroutine if running
        if (CR_Running && coroutine != null) {
            StopCoroutine(coroutine);
        }
        angleRate = 0;
        var endPos = Input.mousePosition;
        endPos.z = transform.position.z - Camera.main.transform.position.z;
        endPos = Camera.main.ScreenToWorldPoint(endPos);
        var dragLength = endPos - startPos;
        Debug.Log("startPos: " + startPos);
        Debug.Log("endPos: " + endPos);

        Debug.Log(">>>> Force Manitude: " + dragLength.magnitude);
        // if (force <= 0 ) {
        //     // that means you might have swiped from right to left.
        // }
        
        float dragSpeed = dragLength.magnitude / (Time.time - startTime);
        if (dragLength.x < 0) {
            // spin reverse
            direction = -1; 
        } else {
            direction = 1;
        }

        if (dragSpeed > 5) {
            //isGlobePushed = true;    
            angleRate = dragSpeed * rotationMultiplier;
            Debug.Log("globe was swiped = " + dragSpeed);
            float timetoStop = dragSpeed / 10;
            coroutine = loseLifeOvertime(angleRate, angleRate, timetoStop);
            StartCoroutine(coroutine);

        } else {
            // Stop the coroutine if running
            if (CR_Running && coroutine != null) {
                StopCoroutine(coroutine);
            }
            angleRate = 0;
            Debug.Log("globe was dragged to a stop = " + dragSpeed);
        }
    }

    // make the rotation angle linearly come to 0 in "duration" seconds
    IEnumerator loseLifeOvertime(float currentLife, float lifeToLose, float duration)
    {

        CR_Running = true;

        float counter = 0;

        //Get the current life of the player
        float startLife = currentLife;

        //Calculate how much to lose
        float endLife = currentLife - lifeToLose;

        //Stores the new player life
        float newPlayerLife = currentLife;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            newPlayerLife = Mathf.Lerp(startLife, endLife, counter / duration);
            //Debug.Log("Current Life: " + newPlayerLife);
            angleRate = newPlayerLife;
            yield return null;
        }
        CR_Running = false;
    }
}
