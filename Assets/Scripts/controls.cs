using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controls : MonoBehaviour
{
    //Player Vars
    public GameObject Player;
    [Range(1.0f, 10.0f)]
    public float playerSpeed = 2.0f;

    //Camera Vars
    public GameObject Camera;
    float yaw = 0, pitch = 0;
    [Range(1.0f, 50.0f)]
    public float sensitivity = 25.0f;

    //Canvas Vars
    private GameObject inputMenu;
    private GameObject errorText;

    //Functionality
    void Start()
    {
        inputMenu = GameObject.Find("InputMenu");
        errorText = GameObject.Find("errorText");
    }

    private void cameraControl()
    {
        yaw += sensitivity * Input.GetAxis("Mouse X");
        pitch += sensitivity * Input.GetAxis("Mouse Y");

        if (pitch >= 90)
        {
            pitch = 90;
        }
        else if (pitch <= -90)
        {
            pitch = -90;
        }

        Camera.transform.eulerAngles = new Vector3(-pitch, yaw, 0.0f);
    }
    private void playerControl()
    {
        if(Input.GetKey(KeyCode.W))
            Player.transform.position = Player.transform.position + Camera.transform.forward * playerSpeed * Time.deltaTime;
        if(Input.GetKey(KeyCode.S))
            Player.transform.position = Player.transform.position - Camera.transform.forward * playerSpeed * Time.deltaTime;
        if(Input.GetKey(KeyCode.A))
            Player.transform.position = Player.transform.position - Camera.transform.right * playerSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            Player.transform.position = Player.transform.position + Camera.transform.right * playerSpeed * Time.deltaTime;
       
    }
    private void menuControl()
    {
        if (Input.GetKeyDown("return"))
        {
            inputMenu.SetActive(!inputMenu.activeInHierarchy);
            if (inputMenu.activeInHierarchy == false)
            {
                errorText.SetActive(true);
                if (fileReader.instance.fileName != null)
                {
                    fileReader.instance.readFile(fileReader.instance.fileName);
                }
            }
            else
                errorText.SetActive(false);
        }
    }

    void Update()
    {
        menuControl();
        if (!inputMenu.activeInHierarchy)
        {
            cameraControl();
            playerControl();
        }
    }
}
