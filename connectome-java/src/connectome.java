import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.UnsupportedEncodingException;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.ProtocolException;
import java.net.URL;
import java.net.URLDecoder;
import java.util.LinkedHashMap;
import java.util.Map;

import javafx.application.Application;
import javafx.beans.value.ChangeListener;
import javafx.beans.value.ObservableValue;
import javafx.concurrent.Worker.State;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.input.KeyEvent;
import javafx.scene.layout.HBox;
import javafx.scene.layout.VBox;
import javafx.scene.web.WebEngine;
import javafx.scene.web.WebView;
import javafx.stage.Stage;

public class connectome extends Application{

	@Override
	public void start(Stage primaryStage) throws Exception {
		/*primaryStage.setTitle("Connectome");
		
		Button button1 = new Button("Facebook");
        Button button2 = new Button("Twitter");
        Button button3 = new Button("Instagram");
        Button button4 = new Button("Settings");
        
        HBox hbox = new HBox(button1, button2, button3, button4);

        Scene scene = new Scene(hbox, 800, 600);
        
        scene.addEventFilter(KeyEvent.KEY_PRESSED, event -> KeyListeners.homeMenuKeyPressed(event.getCode()));
        
        primaryStage.setScene(scene);

        primaryStage.show();*/
		
		// Create the WebView
		WebView webView = new WebView();
		
		// Create the WebEngine
		final WebEngine webEngine = webView.getEngine();

		// LOad the Start-Page
		webEngine.load(Facebook.facebookAuthURL);
		
		// Update the stage title when a new web page title is available
		webEngine.getLoadWorker().stateProperty().addListener(new ChangeListener<State>() 
		{
            public void changed(ObservableValue<? extends State> ov, State oldState, State newState) 
            {
                if (newState == State.SUCCEEDED) 
                {
                    //stage.setTitle(webEngine.getLocation());
                	primaryStage.setTitle(webEngine.getTitle());
                }
            
                String accessToken = "";
                if (webEngine.getLocation().toLowerCase().contains("#access_token")){
                	Facebook facebook = new Facebook();
                	facebook.getAccessToken(webEngine.getLocation());
                	
                	System.out.println(Facebook.accessToken);
                	
                	String newsFeed = facebook.getNewsFeed();
                }
            }
        });

		// Create the VBox
		VBox root = new VBox();
		// Add the WebView to the VBox
		root.getChildren().add(webView);

		// Set the Style-properties of the VBox
		root.setStyle("-fx-padding: 10;" +
				"-fx-border-style: solid inside;" +
				"-fx-border-width: 2;" +
				"-fx-border-insets: 5;" +
				"-fx-border-radius: 5;" +
				"-fx-border-color: blue;");

		// Create the Scene
		Scene scene = new Scene(root);
		// Add  the Scene to the Stage
		primaryStage.setScene(scene);
		// Display the Stage
		primaryStage.show();
	}
	
	public static void main(String[] args) {
        Application.launch(args);
    }
}
