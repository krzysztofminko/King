using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	[Category("Subject")]
	public class AnyActiveTaskProvider : ConditionTask<Subject>{

		[BlackboardOnly]
		public BBParameter<TaskProvider> taskProvider;

		protected override bool OnCheck(){
			TaskProvider tp = TaskProvider.listActive.FindAll(t => (!t.oneSubjectOnly || !t.isReserved)).OrderBy(t => Distance.Manhattan2D(agent.transform.position, t.transform.position)).FirstOrDefault();
			taskProvider.value = tp;
			return tp;
		}
	}
}