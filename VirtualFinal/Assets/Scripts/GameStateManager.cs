using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager m_instance;
    [SerializeField] private GameObject[] m_initSpawnPoints;
    private int m_numPlayers = 0;
    [SerializeField] private PlayerInputManager m_inputManager;

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
    }

    public int AddPlayer()
    {
        m_numPlayers++;

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
}
