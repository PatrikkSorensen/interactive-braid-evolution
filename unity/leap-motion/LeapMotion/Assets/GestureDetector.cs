using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Leap;
using Leap.Unity;

public class GestureDetector : MonoBehaviour
{
    LeapProvider provider;

    private bool m_shouldDrag; 
    public bool shouldDrag
    {
        get { return m_shouldDrag; }
        set { shouldDrag = value; }
    }

    void Start()
    {
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
    }

    void Update()
    {
        //PointingTowards();

        if (DetectFist())
            MoveObjectToHand();

        //DetectSwipeGesture(); 

    }

    void DetectPointMotion()
    {
        // TODO : Implement selection motion
        //TYPE_THUMB = = 0 -
        //TYPE_INDEX = = 1 -
        //TYPE_MIDDLE = = 2 -
        //TYPE_RING = = 3 -
        //TYPE_PINKY = = 4 -
    }

    void DetectSwipeGesture()
    {
        Frame frame = provider.CurrentFrame;
        if (frame.Hands.Count == 0)
            return; 

        if(frame.Hands[0].IsRight)
        {
            float z = frame.Hands[0].PalmNormal.z; 
            float velocity = frame.Hands[0].PalmVelocity.z;
            if (velocity > 0.15f)
            {
                Scrollbar s = FindObjectOfType<Scrollbar>();
                s.value += velocity; // * Time.deltaTime;
                Debug.Log("z: " + z + ", " + velocity);
            }
        }
    }

    bool DetectFist()
    {
        Frame frame = provider.CurrentFrame;

        foreach (Hand hand in frame.Hands)
            if (hand.IsLeft)
            {
                foreach (Finger f in hand.Fingers)
                    if (f.IsExtended)
                    {
                        m_shouldDrag = true;
                        return true;
                    }
                m_shouldDrag = false;
                return false; 
            } else if(hand.IsRight)
            {
                foreach (Finger f in hand.Fingers)
                    if (f.IsExtended)
                    {
                        m_shouldDrag = true;
                        return true;
                    }
                m_shouldDrag = false;
                return false;
            }

        m_shouldDrag = true; 
        return true; 
    }

    void PointingTowards ()
    {
        Frame frame = provider.CurrentFrame;

        foreach (Hand hand in frame.Hands)
            if (hand.IsRight)
                foreach (Finger f in hand.Fingers)
                    if (f.IsExtended)
                        Debug.Log(f.Direction);
    }

    void MoveObjectToHand()
    {
        Frame frame = provider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsLeft)
            {
                transform.position = hand.PalmPosition.ToVector3() +
                                     hand.PalmNormal.ToVector3() *
                                    (transform.localScale.y * .5f + .02f);
                transform.rotation = Quaternion.Euler(Vector3.right); //hand.Basis.Rotation();
            }
        }
    }
}
