
using G4AW2.Questing;

namespace CustomEvents.Editor {
		[UnityEditor.CustomPropertyDrawer(typeof(Reference<PassiveQuest, PassiveQuestVariable, UnityEventPassiveQuest>))]
		public class PassiveQuestPropertyDrawer : AbstractReferenceDrawer { }
}
