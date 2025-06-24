using System;
using System.Windows;
using System.Windows.Controls;

namespace CodeIntroWPF
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Button b = new Button()
                        {
                            Content = "Click Me!"
                        };

            b.Click += (sender, e) =>
            {
                if (b.Content.ToString() == "Click Me!")
                {
                    b.Content = "Clicked !!";
                }
                else
                {
                    b.Content = "Click Me!";
                }
            };

            Window w = new Window()
            {
                Title = "Intro to WPF Code",
                Content = b
            };

            Application a = new Application();
            a.Run(w);
        }
    }
}
