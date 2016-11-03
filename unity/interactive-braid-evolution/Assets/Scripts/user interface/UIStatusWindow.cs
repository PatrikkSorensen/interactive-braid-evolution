using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    void Start()
    {
        t = GameObject.Find("StatusText").GetComponent<Text>();
    }
    public static void SetStatus(STATUS st)
    {
        Debug.Log("Setting status"); 
        t = GameObject.Find("StatusText").GetComponent<Text>();
        switch (st)
        {
            case STATUS.EVOLVING:
                Debug.Log("Setting it to evolging"); 
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
