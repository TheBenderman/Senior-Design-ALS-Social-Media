import javafx.application.Application;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.input.KeyEvent;
import javafx.scene.layout.HBox;
import javafx.stage.Stage;

public class connectome extends Application{

	@Override
	public void start(Stage primaryStage) throws Exception {
		primaryStage.setTitle("Connectome");
		
		Button button1 = new Button("Facebook");
        Button button2 = new Button("Twitter");
        Button button3 = new Button("Instagram");
        Button button4 = new Button("Settings");
        
        HBox hbox = new HBox(button1, button2, button3, button4);

        Scene scene = new Scene(hbox, 800, 600);
        
        scene.addEventFilter(KeyEvent.KEY_PRESSED, event -> KeyListeners.homeMenuKeyPressed(event.getCode()));
        
        primaryStage.setScene(scene);

        primaryStage.show();
	}
	
	public static void main(String[] args) {
        Application.launch(args);
    }
}
