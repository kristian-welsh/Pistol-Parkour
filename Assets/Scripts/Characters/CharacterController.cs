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
		private ParkourModel parkour;

		void Start ()
		{
			view = GetComponent<CharacterView>();
			movement = CreateMovement();
			parkour = new ParkourModel(climbAngleTolerence, climbSpeed);
			parkour.movement = movement;

			view.movement = movement;
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
			parkour.ParkourCheck(view.GetTransform.forward, view.GetTransform.position, view.Velocity);
		}
	}
}
