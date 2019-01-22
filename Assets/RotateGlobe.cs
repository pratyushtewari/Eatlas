using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGlobe : MonoBehaviour
{
    public Rigidbody rb;
    public float angleRate = 5f;
    private float angleRateOrig;
    public float factor = 15.0f;
    public float friction = 20f;
    private float startTime;
    private Vector3 startPos;
    private bool isGlobePushed = false;
    // Start is called before the first frame update
    void Start()
    {
        angleRateOrig = angleRate;
       
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate this at angleRate per second
       transform.Rotate(Vector3.down * angleRate * Time.deltaTime); 

       // Make the skybox rotate with the globe slower than the globe
       RenderSettings.skybox.SetFloat(Shader.PropertyToID("_Rotation"), -1*Time.time * angleRate/10);
       if (isGlobePushed) {
           SlowDown();
       }
    }


    
    void OnMouseDown() {
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
        
        angleRate = force.magnitude * factor;

        isGlobePushed = true;
    }

    void SlowDown() {
        float newAngleRate = angleRate - Time.deltaTime * friction;
        if (angleRate >= 0) {
            angleRate = newAngleRate;
        } else {
            angleRate = 0.0f;
            // very important to stop calling this function after the globe has stopped
            isGlobePushed = false;
        }
    }
}
