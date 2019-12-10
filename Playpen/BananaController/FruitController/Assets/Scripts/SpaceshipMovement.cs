using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;

public class SpaceshipMovement : MonoBehaviour
{
    public float forceAmount = 5f;
    public float ExtraThrust = 0;
    public GameObject[] thrusters;
    public Camera mainCam;
    private Rigidbody rb;
    private Rigidbody2D rb2D;
    private Vector3 thrustPos0;
    private Vector3 thrustPos1;
    private Vector3 thrustPos2;
    private Vector3 thrustPos3;
    int rotationLimit = 20;
    int ZAmplification = 100;

    //Communication
    SerialPort sp = new SerialPort("COM6", 9600);
    private int readValue = -1;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb2D = GetComponent<Rigidbody2D>();

        thrustPos0 = thrusters[0].gameObject.transform.position;
        thrustPos1 = thrusters[1].gameObject.transform.position;
        thrustPos2 = thrusters[2].gameObject.transform.position;
        thrustPos3 = thrusters[3].gameObject.transform.position;

        //Comms
        sp.Open();
        sp.ReadTimeout = 1;
    }

    // Update is called once per frame
    void Update()
    {
        mainCam.transform.position = new Vector3(mainCam.transform.position.x, this.gameObject.transform.position.y, -1);

        if (sp.IsOpen)
        {
            try
            {
                readValue = (int)sp.ReadByte();
                print("got serial data: " + readValue);
            }
            catch (System.Exception)
            {
                readValue = -1;
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || readValue == 0)
        {
            Thrust(1);
        }
        if (Input.GetKey(KeyCode.S) || readValue == 1)
        {
            Thrust(2);
        }
        if (Input.GetKey(KeyCode.K) || readValue == 2)
        {
            Thrust(3);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.L) || readValue == 4)
        {
            Thrust(4);
        }
        if (Input.GetKey(KeyCode.Space) || readValue == 5)
        {
            var locVel = transform.InverseTransformDirection(rb2D.velocity);
            locVel.y = ExtraThrust;
            rb2D.velocity = transform.TransformDirection(locVel);
        }

        if (this.gameObject.transform.rotation.z * ZAmplification <= -98 || this.gameObject.transform.rotation.z * ZAmplification >= 100)
        {
            GameOver();
        }
    }

    void Thrust(int thrustID)
    {
        Vector2 thrustForce;
        thrustForce = new Vector2(0, forceAmount * 10 * Time.deltaTime);

        Vector2 worldForcePosition = new Vector2();
        switch (thrustID)
        {
            case 1: worldForcePosition = transform.TransformPoint(thrustPos0); break;
            case 2: worldForcePosition = transform.TransformPoint(thrustPos1); break;
            case 3: worldForcePosition = transform.TransformPoint(thrustPos2); break;
            case 4: worldForcePosition = transform.TransformPoint(thrustPos3); break;
            default: print("ERROR: PlayerMovement -> Cannot Identify ThrustID"); break;
        }

        rb2D.AddForceAtPosition(thrustForce, worldForcePosition);
    }

    void GameOver()
    {
        print("Dead. Reload Game");
        Scene Scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(Scene.name);
    }
}
