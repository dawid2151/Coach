using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSGSI;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace Coach
{
    public class MainProgram
    {
        public bool IsStartOfCSGO = true;
        public bool HasOpenSession = false;
        public bool IsCSOpened;
        public DateTime TodayDate;
        public DateTime CSGOStartTime;
        public DateTime CSGOTurnOffTime;
        public TrainingSession ActiveTrainnigSession;
        public TrainingDay TodayTrainingDay;
        public TrainingOverall OverallTraining;
        public GameStateListener gameStateListener;
        public CSGOStateControl CSStateControl;

        public MainProgram()
        {
            gameStateListener = new GameStateListener(3000);
            gameStateListener.NewGameState += OnNewGameState;
            gameStateListener.EnableRaisingIntricateEvents = true;

            Task.Run(() =>
            {
                CSStateControl = new CSGOStateControl(this);
            });

            if (!gameStateListener.Start())
                Environment.Exit(-1);

            TodayDate = StandarizeDate(DateTime.Now);
        }

        public void OnNewGameState(GameState gameState)
        {
            if (IsStartOfCSGO)
            {
                CSGOStartTime = DateTime.Now;
                IsStartOfCSGO = false;
                InitializeTrainingData();
            }

            if (CheckIsPlaying(gameState))
            {
                if (ActiveTrainnigSession == null)
                {
                    ActiveTrainnigSession = new TrainingSession(DateTime.Now, gameState.Map.Name);
                    HasOpenSession = true;
                }
                UpdateKills(gameState);

            }
            if(CheckIfShouldEndSession(gameState))
            {
                EndSession();
            }

        }

        public void EndSession()
        {
            ActiveTrainnigSession.EndTime = DateTime.Now;
            TodayTrainingDay.TrainingSessions.Add(ActiveTrainnigSession);
            ActiveTrainnigSession = null;
            HasOpenSession = false;
        }

        public bool CheckIfShouldEndSession(GameState state)
        {
            if (!HasOpenSession)
                return false;

            if (state.Map.Name == "")
                return true;

            return false;
        }

        public void UpdateKills(GameState state)            //TODO new equation on how to calculate kills
        {
            if (state.Provider.SteamID != state.Player.SteamID)     //Player is either dead or observing someone (stats are of the observed one)
                return;
            if (state.Player.MatchStats.Kills == ActiveTrainnigSession.Kills)
                return;

            if (state.Player.MatchStats.Kills > ActiveTrainnigSession.Kills)
            {
                ActiveTrainnigSession.Kills = state.Player.MatchStats.Kills;
                return;
            }
        }

        public bool CheckIsPlaying(GameState state)
        {
            if (state.Map.Name == "")
                return false;

            return true;
        }

        public static DateTime StandarizeDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 12, 0, 0);
        }
        
        public void InitializeTrainingData()
        {
            OverallTraining = new TrainingOverall();
            OverallTraining.TrainingDays = new List<TrainingDay>();
            //Here load TrainingDays from file


            if (OverallTraining.TrainingDays.Count == 0)
            {
                TodayTrainingDay = new TrainingDay(TodayDate);
                //First lunch
            }
            else
                foreach (TrainingDay td in OverallTraining.TrainingDays)  //Check if Today already exist if not create new
                    if (td.TrainingDate == TodayDate)
                    {
                        TodayTrainingDay = td;
                        break;
                    }
                    else
                    {
                        if (TodayTrainingDay == null)
                            TodayTrainingDay = new TrainingDay(TodayDate);
                    }
            LoadTrainingDayFromFile();
        }

        public void LoadOvearallTraining()
        {
            //if(File.Exists(Config.TrainingDataFilePath + Config.TrainingOverallFileName))
                //
        }

        public void LoadTrainingDayFromFile()
        {
            if(File.Exists(Config.GetFilePath(TodayDate)))
            {
                TodayTrainingDay = JsonConvert.DeserializeObject<TrainingDay>(File.ReadAllText(Config.GetFilePath(TodayDate)));
            }
        }

        public void SaveTrainingToFile()
        {
            string json = JsonConvert.SerializeObject(TodayTrainingDay, Formatting.Indented);
            Directory.CreateDirectory(Config.TrainingDataFilePath);
            File.WriteAllText(Config.GetFilePath(TodayDate), json);
            
        }

    }
}
