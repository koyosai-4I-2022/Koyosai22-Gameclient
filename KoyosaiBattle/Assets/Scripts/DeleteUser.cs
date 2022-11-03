using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DeleteUser : MonoBehaviour
{
	public InputField input;

	public void Click()
	{
		StartCoroutine(nameof(ButtonDownDelte));
	}

	async void ButtonDownDelte()
	{
		int id = int.Parse(input.text);

		Debug.Log("Get");
		var str =  await ServerRequestController.DeleteUserJson(id);
		Debug.Log(str);
	}
}
