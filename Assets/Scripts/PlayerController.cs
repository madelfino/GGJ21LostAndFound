using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 50f;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        Camera.main.GetComponent<CameraController>().target = transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime);
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime);
    }
}
