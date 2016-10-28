package keyboardTester;

import java.util.Arrays;

public class test {
	public static void main(String[] args){
		Node n = buildBinaryKeyboard();
		int pathLength = n.getPathLength("a");
		//System.out.println(Arrays.toString(n.getContents()));
		System.out.println(n.getChildren()[0].getChildren()[1].has("b"));

		System.out.println(n.getPathLength("a"));
		System.out.println(n.getPathLength("b"));

		System.out.println(n.getPathLength("c"));
		System.out.println(n.getPathLength("d"));

	}
	
	public static Node buildBinaryKeyboard(){
		Node a = new Node(new String[] {"a"});
		Node b = new Node(new String[] {"b"});
		Node c = new Node(new String[] {"c"});
		Node d = new Node(new String[] {"d"});

		Node ab = new Node(new String[] {"a","b"}, new Node[] {a,b});
		Node cd = new Node(new String[] {"c","d"}, new Node[] {c,d});

		Node root = new Node(new String[] {"a","b","c","d"}, new Node[] {ab,cd});
		
		return root;
	}
}
