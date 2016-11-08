package keyboardTester;

import java.io.IOException;
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

		wList.add("testing");
		//add keyboards to test
		KeyboardList.add(buildGridKeyboard());
		KeyboardList.add(buildBinaryKeyboard());
		KeyboardList.add(buildQWERTYKeyboard());
		KeyboardList.add(buildNaiveGridKeyboard());


		for(Node k : KeyboardList){
			int count = 0;
			for(String s : wList){
				for (char ch : s.replaceAll("[^a-zA-Z]", "").toLowerCase().toCharArray()) {
					count += k.getPathLength(String.valueOf(ch),0);
				}
			}
			System.out.printf("Total Path Jumps: %d\n",count);
		}
		//Node qwerty = buildBinaryKeyboard();
		//System.out.println(qwerty.getPathLength("e",0));
		//System.out.println(qwerty.getPathLength("m",0));

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

		Node bb = new Node(new String[] {"a"}, new Node[] {a});
		Node ab = new Node(new String[] {"a","b"}, new Node[] {a,bb});
		Node dd = new Node(new String[] {"c"}, new Node[] {d});
		Node cd = new Node(new String[] {"c","d"}, new Node[] {c,dd});
		Node ff = new Node(new String[] {"f"}, new Node[] {f});
		Node ef = new Node(new String[] {"e","f"}, new Node[] {e,ff});
		Node hh = new Node(new String[] {"h"}, new Node[] {h});
		Node gh = new Node(new String[] {"g","h"}, new Node[] {g,hh});
		Node jj = new Node(new String[] {"j"}, new Node[] {j});
		Node ij = new Node(new String[] {"i","j"}, new Node[] {i,jj});
		Node ll = new Node(new String[] {"l"}, new Node[] {l});
		Node kl = new Node(new String[] {"k","l"}, new Node[] {k,ll});
		Node nn = new Node(new String[] {"n"}, new Node[] {n});
		Node mn = new Node(new String[] {"m","n"}, new Node[] {m,nn});
		Node pp = new Node(new String[] {"p"}, new Node[] {p});
		Node op = new Node(new String[] {"o","p"}, new Node[] {o,pp});
		Node rr = new Node(new String[] {"r"}, new Node[] {r});
		Node qr = new Node(new String[] {"q","r"}, new Node[] {q,rr});
		Node tt = new Node(new String[] {"t"}, new Node[] {t});
		Node st = new Node(new String[] {"s","t"}, new Node[] {s,tt});
		Node vv = new Node(new String[] {"v"}, new Node[] {v});
		Node uv = new Node(new String[] {"u","v"}, new Node[] {u,vv});
		Node xx = new Node(new String[] {"x"}, new Node[] {x});
		Node wx = new Node(new String[] {"w","x"}, new Node[] {w,xx});
		Node zz = new Node(new String[] {"z"}, new Node[] {z});
		Node yz = new Node(new String[] {"y","z"}, new Node[] {y,zz});
		
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


		Node yy = new Node(new String[] {"y"}, new Node[] {y});
		Node ny = new Node(new String[] {"n","y"}, new Node[] {n,yy});
		Node cny = new Node(new String[] {"c","n","y"}, new Node[] {c,ny});
		Node acny = new Node(new String[] {"a","c","n","y"}, new Node[] {a,cny});

		Node uu = new Node(new String[] {"u"}, new Node[] {u});
		Node tu = new Node(new String[] {"t","u"}, new Node[] {t,uu});
		Node ltu = new Node(new String[] {"l","t","u"}, new Node[] {l,tu});
		Node fltu = new Node(new String[] {"f","l","t","u"}, new Node[] {f,ltu});

		Node ss = new Node(new String[] {"s"}, new Node[] {s});
		Node rs = new Node(new String[] {"r","s"}, new Node[] {r,ss});
		Node hrs = new Node(new String[] {"h","r","s"}, new Node[] {h,rs});
		Node bhrs = new Node(new String[] {"b","h","r","s"}, new Node[] {b,hrs});

		Node ww = new Node(new String[] {"w"}, new Node[] {w});
		Node pw = new Node(new String[] {"p","w"}, new Node[] {p,ww});
		Node epw = new Node(new String[] {"e","p","w"}, new Node[] {e,pw});
		Node depw = new Node(new String[] {"d","e","p","w"}, new Node[] {d,epw});

		Node zz = new Node(new String[] {"z"}, new Node[] {z});
		Node mz = new Node(new String[] {"m","z"}, new Node[] {m,zz});
		Node jmz = new Node(new String[] {"j","m","z"}, new Node[] {j,mz});
		Node ijmz = new Node(new String[] {"i","j","m","z"}, new Node[] {i,jmz});
		Node gijmz = new Node(new String[] {"g","i","j","m","z"}, new Node[] {g,ijmz});

		Node xx = new Node(new String[] {"x"}, new Node[] {x});
		Node vx = new Node(new String[] {"v","x"}, new Node[] {v,xx});
		Node qvx = new Node(new String[] {"q","v","x"}, new Node[] {q,vx});
		Node oqvx = new Node(new String[] {"o","q","v","x"}, new Node[] {o,qvx});
		Node koqvx = new Node(new String[] {"k","o","q","v","x"}, new Node[] {k,oqvx});

		Node ffltu = new Node(new String[] {"f","l","t","u"}, new Node[] {fltu});
		Node acnyfltu = new Node(new String[] {"a","c","n","y","f","l","t","u"}, new Node[] {acny,ffltu});
		Node ddepw = new Node(new String[] {"d","e","p","w"}, new Node[] {depw});
		Node bhrsdepw = new Node(new String[] {"b","h","r","s","d","e","p","w"}, new Node[] {bhrs,ddepw});
		Node kkoqvx = new Node(new String[] {"k","o","q","v","x"}, new Node[] {koqvx});
		Node gijmzkoqvx = new Node(new String[] {"g","i","j","m","z","k","o","q","v","x"}, new Node[] {gijmz,kkoqvx});
	
		Node bottomrow = new Node(new String[] {"g","i","j","m","z","k","o","q","v","x"}, new Node[] {gijmzkoqvx});
		Node middlerow = new Node(new String[] {"b","h","r","s","d","e","p","w","g","i","j","m","z","k","o","q","v","x"}, new Node[] {bhrsdepw,bottomrow});
		return new Node(new String[] {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"}, new Node[] {acnyfltu,middlerow});
	}
	
	public static Node buildQWERTYKeyboard(){
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

		Node pp = new Node(new String[] {"p"}, new Node[] {p});
		Node op = new Node(new String[] {"o","p"}, new Node[] {o,pp});
		Node ip = new Node(new String[] {"i","o","p"}, new Node[] {i,op});
		Node up = new Node(new String[] {"u","i","o","p"}, new Node[] {u,ip});
		Node yp = new Node(new String[] {"y","u","i","o","p"}, new Node[] {y,up});
		Node tp = new Node(new String[] {"t","y","u","i","o","p"}, new Node[] {t,yp});
		Node rp = new Node(new String[] {"r","t","y","u","i","o","p"}, new Node[] {r,tp});
		Node ep = new Node(new String[] {"e","r","t","y","u","i","o","p"}, new Node[] {e,rp});
		Node wp = new Node(new String[] {"w","e","r","t","y","u","i","o","p"}, new Node[] {w,ep});
		Node qp = new Node(new String[] {"q","w","e","r","t","y","u","i","o","p"}, new Node[] {q,wp});

		Node ll = new Node(new String[] {"l"}, new Node[] {l});
		Node kl = new Node(new String[] {"k","l"}, new Node[] {k,ll});
		Node jl = new Node(new String[] {"j","k","l"}, new Node[] {j,kl});
		Node hl = new Node(new String[] {"h","j","k","l"}, new Node[] {h,jl});
		Node gl = new Node(new String[] {"g","h","j","k","l"}, new Node[] {g,hl});
		Node fl = new Node(new String[] {"f","g","h","j","k","l"}, new Node[] {f,gl});
		Node dl = new Node(new String[] {"d","f","g","h","j","k","l"}, new Node[] {d,fl});
		Node sl = new Node(new String[] {"s","d","f","g","h","j","k","l"}, new Node[] {s,dl});
		Node al = new Node(new String[] {"a","s","d","f","g","h","j","k","l"}, new Node[] {a,sl});

		Node mm = new Node(new String[] {"m"}, new Node[] {m});
		Node nm = new Node(new String[] {"n","m"}, new Node[] {n,mm});
		Node bm = new Node(new String[] {"b","n","m"}, new Node[] {b,nm});
		Node vm = new Node(new String[] {"v","b","n","m"}, new Node[] {v,bm});
		Node cm = new Node(new String[] {"c","v","b","n","m"}, new Node[] {c,vm});
		Node xm = new Node(new String[] {"x","c","v","b","n","m"}, new Node[] {x,cm});
		Node zm = new Node(new String[] {"z","x","c","v","b","n","m"}, new Node[] {z,xm});
		
		Node bottomrow = new Node(new String[] {"z","x","c","v","b","n","m"}, new Node[] {zm});
		Node middlerow = new Node(new String[] {"z","x","c","v","b","n","m","a","s","d","f","g","h","j","k","l"}, new Node[] {al,bottomrow});
		return new Node(new String[] {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"}, new Node[] {qp,middlerow});
	}
	
	public static Node buildNaiveGridKeyboard(){
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


		Node dd = new Node(new String[] {"d"}, new Node[] {d});
		Node cd = new Node(new String[] {"c","d"}, new Node[] {c,dd});
		Node bcd = new Node(new String[] {"b","c","d"}, new Node[] {b,cd});
		Node abcd = new Node(new String[] {"a","b","c","d"}, new Node[] {a,bcd});

		Node hh = new Node(new String[] {"h"}, new Node[] {h});
		Node gh = new Node(new String[] {"g","h"}, new Node[] {g,hh});
		Node fgh = new Node(new String[] {"f","g","h"}, new Node[] {f,gh});
		Node efgh = new Node(new String[] {"e","f","g","h"}, new Node[] {e,fgh});

		Node ll = new Node(new String[] {"l"}, new Node[] {l});
		Node kl = new Node(new String[] {"k","l"}, new Node[] {k,ll});
		Node jkl = new Node(new String[] {"j","k","l"}, new Node[] {j,kl});
		Node ijkl = new Node(new String[] {"i","j","k","l"}, new Node[] {i,jkl});

		Node pp = new Node(new String[] {"p"}, new Node[] {p});
		Node op = new Node(new String[] {"o","p"}, new Node[] {o,pp});
		Node nop = new Node(new String[] {"n","o","p"}, new Node[] {n,op});
		Node mnop = new Node(new String[] {"m","n","o","p"}, new Node[] {m,nop});

		Node uu = new Node(new String[] {"u"}, new Node[] {u});
		Node tu = new Node(new String[] {"t","u"}, new Node[] {t,uu});
		Node stu = new Node(new String[] {"s","t","u"}, new Node[] {s,tu});
		Node rstu = new Node(new String[] {"r","s","t","u"}, new Node[] {r,stu});
		Node qrstu = new Node(new String[] {"q","r","s","t","u"}, new Node[] {q,rstu});

		Node zz = new Node(new String[] {"z"}, new Node[] {z});
		Node yz = new Node(new String[] {"y","z"}, new Node[] {y,zz});
		Node xyz = new Node(new String[] {"x","y","z"}, new Node[] {x,yz});
		Node wxyz = new Node(new String[] {"w","x","y","z"}, new Node[] {w,xyz});
		Node vwxyz = new Node(new String[] {"v","w","x","y","z"}, new Node[] {v,wxyz});

		Node eefgh = new Node(new String[] {"e","f","g","h"}, new Node[] {efgh});
		Node abcdefgh = new Node(new String[] {"a","b","c","d","e","f","g","h"}, new Node[] {abcd,eefgh});
		Node mmnop = new Node(new String[] {"m","n","o","p"}, new Node[] {mnop});
		Node ijklmnop = new Node(new String[] {"i","j","k","l","m","n","o","p"}, new Node[] {ijkl,mmnop});
		Node vvwxyz = new Node(new String[] {"v","w","x","y","z"}, new Node[] {vwxyz});
		Node qrstuvwxyz = new Node(new String[] {"q","r","s","t","u","v","w","x","y","z"}, new Node[] {qrstu,vvwxyz});
	
		Node bottomrow = new Node(new String[] {"q","r","s","t","u","v","w","x","y","z"}, new Node[] {qrstuvwxyz});
		Node middlerow = new Node(new String[] {"i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"}, new Node[] {ijklmnop,bottomrow});
		return new Node(new String[] {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"}, new Node[] {abcdefgh,middlerow});
	}
}
