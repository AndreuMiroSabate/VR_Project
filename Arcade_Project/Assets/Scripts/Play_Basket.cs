using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Play_Basket : MonoBehaviour
{
    public GameObject Ball1;
    public GameObject Ball2;
    public GameObject Ball3;

    public GameObject Button;
    public UnityEvent OnPressed;
    public UnityEvent OnReleased;
    private GameObject presser;
    private bool isPressed;

    private float BasketTime;
    public TextMeshPro timeText;

    private bool Playing;

    private void Start()
    {
        isPressed = false;
        Playing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Playing == true)
        {
            BasketTime -= Time.deltaTime;

            if (BasketTime <= 0)
            {
                BasketTime = 0;
                Playing = false;
            }
            timeText.text = BasketTime.ToString();
        }
    }

    public void StartGame()
    {
        BasketTime = 90;
        Ball1.SetActive(true);
        Ball2.SetActive(true);
        Ball3.SetActive(true);

        Playing = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isPressed)
        {
            Button.transform.localPosition -= new Vector3(0.003f, 0, 0);
            presser = other.gameObject;
            OnPressed.Invoke();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == presser)
        {
            Button.transform.localPosition += new Vector3(0.015f,0,0);
            OnReleased.Invoke();
            isPressed = false;
        }
    }
}
