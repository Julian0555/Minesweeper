using Minesweeper;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MineSettings : MonoBehaviour, IPointerClickHandler
{
    public GameObject dropdown;
    public GameObject scoreBomb;
    public GameObject scoreResetButton;
    public Text highscores;
    public Text newHighscore;
    public Minesweep gameInstance;
    // Start is called before the first frame update
    void Start()
    {
        var comp = dropdown.GetComponent<Dropdown>();
        comp.ClearOptions();
        foreach (Minesweep.Difficulty diff in gameInstance.gameSizes)
        {
            comp.options.Add(new Dropdown.OptionData(diff.width.ToString() + " x " + diff.height.ToString()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    byte blinkCount = 0;
    private IEnumerator BlinkRoutine()
    {
        foreach (GameObject obj in gameInstance.timer) obj.SetActive(false);
        while (blinkCount < 10)
        {
            blinkCount++;
            newHighscore.enabled = !newHighscore.enabled;
            foreach (GameObject obj in gameInstance.timer) obj.SetActive(!obj.activeSelf);
            yield return new WaitForSeconds(0.5f);
        }
        foreach (GameObject obj in gameInstance.timer) obj.SetActive(true);
        blinkCount = 0;
    }

    public void BlinkHighscore()
    {
        StartCoroutine(BlinkRoutine());
    }

    void ScoreText()
    {
        highscores.text = "Highscores\n";
        for (int i = 0; i < gameInstance.gameSizes.Length; i++)
        {
            var difficulty = gameInstance.gameSizes[i];
            var highscore = difficulty.width.ToString() + ":" + difficulty.height.ToString();
            var yeah = PlayerPrefs.GetInt(highscore);
            if (yeah != 0)
            {
                highscores.text += highscore.Replace(":", " x ") + " = " + yeah.ToString() + "\n";
            }
        }
    }

    public void ResetScores()
    {
        gameInstance.ResetScores();
        DisableDropdown();
        //foreach (GameObject obj in new GameObject[] { dropdown, scoreBomb, scoreResetButton }) obj.SetActive(false);
    }

    public void BombClick()
    {
        Debug.Log("sadfasd");
        dropdown.SetActive(!dropdown.activeSelf);
        scoreResetButton.SetActive(!scoreResetButton.activeSelf);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        scoreBomb.SetActive(!scoreBomb.activeSelf);
        if (scoreBomb.activeSelf)
        {
            highscores.gameObject.SetActive(true);
            ScoreText();
        }
        else highscores.gameObject.SetActive(false);
        dropdown.SetActive(scoreBomb.activeSelf);
        scoreResetButton.SetActive(false);
    }

    public void DisableDropdown()
    {
        dropdown.SetActive(false);
        highscores.gameObject.SetActive(false);
        scoreBomb.SetActive(false);
        scoreResetButton.SetActive(false);
    }
}
