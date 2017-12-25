using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class MovementScript : MonoBehaviour
{
    public float speed;

    Animator anim;
    Transform cam;
    Rigidbody rb;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float f = Input.GetAxis("Vertical") > 0 ? Input.GetAxis("Vertical") : 0;
        if (f > 0)
        {
            Vector3 camAngle = cam.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, camAngle.y ,0);
        }
        rb.velocity = transform.forward * speed * f;
        anim.SetFloat("Vel", f);
        	
	}
}
