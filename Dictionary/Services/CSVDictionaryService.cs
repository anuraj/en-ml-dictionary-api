using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using Dictionary.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dictionary.Services
{
    public class CSVDictionaryService : IDictionaryService
    {
        private readonly string CSVFileName = "EnglishMalayalam.csv";
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly List<Definition> _definitions;
        public CSVDictionaryService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            var database = Path.Combine(_hostingEnvironment.ContentRootPath, "AppData", CSVFileName);
            using (var streamReader = new StreamReader(database))
            {
                _definitions = new List<Definition>();
                var line = streamReader.ReadLine(); //Skipping the first line since it has headers
                line = streamReader.ReadLine();
                while (line != null)
                {
                    var data = line.Split("\t");
                    
                    _definitions.Add(new Definition()
                    {
                        Id = int.Parse(data[0]),
                        EnglishWord = data[1],
                        PartOfSpeech = data[2],
                        MalayalamDefinition = data[3].Replace("\u200d","").Replace("\u200c","")
                    });

                    line = streamReader.ReadLine();
                }
            }
        }
        public async Task<IEnumerable<Definition>> FindMeaning(string text)
        {
            var results = _definitions.Where
                (x => x.EnglishWord.Equals(text, StringComparison.OrdinalIgnoreCase));
            return results;
        }
    }
}