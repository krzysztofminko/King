using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions{

	[Category("Subject")]
	public class AssignTaskProvider : ActionTask<Subject>{

		public BBParameter<TaskProvider> taskProvider;

		protected override void OnExecute(){
			EndAction(agent.AssignTaskProvider(taskProvider.value));
		}
	}
}