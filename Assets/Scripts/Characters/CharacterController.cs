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
		private MovementDecisionAgent agent;

		private bool initialized = false;

		void Start ()
		{
			Initialize();
		}

		/* Needs to be available seperately to start because we instantiate it manually then
		 * immediately call something on it assuming it's initialized already. Start won't do that.
		 * Consider re-writing RespawnManager to resolve this.
		 */
		public void Initialize()
		{
			if(initialized)
				return;
			agent = CreateAgent();
			movement = new CharacterMovement(agent, speed, jumpPower);
			parkour = new Parkour(climbAngleTolerence, climbSpeed, climbLength);
			view = GetComponent<CharacterView>();
			movement.RegisterEvents(parkour);
			parkour.RegisterEvents(movement);
			view.RegisterEvents(movement, parkour);
			initialized = true;
		}

		protected virtual MovementDecisionAgent CreateAgent()
		{
			return null;
		}

		void OnTriggerEnter(Collider other)
		{
			if(!initialized)
				return;
			if (other.CompareTag("Collectable Gun"))
				movement.TouchHoveringGun(other.gameObject);
		}

		void FixedUpdate()
		{
			if(!(initialized && view.initialized))
				return;
			movement.Recalculate(view.Velocity, view.GetTransform.position, view.GetTransform.forward);
			agent.Recalculate(view.Velocity, view.GetTransform.position);
			parkour.ParkourCheck(view.GetTransform.forward, view.GetTransform.position, view.Velocity, movement.HasClimbed, movement.Grounded);
		}
	}
}
