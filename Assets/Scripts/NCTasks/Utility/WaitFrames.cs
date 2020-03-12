using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{

	[Category("✫ Utility")]
	public class WaitFrames : ActionTask
	{

		public BBParameter<int> waitFrames = 1;
		public CompactStatus finishStatus = CompactStatus.Success;

		private int counter;

		protected override string info
		{
			get { return string.Format("Wait {0} frames.", waitFrames); }
		}

		protected override void OnExecute()
		{
			counter = Time.frameCount;
		}

		protected override void OnUpdate()
		{
			if (Time.frameCount - counter >= waitFrames.value)
				EndAction(finishStatus == CompactStatus.Success ? true : false);
		}
	}
}