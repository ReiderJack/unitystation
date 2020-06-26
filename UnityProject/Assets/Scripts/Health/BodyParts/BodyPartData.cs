﻿using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Health
{
	[CreateAssetMenu(fileName = "BodyPartData", menuName = "ScriptableObjects/Health/BodyPartData")]
	public class BodyPartData : ScriptableObject
	{
		[Tooltip("What archetype of body part is this?")]
		public BodyPartType bodyPartType = BodyPartType.Chest;

		[Tooltip("Is this body part different for boys and girls?")]
		public bool sexualDimorphism = false;

		[Tooltip("What's the max amount of damage this limb can resist")]
		public float maxDamage = 200;

		[Tooltip("Can this limb be mangled by damage?")]
		public bool canBeMangled = true;

		[Tooltip("Percentage of current HP this limb must have to be considered mangled")]
		public float mangledThreshold = 50;

		[Tooltip("Armor values for this body part")] [SerializeField]
		private Armor naturalArmor = new Armor();
		public Armor NaturalArmor => naturalArmor;

		[Tooltip("Should this limb be able to be dismembered?")]
		public bool canBeDismembered = true;

		[Tooltip("Damage threshold to roll a dismember event for this limb")]
		public float dismemberThreshold = 30;

		[Tooltip("Chances of this limb to be dismembered when the damage threshold is passed")][Range(0, 100)]
		public int dismemberChance = 0;

		[Tooltip("Can this limb bleed?")]
		public bool canBleed = true;

		[Tooltip("If this limb's brute damage ever gets to this percent of max hp, it will start bleeding")]
		public float bleedThreshold = 10;

		[Tooltip("Does this limb contain inner organs?")]
		public bool containOrgans = false;

		[ShowIf(nameof(containOrgans))]
		[Tooltip("List of organs this limb contains")]
		public List<string> organs; //TODO make organs data to fill this

		[BoxGroup("In game stuff")]
		[Tooltip("What does this body part look like in the mob's body")]
		public SpriteSheetAndData mobSprite;

		[ShowIf(nameof(sexualDimorphism))]
		[BoxGroup("In game stuff")]
		[Tooltip("Sprites for girls")]
		public SpriteSheetAndData mobSpriteFemale = null;

		[ShowIf(nameof(canBeDismembered))]
		[BoxGroup("In game stuff")]
		[Tooltip("How does this limb look when dismembered")]
		public SpriteSheetAndData dismemberedSprite = null;

		[ShowIf(nameof(canBeMangled))]
		[BoxGroup("In game stuff")]
		[Tooltip("How this limb looks mangled")]
		public SpriteSheetAndData mangledSprite = null;

		[BoxGroup("In game stuff")]
		[Tooltip("In game stuff representation of this limb")]
		public GameObject limbGameObject;

		[BoxGroup("In game stuff")]
		[Tooltip("A list of all possible damage sprite ordered from less to more damaged")]
		public List<SpriteSheetAndData> damageOverlay = null;

		[ShowIf(nameof(IsHead))]
		[BoxGroup("In game stuff")] [Tooltip("Special damage sprites for heads!")]
		public SpriteSheetAndData debrained = null;

		[ShowIf(nameof(IsHead))]
		[BoxGroup("In game stuff")] [Tooltip("Special damage sprites for heads!")]
		public SpriteSheetAndData missingEyes = null;


		[BoxGroup("Health monitor state")] [Tooltip("Sprite for blue state")]
		public Sprite blueDamageMonitorIcon = null;

		[BoxGroup("Health monitor state")] [Tooltip("Sprite for green state")]
		public Sprite greenDamageMonitorIcon = null;

		[BoxGroup("Health monitor state")] [Tooltip("Sprite for yellow state")]
		public Sprite yellowDamageMonitorIcon = null;

		[BoxGroup("Health monitor state")] [Tooltip("Sprite for orange state")]
		public Sprite orangeDamageMonitorIcon = null;

		[BoxGroup("Health monitor state")] [Tooltip("Sprite for dark orange state")]
		public Sprite darkOrangeDamageMonitorIcon = null;

		[BoxGroup("Health monitor state")] [Tooltip("Sprite for red state")]
		public Sprite redDamageMonitorIcon = null;

		[BoxGroup("Health monitor state")] [Tooltip("Sprite for gray state")]
		public Sprite grayDamageMonitorIcon = null;

		private bool IsHead => bodyPartType == BodyPartType.Head;
	}
}