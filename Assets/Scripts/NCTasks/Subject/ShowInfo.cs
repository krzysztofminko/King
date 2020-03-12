using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions{

	[Category("Subject")]
	public class ShowInfo : ActionTask<Subject>{

		protected override void OnExecute(){
			agent.ShowInfo();
			EndAction(true);
		}
	}
}