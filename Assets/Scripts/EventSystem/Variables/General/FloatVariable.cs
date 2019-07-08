namespace CustomEvents {
    [UnityEngine.CreateAssetMenu(menuName = "SO Architecture/Variable/General/Float")]
	public class FloatVariable : Variable<float, UnityEventFloat> {

	    public void Add(float other) {
		    Value += other;
	    }

	    public void Subtract(float other) {
		    Value -= other;
	    }
	}
}
