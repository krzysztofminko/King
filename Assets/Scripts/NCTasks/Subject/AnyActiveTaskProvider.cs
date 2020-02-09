using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;

namespace NodeCanvas.Tasks.Actions{

	[Category("Subject")]
	public class AnyActiveTaskProvider : ConditionTask<Subject>{

		[BlackboardOnly]
		public BBParameter<TaskProvider> taskProvider;

		protected override bool OnCheck(){
			taskProvider.value = TaskProvider.list.FindAll(t => t.active && (!t.oneSubjectOnly || !t.isReserved)).OrderBy(t => Distance.Manhattan2D(agent.transform.position, t.transform.position)).FirstOrDefault();
			
			return taskProvider.value;
		}
	}
}