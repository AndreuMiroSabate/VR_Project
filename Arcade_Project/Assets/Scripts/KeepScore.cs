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

    public AudioClip point;

    AudioSource audioSource = GetComponent<AudioSource>();

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
            audioSource.PlayOneShot(point);

        }
    }

    public void ResetScore()
    {
        Score = 0;
    }
}
