using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models.MD5
{
    public class MD5Model
    {
        public static MD5Model Instance = new MD5Model();
        
        private MD5Model() {

        }
        public async Task<string> FindWord(string md5)
        {
            var retVal = await Task.Run(() => {
                var passwords = File.ReadAllLines("./Data/passwords.txt");
    
                var md5lib = System.Security.Cryptography.MD5.Create();

                return (from word in passwords let dictHash = string.Concat(md5lib.ComputeHash(System.Text.Encoding.ASCII.GetBytes(word)).Select(x => x.ToString("X2"))) where dictHash == md5.ToUpper() select word).FirstOrDefault();
             
            });
            return retVal;
        }
    }
}
