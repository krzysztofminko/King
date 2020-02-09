using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	[Category("Subject")]
	public class Gather : ActionTask<Subject>{

		public BBParameter<Gatherable> gatherable;
		private float timer;

		protected override void OnExecute(){
			timer = 0;
			agent.StartCoroutine(agent.PlayAnimation(gatherable.value.animation, gatherable.value.duration));
		}

		protected override void OnUpdate()
		{
			if (!gatherable.value)
			{
				EndAction(false);
			}
			else
			{
				timer += Time.deltaTime;
				if (timer > gatherable.value.duration)
				{
					Resource.list[gatherable.value.resource] += gatherable.value.count;
					Object.Destroy(gatherable.value.gameObject);
					agent.AssignTaskProvider(null);
					EndAction(true);
				}
			}
		}
	}
}