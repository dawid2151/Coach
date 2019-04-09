using System;

namespace Coach
{
    public static class Config
    {
        //Training files(.prac) are like normal .txt files. Extension is just for recognition

        public const string TrainingOverallFileName = "TrainingFile.prac";

        public static string TrainingDataFilePath { get; set; } = @"C:\Training\";

        //As it's one file for every day, then date can be a name of file

        public static string GetFilePath(DateTime Today)
        {
            return TrainingDataFilePath + Today.ToString("dd-MM-yyyy") + ".prac";
        }
    }
}
