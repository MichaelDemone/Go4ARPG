using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class ScrollingImages : MonoBehaviour {

	public int ScrollSpeed;
	public Image ImagePrefab;
	public int NumberRepeats;
	public BoolReference Playing;
	public bool ForceInt = false;

	public List<Image> Images = new List<Image>();
    private Queue<Image> imQueue = new Queue<Image>();
	private int imageWidth;
	private Vector3 startPosition;
	private Vector3 position;
	private Vector3 roundedPosition;

	private RectTransform _rt;
	private RectTransform rt {
		get {
			if (_rt == null)
				_rt = GetComponent<RectTransform>();
			return _rt;
		}
	}

	// Use this for initialization
	void Start() {
		imageWidth = (int)ImagePrefab.rectTransform.rect.width;
		startPosition = position = rt.localPosition;
        imQueue = new Queue<Image>();
        Images.ForEach(imQueue.Enqueue);
	}

	public void Pause() {
		Playing.Value = false;
	}
	public void Play() {
		Playing.Value = true;
	}

	// Update is called once per frame
	void Update() {
		if (!Playing) return;
		position.x -= Time.deltaTime * ScrollSpeed;
		if (position.x < startPosition.x - imageWidth) {
			Image i = imQueue.Dequeue();
		    i.transform.SetAsLastSibling();
            imQueue.Enqueue(i);
			position.x += imageWidth;
		}

		roundedPosition = position;
		if(ForceInt) roundedPosition.x = Mathf.RoundToInt(position.x);

		rt.localPosition = roundedPosition;
	}

#if UNITY_EDITOR
	[ContextMenu("Clear and Add Images")]
	private void AddImages() {
		ClearImages();
        for (int i = 0; i < NumberRepeats; i++) {
			Images.Add(Instantiate(ImagePrefab, transform));
		}
	}

	[ContextMenu("Clear Images")]
	private void ClearImages() {
		foreach (Image image in Images) {
			if (image == null) continue;
			DestroyImmediate(image.gameObject);
		}
		Images.Clear();
	}

    [ContextMenu("Center Images")]
    private void CenterImages() {
        imageWidth = (int)ImagePrefab.rectTransform.rect.width;
        Vector3 v = transform.localPosition;
        v.x = -1 * (imageWidth * Images.Count) / 2f;
        v.x = Mathf.Round(v.x);
        transform.localPosition = v;
    }
#endif

}
