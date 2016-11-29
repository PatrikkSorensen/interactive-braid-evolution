using UnityEngine;
using System.Collections;
using System;
using DG.Tweening; 

public class ShowcaseStep : MonoBehaviour {

    public GameObject braid1, braid2, braid3;

    private void Start()
    {
        braid1.SetActive(false);
        braid2.SetActive(false);
        braid3.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            StartCoroutine(RainBraids()); 
    }

    private IEnumerator RainBraids()
    {
        int multiplier = 21; 
        braid1.SetActive(true);
        Vector3 endPos = braid1.transform.position + Vector3.down * multiplier;
        braid1.transform.DOMove(endPos, 2.0f); 
        yield return new WaitForSeconds(1.0f);

        braid2.SetActive(true);
        endPos = braid2.transform.position + Vector3.down * multiplier;
        braid2.transform.DOMove(endPos, 2.0f);
        yield return new WaitForSeconds(1.0f);

        braid3.SetActive(true);
        endPos = braid3.transform.position + Vector3.down * multiplier;
        braid3.transform.DOMove(endPos, 2.0f);
        yield return new WaitForSeconds(1.0f);
    }
}
