using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TypingController {
    /*
    * Calculate the distance between two strings, going to a maximum depth
    * Use the overloaded method for automatically calculating an appropriate
    * maximum depth
    */
    public static int Levenshtein(string l, string r, int maxD) {
        if (maxD <= 0) {
            return 100;
        }
        
        // if l is empty then the distance to r is the length of r
        if (string.IsNullOrEmpty(l)) {
            return r.Length;
        }

        // if r is empty then the distance to l is the length of l
        if (string.IsNullOrEmpty(r)) {
            return l.Length;
        }

        // if the heads are equal we can strip them
        if (r[0] == l[0]) {
            return Levenshtein(l.Substring(1), r.Substring(1));
        }

        // we take the minimum of stripping the first character each as well as stripping the first two
        // abc, def
        int[] rec = {
            // L(bc, def)
            Levenshtein(l.Substring(1), r, maxD - 1),
            // L(abc, ef)
            Levenshtein(l, r.Substring(1), maxD - 1),
            // L(bc, ef)
            Levenshtein(l.Substring(1), r.Substring(1), maxD - 1)
        };

        // add one as we made a change
        return 1 + rec.Min();
    }
    
    public static int Levenshtein(string l, string r) {
        return Levenshtein(l, r, 7);
    }
}
