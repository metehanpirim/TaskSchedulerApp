using System;
using TaskSchedulerApp.Initializers;
using TaskSchedulerApp.Utilities;

class Program
{
    static void Main(string[] args)
    {
        // Initialize the application
        var appInitializer = new AppInitializer();

        // Initialize the menu handler
        var menuHandler = new MenuHandler(appInitializer);

        // Show the main menu
        menuHandler.ShowMainMenu();
    }
}
