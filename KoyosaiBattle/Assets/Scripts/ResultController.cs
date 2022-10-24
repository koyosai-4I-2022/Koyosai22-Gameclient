using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultController : MonoBehaviour
{
    List<Vector2> UISizes;
    List<Vector3> UIPosition;
    [SerializeField]
    RectTransform[] UI;

    [SerializeField]
    Text Score;
    [SerializeField]
    Text Explanation;
    [SerializeField]
    Text RankingTop;
    [SerializeField]
    Text RankingUser;

    Vector2 baseScreen;

    [SerializeField]
    RectTransform canvas;

    void Start()
    {
        InitUI();
    }

    async void Update()
    {
        UIUpdate();

        if(Input.GetKeyUp(KeyCode.E))
		{
            Debug.Log(await ServerRequestController.GetRanking());
		}
    }
    void UIUpdate()
	{
        var scr = new Vector2(Screen.width, Screen.height) / baseScreen;

        int i = 0;

        foreach(var rect in UI)
		{
            if(rect != null)
			{
                rect.sizeDelta = new Vector2(scr.x * UISizes[i].x, scr.y * UISizes[i].y);
                //rect.position = new Vector3(scr.x * UIPosition[i].x, 0, scr.x * UIPosition[i].y);
                i++;
            }
		}
	}
    void InitUI()
	{
        UISizes = new List<Vector2>();
        UIPosition = new List<Vector3>();

        baseScreen = canvas.sizeDelta;

        foreach(var rect in UI)
		{
            if(rect != null)
			{
                UISizes.Add(rect.sizeDelta);
                UIPosition.Add(rect.position);
			}
		}
	}
}
