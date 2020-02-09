using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Conditions{

	[Category("TaskProvider")]
	public class IsGatherable : ConditionTask<TaskProvider>{

		[BlackboardOnly]
		public BBParameter<Gatherable> gatherable;
		
		protected override bool OnCheck(){
			return agent.GetComponent<Gatherable>();
		}
	}
}