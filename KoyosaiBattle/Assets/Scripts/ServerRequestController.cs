using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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
    [SerializeField]
    InputField userID;

    static int Id = -1;
    static long Score = 100;

    string BASEURL = "http://localhost:8000/";

    void Start()
    {

    }
    async void Update()
    {

    }
    public void ClickGetUsers()
	{

	}
    public void ClickGetUser()
	{

	}
    public void ClickPostUser()
	{

	}
    public void ClickPutUser()
	{

	}
    public void ClickDeleteUser()
	{

	}
    public void ClickGetScore()
	{

	}
    public void ClickPostScore()
	{

	}
    public void ClickGetRanking()
	{

	}
    public void ClickGetUserRanking()
	{

	}


    // Server応答用のメソッド
    // responseのjsonをTaskにいれて返す
    // var result = Method
    // reslut.Result でstringで取得できる

    // ユーザの一覧を取得
    public async Task<string> GetUsers()
    {
        var client = new HttpClient();
        var result = await client.GetAsync($"{BASEURL}users");
        var json = await result.Content.ReadAsStringAsync();

        Log(json);
        return json;
    }
    // 引数のIDのユーザを取得
    public async Task<string> GetUser(int id)
    {
        var client = new HttpClient();
        var result = await client.GetAsync($"{BASEURL}users/{id}");
        var json = await result.Content.ReadAsStringAsync();

        Log(json);
        return json;
    }
    // 引数の名前のユーザを登録する
    public async Task<string> PostUser(string name)
	{
        string jsonStr = $"{{ \"name\" : \"{name}\" }}";

        var client = new HttpClient();

        var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        var result = await client.PostAsync($"{BASEURL}users", content);
        var json = await result.Content.ReadAsStringAsync();

        var j = JsonUtility.FromJson<PostUserJson>(json);
		SetID(j.id);

        Log(json);
        return json;
    }
    // 引数で指定したIDのユーザの名前を書き換える
    // IDを省略で現在のIDを使う
    public async Task<string> PutUser(string name, int id = -1)
    {
        if(id == -1)
            id = GetID();

        string jsonStr = $"{{ \"name\" : \"{name}\" }}";

        var client = new HttpClient();

        var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        var result = await client.PutAsync($"{BASEURL}users/{id}", content);
        var json = await result.Content.ReadAsStringAsync();

        var j = JsonUtility.FromJson<PostUserJson>(json);
        SetID(j.id);

        Log(json);
        return json;
    }
    // 引数で指定したIDのユーザを削除する
    public async Task<string> DeleteUserJson(int id)
	{
        var client = new HttpClient();
        var result = await client.DeleteAsync($"{BASEURL}users/{id}");
        var json = await result.Content.ReadAsStringAsync();

        return json;
    }
    // IDで指定したユーザのスコアを取得する
    public async Task<string> GetScore(int id)
    {
        var client = new HttpClient();
        var result = await client.GetAsync($"{BASEURL}users/{id}/scores");
        var json = await result.Content.ReadAsStringAsync();

        Log(json);
        return json;
    }
    // 引数で指定したIDのユーザのスコアを登録する
    // ID省略で現在のID使用
    public async Task<string> PostScore(int score, int id = -1)
    {
        if(id == -1)
            id = GetID();

        string jsonStr = $"{{ \"rate\" : \"{score}\" }}";

        var client = new HttpClient();

        var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        var result = await client.PostAsync($"{BASEURL}users/{id}/scores", content);
        var json = await result.Content.ReadAsStringAsync();

        var j = JsonUtility.FromJson<PostUserJson>(json);
        SetID(j.id);

        Log(json);
        return json;
    }
    // ランキングを取得
    public async Task<string> GetRanking()
    {
        var client = new HttpClient();
        var result = await client.GetAsync($"{BASEURL}ranking");
        var json = await result.Content.ReadAsStringAsync();

        Log(json);
        return json;
    }
    // userのランキングを取得
    public async Task<string> UserRanking(int id)
    {
        var client = new HttpClient();
        var result = await client.GetAsync($"{BASEURL}users/{id}/ranking");
        var json = await result.Content.ReadAsStringAsync();

        Log(json);
        return json;
    }

    // テキストに表示するためのメソッド
    void Log(string msg)
    {
        Message.text += msg + "\n";

        var array = Message.text.Split('\n');

        if(array.Length > 7)
        {
            string s = "";
            for(int i = 1; i < array.Length; i++)
            {
                if(array[i] != String.Empty) s += array[i] + "\n";
            }
            Message.text = s;
        }
    }

    public static void SetID(int id) => Id = id;
    public static void SetName(long score) => Score = score;
    public static int GetID() => Id;
    public static long GetScore() => Score;

    [Serializable]
    class PostUserJson
	{
        public string name;
        public int id;
	}
}
