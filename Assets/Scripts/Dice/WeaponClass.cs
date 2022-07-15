using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponClass : MonoBehaviour
{
    /// <summary>
	/// Main action of the weapon.
	/// </summary>
    public abstract void Attack();

	[SerializeField, Tooltip("Amount of ammo or durability the weapon has.")]
	/// <summary>
	/// Amount of ammo or durability the weapon has.
	/// </summary>
	protected int uses;
	public int Uses {get {return uses;} protected set {uses = value;}}
}
