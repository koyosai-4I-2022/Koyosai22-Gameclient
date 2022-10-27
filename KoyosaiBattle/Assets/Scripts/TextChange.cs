using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChange : MonoBehaviour
{
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
            gameObject.GetComponent<Text>().text = "Now Loading";
            yield return new WaitForSeconds (TextChangeSpeed);
            gameObject.GetComponent<Text>().text = "Now Loading.";
            yield return new WaitForSeconds (TextChangeSpeed);
            gameObject.GetComponent<Text>().text = "Now Loading..";
            yield return new WaitForSeconds (TextChangeSpeed);
            gameObject.GetComponent<Text>().text = "Now Loading...";
            yield return new WaitForSeconds (TextChangeSpeed);
        }
    }
}
