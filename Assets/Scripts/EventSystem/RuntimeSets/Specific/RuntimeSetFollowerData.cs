
using System.Collections.Generic;
using G4AW2.Data;
using UnityEngine;

namespace CustomEvents {
	[UnityEngine.CreateAssetMenu(menuName = "SO Architecture/Runtime Set/Specific/FollowerData")]
	public class RuntimeSetFollowerData : RuntimeSetGenericSaveableWithSaveableAndIID<FollowerData, UnityEventFollowerData> {
    }
}
