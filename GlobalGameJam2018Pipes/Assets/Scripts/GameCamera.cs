﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class GameCamera : MonoBehaviour
    {
        /*
    Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
    Converted to C# 27-02-13 - no credit wanted.
    Simple flycam I made, since I couldn't find any others made public.  
    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    shift : Makes camera accelerate
    space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/


        float mainSpeed = 100.0f; //regular speed
        float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
        float maxShift = 1000.0f; //Maximum speed when holdin gshift
        //private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
        private float totalRun = 1.0f;
        private float yPosition = 60; //Zoom

        private int[] xBounds = { -50, 60 };
        private int[] yBounds = { 5, 70 };
        private int[] zBounds = { -50, 30 };

        void Update()
        {

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                yPosition += scroll * -10;
                if (yPosition < yBounds[0])
                {
                    yPosition = yBounds[0];
                }
                if (yPosition > yBounds[1])
                {
                    yPosition = yBounds[1];
                }
            }
            //lastMouse = Input.mousePosition - lastMouse;
            //lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
            //lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
            //transform.eulerAngles = lastMouse;
            //lastMouse = Input.mousePosition;
            //Mouse  camera angle done.  

            //Keyboard commands
            Vector3 p = GetBaseInput();
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }

            p = p * Time.deltaTime;
            Vector3 newPosition = transform.position;
            //if (Input.GetKey(KeyCode.Space))
            //{ //If player wants to move on X and Z axis only
            transform.Translate(p);

            newPosition.x = transform.position.x;
            if (newPosition.x < xBounds[0])
            {
                newPosition.x = xBounds[0];
            }
            if (newPosition.x > xBounds[1])
            {
                newPosition.x = xBounds[1];
            }
            newPosition.z = transform.position.z;
            if (newPosition.z < zBounds[0])
            {
                newPosition.z = zBounds[0];
            }
            if (newPosition.z > zBounds[1])
            {
                newPosition.z = zBounds[1];
            }

            newPosition.y = yPosition;

            transform.position = newPosition;
            //}
            //else
            //{
            //    transform.Translate(p);
            //}

        }

        private Vector3 GetBaseInput()
        { //returns the basic values, if it's 0 than it's not active.
            Vector3 p_Velocity = new Vector3();
            if (Input.GetKey(KeyCode.W))
            {
                p_Velocity += new Vector3(0, 0, 1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                p_Velocity += new Vector3(0, 0, -1);
            }
            if (Input.GetKey(KeyCode.A))
            {
                p_Velocity += new Vector3(-1, 0, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                p_Velocity += new Vector3(1, 0, 0);
            }
            return p_Velocity;
        }
    }
}
