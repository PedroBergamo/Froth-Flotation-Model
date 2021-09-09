using System;
using System.Timers;

namespace Process
{
    class Program
    {
        public static FlotationParameters FlotationCell;
        public static float MiliSecondsForNextSampling = 3000;
        public static Timer timer1;

        private static void Sample()
          {
              Console.Clear();
              Console.WriteLine("press any key to stop");
              Console.WriteLine("Feed Cu Grade: " + FlotationCell.FeedCuGrade);
              Console.WriteLine("Feed As Grade: " + FlotationCell.FeedAsGrade);
              Console.WriteLine("Feed Solids: " + FlotationCell.FeedFlowRate);
              Console.WriteLine("Air flow: " + FlotationCell.AirFlow);
              Console.WriteLine("Froth Thickness: " + FlotationCell.FrothThickness);
              Console.WriteLine("Concentrate Cu Recovery: " + FlotationCell.ConcentrateCuRecovery());
              Console.WriteLine("Concentrate Cu Grade: " + FlotationCell.ConcentrateCuGrade());
              Console.WriteLine("Concentrate As Grade: " + FlotationCell.ConcentrateAsGrade());
              Console.WriteLine("Concentrate Mass flow : " + FlotationCell.ConcentrateMassFlowInTPH());
              Console.WriteLine("Tailings Cu Grade: " + FlotationCell.TailingsCuGrade());
              Console.WriteLine("Tailings As Grade: " + FlotationCell.TailingsAsGrade());
              Console.WriteLine("Tailings Mass: " + FlotationCell.TailingsFlowRate());
              Console.WriteLine("Profit per second: " + FlotationCell.ProfitPerSecond());
          }

        static void Main(string[] args)
        {
          FlotationCell = new FlotationParameters();
          FlotationCell.SetInitialVariables();
          timer1 = new Timer();
          timer1.Elapsed += (sender,e) =>
         {
            Sample();
            FlotationParameters.TimeSinceStartUp += MiliSecondsForNextSampling / 1000;
         };
         timer1.Interval = MiliSecondsForNextSampling;
         timer1.Start();
         Console.ReadKey();
     }
    }
}
