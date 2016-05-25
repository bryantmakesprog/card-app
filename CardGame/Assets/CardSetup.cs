using UnityEngine;
using System.Collections;

public class CardSetup : MonoBehaviour {

    public int cardId;

    private ApiController api;

	// Use this for initialization
	IEnumerator Start () {
        api = GameObject.FindObjectOfType<ApiController>();
        yield return api.GetCardInfo(cardId);
        Debug.Log("CardSetup.Start: " + api.result);
	}

}
