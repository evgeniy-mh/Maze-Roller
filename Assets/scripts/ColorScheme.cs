using UnityEngine;
using System.Collections;

public class ColorScheme {

	public string[] colorShemes;
	private string temp;

	private Color32 mazeColor;
	private Color32 playerColor;
	private Color32 bonusColor;


	public ColorScheme(){
		//цвет лабиринта/ цвет шара/ цвет бонуов
		colorShemes= new string[] {
			"38337B8B2C66552B76",
			"752571B04138B08138",
			"2677587FA434B0A438",
			"313C7925735D79A334",
			"572B7624725EB08D38",
			"226666AA39397A9F35",
			"221A64018ED692EEE9",
			"012227FB0C06DBD5B8",
			"708D91D3199619DD89"};
	  

	}

	private bool checkScheme(string scheme){
		if (scheme.Length == 18)
			return true;
		else
			return false;

	}

	public Color32[] getColorRandomScheme(){
		
		temp=colorShemes[Random.Range(0,colorShemes.Length)];

		if (checkScheme (temp)) {

			mazeColor = HexToColor (temp.Substring (0, 6));
			playerColor = HexToColor (temp.Substring (6, 6));
			bonusColor = HexToColor (temp.Substring (12, 6));
			Color32[] value = new Color32[] {mazeColor,playerColor,bonusColor};

			return value;

		} else {
			Debug.Log("invalid scheme: "+temp);
			return null;
		}

		/*Debug.Log (temp);
		Debug.Log (temp.Substring (0,6));
		Debug.Log (temp.Substring (6,6));
		Debug.Log (temp.Substring (12,6));*/

	}

	private Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b,255);
	}


}
