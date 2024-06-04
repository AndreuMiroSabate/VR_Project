using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Play_Mole : MonoBehaviour
{
    public TextMeshPro timerText;
    public float gameTimer = 30f;

    public GameObject moleContainer;
    private Mole[] moles;
    public float showMoleTimer = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        moles = moleContainer.GetComponentsInChildren<Mole>();
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer -= Time.deltaTime;
        timerText.text = Mathf.FloorToInt(gameTimer).ToString();

        showMoleTimer -= Time.deltaTime;
        if(showMoleTimer < 0f)
        {
            moles[Random.Range(0, moles.Length)].ShowMole();

            showMoleTimer = 1.5f;

        }
    }
}
