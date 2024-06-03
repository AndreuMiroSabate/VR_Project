using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Mole : MonoBehaviour
{
    public float visibleheight = 0.0396f;
    public float hiddenHeight = 0.02781f;

    private Vector3 myNewXYZPosition;

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
        
    }

    public void HideMole()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, hiddenHeight, transform.localPosition.z);

    }
}
