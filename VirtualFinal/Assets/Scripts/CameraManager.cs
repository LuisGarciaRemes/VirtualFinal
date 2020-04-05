using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera[] m_playerCameras;
    private Vector3[] m_position;
    [SerializeField] private float m_speed;

    private static CameraManager m_instance;
    public static CameraManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("GameStateManager").AddComponent<CameraManager>();
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

    // Start is called before the first frame update
    void Start()
    {
        m_position = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            m_position[i] = m_playerCameras[i].transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            m_playerCameras[i].transform.localPosition = Vector3.Lerp(m_playerCameras[i].transform.localPosition, m_position[i], Time.deltaTime * m_speed);
        }
    }

    public void UpdateCameraPosition(int i_playerID, Vector3 i_position)
    {
        m_position[i_playerID-1] = i_position;
    }
}
