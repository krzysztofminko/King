using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	[Category("Subject")]
	public class GoToTarget : ActionTask<Subject>{

		private bool _run;
		public bool run;
		public BBParameter<Transform> target;
		public BBParameter<float> minDistance;

		protected override void OnExecute()
		{
			_run = run;
			if (_run)
				agent.SpeedBoost++;
		}

		protected override void OnUpdate()
		{
			if (!target.value)
				EndAction(false);
			else if (agent.GoTo(target.value.position, minDistance.value))
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