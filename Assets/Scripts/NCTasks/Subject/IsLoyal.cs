using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Conditions{

	[Category("Subject")]
	public class IsLoyal : ConditionTask<Subject>{

		protected override bool OnCheck(){
			return agent.loyalty >= 0;
		}
	}
}