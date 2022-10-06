using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Client;
using SoftGear.Strix.Net;
using SoftGear.Strix;
using SoftGear.Strix.Unity;
using SoftGear.Strix.Unity.Runtime;
using System;
using SoftGear.Strix.Client.Core.Model.Manager.Filter;
using SoftGear.Strix.Client.Core.Model.Manager.Filter.Builder;

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
    static UnityEngine.UI.Text text;

    void Start()
    {
        text = GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>();
        Connect(PCNumber);
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.A))
        {
            SetUp(RoomID, PCNumber);
        }
        if(Input.GetKeyUp(KeyCode.B))
        {
            JoinRoom(RoomID, PCNumber);
        }
        if(Input.GetKeyUp(KeyCode.C))
        {
            StrixNetwork strixNetwork = StrixNetwork.instance;

            var mem = strixNetwork.roomMembers;
            foreach(var member in mem)
            {
                Log(member.Value.GetName());
            }
        }
    }

    public static void Connect(string num)
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
    public static void SetUp(string roomID, string num)
    {
        StrixNetwork strixNetwork = StrixNetwork.instance;

        RoomProperties roomProperties = new RoomProperties();
        roomProperties.name = "Room" + roomID;
        roomProperties.capacity = 2;
        roomProperties.stringKey = roomID + roomID;
        roomProperties.properties = new Dictionary<string, object>
        {
            { "description", "Room" + roomID }
        };

        strixNetwork.CreateRoom(roomProperties, new RoomMemberProperties()
        {
            name = num
        },
        _ => //Sucess
        {
            var room = strixNetwork.room;
            var roomMember = strixNetwork.selfRoomMember;

            Log("Sucess Create Room"
                + "\nRoom name:" + room.GetName()
                + "\nRoom capacity:" + room.GetCapacity()
                + "\nRoom password:" + room.GetPassword()
                + "\nRoom String Key:" + room.GetStringKey()
                + "\nRoom Menber Count:" + room.GetMemberCount()
                + "\nRoom member name:" + roomMember.GetName());
        },
        _ => //Faild
        {
            Log("Faild Create Room");
            JoinRoom(roomID, num);
        });
    }
    public static void JoinRoom(string roomID, string num)
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

                var roomInfo = foundRooms.GetEnumerator().Current;

                RoomJoinArgs roomJoinArgs = new RoomJoinArgs();


                strixNetwork.JoinRoom(
                     host: masterID,
                     port: 9122,
                     protocol: roomInfo.protocol,
                     roomId: roomInfo.roomId,
                     playerName: num,
                     handler: __ => Log("Room joined."),
                     failureHandler: joinError => Log("Join failed.Reason: " + joinError.cause)
                );
            },
                failureHandler: searchError => Log("Search failed.Reason: " + searchError.cause)
        );

    }
    void OnApplicationQuit()
    {
        StrixNetwork strixNetwork = StrixNetwork.instance;
        var room = strixNetwork.room;
        if(room.GetMemberCount() == 1)
        {
            strixNetwork.DeleteRoom(room.GetPrimaryKey(), _ => { Log("Sucess Delete Room"); }, _ => { });
        }
        else
        {
            strixNetwork.LeaveRoom(_ => { Log("Sucess Leave Room"); }, _ => { });
        }
    }
    static void Log(string msg)
    {
        text.text += msg + "\n";
    }
}