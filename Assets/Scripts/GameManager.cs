using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public enum RoundState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class GameManager : MonoBehaviour
{

    [SerializeField] List<GameObject> gameObjPlayers = new List<GameObject>();
    
    [SerializeField] List<TextMeshProUGUI> lifeTextsPlayersText = new List<TextMeshProUGUI>();
    [SerializeField] List<TextMeshProUGUI> PointsTextsPlayersText = new List<TextMeshProUGUI>();
    [SerializeField] List<TextMeshProUGUI> RollDicePlayersText = new List<TextMeshProUGUI>();
    [SerializeField] List<TextMeshProUGUI> BetPlayersText = new List<TextMeshProUGUI>();
    
    [SerializeField] GameObject betModal;
    [SerializeField] GameObject winModal;
    [SerializeField] GameObject loseModal;
    [SerializeField] TextMeshProUGUI betInput;


    public List<int> playersRoundPoints = new List<int>();

    int[] playersPoints;
    int[] playersRollResult;
    int[] playersLife;

    bool[] playersAlive;

    int MaxRound;
    int turn;

    public int[] playersBet;

    public RoundState state;



    private void Start()
    {
        state = RoundState.START;
        StartCoroutine(GameStartSetup());
    }

 

    void RoundStart()
    {
        PlayersAliveVerification();
        betModal.SetActive(true);
        

    }

    int ToInt(string t)
    {
        int result;
        int.TryParse(t, out result);
        return result;
    }

    public void SetBet()
    {
        
        //playersBet[0] = ToInt(betInput.GetComponent<TextMeshProUGUI>().text);
        int.TryParse(betInput.GetComponent<TextMeshProUGUI>().text,out playersBet[0]);
        Debug.Log(playersBet[0]);
        Debug.Log(int.TryParse(betInput.GetComponent<TextMeshProUGUI>().text + "", out playersBet[0]));
        
        
        
        for (int i = 1; i < 5; i++)
            playersBet[i] = Random.Range(0, 3);

        
        for (int i = 0; i < 5; i++)
            BetPlayersText[i].text = playersBet[i].ToString();

        betModal.SetActive(false);
        state = RoundState.PLAYERTURN;
    }

    private void PlayersAliveVerification()
    {
        foreach (var player in playersLife)
        {
            if (playersLife[player] == 0)
            {
                gameObjPlayers[player].SetActive(false);
            }
        }
    }

    public IEnumerator PlayersRoll()
    {
        //animação dos dados - aqui
        
        
        yield return new WaitForSeconds(0.5f);

        playersRollResult[0] = Roll(3);

        state = RoundState.ENEMYTURN;
        
        for (int i = 1; i < 5; i++)
            playersRollResult[i] = Roll(Random.Range(0, 3));

        for (int i = 0; i < 5; i++)
            RollDicePlayersText[i].text = playersRollResult[i].ToString();
        
        yield return new WaitForSeconds(1f);
        
        
        CheckWinRound(playersRollResult);
    }


    public int Roll(int diceType)
    {
        int result;
        switch (diceType)
        {
            case 0:
                result = Random.Range(1, 5);
                return result;
            case 1:
                result = Random.Range(1, 7);
                return result;
            case 2:
                result = Random.Range(1, 11);
                return result;
            case 3:
                result = Random.Range(1, 19);
                return result;
            default:
                result = 0;
                return result;
        }
    }


    IEnumerator GameStartSetup()
    {
        MaxRound = 4;
        playersLife = new int[5] { 1, 1, 1, 1, 1 };
        playersAlive = new bool[5] {true,true, true, true, true};
        playersBet = new int[5] {0,0,0,0,0};
        playersPoints = new int[5] {0,0,0,0,0};
        playersRollResult = new int[5] {0,0,0,0,0};
        winModal.SetActive(false);
        loseModal.SetActive(false);
        betModal.SetActive(false);


        for (int player = 0; player < lifeTextsPlayersText.Count; player++)
        {
            lifeTextsPlayersText[player].text = playersLife[player].ToString();
        }

        for (int player = 0; player < lifeTextsPlayersText.Count; player++)
        {
            lifeTextsPlayersText[player].text = playersLife[player].ToString();
        }

        yield return new WaitForSeconds(0.2f);

        

        RoundStart();

    }

    void CheckWinRound(int[] results)
    {
        int winnerID = 0;
        int highestValue = 0;
        for (int i = 0; i < results.Length; i++)
        {
            if(results[i] > highestValue)
            {
                highestValue = results[i];
                winnerID = i;
            }
        }
        playersPoints[winnerID]++;

            


        for (int i = 0; i < 5; i++)
            PointsTextsPlayersText[i].text = playersPoints[i].ToString();

        turn++;
        state = RoundState.PLAYERTURN;
        if (turn >= MaxRound)
        {
            for (int i = 0; i < 5; i++)
                if(playersBet[i] != playersPoints[i] ) playersLife[i]--;
            if (playersLife[0] > 0) WinGame();
            else GameOver();
        }
    }

    private void WinGame()
    {
        state = RoundState.WON;
 
        winModal.SetActive(true);

    }
    public void DiceRoll()
    {
        if (state != RoundState.PLAYERTURN)
            return;

        StartCoroutine(PlayersRoll());
    }
    private void GameOver()
    {
        state = RoundState.LOST;
        loseModal.SetActive(true);
    }
}
