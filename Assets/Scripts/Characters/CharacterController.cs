using UnityEngine;

namespace Kristian
{
	/* Coordinates models & views & agent input to simulate a character
	 * Groups many settings to one convenient place within the unity editor
	 * Recieves Unity engine updates through method calls, protecting models from that depenency
	 */
	public class CharacterController : MonoBehaviour
	{
		[Header("Parkour")]
		// max range of tollerable agent mistake in approach angle when initiating parkour
		public int climbAngleTolerence = 10;
		public int climbSpeed = 5;
		// time in seconds to parkour for before falling
		public int climbLength = 2;

		[Header("Movement")]
		public float speed = 20f;
		public float jumpPower = 20f;

		// displays character & updates physics engine
		protected CharacterView view;
		// model, controls running & jumping
		protected CharacterMovement movement;
		// model, decides when climbing is initiated
		private Parkour parkour;
		// makes decisions about model's actions
		private MovementDecisionAgent agent;
		// needed when initiating manually during respawn to not recieve calls from unity until ready
		private bool initialized = false;

		/* Initialize
		 */
		void Start ()
		{
			Initialize();
		}

		/* Initialize components & subscribe to each other's events, fetch view from object
		 * Needs to be available seperately to start because we instantiate it manually then
		 * immediately call something on it assuming it's initialized already.
		 * Start won't be called in time (or at all? I'm unsure, but this does work).
		 */
		public void Initialize()
		{
			if(initialized)
				return;
			agent = CreateAgent();
			movement = new CharacterMovement(agent, speed, jumpPower);
			parkour = new Parkour(climbAngleTolerence, climbSpeed, climbLength);
			// view is a monobehaviour so it has to be attached in the editor and retrieved like this
			view = GetComponent<CharacterView>();
			movement.RegisterEvents(parkour);
			parkour.RegisterEvents(movement);
			view.RegisterEvents(movement, parkour);
			initialized = true;
		}
		
		/* Agent builder, overriden in subclasses to create the appropriate agent for character type
		 */
		protected virtual MovementDecisionAgent CreateAgent()
		{
			return null;
		}

		/* Collect a gun if you collide with one
		 */
		void OnTriggerEnter(Collider other)
		{
			if(!initialized)
				return;
			if (other.CompareTag("Collectable Gun"))
				movement.TouchHoveringGun(other.gameObject);
		}

		/* Update models based on new physics information
		 */
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
