using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistralMessageHistory {
    internal class MessageHistory {
        public static async Task AddPair() {
            string docPath = "./";//Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using StreamWriter outputFile = new(Path.Combine(docPath, "WriteTextAsync.txt"));
            await outputFile.WriteAsync("This is a sentence.");

        }
    }
};
