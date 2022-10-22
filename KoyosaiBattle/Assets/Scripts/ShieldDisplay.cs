using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDisplay : MonoBehaviour
{
    
    //ä÷êîÇÃéQè∆
    public static ShieldDisplay instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Create()
    {
        gameObject.SetActive(true);
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}
