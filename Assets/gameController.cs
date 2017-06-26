using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{

    public static gameController instance;
    float counter;
    float interval;
    float iterate;


    public Sprite[] allSprite;
    public int[] deck;


    public GameObject prefabCard;
    public GameObject posDeck;
    public GameObject textHand;
    public GameObject iaTextHand;
    public GameObject iaHAND;
    GameObject currentGo;
    public GameObject myHand;
    List<GameObject> allCard;
    
    bool fly;
    int nbTake;
    Vector3 positionToGo;
    Vector3 positionToGo2;
    Vector3 cardGoHere;
    bool beginGame;
    int textHandValue;
    int iaTextHandValue;
    bool stay;
    bool endGame;
    int earlyGame;
    bool playerWin;
    bool oneTimeAction;

    bool startTheGame;

    public GameObject holderCanvas;
    public GameObject holderObject;
    public GameObject textBet;
    public int valBet;
    public int playerCoin;
    public Text totalCoin;
    public GameObject holderEarly;
    public GameObject holderBet;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        PlayerPrefs.DeleteKey("playerCoin");
        playerCoin = PlayerPrefs.GetInt("playerCoin", 1000);
        totalCoin.text = playerCoin + "$";

        startTheGame = false;
        oneTimeAction = false;
        playerWin = false;
        earlyGame = 0;
        endGame = false;
        stay = false;
        beginGame = false;
        counter = 0;
        interval = 1f;
        nbTake = 0;
        textHandValue = 0;
        iaTextHandValue = 0;
        textHand.GetComponent<TextMesh>().text = textHandValue + "";
        iaTextHand.GetComponent<TextMesh>().text = iaTextHandValue + "";
        positionToGo = iaHAND.transform.position;
        positionToGo2 = myHand.transform.position;
        cardGoHere = positionToGo;
        fly = false;
        allCard = new List<GameObject>();
        int xVal = 1;

        for (int i = 0; i < allSprite.Length; i++)
        {
            GameObject card = (GameObject)Instantiate(prefabCard, posDeck.transform.position, Quaternion.identity);
            card.GetComponent<SpriteRenderer>().sprite = allSprite[i];
            card.GetComponent<cardScript>().value = xVal;
            card.SetActive(false);
            xVal++;
            if (xVal > 13)
                xVal = 1;
            allCard.Add(card);
        }
       
    }


    public void validBet()
    {
        if (valBet > 0)
        {
            holderEarly.SetActive(false);
            holderBet.SetActive(false);
            holderCanvas.SetActive(true);
            holderObject.SetActive(true);
            startTheGame = true;
        }
        else
        {
            print("BET MORE");
        }
    }


    public void addBet(int val)
    {
        if (startTheGame == false)
        {
            playerCoin -= val;
            PlayerPrefs.SetInt("playerCoin", playerCoin);
            totalCoin.text = playerCoin + "$";
            valBet += val;
            textBet.GetComponent<TextMesh>().text = valBet + " $";
        }
    }

    void drawCard()
    {
        fly = true;
        bool ok = false;
        cardGoHere = positionToGo;
        while (ok == false)
        {
            currentGo = allCard[Random.Range(0, allCard.Count)];
            if (currentGo.activeSelf == false)
            {
                nbTake++;
                currentGo.SetActive(true);
                ok = true;
            }
        }

    }

    public void playerDraw()
    {
        print("player Draw");
        if (fly == false && nbTake < 15 && stay == false && endGame == false)
        {
            cardGoHere = positionToGo2;
            bool ok = false;
            while (ok == false)
            {
                currentGo = allCard[Random.Range(0, allCard.Count)];
                if (currentGo.activeSelf == false)
                {
                    nbTake++;
                    currentGo.SetActive(true);
                    ok = true;
                }
            }

            fly = true;
        }
    }

    public void playerStay()
    {
        print("player Stay");
        if (endGame == false)
        {
            stay = true;

            playerStayDraw();
        }
    }

    void playerStayDraw()
    {
        bool ok = false;
        cardGoHere = positionToGo;
        while (ok == false)
        {
            currentGo = allCard[Random.Range(0, allCard.Count)];
            if (currentGo.activeSelf == false)
            {
                nbTake++;
                currentGo.SetActive(true);
                ok = true;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

        if (startTheGame == true)
        {
            counter += Time.deltaTime;


            if (counter >= interval && beginGame == false)
            {
                beginGame = true;
                drawCard();
            }

        }
        if (endGame == true && oneTimeAction == false)
        {
            if(playerWin == true)
            {
                print("GIVE TO PLAYER");
            }else
            {
                print("GIVE TO IA");
            }
            oneTimeAction = true;
        }

        if (beginGame == true && endGame == false)
        {
            if (fly == true)
            {

                currentGo.transform.position = Vector3.MoveTowards(currentGo.transform.position, cardGoHere, 10f * Time.deltaTime);

                if(Vector3.Distance(currentGo.transform.position, cardGoHere) <= 0.01f)
                {
                    fly = false;
                    earlyGame++;
                    if (cardGoHere == positionToGo2)
                    {
                        positionToGo2.x += 0.8f;
                        positionToGo2.z -= 1;

                        int valueCard = currentGo.GetComponent<cardScript>().value;
                        if (valueCard == 1)
                        {
                            if (textHandValue + 11 < 22)
                            {
                                valueCard = 11;
                            }
                            else
                            {
                                valueCard = 1;
                            }
                        }
                        else if (valueCard > 10)
                            valueCard = 10;

                        textHandValue += valueCard;
                        textHand.GetComponent<TextMesh>().text = textHandValue + "";

                        if (textHandValue > 21)
                        {
                            playerWin = false;
                            print("IA WIN");
                            endGame = true;
                        }else if (textHandValue == 21)
                        {
                            print("Player WIN");
                            playerWin = true;
                            endGame = true;
                        }
                    }  
                    else
                    {
                        positionToGo.x += 0.8f;
                        positionToGo.z -= 1;

                        int valueCard = currentGo.GetComponent<cardScript>().value;
                        if (valueCard == 1)
                        {
                            if (iaTextHandValue + 11 < 22)
                            {
                                valueCard = 11;
                            }
                            else
                            {
                                valueCard = 1;
                            }
                        }
                        else if (valueCard > 10)
                            valueCard = 10;

                        iaTextHandValue += valueCard;
                        iaTextHand.GetComponent<TextMesh>().text = iaTextHandValue + "";
                    }
                    if(earlyGame < 3)
                    {
                        playerDraw();
                    }
                        
                }
            }

            if (stay == true && endGame == false)
            {
                currentGo.transform.position = Vector3.MoveTowards(currentGo.transform.position, cardGoHere, 10f * Time.deltaTime);

                if (Vector3.Distance(currentGo.transform.position, cardGoHere) <= 0.01f)
                {
                    positionToGo.x += 0.8f;
                    positionToGo.z -= 1;

                    int valueCard = currentGo.GetComponent<cardScript>().value;

                    if(valueCard == 1)
                    {
                        if(iaTextHandValue + 11 < 22)
                        {
                            valueCard = 11;
                        }
                        else
                        {
                            valueCard = 1;
                        }
                    }
                   else if (valueCard > 10)
                        valueCard = 10;

                    iaTextHandValue += valueCard;
                    iaTextHand.GetComponent<TextMesh>().text = iaTextHandValue + "";

                    if(iaTextHandValue > 21)
                    {
                        endGame = true;
                        print("PlayerWin");
                        playerWin = true;
                    }
                    else if (iaTextHandValue > textHandValue)
                    {
                        endGame = true;
                        print("IAWIN");
                        playerWin = false;
                    }
                    else if(iaTextHandValue < 17)
                    {
                        playerStayDraw();
                    }
                    else
                    {
                        endGame = true;
                        print("PlayerWin");
                        playerWin = true;
                    }
                         
                }

            }
        }
    }
}
