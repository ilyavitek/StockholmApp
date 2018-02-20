using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartController : MonoBehaviour {

    [SerializeField] float m_amountOfSecondsToRestart;

    [SerializeField] float m_amountOfSecondsBeforeRestartToShowAllert;

    Timer m_timer;

    bool m_isWarningShowed = false;

	void Start () {
        m_timer = new Timer (m_amountOfSecondsToRestart, true);
    }
	
	void Update () {
        if(m_timer.CurrentTime <= m_amountOfSecondsBeforeRestartToShowAllert && !m_isWarningShowed) {
            Debug.Log ("Show WARNING");
            m_isWarningShowed = true;
        }

		if(m_timer.Launch()) {
            SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
        }
	}
}
