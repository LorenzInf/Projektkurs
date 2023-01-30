using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatController : MonoBehaviour
{
	public static int _level=1;
    public static int _rugh = 0;
	public static int _tempRugh = 0;
	public static int enemiesKilled = 0;
	public static int wordsTyped = 0;
	public static int mistakesMade = 0;
	public static bool lastRunSuccessful = false;

	public static void reset() {
		if (lastRunSuccessful) {
			_rugh += _tempRugh;
		}
		_tempRugh = 0;
		enemiesKilled = 0;
		wordsTyped = 0;
		mistakesMade = 0;
	}
}
