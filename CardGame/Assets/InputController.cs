using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

    public int game, player;

    private ApiController api;

	// Use this for initialization
	void Start () {
        api = GameObject.FindObjectOfType<ApiController>();
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

    private void DisablePurchaseButtons()
    {
        CardButton[] allPurchaseButtons = GameObject.FindObjectsOfType<CardButton>();
        foreach(CardButton button in allPurchaseButtons)
        {
            button.gameObject.SetActive(false);
        }
    }
}
