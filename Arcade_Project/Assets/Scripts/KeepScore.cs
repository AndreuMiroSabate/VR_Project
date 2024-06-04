using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeepScore : MonoBehaviour
{
    // Start is called before the first frame update
    private int Score;
    public TextMeshPro scoreUI;
    public AudioClip pointSound;  // Variable para el sonido
    public AudioSource audioSource;  // Variable para el AudioSource

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();  // Obtén el componente AudioSource
    }

    // Update is called once per frame
    void Update()
    {
        scoreUI.text = Score.ToString();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Ball")
        {
            Score += 1;
            PlayPointSound();  // Reproduce el sonido después de sumar un punto
        }
    }

    void PlayPointSound()
    {
        if (pointSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pointSound);
        }
    }

    public void ResetScore()
    {
        Score = 0;
    }
}
