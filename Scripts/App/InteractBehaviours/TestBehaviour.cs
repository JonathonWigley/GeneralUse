using System;
using UnityEngine;
using TouchScript.Gestures;

/// <summary>
/// Created By: Jonathon Wigley - 11/11/2018
/// Last Edited By: Jonathon Wigley - 11/11/2018
/// </summary>

/// <summary>
/// Component used for testing behaviours on generic objects
/// </summary>
public class TestBehaviour : MonoBehaviour
{
    PressGesture pressGesture;
    FlickGesture flickGesture;

    IInteractBehaviour pressBehaviour = new RollBehaviour();
    IInteractBehaviour flickBehaviour = new LobBehaviour();

    private void Awake()
    {
        pressGesture = GetComponent<PressGesture>();
        flickGesture = GetComponent<FlickGesture>();
    }

    private void OnEnable()
    {
        // pressGesture.Pressed += PressGesture_Pressed;
        flickGesture.Flicked += FlickGesture_Flicked;
    }

    private void OnDisable()
    {
        // pressGesture.Pressed -= PressGesture_Pressed;
        flickGesture.Flicked -= FlickGesture_Flicked;
    }

    private void PressGesture_Pressed(object sender, EventArgs args)
    {
        InputData input = new InputData();
        input.ScreenPosition = pressGesture.ScreenPosition;
        input.Direction = Vector2.zero;

        pressBehaviour.Interact(gameObject, input);
    }

    private void FlickGesture_Flicked(object sender, EventArgs args)
    {
        InputData input = new InputData();
        input.ScreenPosition = flickGesture.ScreenPosition;
        input.Direction = flickGesture.ScreenFlickVector;

        flickBehaviour.Interact(gameObject, input);
    }
}