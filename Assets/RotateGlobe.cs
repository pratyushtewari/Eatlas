using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGlobe : MonoBehaviour
{
    public Rigidbody rb;
    public float angleRate = 5f;
    public float dragSpeedFactor = 1000;
    private float angleRateOrig;
    public float rotationMultiplier = 15.0f;
    private float startTime;
    private Vector3 startPos;
    private bool isGlobePushed = false;
    private float dragForce = 0f;
    // Start is called before the first frame update
    void Start()
    {
        angleRateOrig = angleRate;
       
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate this at angleRate per second
       transform.Rotate(Vector3.down * angleRate / 100 ); 

       // Make the skybox rotate with the globe slower than the globe
       RenderSettings.skybox.SetFloat("_Rotation",  angleRate*Time.time/1000);
    }

    void OnMouseDrag() {
        float rotY = Input.GetAxis("Mouse X")*dragSpeedFactor*Mathf.Deg2Rad;
        transform.Rotate(Vector3.down, rotY);
        
    }
    
    void OnMouseDown() {
        angleRate = 0;
        startTime = Time.time;
        startPos = Input.mousePosition;
        startPos.z = transform.position.z - Camera.main.transform.position.z;
        startPos = Camera.main.ScreenToWorldPoint(startPos);
    }
    
    void OnMouseUp() {
        var endPos = Input.mousePosition;
        endPos.z = transform.position.z - Camera.main.transform.position.z;
        endPos = Camera.main.ScreenToWorldPoint(endPos);
        var force = endPos - startPos;
        force.z = force.magnitude;
        force /= (Time.time - startTime);
        
        // angleRate = force.magnitude * factor;
        dragForce = force.magnitude;
        if (dragForce > 5) {
            //isGlobePushed = true;    
            angleRate = force.magnitude * rotationMultiplier;
            Debug.Log("globe was swiped = " + dragForce);
            StartCoroutine(loseLifeOvertime(angleRate, angleRate, 2));

        } else {
            Debug.Log("globe was dragged to a stop = " + dragForce);
        }
    }

    // make the rotation angle linearly come to 0 in "duration" seconds
    IEnumerator loseLifeOvertime(float currentLife, float lifeToLose, float duration)
    {
        //Make sure there is only one instance of this function running
        if (isGlobePushed)
        {
            yield break; ///exit if this is still running
        }
        isGlobePushed = true;

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

        isGlobePushed = false;
    }

    // //inside class
    // Vector2 firstPressPos;
    // Vector2 secondPressPos;
    // Vector2 currentSwipe;

    // public void Swipe()
    // {
    //     if(Input.touches.Length > 0)
    //     {
    //         Touch t = Input.GetTouch(0);

    //         if(t.phase == TouchPhase.Began)
    //         {
    //             //save began touch 2d point
    //             firstPressPos = new Vector2(t.position.x,t.position.y);
    //         }

    //         if(t.phase == TouchPhase.Ended)
    //         {
    //             //save ended touch 2d point
    //             secondPressPos = new Vector2(t.position.x,t.position.y);

    //             //create vector from the two points
    //             currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

    //             //normalize the 2d vector
    //             currentSwipe.Normalize();

    //             //swipe upwards
    //             if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
    //             {
    //                 Debug.Log("up swipe");
    //             }

    //             //swipe down
    //             if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
    //             {
    //                 Debug.Log("down swipe");
    //             }

    //             //swipe left
    //             if(currentSwipe.x < 0 && currentSwipe.y > -0.5f &&  currentSwipe.y < 0.5f)
    //             {
    //                 Debug.Log("left swipe");
    //             }

    //             //swipe right
    //             if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
    //             {
    //                 Debug.Log("right swipe");
    //             }
    //         }
    //     }
    // }
}
