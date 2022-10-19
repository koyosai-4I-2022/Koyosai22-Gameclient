using System;
using System.Collections.Generic;
using SoftGear.Strix.Client.Core.Auth.Message;
using SoftGear.Strix.Client.Core.Error;
using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Net.Logging;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using SoftGear.Strix.Unity.Runtime.Event;

public class StrixConnectGUI : MonoBehaviour {
    public string host = "127.0.0.1";
    public int port = 9122;
    public string applicationId = "00000000-0000-0000-0000-000000000000";
    public Level logLevel = Level.INFO;
    public InputField playerNameInputField;
    public Text statusText;
    public Button connectButton;
    public UnityEvent OnConnect;

    // Use this for initialization
    void Start()
    {
    }

    void OnEnable()
    {
        statusText.text = "";
        connectButton.interactable = true;
    }

    public void Connect() {
        LogManager.Instance.Filter = logLevel;

        StrixNetwork.instance.applicationId = applicationId;
        StrixNetwork.instance.playerName = playerNameInputField.text;
        StrixNetwork.instance.ConnectMasterServer(host, port, OnConnectCallback, OnConnectFailedCallback);

        statusText.text = "Connecting MasterServer " + host + ":" + port;

        connectButton.interactable = false;
    }

    private void OnConnectCallback(StrixNetworkConnectEventArgs args)
    {
        statusText.text = "Connection established";

        OnConnect.Invoke();

        gameObject.SetActive(false);
    }

    private void OnConnectFailedCallback(StrixNetworkConnectFailedEventArgs args) {
        string error = "";

        if (args.cause != null) {
            error = args.cause.Message;
        }

        statusText.text = "Connect " + host + ":" + port + " failed. " + error;
        connectButton.interactable = true;
    }
}
