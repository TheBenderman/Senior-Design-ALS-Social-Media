import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;

public class Facebook {

	public static String facebookAuthURL = "https://www.facebook.com/v2.8/dialog/oauth?client_id=1802938096591520&redirect_uri=https://www.facebook.com/connect/login_success.html&response_type=token";
	
	public static String accessToken = "";
	
	public void getAccessToken(String URL) {
		accessToken = URL.substring(URL.indexOf("#access_token") + 14, URL.indexOf("&", URL.indexOf("#access_token") + 14));
	}
	
	public String getNewsFeed() {
		String newsFeedUrl = "https://graph.facebook.com/me/feed?access_token=" + accessToken;
    	
    	StringBuilder result = new StringBuilder();
        URL url;
        HttpURLConnection conn;
        BufferedReader rd;
        String line;
        
		try {
			url = new URL(newsFeedUrl);
			conn = (HttpURLConnection) url.openConnection();
			conn.setRequestMethod("GET");
			
			rd = new BufferedReader(new InputStreamReader(conn.getInputStream()));
			
			while ((line = rd.readLine()) != null) {
			   result.append(line);
			}
			rd.close();
		} catch (MalformedURLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
        
		
        System.out.println(result.toString());
        
        return result.toString();
	}
}
