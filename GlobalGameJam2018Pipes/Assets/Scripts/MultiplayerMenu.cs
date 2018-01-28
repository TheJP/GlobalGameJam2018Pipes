using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerMenu : MonoBehaviour
{
    private const string DefaultUsername = "Camibär";

    public Text chatOutput;
    public InputField chatInput;
    public InputField portInput;

    public Button openServerButton;
    public Button startButton;
    public Button chatButton;

    public void OnBackButtonClicked()
    {
        GameManager.Multiplayer.Network.Stop();
        GameManager.Multiplayer.Network.AlchemistConnected -= OnAlchemistConnected;
        GameManager.Multiplayer.Network.AlchemistDisconnected -= OnAlchemistDisconnected;
        GameManager.Multiplayer.Network.ReceivedMessage -= OnChatMessageReceived;
        GameManager.Multiplayer = null;

        SceneManager.LoadScene("MainMenu");
    }

    public void OnOpenServerButtonClicked()
    {
        UInt16 port = 5555;

        if (portInput.text.Length > 0)
        {
            if (!UInt16.TryParse(portInput.text, out port))
            {
                port = 5555;
            }
        }

        GameManager.Multiplayer = new Multiplayer();
        GameManager.Multiplayer.Network.AlchemistConnected += OnAlchemistConnected;
        GameManager.Multiplayer.Network.AlchemistDisconnected += OnAlchemistDisconnected;
        GameManager.Multiplayer.Network.ReceivedMessage += OnChatMessageReceived;

        GameManager.Multiplayer.Network.Start(DefaultUsername, port);
    }

    private void OnChatMessageReceived(string message)
    {
        AppendToChat(GameManager.Multiplayer.RemoteUserName, message);
    }

    private void OnAlchemistConnected(string username)
    {
        GameManager.Multiplayer.RemoteUserName = username;
        AppendToChat("System", $"{username} joined the game");

        startButton.enabled = true;
        chatButton.enabled = true;
        chatInput.enabled = true;
    }

    private void OnAlchemistDisconnected()
    {
        AppendToChat("System", $"{GameManager.Multiplayer.RemoteUserName} left the game");
        GameManager.Multiplayer.RemoteUserName = null;

        startButton.enabled = false;
        chatButton.enabled = false;
        chatInput.enabled = false;
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("MainScene");

        GameManager.Multiplayer.Network.AlchemistConnected -= OnAlchemistConnected;
        GameManager.Multiplayer.Network.AlchemistDisconnected -= OnAlchemistDisconnected;
        GameManager.Multiplayer.Network.ReceivedMessage -= OnChatMessageReceived;
    }

    public void OnChatButtonClicked()
    {
        AppendToChat(DefaultUsername, chatInput.text);

        if (GameManager.Multiplayer.RemoteUserName != null)
        {
            GameManager.Multiplayer.Network.SendMessage(chatInput.text);
        }
    }

    public void AppendToChat(string userName, string message)
    {
        if(chatOutput.text.Length > 0)
        {
            chatOutput.text += "\r\n";
        }

        chatOutput.text += $"[{userName}] {message}";
    }

	// Use this for initialization
	void Start ()
	{
	    chatButton.enabled = false;
	    chatInput.enabled = false;
	    startButton.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (GameManager.Multiplayer != null)
	    {
	        GameManager.Multiplayer.DispatchEvents();
	    }
	}
}
