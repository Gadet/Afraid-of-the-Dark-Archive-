using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject door;
    private Animator openDoorAnim;
    private Boolean playerintrigger;
    private GameObject[] lights;

    // Start is called before the first frame update
    void Start()
    {
        openDoorAnim = door.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerintrigger && Input.GetKeyDown(KeyCode.E))
        {
            openDoorAnim.SetBool("open", true);
            //play access granted
            GetComponent<AudioSource>().Play();

            // turn off all of the lights after using the first computer
            if (CompareTag("FirstComputer"))
            {
                GameObject remove = GameObject.Find("StartCanvas");
                remove.SetActive(false);
                StartCoroutine(specialScene());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered trigger area");
            playerintrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player left trigger area");
            playerintrigger = false;
        }
    }

    private IEnumerator specialScene()
    {
        yield return new WaitForSeconds(2);
        // play power out
        GameObject.Find("Soundholder1").GetComponent<AudioSource>().Play();
        // turn lights off
        lights = GameObject.FindGameObjectsWithTag("BuildingLights");
        foreach (GameObject i in lights)
        {
            i.GetComponent<Light>().enabled = false;
        }
        // rawr
        yield return new WaitForSeconds(6);
        GameObject.Find("Soundholder2").GetComponent<AudioSource>().Play();
    }
}
