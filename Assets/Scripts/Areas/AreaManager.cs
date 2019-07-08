using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.Area;
using G4AW2.Followers;
using G4AW2.Questing;
using UnityEngine;
using UnityEngine.UI;

namespace G4AW2.UI.Areas {
	public class AreaManager : MonoBehaviour {
		public Area Area;

		public ScrollingImages backgrounds;
		public ScrollingImages clouds1;
		public ScrollingImages clouds2;
		public Image[] SkyImages;

		public void SetArea( Area area ) {
			this.Area = area;

            backgrounds.Images.ForEach(im => im.sprite = area.Background);
			clouds1.Images.ForEach(i => i.sprite = area.Clouds1);
			clouds2.Images.ForEach(i => i.sprite = area.Clouds2);
			SkyImages.ForEach(i => i.sprite = area.Sky);
		}

		// Must do this due to Unity events... Is there a better way to do this?
		public void SetAreaFromQuest(ActiveQuestBase quest) {
            if(quest.Area != Area)
			    SetArea(quest.Area);
        }

	    [ContextMenu("Set Area")]
	    public void SetArea() {
	        SetArea(Area);
	    }
	}

}

