using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChange : MonoBehaviour
{
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
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
            yield return new WaitForSeconds (1.0f);
            gameObject.GetComponent<Text>().text = "Now Loading.";
            yield return new WaitForSeconds (1.0f);
            gameObject.GetComponent<Text>().text = "Now Loading..";
            yield return new WaitForSeconds (1.0f);
            gameObject.GetComponent<Text>().text = "Now Loading...";
            yield return new WaitForSeconds (1.0f);
        }
    }
}
