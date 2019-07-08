
using G4AW2.Questing;

namespace CustomEvents.Editor {
		[UnityEditor.CustomPropertyDrawer(typeof(Reference<ActiveQuestBase, ActiveQuestBaseVariable, UnityEventActiveQuestBase>))]
		public class ActiveQuestBasePropertyDrawer : AbstractReferenceDrawer { }
}
