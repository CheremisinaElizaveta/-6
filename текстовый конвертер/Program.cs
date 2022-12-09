using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using текстовый_конвертер;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            List<Class> cats = new List<Class>();
            Console.WriteLine("Enter source file path");
            string sourcePath = Console.ReadLine();
            string sourceExt = Path.GetExtension(sourcePath);
            switch (sourceExt)
            {
                case ".txt":
                    using (StreamReader reader = new StreamReader(sourcePath))
                    {
                        while (!reader.EndOfStream)
                        {
                            string name = reader.ReadLine();
                            Console.WriteLine(name);
                            int age = int.Parse(reader.ReadLine());
                            Console.WriteLine(age);
                            cats.Add(new Class (name, age));
                        }
                    }
                    break;
                case ".json":
                    string json = File.ReadAllText(sourcePath);
                    cats = JsonConvert.DeserializeObject<List<Class>>(json);
                    break;
                case ".xml":
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Class>));
                    using (FileStream input = new FileStream(sourcePath, FileMode.Open))
                    {
                        cats = (List<Class>)serializer.Deserialize(input);
                    }
                    break;
            }
            Console.WriteLine("Press F1 to save, escape to exit");
            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.F1)
                {
                    Console.WriteLine("Enter destination file path");
                    string destPath = Console.ReadLine();
                    string destExt = Path.GetExtension(destPath);
                    switch (destExt)
                    {
                        case ".txt":
                            using (StreamWriter writer = new StreamWriter(destPath))
                            {
                                foreach (var cat in cats)
                                {
                                    writer.WriteLine(cat.name);
                                    writer.WriteLine(cat.age);
                                }
                            }
                            Console.WriteLine("Saved plain text");
                            return;
                        case ".json":
                            string result = JsonConvert.SerializeObject(cats);
                            File.WriteAllText(destPath, result);
                            Console.WriteLine("Saved JSON");
                            return;
                        case ".xml":
                            XmlSerializer serializer = new XmlSerializer(typeof(List<Class>));
                            using (FileStream output = new FileStream(destPath, FileMode.Create))
                            {
                                serializer.Serialize(output, cats);
                            }
                            Console.WriteLine("Saved XML");
                            return;
                    }
                }
            } while (key != ConsoleKey.Escape);
        }
    }
}
