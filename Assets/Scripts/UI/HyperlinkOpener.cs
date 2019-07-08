using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class HyperlinkOpener : MonoBehaviour, IPointerClickHandler {

    public Camera cam;
    TextMeshProUGUI ugui;

    void Awake() {
        ugui = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(ugui, Input.mousePosition, cam);
        if(linkIndex != -1) { // was a link clicked?
            TMP_LinkInfo linkInfo = ugui.textInfo.linkInfo[linkIndex];

            // open the link id as a url, which is the metadata we added in the text field
            Application.OpenURL(linkInfo.GetLinkID());
        }
    }
}