using SoftGear.Strix.Client.Core.Model.Manager.Filter;
using SoftGear.Strix.Client.Core.Model.Manager.Filter.Builder;
using SoftGear.Strix.Client.Match.Room.Model;
using SoftGear.Strix.Net.Logging;
using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Unity.Runtime.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkController : MonoBehaviour
{
//    [SerializeField]
//    string Name;
//    [SerializeField]
//    string PCNumber;
//    [SerializeField]
//    string ConnectionID;

//    [NonSerialized]
//    public StrixNetwork strixNetwork;

//    private static string applicationID = "c2733c28-3bc3-44a5-bdcd-dea8da81f075";

//    void Start()
//    {
//        Connect(Name);
//    }
//    void Update()
//    {
//		if(Input.GetKeyDown(KeyCode.K))
//		{
//            var dic = new Dictionary<string, object>()
//			{
//                { "name", "Room" + ConnectionID },
//                { "capacity", 2 },
//                { "password", ConnectionID },
//                { "state", 0 },
//                { "inJoinable", true },
//                { "stringKey", ConnectionID }
//			};
//            RoomProperties properties = new RoomProperties();
//            properties.FromDictionary(dic);
//            CreateRoom(properties, ConnectionID + ConnectionID);
//		}
//        if(Input.GetKeyDown(KeyCode.L))
//		{
//            IConditionBuilder builder = ConditionBuilder.Builder().Field("name").EqualTo("Room" + ConnectionID);
//            JoinRoom(builder.Build(), _ => Debug.Log("Join Room Success"), _ => Debug.Log("Join Room Faild"));
//        }
//    }

//    public void Connect(string name)
//	{
//        strixNetwork = StrixNetwork.instance;
//        LogManager.Instance.Filter = SoftGear.Strix.Net.Logging.Level.INFO;

//        strixNetwork.applicationId = applicationID;
//        strixNetwork.playerName = name;

//        strixNetwork.ConnectMasterServer("18b43b8a75612c7572a9fd1f.game.strixcloud.net", 9122,
//            _ => Debug.Log("Success to Connect MasterServer."),
//            _ => Debug.Log("Failed to Connect MasterServer.\n"));
//	}
//    void CreateRoom(RoomProperties properties,string password)
//	{
//        properties.password = password;

//        strixNetwork.CreateRoom(properties, new RoomMemberProperties()
//        {
//            name = StrixNetwork.instance.playerName,
//        }, 
//        _ => Debug.Log("Room Created!"), 
//        _ => Debug.Log("Room Create Failed!"));
//	}
//    void JoinRoom(ICondition con, RoomJoinEventHandler successEvent, FailureEventHandler failureEvent)
//	{
//        SearchRoom(con, info =>
//        {
//             RoomJoinArgs args = new RoomJoinArgs()
//             {
//                 host = info.host,
//                 port = info.port,
//                 protocol = "TCP",
//                 password = ConnectionID + ConnectionID,
//                 roomId = info.roomId,
//                 memberProperties = new RoomMemberProperties()
//                 {
//                     name = StrixNetwork.instance.playerName
//                 }
//             };

//             strixNetwork.JoinRoom(args, successEvent, failureEvent);
//        }, 
//        () => Debug.Log("Room not found!"));
//	}
//    void SearchRoom(ICondition con, Action<RoomInfo> successEvent, Action failureEvent)
//	{
//        strixNetwork.SearchJoinableRoom(con, new Order("memberCount",OrderType.Ascending), 10, 0,
//            args =>
//            {
//                List<RoomInfo> roomList = new List<RoomInfo>(args.roomInfoCollection);
//                if (roomList.Count == 0)
//                {
//                    failureEvent();
//                    return;
//                }

//                RoomInfo info = roomList[UnityEngine.Random.Range(0, roomList.Count)];
//                successEvent(info);
//            }, 
//            args => Debug.Log($"Failed to get Room Data.\n{args.cause.Message}"));
//	}

//    private void OnApplicationQuit()
//	{
//        if (strixNetwork != null)
//        {
//            int count = StrixNetwork.instance.roomMembers.Count;

//            if(count == 1)
//            {
//                StrixNetwork.instance.DeleteRoom(
//                    StrixNetwork.instance.room.GetPrimaryKey(),
//                    (_) => Debug.Log("Success Delete Room."), (_) => Debug.Log("Faild to Delete Room."));
//            }

//            strixNetwork.DisconnectMasterServer();
//        }
//        strixNetwork = null;
//    }

//    public static void Quit()
//    {
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#elif UNITY_STANDALONE
//                Application.Quit();
//#endif
//    }
}