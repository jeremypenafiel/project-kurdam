using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class footsteps : MonoBehaviour

{
    public AudioSource footstepsSound, sprintSound;



    void Update()
    {

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.X))
            {
                footstepsSound.enabled = false;
                sprintSound.enabled = true;
            }

            else if (Input.GetKey(KeyCode.C))
            {
                footstepsSound.enabled = false;
                sprintSound.enabled = false;
            }

            else

            {
                footstepsSound.enabled = true;
                sprintSound.enabled = false;
            }
        }

        else
        {
            footstepsSound.enabled = false;
            sprintSound.enabled = false;
        }

    }

}