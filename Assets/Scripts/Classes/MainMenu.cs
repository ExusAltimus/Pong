using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class MainMenu : MonoBehaviour {

    public Toggle HitmarkerCheckbox;
    public Toggle SezuireModeCheckbox;
    public Dropdown PlayerModeDropdown;
    public InputField GameSpeedInputField;
    public InputField AiDifficultyInputField;
    public InputField BallSpeedIncrementInputField;
    public Text MessageText;

    // Use this for initialization
    void Start () {
        PlayerModeDropdown.ClearOptions();
        PlayerModeDropdown.AddOptions(System.Enum.GetNames(typeof(PlayerMode)).Select(x => new Dropdown.OptionData(x)).ToList()); //Populate options
        if (!String.IsNullOrEmpty(GameManager.Instance.Message))
        {
            MessageText.text = GameManager.Instance.Message;
        }
        else
        {
            MessageText.text = String.Empty;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play()
    {
        GameManager.Instance.SezuireMode = false;
        GameManager.Instance.Hitmarker = HitmarkerCheckbox.isOn;
        GameManager.Instance.SezuireMode = SezuireModeCheckbox.isOn;
        GameManager.Instance.PlayerMode = (PlayerMode)PlayerModeDropdown.value;
        GameManager.Instance.GameSpeed = Single.Parse(GameSpeedInputField.text);
        GameManager.Instance.AiDifficulty = Single.Parse(AiDifficultyInputField.text);
        GameManager.Instance.BallSpeedIncrement = Single.Parse(BallSpeedIncrementInputField.text);
        PongSceneManager.PlayPong();
    }

    public void Exit()
    {
        PongSceneManager.Exit();
    }


}
