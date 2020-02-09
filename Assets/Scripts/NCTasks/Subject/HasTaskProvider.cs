using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Conditions{

	[Category("Subject")]
	public class HasTaskProvider : ConditionTask<Subject>{

		[BlackboardOnly]
		public BBParameter<TaskProvider> taskProvider;

		protected override bool OnCheck(){
			return agent.TaskProvider;
		}
	}
}