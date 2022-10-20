using System;
using System.Collections.Generic;
using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Unity.Runtime.Event;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StrixRoomListGUI : MonoBehaviour {
    public int page = 0;
    public int maxItemCount = 10;
    public StrixListView listView;
    public Text pageText;
    public Button prevPageButton;
    public Button nextPageButton;
    public UnityEvent OnRoomJoined;
    private ICollection<RoomInfo> roomInfoCollection;
    private bool isUpdated = false;

    void Start() {
    }

    void OnEnable() {
        page = 0;

        SearchRooms();
        UpdateRoomList();
    }

    // Update is called once per frame
    void Update() {
        if (!isUpdated)
            return;

        UpdateRoomList();

        isUpdated = false;
    }

    public void OnCreateRoomButtonClick() {
        CreateRoom();
    }

    public void OnJoinRandomRoomButtonClick() {
        StrixNetwork.instance.JoinRandomRoom(
            StrixNetwork.instance.playerName,
            args => {
                RoomJoined();
            },
            args => {
                CreateRoom();
            });
    }

    public void OnNextPageButtonClick() {
        if (roomInfoCollection == null || roomInfoCollection.Count > maxItemCount) {
            page++;
        }

        SearchRooms();
    }

    public void OnPrevPageButtonClick() {
        page--;

        if (page < 0) {
            page = 0;
        }

        SearchRooms();
    }

    private void CreateRoom() {
        RoomProperties roomProperties = new RoomProperties {
            name = "New Room",
            capacity = 4,
        };

        RoomMemberProperties memberProperties = new RoomMemberProperties {
            name = StrixNetwork.instance.playerName
        };

        StrixNetwork.instance.CreateRoom(
            roomProperties,
            memberProperties,
            args => {
                RoomJoined();
            },
            args => {
                Debug.unityLogger.Log("Strix", "Create room failed!");
            }
        );
    }

    private void SearchRooms() {
        StrixNetwork.instance.SearchRoom(maxItemCount + 1, page * maxItemCount, OnRoomSearch, null);
        prevPageButton.interactable = false;
        nextPageButton.interactable = false;
    }

    private void UpdateRoomList() {
        listView.ClearListItems();

        if (roomInfoCollection == null)
            return;

        int count = 0;

        foreach (RoomInfo roomInfo in roomInfoCollection) {
            GameObject item = listView.AddListItem();
            StrixRoomListItem roomListItem = item.GetComponent<StrixRoomListItem>();
            roomListItem.roomInfo = roomInfo;
            roomListItem.roomList = this;

            roomListItem.UpdateGUI();

            count++;

            if (count >= maxItemCount) {
                break;
            }
        }
    }

    private void OnRoomSearch(RoomSearchEventArgs args) {
        roomInfoCollection = args.roomInfoCollection;

        pageText.text = "" + (page + 1);

        isUpdated = true;
        prevPageButton.interactable = true;
        nextPageButton.interactable = true;
    }

    private void RoomJoined() {
        OnRoomJoined.Invoke();
        gameObject.SetActive(false);
    }
}
