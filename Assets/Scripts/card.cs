using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip flip;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenCard()//카드를 뒤집는다.
    {
        if(gameManager.I.isStart == true) // 게임이 진행중일 때만 카드를 열 수 있게 함
        {
            anim.SetBool("isOpen",true);
            audioSource.PlayOneShot(flip);
            transform.Find("front").gameObject.SetActive(true);
            transform.Find("back").gameObject.SetActive(false);
            if(gameManager.I.firstCard == null)
            {
                gameManager.I.firstCard = gameObject;
            }
            else
            {
                gameManager.I.secondCard = gameObject;
                gameManager.I.IsMatched();
            }
        }     
    }
    public void destroyCard()
    {
        Invoke("destroyCardInvoke", 0.5f);
    }

    void destroyCardInvoke()
    {
        Destroy(gameObject);//카드를 삭제하고
        gameManager.I.AbstractCount();//남은 카드수를 하나 뺀다
        if(gameManager.I.TotalCardIs() == 0)//남은 카드의 수가 0이면
        {
            gameManager.I.GameStop();//게임을 멈춘다
        }
    }

    public void closeCard()
    {
        Invoke("closeCardInvoke", 0.5f);
    }

    void closeCardInvoke()
    {
        anim.SetBool("isOpen", false);
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("front").gameObject.SetActive(false);
    }
}
