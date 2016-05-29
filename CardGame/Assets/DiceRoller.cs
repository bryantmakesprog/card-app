using UnityEngine;
using System.Collections;

public class DiceRoller : MonoBehaviour {

    public UILabel numDice;
    int currentNumDice;
    public int minNumDice, maxNumDice;

	// Use this for initialization
	void Start () {
        currentNumDice = minNumDice;
        SetNumDice(currentNumDice);
	}
	
	void SetNumDice(int newNumDice)
    {
        currentNumDice = newNumDice;
        numDice.text = newNumDice.ToString();
    }

    public void IterateNumDice()
    {
        currentNumDice += 1;
        if (currentNumDice > maxNumDice)
            currentNumDice = minNumDice;
        SetNumDice(currentNumDice);
    }

    public void RollDice()
    {
        int totalRoll = 0;
        int i = 0;
        while(i < currentNumDice)
        {
            totalRoll += Random.Range(1, 6);
            i++;
        }
        GameObject.FindObjectOfType<InputController>().RequestDiceRoll(totalRoll);
    }
}
