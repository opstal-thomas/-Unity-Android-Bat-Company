using System;

[Serializable]
public class OptionsSave : SaveObject {

    private float controlSensitivity;

    public OptionsSave() {
        // default values
        // TODO: Move this to a config file
        this.controlSensitivity = 10f;
    }

    public float GetControlSensitivity() {
        return this.controlSensitivity;
    }

    public void SetControlSensitivity(float sensitivity) {
        this.controlSensitivity = sensitivity;
    }
}
