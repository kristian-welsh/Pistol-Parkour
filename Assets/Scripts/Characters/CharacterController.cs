using UnityEngine;

namespace Kristian
{
	public class CharacterController : MonoBehaviour
	{
		[Header("Parkour")]
		public int climbAngleTolerence = 10;
		public int climbSpeed = 5;
		public int climbLength = 2;

		[Header("Movement")]
		public float speed = 20f;
		public float jumpPower = 20f;

		protected CharacterView view;
		protected CharacterMovementModel movement;
		private ParkourModel parkour;

		void Start ()
		{
			view = GetComponent<CharacterView>();
			movement = CreateMovement(view);
			parkour = new ParkourModel(climbAngleTolerence, climbSpeed);
			parkour.movement = movement;
			parkour.view = view;
		}

		protected virtual CharacterMovementModel CreateMovement(CharacterView view)
		{
			return new CharacterMovementModel(view, speed, jumpPower, climbLength);
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
}
