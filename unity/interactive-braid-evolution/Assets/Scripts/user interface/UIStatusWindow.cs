using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using APP_STATUS;

public class UIStatusWindow : MonoBehaviour
{
    public float period = 1.0f;
    public int numDots = 0;
    public static STATUS currentStatus;
    public static int modelsImported, totalModels;


    private static Text t;
    private Regex rgx; 
    private float nextActionTime = 0.0f;



    void Start()
    {
        currentStatus = STATUS.UNINITIALIZED; 
        t = GameObject.Find("StatusText").GetComponent<Text>();
        rgx = new Regex("[^a-zA-Z0-9 -]");
        modelsImported = totalModels = 0; 
    }

    void Update()
    {
        if (modelsImported == totalModels)
        {
            modelsImported = 0; 
            SetStatus(STATUS.SIMULATING);
        }

        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            if (currentStatus == STATUS.MODELLING)
                t.text = "modelling " + modelsImported + " out of " + totalModels;
            else
            {
                if (numDots == 3)
                {
                    t.text = rgx.Replace(t.text, "");
                    numDots = 0;
                }
                else
                {
                    t.text = t.text + ".";
                    numDots++;
                }
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
                currentStatus = STATUS.EVOLVING;
                t.text = "evolving";
                break;
            case STATUS.MODELLING:
                currentStatus = STATUS.MODELLING;
                t.text = "modelling " + modelsImported + " out of " + totalModels;
                break;
            case STATUS.IMPORTING:
                currentStatus = STATUS.IMPORTING;
                t.text = "importing models";
                break;
            case STATUS.SIMULATING:
                currentStatus = STATUS.SIMULATING;
                t.text = "simulating";
                break;
            default:
                Debug.Log("Nothing happens..."); 
                break;
        }
    }
}

namespace APP_STATUS
{
    public enum STATUS
    {
        UNINITIALIZED,
        MODELLING,
        EVOLVING,
        IMPORTING,
        SIMULATING
    }
}
