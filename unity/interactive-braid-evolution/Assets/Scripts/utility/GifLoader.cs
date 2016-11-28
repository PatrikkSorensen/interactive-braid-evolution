using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GifLoader : MonoBehaviour {

    Texture2D[] frames;
    RawImage image; 
    float framesPerSecond;


    void Start()
    {
        image = GetComponent<RawImage>(); 
    }

	void Update () {
        int index =  (int) (Time.time * framesPerSecond);

        index = index % frames.Length;
        image.texture = frames[index];
    }
}
