using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	[Category("Subject")]
	public class GoToPosition : ActionTask<Subject>{

		private bool _run;
		public bool run;
		public BBParameter<Vector3> position;
		public BBParameter<float> minDistance;

		protected override void OnExecute()
		{
			_run = run;
			if (_run)
				agent.SpeedBoost++;
		}

		protected override void OnUpdate(){
			if (agent.GoTo(position.value, minDistance.value))
				EndAction(true);
		}

		protected override void OnStop()
		{
			if (_run)
				agent.SpeedBoost--;
			agent.StopMovement();
		}
	}
}