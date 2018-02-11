using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AsyncOperation = UnityEngine.AsyncOperation;


public class OptionsMenu : MonoBehaviour
{
	public InputField colorSourceRedInput;
	public InputField colorSourceOrangeInput;
	public InputField colorSourceYellowInput;
	public InputField colorSourceGreenInput;
	public InputField colorSourceBlueInput;
	public InputField colorSourceVioletInput;
	public InputField colorSourceBlackInput;
	
	public InputField colorSinkRedInput;
	public InputField colorSinkOrangeInput;
	public InputField colorSinkYellowInput;
	public InputField colorSinkGreenInput;
	public InputField colorSinkBlueInput;
	public InputField colorSinkVioletInput;
	public InputField colorSinkBlackInput;

	public Button cancelButton;
	public Button saveButton;


	private Dictionary<MaterialColor, InputField> sourceColorFields; 
	private Dictionary<MaterialColor, InputField> sinkColorFields; 
	
	// Use this for initialization
	void Start () {		
		Init();
	}

	private void Init ()
	{
		Debug.Log("OptionsMenu.Init called");
		// bind buttons to colors, for easier handling when dialog is called
		sourceColorFields = new Dictionary<MaterialColor, InputField>();
		sourceColorFields[MaterialColor.Red] = colorSourceRedInput;
		sourceColorFields[MaterialColor.Orange] = colorSourceOrangeInput;
		sourceColorFields[MaterialColor.Yellow] = colorSourceYellowInput;
		sourceColorFields[MaterialColor.Green] = colorSourceGreenInput;
		sourceColorFields[MaterialColor.Blue] = colorSourceBlueInput;
		sourceColorFields[MaterialColor.Violet] = colorSourceVioletInput;
		sourceColorFields[MaterialColor.Black] = colorSourceBlackInput;

		sinkColorFields = new Dictionary<MaterialColor, InputField>();
		sinkColorFields[MaterialColor.Red] = colorSinkRedInput;
		sinkColorFields[MaterialColor.Orange] = colorSinkOrangeInput;
		sinkColorFields[MaterialColor.Yellow] = colorSinkYellowInput;
		sinkColorFields[MaterialColor.Green] = colorSinkGreenInput;
		sinkColorFields[MaterialColor.Blue] = colorSinkBlueInput;
		sinkColorFields[MaterialColor.Violet] = colorSinkVioletInput;
		sinkColorFields[MaterialColor.Black] = colorSinkBlackInput;
		Debug.Log("OptionsMenu.Init called");
	}
	
	void OnEnable()
	{
		if (sinkColorFields == null) Init();
		
		// load values from Options
		foreach (KeyValuePair<MaterialColor, InputField> entry in sourceColorFields)
		{
			int raw = GameManager.options.GetSourceProbabilityRaw(entry.Key);
			entry.Value.text = raw.ToString();
		}
		foreach (KeyValuePair<MaterialColor, InputField> entry in sinkColorFields)
		{
			int raw = GameManager.options.GetSinkProbabilityRaw(entry.Key);
			entry.Value.text = raw.ToString();
		}
	}

	public void OnCancelButtonClicked()
	{
		// do not write values to Options, just forget them
		SwitchToGame();
	}


	public void OnSaveButtonClicked()
	{
		// write values to Options
		foreach (KeyValuePair<MaterialColor, InputField> entry in sourceColorFields)
		{
			int colorOccur = 0;
			if (entry.Value.text.Length > 0)
			{			
				if (!Int32.TryParse(entry.Value.text, out colorOccur))
					colorOccur = 10;
				GameManager.options.SetSourceProbability(entry.Key, colorOccur);
			}
		}
		foreach (KeyValuePair<MaterialColor, InputField> entry in sinkColorFields)
		{
			int colorOccur = 0;
			if (entry.Value.text.Length > 0)
			{			
				if (!Int32.TryParse(entry.Value.text, out colorOccur))
					colorOccur = 10;
				GameManager.options.SetSinkProbability(entry.Key, colorOccur);
			}
		}
		SwitchToGame();
	}

	// TODO game is restarted when coming back from options, would be nice if it continued
	private void SwitchToGame()
	{
		string sceneName = "MainScene";
		Scene scene = SceneManager.GetSceneByName(sceneName);
		if (scene.IsValid())
		{
			Debug.Log($"will set scene active ({sceneName})");
			SceneManager.SetActiveScene(scene);
		}                
		else
		{
			Debug.Log($"will load scene in coroutine ({sceneName})");
			StartCoroutine(LoadScene(sceneName));
		}

	}

	// copy from GameManager - should be better solution
	private IEnumerator LoadScene(string sceneName)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
		//Wait until the last operation fully loads to return anything
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
		Debug.Log($"scene {sceneName} is loaded");
	}


}
