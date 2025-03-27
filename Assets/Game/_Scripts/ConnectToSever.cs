using Photon.Pun;
using UnityEngine;

public class ConnectToSever : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject LoadingGUI;
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        LoadingGUI.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        LoadingGUI.SetActive(false);
    }
}
