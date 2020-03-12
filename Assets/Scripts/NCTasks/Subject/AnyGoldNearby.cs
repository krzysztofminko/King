using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{

	[Category("Subject")]
	public class AnyGoldNearby : ConditionTask<Subject>
	{

		[BlackboardOnly]
		public BBParameter<Gold> gold;

		protected override bool OnCheck()
		{
			gold.value = Gold.listPickable.OrderBy(g => Distance.Manhattan2D(agent.transform.position, g.transform.position)).FirstOrDefault();

			return gold.value;
		}
	}
}