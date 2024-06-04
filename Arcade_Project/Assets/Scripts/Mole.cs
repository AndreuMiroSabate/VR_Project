using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Mole : MonoBehaviour
{
    public float visibleheight = 0.0396f;
    public float hiddenHeight = 0.02781f;

    private Vector3 myNewXYZPosition;

    public float speed = 4f;

    public float hideMoleTimer = 1.5f;

    public TextMeshPro ScoreText;
    private int score = 0;

    void Awake()
    {
        HideMole();

        transform.localPosition = myNewXYZPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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

        ScoreText.text = score.ToString();
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


    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.transform.tag == "Mazo")
    //    {
    //        score += 1;
    //        HideMole();
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Mazo")
        {
            score += 1;
            HideMole();
        }
    }


}
