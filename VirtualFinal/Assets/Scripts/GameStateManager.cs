using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager m_instance;
    [SerializeField] private GameObject[] m_initSpawnPoints;
    internal int m_numPlayers = 0;
    [SerializeField] private PlayerInputManager m_inputManager;

    [SerializeField] private GameObject[] playerPanels;
    [SerializeField] private GameObject[] playerHUDs;


    internal bool m_gameStarted = false;

    public static GameStateManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("GameStateManager").AddComponent<GameStateManager>();
            }
            return m_instance;
        }
    }

    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else if (m_instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        PauseGame();
    }

    public int AddPlayer(out GameObject o_playerHUD)
    {
        gameObject.GetComponent<CameraManager>().SetToDisplay(m_numPlayers);
        m_numPlayers++;
        Text tempText = null;
        o_playerHUD = null;

        switch(m_numPlayers)
        {
            case 1:
                o_playerHUD = playerHUDs[0];
                tempText = playerPanels[0].GetComponentInChildren<Text>();
                tempText.text = "Player 1 ready!\nPress Start to beging the match";
                break;
            case 2:
                o_playerHUD = playerHUDs[1];
                tempText = playerPanels[1].GetComponentInChildren<Text>();
                tempText.text = "Player 2 ready!\nPress Start to beging the match";
                break;
            case 3:
                o_playerHUD = playerHUDs[2];
                tempText = playerPanels[2].GetComponentInChildren<Text>();
                tempText.text = "Player 3 ready!\nPress Start to beging the match";
                break;
            case 4:
                o_playerHUD = playerHUDs[3];
                tempText = playerPanels[3].GetComponentInChildren<Text>();
                tempText.text = "Player 4 ready!\nPress Start to beging the match";
                break;
            default:
                tempText = null;
                break;
        }

        return m_numPlayers;
    }

    public Vector3 GetInitialSpawnPoint(int i_playerID)
    {
        return m_initSpawnPoints[i_playerID-1].transform.position;
    }

    public void SetSplitScreen(bool i_bool)
    {
        m_inputManager.splitScreen = i_bool;
    }

    public void StartGame()
    {
        m_gameStarted = true;
        SetSplitScreen(true);
        
        foreach(GameObject panel in playerPanels)
        {
            panel.SetActive(false);
        }

        for(int i = 0; i < m_numPlayers; i++)
        {
            playerHUDs[i].SetActive(true);
        }

        ResumeGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
    }
}
