package keyboardTester;

import java.io.File;
import java.io.IOException;
import java.net.URI;
import java.nio.charset.Charset;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.List;

public class test {
	static ArrayList<Node> KeyboardList = new ArrayList<Node>();
	static List<String> wList = new ArrayList<String>();
	static final double threshhold = 1.5;
	
	public static void main(String[] args){
		//import frequent phrases
		try {
			Path p = Paths.get("./KeyBoardTester/src/resource/", "words.txt");
			Charset charset = Charset.forName("ISO-8859-1");
	    	wList = Files.readAllLines(p, charset);
		} catch (IOException e) {
			e.printStackTrace();
		}
		
		System.out.println("Size of words: "+wList.size());
		//add keyboards to test
		KeyboardList.add(buildGridKeyboard());
		KeyboardList.add(buildBinaryKeyboard());

		for(Node k : KeyboardList){
			int count = 0;
			for(String s : wList){
				for (char ch : s.replaceAll("[^a-zA-Z]", "").toLowerCase().toCharArray()) {
					count += k.getPathLength(String.valueOf(ch));
				}
			}
			System.out.printf("Total Time: %f\n",count*threshhold);
		}
	}
	
	public static Node buildBinaryKeyboard(){
		Node a = new Node(new String[] {"a"});
		Node b = new Node(new String[] {"b"});
		Node c = new Node(new String[] {"c"});
		Node d = new Node(new String[] {"d"});
		Node e = new Node(new String[] {"e"});
		Node f = new Node(new String[] {"f"});
		Node g = new Node(new String[] {"g"});
		Node h = new Node(new String[] {"h"});
		Node i = new Node(new String[] {"i"});
		Node j = new Node(new String[] {"j"});
		Node k = new Node(new String[] {"k"});
		Node l = new Node(new String[] {"l"});
		Node m = new Node(new String[] {"m"});
		Node n = new Node(new String[] {"n"});
		Node o = new Node(new String[] {"o"});
		Node p = new Node(new String[] {"p"});
		Node q = new Node(new String[] {"q"});
		Node r = new Node(new String[] {"r"});
		Node s = new Node(new String[] {"s"});
		Node t = new Node(new String[] {"t"});
		Node u = new Node(new String[] {"u"});
		Node v = new Node(new String[] {"v"});
		Node w = new Node(new String[] {"w"});
		Node x = new Node(new String[] {"x"});
		Node y = new Node(new String[] {"y"});
		Node z = new Node(new String[] {"z"});

		Node ab = new Node(new String[] {"a","b"}, new Node[] {a,b});
		Node cd = new Node(new String[] {"c","d"}, new Node[] {c,d});
		Node ef = new Node(new String[] {"e","f"}, new Node[] {e,f});
		Node gh = new Node(new String[] {"g","h"}, new Node[] {g,h});
		Node ij = new Node(new String[] {"i","j"}, new Node[] {i,j});
		Node kl = new Node(new String[] {"k","l"}, new Node[] {k,l});
		Node mn = new Node(new String[] {"m","n"}, new Node[] {m,n});
		Node op = new Node(new String[] {"o","p"}, new Node[] {o,p});
		Node qr = new Node(new String[] {"q","r"}, new Node[] {q,r});
		Node st = new Node(new String[] {"s","t"}, new Node[] {s,t});
		Node uv = new Node(new String[] {"u","v"}, new Node[] {u,v});
		Node wx = new Node(new String[] {"w","x"}, new Node[] {w,x});
		Node yz = new Node(new String[] {"y","z"}, new Node[] {y,z});
		
		Node abcd = new Node(new String[] {"a","b","c","d"}, new Node[] {ab,cd});
		Node efgh = new Node(new String[] {"e","f","g","h"}, new Node[] {ef,gh});
		Node ijkl = new Node(new String[] {"i","f","k","l"}, new Node[] {ij,kl});
		Node mnop = new Node(new String[] {"m","n","o","p"}, new Node[] {mn,op});
		Node qrst = new Node(new String[] {"q","r","s","t"}, new Node[] {qr,st});
		Node uvwx = new Node(new String[] {"u","v","w","x"}, new Node[] {uv,wx});

		Node abcdefgh = new Node(new String[] {"a","b","c","d","e","f","g","h"}, new Node[] {abcd,efgh});
		Node ijklmnop = new Node(new String[] {"i","j","k","l","m","n","o","p"}, new Node[] {ijkl,mnop});
		Node qrstuvwx = new Node(new String[] {"q","r","s","t","u","v","w","x"}, new Node[] {qrst,uvwx});

		Node abcdefghijklmnop = new Node(new String[] {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p"}, new Node[] {abcdefgh,ijklmnop});
		Node qrstuvwxyz = new Node(new String[] {"q","r","s","t","u","v","w","x","y","z"}, new Node[] {qrstuvwx,yz});
		
		return new Node(new String[] {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"}, new Node[] {abcdefghijklmnop,qrstuvwxyz});
	}
	
	public static Node buildGridKeyboard(){
		Node a = new Node(new String[] {"a"});
		Node b = new Node(new String[] {"b"});
		Node c = new Node(new String[] {"c"});
		Node d = new Node(new String[] {"d"});
		Node e = new Node(new String[] {"e"});
		Node f = new Node(new String[] {"f"});
		Node g = new Node(new String[] {"g"});
		Node h = new Node(new String[] {"h"});
		Node i = new Node(new String[] {"i"});
		Node j = new Node(new String[] {"j"});
		Node k = new Node(new String[] {"k"});
		Node l = new Node(new String[] {"l"});
		Node m = new Node(new String[] {"m"});
		Node n = new Node(new String[] {"n"});
		Node o = new Node(new String[] {"o"});
		Node p = new Node(new String[] {"p"});
		Node q = new Node(new String[] {"q"});
		Node r = new Node(new String[] {"r"});
		Node s = new Node(new String[] {"s"});
		Node t = new Node(new String[] {"t"});
		Node u = new Node(new String[] {"u"});
		Node v = new Node(new String[] {"v"});
		Node w = new Node(new String[] {"w"});
		Node x = new Node(new String[] {"x"});
		Node y = new Node(new String[] {"y"});
		Node z = new Node(new String[] {"z"});

		Node acny = new Node(new String[] {"a","c","n","y"}, new Node[] {a,c,n,y});
		Node fltu = new Node(new String[] {"f","l","t","u"}, new Node[] {f,l,t,u});
		Node bhrs = new Node(new String[] {"b","h","r","s"}, new Node[] {b,h,r,s});
		Node depw = new Node(new String[] {"d","e","p","w"}, new Node[] {d,e,p,w});
		Node gijmz = new Node(new String[] {"g","i","j","m","z"}, new Node[] {g,i,j,m,z});
		Node koqvx = new Node(new String[] {"k","o","q","v","x"}, new Node[] {k,o,q,v,x});

		Node acnyfltu = new Node(new String[] {"a","c","n","y","f","l","t","u"}, new Node[] {acny,fltu});
		Node bhrsdepw = new Node(new String[] {"b","h","r","s","d","e","p","w"}, new Node[] {bhrs,depw});
		Node gijmzkoqvx = new Node(new String[] {"g","i","j","m","z","k","o","q","v","x"}, new Node[] {gijmz,koqvx});
	
		return new Node(new String[] {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"}, new Node[] {acnyfltu,bhrsdepw,gijmzkoqvx});
	}
}
