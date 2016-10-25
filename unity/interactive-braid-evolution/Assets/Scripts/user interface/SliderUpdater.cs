using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class SliderUpdater : MonoBehaviour {

    public string key; 

    private Text txt;
    private Slider sl;
    private UIMsgDraftWindow draftWindow;

    void Start () {

        //draftWindow = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIMsgDraftWindow>();

        sl = GetComponentInChildren<Slider>();
        //sl.onValueChanged.AddListener(delegate { OnValueChange(); });

        txt = GetComponentInChildren<Text>();
        txt.text = key + " : " + sl.value; 

        //draftWindow.AddParam(key, (int)sl.value);
    }

    public void OnValueChange()
    {
        draftWindow.AddParam(key, (int)sl.value);
        txt.text = key + " : " + sl.value;
    }
}
