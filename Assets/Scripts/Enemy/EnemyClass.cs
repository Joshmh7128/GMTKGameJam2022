using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyClass : MonoBehaviour
{
    public int HP;
    public abstract void TakeDamage(int damage);
    public abstract void TakeDamage(int damage, bool knockBack);
}
