using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerRequestController : MonoBehaviour
{
    [SerializeField]
    Text Message;

    [SerializeField]
    InputField userName;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
		{
            GetUsers();
		}
    }
    public IEnumerator GetUser()
	{
        using(var req = UnityWebRequest.Get(@"http://localhost:8000/users/"))
        {
            yield return req.SendWebRequest();
            if(req.error != null)
            {
                Debug.Log(req.error);
            }
            else
            {
                Debug.Log(req.downloadHandler.text);
            }
        }
    }
    public async void GetUsers()
	{
        var client = new HttpClient();
        var resulut = await client.GetAsync(@"localhost:8000/users/");
        var json = await resulut.Content.ReadAsStringAsync();

        Log(json.ToString());
	}

    // テキストに表示するためのメソッド
    void Log(string msg)
    {
        Message.text += msg + "\n";

        var array = Message.text.Split('\n');

        if(array.Length > 8)
        {
            string s = "";
            for(int i = 1; i < array.Length; i++)
            {
                s += array[i] + "\n";
            }
            Message.text = s;
        }
    }
}
