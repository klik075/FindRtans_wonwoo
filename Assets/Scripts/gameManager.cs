using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public GameObject card;// 카드 프리팹
    public GameObject level;//게임의 레벨 (카드의 수)

    public GameObject firstCard;//첫번째 선택한 카드
    public GameObject secondCard;//두번째 선택한 카드
    public Text timeText;// 시간 Text
    public GameObject endText;// 끝 Text

    public AudioClip matchClip;
    public AudioSource audioSource;
    float time;//현재 시간
    float cardR = 0.7f;//카드의 반지름
    int totalCard = 0;//현재 게임상 남은 카드의 수
    public bool isStart = false;//게임을 시작했는지 했다면 true
    float timeOver = 30f;
    int[] rtans;// 카드의 배열

    public static gameManager I;
    // Start is called before the first frame update
    void Awake() 
    {
        I = this;
    }
    void Start()
    {
        level.SetActive(true);//단계 선택
        time = 0f;//시간 초기화
        Time.timeScale =1f;//시간흐름 1배속
    }

    // Update is called once per frame
    void Update()
    {
        if(isStart)// 단계를 선택했다면
        {
            Time.timeScale =1f;
            time += Time.deltaTime;//현재 시간 최신화
            timeText.text = time.ToString("N2");
            if(time > timeOver)//게임시작 후 30초가 지나면
                GameStop();//게임 멈춤
        }  
    }
    public void MakeCard()
    {
        int cardCount = 0;// 현재 카드의 idx
        for (int i = -2 ; i < 2; i++)
        {
            for (int j = -2; j < 2; j++)
            {
                if(cardCount >= totalCard)
                    return;
                float x = (i * 2f * cardR) + cardR;//카드 위치 생성
                float y = (j * 2f * cardR) + cardR;//카드 위치 생성
                GameObject gameObject = Instantiate(card);// 카드 복제
                card newCard = gameObject.GetComponent<card>();
                newCard.transform.parent = GameObject.Find("cards").transform;//현재카드를 cards의 자식으로 들어감
                newCard.transform.position = new Vector3(x,y,0);
                
                string rtanName = "rtan"+ rtans[cardCount].ToString();
                newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);

                cardCount++;
            }
        }
    }
    public void IsMatched()// 카드 매칭
    {
        string fisrtCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if(fisrtCardImage == secondCardImage) // 이미지가 같다면
        {
            audioSource.PlayOneShot(matchClip);
            firstCard.GetComponent<card>().destroyCard(); //카드 삭제
            secondCard.GetComponent<card>().destroyCard();
        }
        else
        {
            firstCard.GetComponent<card>().closeCard();//카드 덮기
            secondCard.GetComponent<card>().closeCard();
        }

        firstCard = null;// 두 장을 뒤집어서 null로 초기화
        secondCard = null;
    }
    public void SetRtans()// 카드수에 맞게 카드배열 초기화
    {
        rtans = new int[totalCard]; //01 23
        for (int i = 0; i < totalCard; i++)//01 23 45 67
        {
            if(i%2 == 0)//짝수
                rtans[i] = i/2;
            else//홀수
                rtans[i] = i/2;
        }
        rtans = rtans.OrderBy(item => Random.Range(-1.0f,1.0f)).ToArray();//랜덤 섞기
    }
    public void SetLevel12()// 카드 12개로 설정 및 카드 생성
    {  
        totalCard = 12;
        isStart =true;
        level.SetActive(false);
        SetRtans();
        MakeCard();
    }
    public void SetLevel14()// 카드 14개로 설정 및 카드 생성
    {
        totalCard = 14;
        isStart =true;
        level.SetActive(false);
        SetRtans();
        MakeCard();
    }
    public void SetLevel16()// 카드 16개로 설정 및 카드 생성
    {
        totalCard = 16;
        isStart =true;
        level.SetActive(false);
        SetRtans();
        MakeCard();
    }
    public void GameStop()// 게임멈춤
    {
        endText.SetActive(true);
        isStart = false;
        Time.timeScale = 0f;
    }
    public void AbstractCount()// 게임상 카드의 개수에서 하나 빼기
    {
        totalCard--;
        //Debug.Log("total : " + totalCard);
    }
    public int TotalCardIs() // 게임상 카드의 개수 반환
    {
        return totalCard;
    }
}
