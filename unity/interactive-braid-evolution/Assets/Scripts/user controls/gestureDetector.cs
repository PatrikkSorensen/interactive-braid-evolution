using UnityEngine;
using System.Collections.Generic;
//using Leap;
//using Leap.Unity;

public class gestureDetector : MonoBehaviour
{
    //LeapProvider provider;

    //void Start()
    //{
    //    provider = FindObjectOfType<LeapProvider>() as LeapProvider;
    //}

    //void Update()
    //{
    //    PointingTowards();

    //    if (DetectFist())
    //        MoveObjectToHand();
    //}

    //bool DetectFist()
    //{
    //    Frame frame = provider.CurrentFrame;

    //    foreach (Hand hand in frame.Hands)
    //        if(hand.IsLeft)
    //            foreach(Finger f in hand.Fingers)
    //                if (f.IsExtended)
    //                    return false;  

    //    return true; 
    //}

    //void PointingTowards ()
    //{
    //    Frame frame = provider.CurrentFrame;

    //    foreach (Hand hand in frame.Hands)
    //        if (hand.IsRight)
    //            foreach (Finger f in hand.Fingers)
    //                if (f.IsExtended)
    //                    Debug.Log(f.Direction);
    //}

    //void MoveObjectToHand()
    //{
    //    Frame frame = provider.CurrentFrame;
    //    foreach (Hand hand in frame.Hands)
    //    {
    //        if (hand.IsLeft)
    //        {
    //            transform.position = hand.PalmPosition.ToVector3() +
    //                                 hand.PalmNormal.ToVector3() *
    //                                (transform.localScale.y * .5f + .02f);
    //            transform.rotation = Quaternion.Euler(Vector3.right); //hand.Basis.Rotation();
    //        }
    //    }
    //}
}
