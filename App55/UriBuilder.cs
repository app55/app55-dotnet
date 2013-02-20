using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App55 {
    class UriBuilder {
        private static IDictionary<char, bool> RFC2396ReservedCharacters = new Dictionary<char, bool>();
        static UriBuilder() {
            RFC2396ReservedCharacters.Add('!', true);
            RFC2396ReservedCharacters.Add('*', true);
            RFC2396ReservedCharacters.Add('\'', true);
            RFC2396ReservedCharacters.Add('(', true);
            RFC2396ReservedCharacters.Add(')', true);
            RFC2396ReservedCharacters.Add('-', true);
            RFC2396ReservedCharacters.Add('.', true);
            RFC2396ReservedCharacters.Add('_', true);
            RFC2396ReservedCharacters.Add('~', true);

            for (char i = 'a'; i <= 'z'; i++)
                RFC2396ReservedCharacters.Add(i, true);

            for (char i = 'A'; i <= 'Z'; i++)
                RFC2396ReservedCharacters.Add(i, true);

            for (char i = '0'; i <= '9'; i++)
                RFC2396ReservedCharacters.Add(i, true);
        }

        public static string Encode(string s) {
            StringBuilder sb = new StringBuilder(s.Length * 3);
            char[] chars = s.ToCharArray();

            for (int i = 0; i < chars.Length; i++) {
                if (RFC2396ReservedCharacters.ContainsKey(chars[i])) sb.Append(chars[i]);
                else sb.Append(Uri.HexEscape(chars[i]));
            }

            return sb.ToString();
        }
    }
}
