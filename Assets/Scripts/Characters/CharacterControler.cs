using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    [Header("Parkour Settings")]
    public int climbAngleTolerence = 10;
    public int climbSpeed = 5;
    public int climbLength = 2;

	private CharacterView view;
	private CharacterMovementModel movement;
	private ParkourModel parkour;

	void Start ()
	{
		view = GetComponent<CharacterView>();
		movement = GetComponent<CharacterMovementModel>();
		parkour = new ParkourModel(climbAngleTolerence, climbSpeed, climbLength);
		parkour.movement = movement;
		parkour.view = view;
	}

	void Update ()
	{
		// user input?
	}

	void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable Gun"))
    		movement.TouchHoveringGun(other.gameObject);
    }

    void FixedUpdate()
    {
    	movement.Recalculate();
    	parkour.ParkourCheck();
    }
}
