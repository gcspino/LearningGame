using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningGame.GUI
{
    public class ResponseResources
    {
        Dictionary<string, List<ResponseUIPair>> Resources;
        Random rnd = new Random();
        const string IMAGE_EXT = ".jpg";

        public ResponseResources(string initPath, List<string> categories)
        {
            Resources = new Dictionary<string, List<ResponseUIPair>>();

            foreach(string category in categories)
            {
                List<ResponseUIPair> Responses = new List<ResponseUIPair>();

                List<string> files = Directory.EnumerateFiles(initPath)
                    .Where(c=>c.ToLower().Contains(category) && c.ToLower().Contains(IMAGE_EXT))
                    .Select(f=>f.ToLower())
                    .ToList();

                foreach (string file in files)
                {
                    ResponseUIPair response = new ResponseUIPair(file, file.Replace(IMAGE_EXT, ".wav"));
                    Responses.Add(response);
                }

                Resources.Add(category.ToLower(), Responses);
            }
        }

        internal ResponseUIPair GetResponse(string Category)
        {
            string categoryLower = Category.ToLower();

            if (Resources.ContainsKey(categoryLower))
            {
                List<ResponseUIPair> categoryList = Resources[categoryLower];

                if (categoryList.Any())
                {
                    int index = rnd.Next(0, categoryList.Count());
                    return categoryList[index];
                }
            }

            return null;

        }

    }
}
