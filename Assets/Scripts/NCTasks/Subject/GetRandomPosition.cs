using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	[Category("Subject")]
	public class GetRandomPosition : ActionTask<Subject>{

		public BBParameter<float> range;
		[BlackboardOnly]
		public BBParameter<Vector3> position;

		protected override void OnExecute(){
			position.value = agent.transform.position + new Vector3(Random.Range(-range.value, range.value), 0, Random.Range(-range.value, range.value));
			EndAction(true);
		}
	}
}