using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusWindow : MonoBehaviour
{

    public enum STATUS
    {
        MODELLING,
        EVOLVING,
        IMPORTING,
    }

    Text t; 
    private static STATUS m_st; 

    void Start()
    {
         t = GameObject.Find("StatusText").GetComponent<Text>();
    }

    void Update()
    {
        if (m_st == STATUS.MODELLING)
        {
            t.text = "Modelling";
        }
        else if (m_st == STATUS.IMPORTING)
        {
            t.text = "IMPORTING";
        }
        else if (m_st == STATUS.EVOLVING)
        {
            t.text = "EVOLVING";
        }
    }

    public static void SetStatus(STATUS st)
    {
        m_st = st; 
    }
}
