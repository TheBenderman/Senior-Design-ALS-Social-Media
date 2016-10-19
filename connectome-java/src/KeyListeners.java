import javafx.scene.input.KeyCode;

public class KeyListeners {
	public static void homeMenuKeyPressed (KeyCode keyCode) {
		System.out.println("key pressed: " + keyCode.toString());
		
		switch (keyCode.toString()){
			case "RIGHT" :
			case "LEFT" :
			case "ENTER" :
			case "UP" :
			case "DOWN" :
			default:
				break;
		}
	}
}
