using UnityEngine;

public class FPSHealth : MonoBehaviour
{
	public int startingHealth = 100;

	private int health;

	void Start ()
	{
		Reset();
	}

	public void Reset()
	{
		health = startingHealth;
	}

	public void Damage(int amount)
	{
		health -= amount;
		if(health < 0)
		{
			health = 0;
			// todo: trigger death animations etc here
		}
	}
}