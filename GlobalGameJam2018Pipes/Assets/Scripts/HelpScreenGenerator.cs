using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class HelpScreenGenerator : MonoBehaviour
{
	public GameObject helpTextColorPrefab;

	private MixerScript mixer = new MixerScript();
		
	// Use this for initialization
	void Start () {
		
		Debug.Log($"in start help screen");
		Debug.Log($"this component is {this.name}");

		GameObject parent = GameObject.Find("ColorMixContainer");

		CreateColormixDisplay(parent, -100, 130, MaterialColor.Red, MaterialColor.Yellow);
		CreateColormixDisplay(parent, -100, 110, MaterialColor.Yellow, MaterialColor.Blue);
		CreateColormixDisplay(parent, -100, 90, MaterialColor.Blue, MaterialColor.Red);

		CreateColormixDisplay(parent, -100, 60, MaterialColor.Red, MaterialColor.Orange);
		CreateColormixDisplay(parent, -100, 40, MaterialColor.Red, MaterialColor.Green);
		CreateColormixDisplay(parent, -100, 20, MaterialColor.Red, MaterialColor.Violet);
		CreateColormixDisplay(parent, -100, 0, MaterialColor.Yellow, MaterialColor.Orange);
		CreateColormixDisplay(parent, -100, -20, MaterialColor.Yellow, MaterialColor.Green);
		CreateColormixDisplay(parent, -100, -40, MaterialColor.Yellow, MaterialColor.Violet);
		CreateColormixDisplay(parent, -100, -60, MaterialColor.Blue, MaterialColor.Orange);
		CreateColormixDisplay(parent, -100, -80, MaterialColor.Blue, MaterialColor.Green);
		CreateColormixDisplay(parent, -100, -100, MaterialColor.Blue, MaterialColor.Violet);

		CreateColormixDisplay(parent, 0, 130, MaterialColor.Orange, MaterialColor.Green);
		CreateColormixDisplay(parent, 0, 110, MaterialColor.Green, MaterialColor.Violet);
		CreateColormixDisplay(parent, 0, 90, MaterialColor.Violet, MaterialColor.Orange);

		CreateColormixDisplay(parent, 0, 60, MaterialColor.Red, MaterialColor.Black);
		CreateColormixDisplay(parent, 0, 40, MaterialColor.Yellow, MaterialColor.Black);
		CreateColormixDisplay(parent, 0, 20, MaterialColor.Blue, MaterialColor.Black);

		CreateColormixDisplay(parent, 0, 0, MaterialColor.Orange, MaterialColor.Black);
		CreateColormixDisplay(parent, 0, -20, MaterialColor.Green, MaterialColor.Black);
		CreateColormixDisplay(parent, 0, -40, MaterialColor.Violet, MaterialColor.Black);

	}

	private void CreateColormixDisplay(GameObject parent, int x, int y, MaterialColor color1, MaterialColor color2)
	{
		GameObject newMix = Instantiate(helpTextColorPrefab, parent.transform);
		newMix.transform.localPosition = new Vector3(x, y, 0);
		newMix.name = $"Mix_{color1}+{color2}";

		SpriteRenderer[] renderers = newMix.GetComponentsInChildren<SpriteRenderer>();
		renderers[0].color = MixerScript.ConvertMaterialColor(color1);
		renderers[1].color = MixerScript.ConvertMaterialColor(color2);
		renderers[2].color = MixerScript.ConvertMaterialColor(mixer.MixColor(color1, color2));
	}

	// Update is called once per frame
	void Update () {
		
	}
}
