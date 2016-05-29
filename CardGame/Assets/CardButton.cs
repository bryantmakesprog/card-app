using UnityEngine;
using System.Collections;

public class CardButton : MonoBehaviour {

	public void OnCardClick()
    {
        InputController controller = GameObject.FindObjectOfType<InputController>();
        controller.RequestPurchase(gameObject.transform.parent.GetComponent<CardSetup>().cardId, gameObject.transform.parent.GetComponent<CardSetup>().cost);
    }
}
