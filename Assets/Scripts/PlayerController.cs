using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 50f;
    Rigidbody rb;

    public TextMeshProUGUI infoLog;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        infoLog = GameObject.Find("Canvas").GetComponentInChildren<TextMeshProUGUI>();
        Camera.main.GetComponent<CameraController>().target = transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime);
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            infoLog.text = "Goal Reached";
        }
    }
}
