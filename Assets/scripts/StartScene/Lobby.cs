using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Lobby : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomNameInput;
    public Button createBtn;
    public TextMeshProUGUI connectInfoTxt;
    public Transform content;
    public GameObject roomBtnPref;
    
    List<RoomInfo> allRoomList = new List<RoomInfo>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        createBtn.onClick.AddListener(OnClcikCreate);
    }

    void OnClcikCreate()
    {
        if (roomNameInput.text == "")
        {
            return;
        }

        ButtonOnOff(false);
        
        RoomOptions roomOptions = new RoomOptions();

        roomOptions.MaxPlayers = 2;
        
        PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions);
        connectInfoTxt.text = "Creating room...";
    }

    void OnClickJoin(string roomName)
    {
        ButtonOnOff(false);
        
        PhotonNetwork.JoinRoom(roomName);
    }

    void ButtonOnOff(bool isOn)
    {
        createBtn.interactable = isOn;

        foreach (var roomBtns in content.GetComponentsInChildren<Button>())
        {
            roomBtns.interactable = isOn;
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("InGame");
        connectInfoTxt.text = "Successfully connected to the room!";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ButtonOnOff(true);
        connectInfoTxt.text = "Failed to create room";
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var changedRoom in roomList)
        {
            if (allRoomList.Contains(changedRoom))
            {
                allRoomList.Remove(changedRoom);
            }
            else
            {
                allRoomList.Add(changedRoom);
            }
        }

        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        foreach (var room in allRoomList)
        {
            GameObject roomBtn = Instantiate(roomBtnPref, content);
            
            Text roomNameTxt = roomBtn.GetComponentInChildren<Text>();
            roomNameTxt.text = room.Name;
            
            roomBtn.GetComponent<Button>().onClick.AddListener(() => OnClickJoin(room.Name));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
