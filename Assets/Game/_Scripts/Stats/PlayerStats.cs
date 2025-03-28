using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerStats : Stats
{
    [SerializeField] private TextMeshProUGUI nickName;
    private string savedNickName;
    [Header("FX")]
    [SerializeField] private GameObject LevelFX;
    protected override void OnEnable()
    {
        base.OnEnable();
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
            savedNickName = PlayerPrefs.GetString("NickName");
            PhotonNetwork.NickName = savedNickName;
            nickName.text = savedNickName;
        }
        if(!photonView.IsMine)
            nickName.color = Color.red;
    }
    protected override void LevelUp()
    {
        if(!LevelFX.activeInHierarchy)
            LevelFX.SetActive(true);
        base.LevelUp();
    }
    public float GetPercentHealth(float percent)
    {
        return maxHealth * percent;
    }
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);
        if (stream.IsWriting)
        {
            stream.SendNext(savedNickName);

        }
        else
        {
            savedNickName = (string)stream.ReceiveNext();
            SetNickName();
        }
    }
}
