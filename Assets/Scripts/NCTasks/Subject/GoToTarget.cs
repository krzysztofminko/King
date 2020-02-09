using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	[Category("Subject")]
	public class GoToTarget : ActionTask<Subject>{

		public BBParameter<Transform> target;
		public BBParameter<float> minDistance;

		protected override void OnUpdate()
		{
			if (!target.value)
				EndAction(false);
			else if (agent.GoTo(target.value.position, minDistance.value))
				EndAction(true);
		}
	}
}