using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //public float speed = 5f;
    //public float turnSpeed = 50f;
    //Rigidbody rb;
    GameManager gm;

    //public TextMeshProUGUI infoLog;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //rb = transform.GetComponent<Rigidbody>();
        //infoLog = GameObject.Find("Canvas").GetComponentInChildren<TextMeshProUGUI>();
        GameObject.Find("MinimapCam").GetComponent<CameraController>().target = transform;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime);
        //transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            //infoLog.text = "Goal Reached";
            gm.NextLevel();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene("DeathScreen");
        }
    }
}
