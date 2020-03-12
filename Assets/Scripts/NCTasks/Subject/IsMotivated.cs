using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Conditions{

	[Category("Subject")]
	public class IsMotivated : ConditionTask<Subject>{

		protected override bool OnCheck(){
			return agent.Motivation > 0;
		}
	}
}