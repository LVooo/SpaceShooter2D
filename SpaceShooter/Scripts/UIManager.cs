using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _ScoreText;
    [SerializeField]
    private int _Score;
    [SerializeField]
    private Sprite[] _LiveSprites;
    [SerializeField]
    private Image _LivesImage;
    [SerializeField]
    private Text _GameOverText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void UpdateScore()
    {
        _Score = _Score + 10;
        _ScoreText.text = "Score: " + _Score;
    }

    public void UpdateLives(int CurrentLive)
    {
        _LivesImage.sprite = _LiveSprites[CurrentLive];
        if (CurrentLive < 1)
        {
            _GameOverText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlicker());
        }
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _GameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _GameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
