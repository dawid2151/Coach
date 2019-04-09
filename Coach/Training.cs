using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach
{
    public class TrainingOverall
    {
        public DateTime TrainingStartDate { get; set; }
        public List<TrainingDay> TrainingDays { get; set; }

        public TrainingOverall()
        {
            TrainingDays = new List<TrainingDay>();
        }
    }

    public class TrainingDay
    {
        public List<TrainingSession> TrainingSessions { get; set; }
        public int SessionsCount { get
            {
                return TrainingSessions.Count;
            } }
        public DateTime TrainingDate { get; set; }
        public TimeSpan Playtime { get
            {
                TimeSpan pt = TimeSpan.Zero;
                foreach (TrainingSession ts in TrainingSessions)
                    pt += ts.Playtime;

                return pt;
            } }
        public int Kills { get
            {
                int k = 0;
                foreach (TrainingSession ts in TrainingSessions)
                    k += ts.Kills;

                return k;
            } }

        public TrainingDay(DateTime trainingDate)
        {
            TrainingSessions = new List<TrainingSession>();
            TrainingDate = trainingDate;
        }
    }

    public class TrainingSession
    {
        private TimeSpan Pltime;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Playtime { get
            {
                return EndTime - StartTime;
            }
            set
            {
                Pltime = value;
            }
        }
        public int Kills { get; set; }
        public string Map { get; set; }

        [JsonConstructor]
        public TrainingSession(DateTime startTime, DateTime endTime, TimeSpan playTime, int kills, string map)
        {
            StartTime = startTime;
            EndTime = endTime;
            Playtime = playTime;
            Kills = kills;
            Map = map;
        }

        public TrainingSession(DateTime startTime, string map)
        {
            CleanData();
            StartTime = startTime;
            Map = map;
        }

        public TrainingSession(DateTime startTime)
        {
            CleanData();
            StartTime = startTime; 
        }

        private void CleanData()
        {
            EndTime = new DateTime(1999, 06, 15);
            Kills = 0;
            Map = "None";
        }

    }
}
