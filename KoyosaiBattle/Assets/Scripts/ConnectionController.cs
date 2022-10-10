using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoftGear.Strix.Client;
using SoftGear.Strix.Net;
using SoftGear.Strix;
using SoftGear.Strix.Unity;
using SoftGear.Strix.Unity.Runtime;
using System;
using SoftGear.Strix.Client.Core.Model.Manager.Filter;
using SoftGear.Strix.Client.Core.Model.Manager.Filter.Builder;
using SoftGear.Strix.Client.Core.Request;

public class ConnectionController : MonoBehaviour
{
    [SerializeField]
    string PCNumber;
    [SerializeField]
    string RoomID;
    [SerializeField]
    string appID;
    [SerializeField]
    string masterID;

    [SerializeField]
    Text text;

    [SerializeField]
    Text buttom1;
    [SerializeField]
    Text buttom2;

    [SerializeField]
    InputField ConnectionID;
    [SerializeField]
    InputField PCID;

    bool isConnectMaster = false;

    void Start()
    {
        buttom2.gameObject.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.A))
        {
            Log("A");
        }
        if(Input.GetKeyUp(KeyCode.B))
		{
            var strixNetwork = StrixNetwork.instance;
            var roomMenbers = strixNetwork.roomMembers;
            if(roomMenbers != null)
			{
                foreach(var member in roomMenbers)
				{
                    Log(member.Value.GetName());
				}
			}
		}
    }
    public void ConnectClick()
	{
        string conID = ConnectionID.text;
        string pcID = PCID.text;

        if(isConnectMaster)
        {
            if(pcID != null && conID != null)
            {
                SetUp(conID, pcID);
                buttom1.text = "Connect";
            }
        }
        else
        {
            if(pcID != null)
            {
                Connect(pcID);
                buttom1.text = "Create";
                buttom2.gameObject.SetActive(true);
                isConnectMaster = true;
            }
        }
	}
    public void JoinClick()
	{
        string conID = ConnectionID.text;
        string pcID = PCID.text;

        if(isConnectMaster)
        {
            if(pcID != null && conID != null)
            {
                JoinRoom(conID, pcID);
            }
        }
    }
    public void Connect(string num)
    {
        StrixNetwork strixNetwork = StrixNetwork.instance;
        strixNetwork.applicationId = appID;
        strixNetwork.playerName = num;
        string master = masterID;

        strixNetwork.ConnectMasterServer(
            host: master,
            port: 9122,
            connectEventHandler: _ =>
            {
                Log("Sucess Connect Master");
            },
            errorEventHandler: _ =>
            {
                Log("Faild Connect Master");
            });

    }
    public void SetUp(string roomID, string num)
    {
        StrixNetwork strixNetwork = StrixNetwork.instance;

		RoomProperties roomProperties = new RoomProperties
		{
			name = "Room" + roomID,
			capacity = 3,
			stringKey = roomID + roomID,
			properties = new Dictionary<string, object>
		    {
			    { "description", "Room" + roomID }
		    }
		};
        strixNetwork.CreateRoom(
            roomProperties,
            new RoomMemberProperties()
            {
                name = num
            },
            _ => //Sucess
            {
                var room = strixNetwork.room;
                var roomMember = strixNetwork.selfRoomMember;

                Log("Success Create Room");
                //Log("Sucess Create Room"
                //    + "\nRoom name:" + room.GetName()
                //    + "\nRoom capacity:" + room.GetCapacity()
                //    + "\nRoom password:" + room.GetPassword()
                //    + "\nRoom String Key:" + room.GetStringKey()
                //    + "\nRoom Menber Count:" + room.GetMemberCount()
                //    + "\nRoom member name:" + roomMember.GetName());
            },
            e => //Faild
            {
                Log("Faild Create Room");
                Log(e.cause.Message);
            }
        );
    }
	public void JoinRoom(string roomID, string num)
	{
		StrixNetwork strixNetwork = StrixNetwork.instance;

		strixNetwork.SearchJoinableRoom(
			condition: ConditionBuilder.Builder().Field("stringKey").EqualTo(roomID + roomID).Build(),
			order: new Order("memberCount", OrderType.Ascending),
			limit: 10,
			offset: 0,
			handler: searchResults =>
			{
				var foundRooms = searchResults.roomInfoCollection;
				Log(foundRooms.Count + " rooms found.");

				if(foundRooms.Count == 0)
				{
					Log("No joinable rooms found.");
					return;
				}

                var roomInfo = foundRooms.GetEnumerator();
                foreach(var room in foundRooms)
				{
                    RoomJoinArgs roomJoinArgs = new RoomJoinArgs();

                    if(strixNetwork.room != null)
                        break;

                    Debug.Log($"host:{room.host}\n"
                        + $"port:{room.port}"
                        + $"protocol:{room.protocol}"
                         + $"roomID:{room.roomId}");

                    strixNetwork.JoinRoom(
                         host: room.host,
                         port: room.port,
                         protocol: room.protocol,
                         roomId: room.roomId,
                         playerName: num,
                         handler: __ => Log("Room joined."),
                         failureHandler: joinError => Log("Join failed.Reason: " + joinError.cause)
                    );
                }
			},
				failureHandler: searchError => Log("Search failed.Reason: " + searchError.cause)
		);

	}
	void OnApplicationQuit()
    {
        StrixNetwork strixNetwork = StrixNetwork.instance;
        var room = strixNetwork.room;
        if(room == null)
		{
            strixNetwork.DisconnectMasterServer();
            strixNetwork.Destroy();
            return;
		}
        if(room.GetMemberCount() == 1)
        {
            strixNetwork.DeleteRoom(room.GetPrimaryKey(), _ => { Debug.Log("Sucess Delete Room"); strixNetwork.DisconnectMasterServer(); strixNetwork.Destroy(); }, _ => { });
        }
        else
        {
            strixNetwork.LeaveRoom(_ => { Debug.Log("Sucess Leave Room"); strixNetwork.DisconnectMasterServer(); strixNetwork.Destroy(); }, _ => { });
        }
    }
    void Log(string msg)
    {
        text.text += msg + "\n";

        var array = text.text.Split('\n');

        if(array.Length > 8)
        {
            string s = "";
            for(int i = 1;i < array.Length;i++)
			{
                s += array[i] + "\n";
            }
            text.text = s;
        }
    }
}