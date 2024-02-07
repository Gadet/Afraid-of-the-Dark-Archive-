using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HearingAid : MonoBehaviour
{
    public Transform player;
    public Transform monster;

    private string[] lowAlert = { "#00FF19", "30", "0.3" };
    private string[] medAlert = { "#FF9800", "45", "0.6" };
    private string[] highAlert = { "#FF1600", "60", "0.9" };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, monster.position);

        if (distance <= 10f)
        {
            SetColour(highAlert);
        }
        else if (distance <= 17f)
        {
            SetColour(medAlert);
        }
        else
        {
            SetColour(lowAlert);
        }
    }

    void SetColour(string[] lst)
    {
        Color i;
        bool lonely = ColorUtility.TryParseHtmlString(lst[0], out i);
        GetComponent<Image>().color = i;
        transform.position = new Vector2(137f, float.Parse(lst[1]));
        transform.localScale = new Vector2(0.3f, float.Parse(lst[2]));
    }
}
