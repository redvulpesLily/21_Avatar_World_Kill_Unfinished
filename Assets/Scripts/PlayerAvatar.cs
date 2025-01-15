using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAvatar : MonoBehaviourPun
{
    [SerializeField] private Text       txtNickname;
    [SerializeField] private Text       txtChat;
    [SerializeField] private GameObject chatBubble;

    private AvatarController _controller;
    private string _nickname;
    public int    actorNumber;
    public Material avataMaterial;

    private void Awake()
    {
        _controller = GetComponent<AvatarController>();
        avataMaterial = GetComponent<Material>();
        
        chatBubble.SetActive(false);
    }

    [PunRPC]
    public void SetNickname(string nickname, int actorNumber)
    {
        _nickname = nickname;
        this.actorNumber = actorNumber;
        
        txtNickname.text = nickname;
    }

    public void SetMovable(bool value)
    {
        _controller.canMove = value;
    }

    public bool IsTargetAvatar(int actorNumber) => this.actorNumber == actorNumber;

    public void ShowChat(string chat)
    {
        if (_chatCoroutine != null)
            StopCoroutine(_chatCoroutine);

        _chatCoroutine = ChatCoroutine(chat);
        StartCoroutine(_chatCoroutine);
    }

    private IEnumerator _chatCoroutine;
    private IEnumerator ChatCoroutine(string chat)
    {
        chatBubble.SetActive(true);

        txtChat.text = chat;

        yield return new WaitForSeconds(3f);

        txtChat.text = null;

        chatBubble.SetActive(false);
    }

    public void PlayerDie(int targetActorNumber)
    {
        if (_changeColor != null)
            StopCoroutine(_changeColor);

        _changeColor = ChangeColor(targetActorNumber);
        StartCoroutine(_changeColor);
    }

    private IEnumerator _changeColor;
    private IEnumerator ChangeColor(int targetActorNumber)
    {
        avataMaterial.color = Color.red;

        yield return new WaitForSeconds(5f);

        avataMaterial.color = Color.white;
    }
}
