using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Coach
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainProgram mainProgram;
        public TrainingDay selectedTrainingDay { get; set; }
        public TrainingSession selectedTrainingSession { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            RightTab.DataContext = this;
            mainProgram = new MainProgram();
            selectedTrainingDay = mainProgram.TodayTrainingDay;
            DaysList.ItemsSource = mainProgram.OverallTraining.TrainingDays;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainProgram.SaveTrainingToFile();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            Process.GetCurrentProcess().Kill();
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                if (WindowState == WindowState.Normal)
                    WindowState = WindowState.Maximized;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void DaysList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DaysList.SelectedItem == null)
                return;
            selectedTrainingDay = DaysList.SelectedItem as TrainingDay;
            UpdateUIStats();
        }
        private void UpdateUIStats()
        {
            //It's such a bad way to do things... MWM went to sleep i guess
            kills.Content = selectedTrainingDay.Kills.ToString();
            playtime.Content = selectedTrainingDay.Playtime.ToString();
            SessionDataGrid.ItemsSource = selectedTrainingDay.TrainingSessions;
        }


    }
}
