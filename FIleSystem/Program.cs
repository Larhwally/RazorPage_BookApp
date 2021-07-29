using System;
using System.IO;

namespace FIleSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            string story = "As a programmer, \nyour job is to use and orchestrate each of these resources to solve the problem that you need to solve and analyze the data you get from the solution.\n As a programmer you will mostly be “talking” to the CPU and telling it what to do next. \nSometimes you will tell the CPU to use the main memory, secondary memory, network, or the input/output devices.";
            File.WriteAllText("C:/Users/lawal/Desktop/mystory.txt", story);
            
            string readText = File.ReadAllText("C:/Users/lawal/Desktop/mystory.txt");
            Console.WriteLine(readText);

            //File.Coppy("C:/Users/lawal/Desktop/mystory.txt", "C:/Users/lawal/Desktop/myNewStory.txt");

            //File.Delete("C:/Users/lawal/Desktop/myNewStory.txt");
            //Console.WriteLine("File deleted successfully!");
            File.Replace("C:/Users/lawal/Desktop/mybackupstory.txt", "C:/Users/lawal/Desktop/replacementFile.txt", "C:/Users/lawal/Desktop/mystory.txt");
;
              

        }
    }
}
