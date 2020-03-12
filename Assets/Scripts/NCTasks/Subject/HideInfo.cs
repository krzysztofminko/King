using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions{

	[Category("Subject")]
	public class HideInfo : ActionTask<Subject>{

		protected override void OnExecute()
		{
			agent.HideInfo();
			EndAction(true);
		}
	}
}