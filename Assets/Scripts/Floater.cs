using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class Floater : MonoBehaviour {

	// animate the game object from -1 to +1 and back
	public float minimum = -9.0F;
	public float maximum =  9.0F;

	// starting value for the Lerp
	static float t = 0.0f;
	RectTransform myRectComponent;

	void Start()
	{
		myRectComponent = GetComponent<RectTransform> ();
		myRectComponent.DOAnchorPos (new Vector2 (myRectComponent.anchoredPosition.x, -1), 0.3f, false).SetLoops (-1, LoopType.Yoyo);

		// transform.DOMoveX(4, 1).SetLoops(3, LoopType.Yoyo);
	}

}
