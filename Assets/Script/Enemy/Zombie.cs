using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public ZombieHand zombieHand;
    public int zombieDamage;

    private void Start()
    {
        zombieHand.Damage = zombieDamage;
    }
}
