using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

    public int game, player;

    private ApiController api;

	// Use this for initialization
	void Start () {
        api = GameObject.FindObjectOfType<ApiController>();
	}
	
	public IEnumerator PurchaseCard(int card)
    {
        yield return api.PostPurchaseInfo(game, player, card);
    }

    public void RequestPurchase(int card)
    {
        gameObject.SendMessage("PurchaseCard", card, SendMessageOptions.DontRequireReceiver);
    }
}
