using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Server : MonoBehaviourPunCallbacks
{
    public Button startBtn;
    public GameObject lobby;
    public TextMeshProUGUI connectInfoTxt;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startBtn.onClick.AddListener(OnClickStart);
        
        startBtn.interactable = false;
        
        PhotonNetwork.ConnectUsingSettings();
        connectInfoTxt.text = "Connecting to server...";
    }

    void OnClickStart()
    {
        PhotonNetwork.JoinLobby();
        connectInfoTxt.text = "Connecting to lobby...";
    }

    public override void OnConnectedToMaster()
    {
        startBtn.interactable = true;
        connectInfoTxt.text = "Successfully connected to the server!";
    }

    public override void OnJoinedLobby()
    {
        gameObject.SetActive(false);
        lobby.SetActive(true);
        connectInfoTxt.text = "Successfully connected to the lobby!";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();
        connectInfoTxt.text = "Connection failed, reconnecting to server...";
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
