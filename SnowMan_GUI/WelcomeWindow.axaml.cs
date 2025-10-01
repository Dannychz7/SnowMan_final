using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;

namespace SnowMan_GUI;

public partial class WelcomeWindow : Window
{
    public WelcomeWindow()
    {
        InitializeComponent();
        DrawSnowflakes();
    }

    private void DrawSnowflakes(int count = 30)
    {
        Random rnd = new Random();
        for (int i = 0; i < count; i++)
        {
            Ellipse snowflake = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.White,
                Opacity = rnd.NextDouble() * 0.8 + 0.2
            };
            Canvas.SetLeft(snowflake, rnd.Next(0, 380));
            Canvas.SetTop(snowflake, rnd.Next(0, 580));
            SnowflakeCanvas.Children.Add(snowflake);
        }
    }
    
    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }
}
