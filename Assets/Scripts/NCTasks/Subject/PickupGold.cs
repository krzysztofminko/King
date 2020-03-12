using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	[Category("Subject")]
	public class PickupGold : ActionTask<Subject>{

		[RequiredField]
		public BBParameter<Gold> gold;

		protected override void OnExecute()
		{
			if (gold.value)
			{
				Object.Destroy(gold.value.gameObject);
				agent.Motivation++;
				agent.loyalty++;
				EndAction(true);
			}
			else
			{
				EndAction(false);
			}
		}

		protected override void OnUpdate(){
			
		}
	}
}