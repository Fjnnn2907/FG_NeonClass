using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerStats : Stats
{
    [SerializeField] private TextMeshProUGUI nickName;
    protected override void Start()
    {
        base.Start();
        SetNickName();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddExp(10);
        }
    }

    private void SetNickName()
    {
        if (PlayerPrefs.HasKey("NickName"))
        {
            string savedNickName = PlayerPrefs.GetString("NickName");
            PhotonNetwork.NickName = savedNickName;
            nickName.text = savedNickName;
        }
        if(!photonView.IsMine)
            nickName.color = Color.red;
    }
}
