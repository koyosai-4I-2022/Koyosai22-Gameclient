using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChange : MonoBehaviour
{
    // UIControllerÇ…ÉRÅ[ÉhÇà⁄êA

    public Text text;
    public float TextChangeSpeed;
    // Start is called before the first frame update
    void Start()
    {
        TextChangeSpeed = 1.0f;
        StartCoroutine ("Load");
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator Load()
    {
        for(;;){
            text.text = "Now Loading";
            yield return new WaitForSeconds (TextChangeSpeed);
            text.text = "Now Loading.";
            yield return new WaitForSeconds (TextChangeSpeed);
            text.text = "Now Loading..";
            yield return new WaitForSeconds (TextChangeSpeed);
            text.text = "Now Loading...";
            yield return new WaitForSeconds (TextChangeSpeed);
        }
    }
}
