using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class NetwrokManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField InputCreateRoom;
    [SerializeField] private TMP_InputField InputJoinRoom;

    [SerializeField] private Button createRoomBtn;
    [SerializeField] private Button joinRoomBtn;

    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private Transform roomParent;
    private Dictionary<string, GameObject> currentRoomUIs = new Dictionary<string, GameObject>();

    [SerializeField] private TMP_InputField InputNickName;
    private void Start()
    {
        createRoomBtn.onClick.AddListener(CreateRoom);
        joinRoomBtn.onClick.AddListener(JoinRoom);
    }
    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(InputCreateRoom.text, roomOptions);

    }
    private void JoinRoom()
    {
        PhotonNetwork.JoinRoom(InputJoinRoom.text);

    }
    public void JoinRoomByName(string nameRoom)
    {
        PhotonNetwork.JoinRoom(nameRoom);

    }
    public override void OnJoinedRoom()
    {
        CreateNickName();
        PhotonNetwork.LoadLevel("Map1");
    }
    public void CreateNickName()
    {
        string nickName;
        if (string.IsNullOrEmpty(InputNickName.text))
            nickName = $"Player{Random.Range(0, 1000)}";
        else
            nickName = InputNickName.text;

        PlayerPrefs.SetString("NickName", nickName);
        PlayerPrefs.Save();

        PhotonNetwork.NickName = nickName;
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform child in roomParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (RoomInfo room in roomList)
        {
            if (room.IsOpen && room.IsVisible && room.PlayerCount > 0)
            {
                GameObject newRoom = Instantiate(roomPrefab, roomParent.transform);
                newRoom.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
                newRoom.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{room.PlayerCount} / {room.MaxPlayers}";
            }
        }
    }
}
