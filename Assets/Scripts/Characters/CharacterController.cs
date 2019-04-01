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
		protected CharacterMovement movement;
		private Parkour parkour;

		void Start ()
		{
			movement = CreateMovement();
			view = GetComponent<CharacterView>();
			view.RegisterEvents(movement);
			parkour = new Parkour(climbAngleTolerence, climbSpeed);
			parkour.movement = movement;
		}

		protected virtual CharacterMovement CreateMovement()
		{
			return new CharacterMovement(speed, jumpPower, climbLength);
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Collectable Gun"))
				movement.TouchHoveringGun(other.gameObject);
		}

		void FixedUpdate()
		{
			movement.Recalculate(view.Velocity, view.GetTransform.position, view.GetTransform.forward);
			ParkourResult parkourResult = parkour.ParkourCheck(view.GetTransform.forward, view.GetTransform.position, view.Velocity, movement.HasClimbed, movement.Grounded);
			if(parkourResult != null)
				movement.StartParkour(parkourResult.normal, parkourResult.velocity);
		}
	}
}
