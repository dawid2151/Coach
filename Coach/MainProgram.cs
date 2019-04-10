using System;
using System.Windows;
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
            InitializeTrainingData();
        }

        public void OnNewGameState(GameState gameState)
        {
            if (IsStartOfCSGO)
            {
                CSGOStartTime = DateTime.Now;
                IsStartOfCSGO = false; 
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
            if (ActiveTrainnigSession == null)
                return;

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
            LoadOvearallTraining();

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
        }

        public void LoadOvearallTraining()
        {
            if (File.Exists(Config.TrainingDataFilePath + Config.TrainingOverallFileName))
                OverallTraining = new TrainingOverall(DateTime.Parse(File.ReadAllText(Config.TrainingDataFilePath + Config.TrainingOverallFileName)));
            else
            {
                OverallTraining = new TrainingOverall(TodayDate.Date);
                //Training conf file not found
            }

            for(DateTime date = OverallTraining.TrainingStartDate; date.Date <= TodayDate.Date; date = date.AddDays(1))
            {
                if (File.Exists(Config.GetFilePath(date)))
                {
                    OverallTraining.TrainingDays.Add(JsonConvert.DeserializeObject<TrainingDay>(File.ReadAllText(Config.GetFilePath(date))));
                }
            }

        }

        public void SaveOverallTraining()
        {
            if (!File.Exists(Config.TrainingDataFilePath + Config.TrainingOverallFileName))
            {
                Directory.CreateDirectory(Config.TrainingDataFilePath);
                File.WriteAllText(Config.TrainingDataFilePath + Config.TrainingOverallFileName, OverallTraining.TrainingStartDate.ToString());
            }
        }

        public void SaveTrainingToFile()
        {
            try
            {
                EndSession();
                SaveOverallTraining();
                if (TodayTrainingDay.TrainingSessions.Count != 0)
                {
                    string json = JsonConvert.SerializeObject(TodayTrainingDay, Formatting.Indented);
                    Directory.CreateDirectory(Config.TrainingDataFilePath);
                    File.WriteAllText(Config.GetFilePath(TodayDate), json);
                }
                MessageBox.Show("Saved data to " + Config.TrainingDataFilePath, "Success!");
            } catch (Exception e)
            {
                MessageBox.Show(e.Message,"Problem occured...");
            }
        }

    }
}
