using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AglTest
{
    class Program
    {
        
        static void Main(string[] args)
        {

            ListCatsAsync();
            Console.ReadLine();
        }

        /// <summary>
        /// Note - this implementation lacks error/exception handling
        /// </summary>
        /// <returns></returns>
        private static async Task ListCatsAsync()
        {
            var jsonHelper = new Newtonsoft.Json.JsonSerializer();
            var client = new System.Net.Http.HttpClient();
            
            var jsonFormatDefinition = new[] { new { name = "", gender = "", age = 0, pets = new[] { new { name = "", type = "" } } } };
            var rawData = await client.GetStringAsync("http://agl-developer-test.azurewebsites.net/people.json");
            var people = JsonConvert.DeserializeAnonymousType(rawData, jsonFormatDefinition);

            var genderGroups = from person in people
                               where person.pets != null
                               from pets in person.pets
                               where pets.type == "Cat"
                               orderby pets.name
                               group pets by person.gender into genders
                               select genders;

            foreach (var gender in genderGroups)
            {
                Console.WriteLine(gender.Key);

                foreach (var cat in gender)
                {
                    Console.WriteLine(cat.name);
                }

                Console.WriteLine();
            }
        }
    }
}
