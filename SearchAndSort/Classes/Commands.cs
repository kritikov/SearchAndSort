using System.Windows.Input;

namespace SearchAndSort.Classes
{
    public class Commands
    {
        public static readonly RoutedUICommand NewState = new RoutedUICommand(
            "new state",
            "new state",
            typeof(Commands)
        );

        public static readonly RoutedUICommand RandomState = new RoutedUICommand(
            "random state",
            "random state",
            typeof(Commands)
        );

        public static readonly RoutedUICommand UCSAnalysis = new RoutedUICommand(
            "UCS Analysis",
            "UCS Analysis",
            typeof(Commands)
        );

        public static readonly RoutedUICommand ASTARAnalysis = new RoutedUICommand(
            "A* Analysis",
            "A* Analysis",
            typeof(Commands)
        );

        public static readonly RoutedUICommand StopAnalysis = new RoutedUICommand(
            "stop",
            "stop",
            typeof(Commands)
        );
    }
}
