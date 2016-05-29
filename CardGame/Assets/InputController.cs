using UnityEngine;
using System.Collections;
using SimpleJSON;

public class InputController : MonoBehaviour {

    public int game, player;

    private ApiController api;

	// Use this for initialization
	void Start () {
        api = GameObject.FindObjectOfType<ApiController>();
        player = PlayerPrefs.GetInt("player");
        game = PlayerPrefs.GetInt("gameToJoin");
	}
	
	IEnumerator PurchaseCard(int card)
    {
        Debug.Log("purchasing");
        yield return api.PostPurchaseInfo(game, player, card);
    }

    public void RequestPurchase(int card, int cost)
    {
        Debug.Log("requesting purchase: " + card);
        SendMessage("PurchaseCard", card, SendMessageOptions.RequireReceiver);
        DisablePurchaseButtons();
    }

    public void RequestDiceRoll(int roll)
    {
        SendMessage("RollDice", roll, SendMessageOptions.RequireReceiver);
    }

    IEnumerator RollDice(int roll)
    {
        Debug.Log("Player " + player.ToString() + " rolled " + roll.ToString());
        yield return api.PostRollInfo(game, player, roll);
        GameObject.FindObjectOfType<DiceRoller>().gameObject.SetActive(false);
        var stateInfo = JSON.Parse(api.result);
        UpdateStateInfoForRoller(int.Parse(stateInfo["points"]));
    }

    private void UpdateStateInfoForRoller(int newPoints)
    {
        StateSetup[] allStates = GameObject.FindObjectsOfType<StateSetup>();
        foreach(StateSetup playerState in allStates)
        {
            if(playerState.player == player)
            {
                playerState.points = newPoints;
                Debug.Log("Player " + player + " now has " + newPoints + " points!");
            }
        }
    }

    private void DisablePurchaseButtons()
    {
        CardButton[] allPurchaseButtons = GameObject.FindObjectsOfType<CardButton>();
        foreach(CardButton button in allPurchaseButtons)
        {
            button.gameObject.SetActive(false);
        }
    }
}
