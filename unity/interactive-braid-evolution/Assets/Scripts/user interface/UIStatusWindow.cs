using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class UIStatusWindow : MonoBehaviour
{

    public enum STATUS
    {
        UNINITIALIZED,
        MODELLING,
        EVOLVING,
        IMPORTING,
        SIMULATING
    }

    private static Text t;
    private Regex rgx; 

    private float nextActionTime = 0.0f;
    public float period = 1.0f;
    public int numDots = 0; 

    void Start()
    {
        t = GameObject.Find("StatusText").GetComponent<Text>();
        rgx = new Regex("[^a-zA-Z0-9 -]");
    }

    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;

            if (numDots == 3)
            {
                t.text = rgx.Replace(t.text, "");
                numDots = 0; 
            } else
            {
                t.text = t.text + ".";
                numDots++;
            }
        }
    }
    public static void SetStatus(STATUS st)
    {
        if(!t)
            t = GameObject.Find("StatusText").GetComponent<Text>();

        switch (st)
        {
            case STATUS.EVOLVING:
                t.text = "evolving";
                break;
            case STATUS.MODELLING:
                t.text = "modelling";
                break;
            case STATUS.IMPORTING:
                t.text = "importing models";
                break;
            case STATUS.SIMULATING:
                t.text = "simulating";
                break;
            case STATUS.UNINITIALIZED:
                t.text = "uninitialized";
                break;
            default:
                Debug.Log("Nothing happens..."); 
                break;
        }
    }
}
