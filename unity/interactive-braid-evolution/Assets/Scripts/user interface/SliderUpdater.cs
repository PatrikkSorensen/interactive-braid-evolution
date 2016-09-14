using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class SliderUpdater : MonoBehaviour {

    private Text txt;
    private Slider sl;
    private UIMsgDraftWindow draftWindow;

    void Start () {
        draftWindow = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIMsgDraftWindow>();

        sl = GetComponentInChildren<Slider>(); 
        txt = GetComponentInChildren<Text>();

        draftWindow.AddMessage(sl.value.ToString());
    }
}
