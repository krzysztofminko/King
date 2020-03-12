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

			agent.GetComponent<Animator>().Play(gatherable.value.animation, 0);
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
				if (timer > gatherable.value.duration / (agent.SpeedBoost > 0 ? 2 : 1))
				{
					agent.GetComponent<Animator>().SetTrigger("stop");
					Resource.list[gatherable.value.resource] += gatherable.value.count;
					Object.Destroy(gatherable.value.gameObject);
					agent.AssignTaskProvider(null);
					EndAction(true);
				}
			}
		}
	}
}