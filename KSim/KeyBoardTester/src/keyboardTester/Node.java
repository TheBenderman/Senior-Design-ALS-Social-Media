package keyboardTester;

import java.util.Arrays;
import java.util.Random;

public class Node {
	String[] contents;
	Node[] children = new Node[0];
	Double accuracy = 1.0;
	Random random = new Random();
	
	public Node(String[] contents, Node[] children, Double a) {
		this.accuracy = a;
		this.contents = contents;
		this.children = children;
	}
	
	public Node(String[] contents, Node[] children) {
		this.contents = contents;
		this.children = children;
	}
	
	public Node(String[] contents) {
		this.contents = contents;
		this.children = null;
	}

	public String[] getContents() {
		return contents;
	}

	public void setContents(String[] contents) {
		this.contents = contents;
	}

	public Node[] getChildren() {
		return children;
	}

	public void setChildren(Node[] children) {
		this.children = children;
	}
	
//	//TODO: Add Exception
//	public int getPathLength(String s){
//		int count = 0;
//		Node root = this;
//		Node child = null;
//		int len = root.children.length;
//		for(int i =0; i<len;i++){
//			child = root.children[i];
//			if(child.has(s)){
//				if(child.children == null){
//					return ++count;
//				}
//				root = child;
//				len = root.getChildren().length;
//				i=-1;
//				count++;
//			}
//		}
//		return -1;
//	}
	
	public int getPathLength(String s, int c){
		if(getChildren() == null && this.has(s)){
			return c;
		}
		for(Node child : getChildren()){
			if(child.has(s)){
				return child.getPathLength(s,++c);
			}
		}
		//find longest path in tree based on current node
		//add this to c
		//return 
		return -1;
	}
	
	public boolean has(String s){
		for(String c : this.getContents()){
			if(c.equals(s)){
				return true;
			}
		}
		return false;
	}

	@Override
	public String toString() {
		return "Node [contents=" + Arrays.toString(contents)
				+ ", children=" + Arrays.toString(children) + "]";
	}
}
