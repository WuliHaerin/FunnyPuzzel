﻿#pragma strict

public class RcControllerJS extends PhysicsObjectJS
{
    public var car				: RcCarJS;				//Holds a link to the RC car
    public var knob				: Transform;			//Holds a link to the controller's knob
    public var body				: Transform;			//Holds a link to the body of the controller
    public var particle			: ParticleSystem;		//Holds the radio wave particle
    
    private var maxKnobMovement : float = 0.11f;		//The max vertical movement the knob is allowed to do
    private var signalStrength	: float;				//Holds the knob pressed percentage [0% .. 100%]
    private var inPlayMode		: boolean;				//The level is in play mode/test mode

    private var knobStartDist	: Vector2;				//The starting distance between the knob and the body
    private var bodyStartPos	: Vector2;				//The starting position of the body

    function Awake()
    {
        knobStartDist = knob.localPosition - body.localPosition;
        bodyStartPos = body.transform.localPosition;
    }

    //Called at a fixed interval
    public function FixedUpdate()
    {
        UpdateKnobPosition();
        CalculateSignalStrength();

        if (inPlayMode)  
            SendSignal();
    }

    //Called when the level is enabled
    public override function Enable()
    {
        inPlayMode = true;

        body.rigidbody2D.gravityScale = gravity;
    }
    //Called when the level is disabled
    public override function Reset()
    {
        inPlayMode = false;

        body.rigidbody2D.gravityScale = 0;
        body.rigidbody2D.velocity = Vector2.zero;

        body.localPosition = bodyStartPos;
        knob.localPosition = bodyStartPos + knobStartDist;
    }

    //Updates the position of the knob
    private function UpdateKnobPosition()
    {
        //Make sure that the knob stays in the right position on the y axis
        knob.transform.localPosition = new Vector2(body.localPosition.x + knobStartDist.x, knob.transform.localPosition.y);

        //If the knob is pushed down by an object, apply upward force on it
        if (knob.localPosition.y - body.localPosition.y < knobStartDist.y)
            knob.rigidbody2D.AddForce(Vector2.up * 5);
        else
        {
            knob.rigidbody2D.velocity = new Vector2(0, 0);
            knob.transform.localPosition = new Vector2(knob.transform.localPosition.x, body.localPosition.y + knobStartDist.y);
        }
    }
    //Calculates signal strength
    private function CalculateSignalStrength()
    {
        signalStrength = Mathf.Abs((knob.localPosition.y - body.localPosition.y) - knobStartDist.y) / (maxKnobMovement / 100);
        
        //If the signal is stronger than 1%, activate emission      
        if (signalStrength > 1)
        {
            particle.enableEmission = true;
        }
        //If the signal is too weak, disable emission
        else
        {
            particle.enableEmission = false;
            signalStrength = 0;
        }
    }
    //Send the signal to the car
    private function SendSignal()
    {
        //If the car object is not set, look for it
        if (car == null)
            car = GameObject.FindObjectOfType(RcCarJS);
        //If we have the car, send the signal strength to it
        else
            car.SetAcceloration(signalStrength);
    }
}