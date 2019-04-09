using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach
{
    public static class Config
    {
        public static string TrainingDataFilePath { get; set; } = @"C:\Training\";

        //As it's one file for every day, then date can be a name of file

        public static string GetFilePath(DateTime Today)
        {
            return TrainingDataFilePath + Today.ToString("dd-MM-yyyy") + ".prac";
        }
    }
}
