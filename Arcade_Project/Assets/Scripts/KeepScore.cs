using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class KeepScore : MonoBehaviour
{
    // Start is called before the first frame update
    private int Score;
    public TextMeshPro scoreUI;

    // Update is called once per frame
    void Update()
    {
        scoreUI.text = Score.ToString();
    }

    void OnTriggerExit(Collider other)
    {
        if(other.transform.tag=="Ball")
        {
            Score += 1;
        }
    }

    public void ResetScore()
    {
        Score = 0;
    }
}
