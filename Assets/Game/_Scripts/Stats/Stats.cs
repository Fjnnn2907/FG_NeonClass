using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Stats : MonoBehaviour, IPunObservable
{
    [SerializeField] protected PhotonView photonView;
    
    [Header("Health")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;  
    [SerializeField] protected Image fill;
    
    [Header("Exp")]    
    [SerializeField] protected int level;
    [SerializeField] protected int maxExp;
    [SerializeField] protected int exp;
    [SerializeField] protected TextMeshProUGUI levelText;
    
    [Header("Damage")]
    [SerializeField] protected int damage = 10;

    public float Maxhealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public int Damage => damage;

    protected virtual void OnEnable()
    {
        if(!photonView.IsMine)
            currentHealth = maxHealth;
    }

    #region Damage
    public void TakeDamage(float damage)
    {
        if (!photonView.IsMine) return;
        photonView.RPC(nameof(RPC_TakeDamage), RpcTarget.All, damage);

    }

    [PunRPC]
    public void RPC_TakeDamage(float damage)
    {
        if (currentHealth <= 0) return;
        currentHealth -= damage;
        UpdateHealthBar();
    }
    #endregion
    #region Health
    public void AddHealth(float health)
    {
        if (!photonView.IsMine) return;
        photonView.RPC(nameof(RPC_AddHealth), RpcTarget.All, health);
    }
    [PunRPC]
    public void RPC_AddHealth(float health)
    {
        if (currentHealth >= 100) return;
        currentHealth += health;
        UpdateHealthBar();
    }
    public void HealthLevelUp()
    {
        if (!photonView.IsMine) return;
        photonView.RPC(nameof(RPC_HealthLevelUp), RpcTarget.All);
    }
    [PunRPC]
    public void RPC_HealthLevelUp()
    {
        maxHealth = Mathf.CeilToInt(maxHealth * level * 1.3f);
        damage = Mathf.CeilToInt(damage * level * 1.3f);
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
    public void HealthReborn()
    {
        currentHealth = 0;
    }
    #endregion
    #region EXP
    public void AddExp(int exp)
    {
        if (!photonView.IsMine) return;
        photonView.RPC(nameof(RPC_AddExp), RpcTarget.All, exp);
    }
    [PunRPC]
    public void RPC_AddExp(int exp)
    {
        if (this.exp >= maxExp) return;

        this.exp += exp;

        while (this.exp >= maxExp)
        {
            LevelUp();
        }
    }
    protected virtual void LevelUp()
    {
        this.exp -= maxExp;
        level++;
        maxExp = Mathf.CeilToInt(maxExp * 1.3f);
        HealthLevelUp();
        UpdateLevel();
    }
    #endregion
    #region GUI
    protected void UpdateLevel()
    {
        levelText.text = level.ToString();
    }
    protected void UpdateHealthBar()
    {
        if (!photonView.IsMine)
            fill.color = Color.red;
        float health = currentHealth / maxHealth;
        StartCoroutine(SmoothHealthBar(health));        
    }

    protected IEnumerator SmoothHealthBar(float targetFill)
    {
        float startFill = fill.fillAmount;
        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            fill.fillAmount = Mathf.Lerp(startFill, targetFill, elapsedTime / duration);
            yield return null;
        }

        fill.fillAmount = targetFill;
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
            stream.SendNext(exp);
            stream.SendNext(level);
        }
        else
        {
            currentHealth = (float)stream.ReceiveNext();
            exp = (int)stream.ReceiveNext();
            level = (int)stream.ReceiveNext();
            UpdateHealthBar();
            UpdateLevel();
        }
    }
    #endregion
}
