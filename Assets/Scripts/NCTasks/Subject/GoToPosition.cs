using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	[Category("Subject")]
	public class GoToPosition : ActionTask<Subject>{

		public BBParameter<Vector3> position;
		public BBParameter<float> minDistance;

		protected override void OnUpdate(){
			if (agent.GoTo(position.value, minDistance.value))
				EndAction(true);
		}
	}
}