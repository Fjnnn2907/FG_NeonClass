using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                Transform followPoint = player.transform;
                virtualCamera.Follow = followPoint;
                break;
            }
        }
    }
}

