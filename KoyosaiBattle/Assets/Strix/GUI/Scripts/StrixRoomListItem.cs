using System;
using System.Collections.Generic;
using SoftGear.Strix.Client.Core.Auth.Message;
using SoftGear.Strix.Client.Core.Error;
using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Unity.Runtime.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StrixRoomListItem : MonoBehaviour
{
    public RoomInfo roomInfo;
    public Button button;
    public Text text;

    [HideInInspector] 
    public StrixRoomListGUI roomList;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateGUI() {
        text.text = roomInfo.name + " " + roomInfo.memberCount + "/" + roomInfo.capacity;
    }

    public void OnClick() {
        button.interactable = false;
        StrixNetwork.instance.JoinRoom(roomInfo.host, roomInfo.port, roomInfo.protocol, roomInfo.roomId, StrixNetwork.instance.playerName, OnRoomJoin, OnRoomJoinFailed);
    }

    private void OnRoomJoin(RoomJoinEventArgs args) {
        button.interactable = true;

        if (roomList != null) {
            roomList.OnRoomJoined.Invoke();
            roomList.gameObject.SetActive(false);
        }
    }

    private void OnRoomJoinFailed(FailureEventArgs args) {
        button.interactable = true;

        string error = "";

        if (args.cause != null) {
            error = args.cause.Message;
        }

        Debug.unityLogger.Log("Strix", "Room join failed. " + error);
    }
}
