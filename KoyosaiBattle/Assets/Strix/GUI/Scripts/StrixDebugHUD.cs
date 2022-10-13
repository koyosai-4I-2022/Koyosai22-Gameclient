using System.Collections;
using System.Collections.Generic;
using SoftGear.Strix.Client.Match.Room.Model;
using UnityEngine;
using UnityEngine.UI;

namespace SoftGear.Strix.Unity.Runtime {
    public class StrixDebugHUD : MonoBehaviour {
        public Text MasterSessionStatusText;
        public Text RoomSessionStatusText;
        public Text RoomIdText;
        public Text RoomNameText;
        public Text RoomOwnerUidText;
        public Text RoomMemberCountText;
        public Text RoomMemberList;

        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
            var strixNetwork = StrixNetwork.instance;

            if (strixNetwork == null)
                return;

            if (MasterSessionStatusText != null) {
                MasterSessionStatusText.text = strixNetwork.masterSession.IsConnected ? strixNetwork.masterSession.messageChannel.GetRemoteAddress().ToString() : "Not connected";
            }

            if (RoomSessionStatusText != null) {
                RoomSessionStatusText.text = strixNetwork.roomSession.IsConnected ? strixNetwork.roomSession.messageChannel.GetRemoteAddress().ToString() : "Not connected";
            }

            CustomizableMatchRoom room = strixNetwork.roomSession.room;

            if (RoomIdText != null) {
                RoomIdText.text = (room != null ? room.GetPrimaryKey().ToString() : "");
            }

            if (RoomNameText != null) {
                RoomNameText.text = (room != null ? room.GetName() : "");
            }

            if (RoomOwnerUidText != null) {
                RoomOwnerUidText.text = (room != null ? room.GetOwnerUid().ToString() : "");
            }

            if (RoomMemberCountText != null) {
                RoomMemberCountText.text = (room != null ? room.GetMemberCount() + "/" + room.GetCapacity() : "");
            }

            if (RoomMemberList != null) {
                string text = "";

                if (strixNetwork.sortedRoomMembers != null) {
                    foreach (var entry in strixNetwork.sortedRoomMembers) {
                        CustomizableMatchRoomMember member = (CustomizableMatchRoomMember)entry;
                        string properties = "";

                        if (member.GetProperties() != null) {
                            foreach (KeyValuePair<string, object> v in member.GetProperties()) {
                                properties += " " + v.Key + ":" + v.Value;
                            }
                        }

                        text += member.GetPrimaryKey() + " " + member.GetUid() + " " + member.GetName() + properties + "\n";
                    }
                }

                RoomMemberList.text = text;
            }
        }
    }
}