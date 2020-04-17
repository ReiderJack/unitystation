using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Light2D;
using Mirror;
using UnityEngine;

[ExecuteInEditMode]
public class LightSourceV2 : ObjectTrigger
{
	private const LightState InitialState = LightState.Off;

	[Header("Generates itself if this is null:")]
	public GameObject mLightRendererObject;

	private LightState mState;

	public LightState lightState => mState;
	private SpriteRenderer Renderer;
	private float fullIntensityVoltage = 240;
	public float Resistance = 1200;
	private float _intensity;
	private LightMountStatesV2 wallMount;

	/// <summary>
	/// Current intensity of the lights, automatically clamps and updates sprites when set
	/// </summary>
	private float Intensity
	{
		get { return _intensity; }
		set
		{
			value = Mathf.Clamp(value, 0, 1);
			if (_intensity != value)
			{
				_intensity = value;
				OnIntensityChange();
			}
		}
	}
	public APC RelatedAPC;

	public LightSwitch relatedLightSwitch;
	public Color customColor; //Leave null if you want default light color.

	private LightState State
	{
		get { return mState; }

		set
		{
			if (mState == value)
				return;

			mState = value;

			OnStateChange(value);
		}
	}

	public override void Trigger(bool iState)
	{
		State = iState ? LightState.On : LightState.Off;
	}

	private void OnIntensityChange()
	{
		//we were getting an NRE here internally in GetComponent so this checks if the object lifetime
		//is up according to Unity
		if (this == null) return;
		var lightSprites = GetComponentInChildren<LightSprite>();
		if (lightSprites)
		{
			lightSprites.Color.a = Intensity;
		}
	}

	private void OnStateChange(LightState iValue)
	{
		// Switch Light renderer.
		if (mLightRendererObject != null)
			mLightRendererObject.SetActive(iValue == LightState.On);
	}

	public void PowerLightIntensityUpdate(float Voltage)
	{
		if (State == LightState.Off)
		{
			//RelatedAPC.ListOfLights.Remove(this);
			//RelatedAPC = null;
		}
		else
		{
			// Intensity clamped between 0 and 1, and sprite updated automatically with custom get set
			Intensity = Voltage / fullIntensityVoltage;
		}
	}

	private void Awake()
	{
		if (!Application.isPlaying)
		{
			return;
		}

		Renderer = GetComponentInChildren<SpriteRenderer>();

		if (mLightRendererObject == null)
		{
			mLightRendererObject = LightSpriteBuilder.BuildDefault(gameObject, new Color(0, 0, 0, 0), 12);
		}

		wallMount = GetComponent<LightMountStatesV2>();

		State = InitialState;

		GetComponent<Integrity>().OnWillDestroyServer.AddListener(OnWillDestroyServer);
	}

	private void OnWillDestroyServer(DestructionInfo arg0)
	{
		Spawn.ServerPrefab("GlassShard", gameObject.TileWorldPosition().To3Int(), transform.parent, count: 2,
			scatterRadius: Spawn.DefaultScatterRadius, cancelIfImpassable: true);
	}

#if UNITY_EDITOR
	void Update()
	{
		if (!Application.isPlaying)
		{
			if (gameObject.tag == "EmergencyLight")
			{
				if (RelatedAPC == null)
				{
					Logger.LogError("EmergencyLight is missing APC reference, at " + transform.position,
						Category.Electrical);
					RelatedAPC.Current =
						1; //so It will bring up an error, you can go to click on to go to the actual object with the missing reference
				}
			}

			return;
		}
	}
#endif

	void Start()
	{
		if (!Application.isPlaying)
		{
			return;
		}

		Color _color;

		if (customColor == new Color(0, 0, 0, 0))
		{
			_color = new Color(0.7264151f, 0.7264151f, 0.7264151f, 0.8f);
		}
		else
		{
			_color = customColor;
		}

		mLightRendererObject.GetComponent<LightSprite>().Color = _color;
	}

	public void SubscribeToSwitch(ref Action<bool> triggerEvent)
	{
		Debug.Log("Light source is subscribed");
		triggerEvent += onSwitchTrigger;
	}

	public void onSwitchTrigger(bool isOn)
	{
		if (wallMount.State == LightMountStatesV2.LightMountState.Broken ||
		    wallMount.State == LightMountStatesV2.LightMountState.MissingBulb)
		{
			return;
		}
		wallMount.SwitchChangeState(isOn ? LightState.On : LightState.Off);
		Trigger(isOn);
	}

}
