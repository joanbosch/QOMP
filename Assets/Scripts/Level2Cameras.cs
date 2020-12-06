﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Cameras : CamerasScript
{
    private int cameraState = 1;
    List<Vector3> cameras;
    private GameObject Player;

    // Smooth moving camera variables
    private bool firstIt;
    private bool movingCamera;
    private float elapsedTime;
    public float transitionTime = 1f; // Time in seconds
    private int lastCameraState = 1;
    private Vector3 lastCameraPos;


    // Smooth transition of the z component inside pipes!
    private bool firstItPipes;
    private bool movingCameraPipes;
    private float elapsedTimePipes;
    public float transitionTimePipes = 0.6f; // Time in seconds

    // Smooth transition to move the camera to the origin of the game.
    private bool cameraToOrigin;

    // ReEnable the Snake Mode
    //private EnableSnakeMode sm;

    // Start is called before the first frame update
    void Start()
    {
        setUpMovingCamera();
        setUpMovingPipesCamera();
        cameras = new List<Vector3>();
        cameras.Add(new Vector3(-5.28f, 0.3f, -5.3f)); // FOV: 60
        cameras.Add(new Vector3(-0.27f, 0.65f, -4.9f)); // FOV: 60
        cameras.Add(new Vector3(0.15f, -5.1f, -5.3f)); // FOV: 60
        cameras.Add(new Vector3(4.38f, -5.1f, -5.3f)); // FOV: 60
        cameras.Add(new Vector3(10.57f, -5.14f, -5.2f)); //FOV: 60
        cameras.Add(new Vector3(5.28f, 5.68f, -5.2f)); // FOV: 60
        cameras.Add(new Vector3(10.38f, 5.68f, -5.4f)); // FOV: 60
        cameras.Add(new Vector3(10.81f, 0.23f, -5f)); // FOV: 60 OK!
        cameras.Add(new Vector3(15.93f, 0.12f, -4.5f));
        Player = GameObject.Find("Player");
        cameraState = 1;
        gameObject.transform.position = cameras[0];

        // Move camera to origin variables
        cameraToOrigin = false;

        // If the camera is reset, reset the snake mode.
        //sm = GameObject.Find("Snake5").GetComponent<EnableSnakeMode>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingCamera)
        {
            moveCameraTo(lastCameraPos, cameras[cameraState - 1]);
        }
        else if (cameraToOrigin)
        {
            // The origin game must be in the first position of the cameras array!

            moveCameraTo(lastCameraPos, cameras[0]);
        }
        else
        {
            int nextCameraState = nextState(Player.transform.position, cameraState);
            if (cameraState == 10) followPlayer(lastCameraPos, new Vector3(Player.transform.position.x, Player.transform.position.y, -4f));
            if (cameraState != nextCameraState)
            {
                if ((nextCameraState == 10)) { setUpMovingPipesCamera(); }
                else movingCamera = true;
                lastCameraState = cameraState;
                lastCameraPos = gameObject.transform.position;
                cameraState = nextCameraState;

            }
        }
    }

    private void setUpMovingPipesCamera()
    {
        firstItPipes = true;
        movingCameraPipes = false;
        elapsedTimePipes = 0f;
    }

    private void followPlayer(Vector3 from, Vector3 to)
    {
        if (!firstItPipes) elapsedTimePipes += Time.deltaTime;
        else firstItPipes = false;

        float t = elapsedTimePipes / transitionTimePipes;
        if (t > 1.0f) t = 1.0f;
        Debug.Log(t);

        gameObject.transform.position = Vector3.Lerp(from, to, t);

    }

    private void setUpMovingCamera()
    {
        movingCamera = false;
        cameraToOrigin = false;
        firstIt = true;
        elapsedTime = 0f;
    }

    private void moveCameraTo(Vector3 from, Vector3 to)
    {
        if (!firstIt) elapsedTime += Time.deltaTime;
        else firstIt = false;

        float t = elapsedTime / transitionTime;
        if (t >= 1f)
        {
            t = 1f;
            setUpMovingCamera();
        }

        gameObject.transform.position = Vector3.Lerp(from, to, t);
    }

    public override void moveCameraToOrigin()
    {
        cameraToOrigin = true;
        cameraState = 1;
        //sm.reActive();
    }

    private int nextState(Vector3 playerPos, int cameraState)
    {
        float x = playerPos.x;
        float y = playerPos.y;
        if (cameraState == 1)
        {
            if (x >= -3.28f) return 2;
            else return 1;
        }

        else if (cameraState == 2)
        {
            if (x <= -3.28f) return 1;
            else if (y <= -2.02f) return 3;
            else return 2;
        }

        else if (cameraState == 3)
        {
            if (y >= -2.02f) return 2;
            else if (x >= 2.63f) return 4;
            else return 3;
        }

        else if (cameraState == 4)
        {
            if (x <= 2.63f) return 3;
            else if (x >= 7.66f) return 5;
            else if (y >= -2.46f) return 10;
            else return 4;
        }

        else if (cameraState == 5)
        {
            if (x <= 7.66f) return 4;
            else if (y >= -2.38f) return 8;
            else return 5;
        }

        else if (cameraState == 6)
        {
            if (x >= 6.86f) return 7;
            else if (y <= 3f) return 10;
            else return 6;
        }

        else if (cameraState == 7)
        {
            if (x <= 6.86f) return 6;
            else if (y <= 2.83f) return 8;
            else return 7;
        }

        else if (cameraState == 8)
        {
            if (y <= -2.38f) return 5;
            else if (y >= 2.83f) return 7;
            else if (x >= 13.65f) return 9;
            else return 8;
        }

        else if (cameraState == 9)
        {
            if (x <= 13.65f) return 8;
            else return 9;
        }

        else if (cameraState == 10)
        {
            if (y <= -2.46f) return 4;
            else if (y >= 3f) return 6;
            else return 10;
        }

        return 0;
    }
}
