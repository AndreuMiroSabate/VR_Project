using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;

public class Mole : MonoBehaviour
{
    public float visibleheight = 0.0396f;
    public float hiddenHeight = 0.02781f;

    private Vector3 myNewXYZPosition;

    public float speed = 4f;

    public float hideMoleTimer = 1.5f;

    void Awake()
    {
        HideMole();

        transform.localPosition = myNewXYZPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, myNewXYZPosition, Time.deltaTime * speed);

        hideMoleTimer -= Time.deltaTime;    
        if(hideMoleTimer < 0) 
        {
            HideMole();
        }
    }

    public void HideMole()
    {
        myNewXYZPosition = new Vector3(transform.localPosition.x, hiddenHeight, transform.localPosition.z);

    }

    public void ShowMole()
    {
        myNewXYZPosition = new Vector3(transform.localPosition.x, visibleheight, transform.localPosition.z);

        hideMoleTimer = 1.5f;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Mazo")
        {
            HideMole();
        }
    }

}
