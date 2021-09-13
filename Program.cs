using System;
using System.Timers;

namespace Process
{
    class Program
    {
        public static FlotationParameters FlotationCell;
        public static float MiliSecondsForNextSampling = 1000;
        public static Timer timer;

        private static void Sample()
          {
              Console.Clear();
              Console.WriteLine("-----SIMULATION-----");
              Console.WriteLine("<Esc> to stop simulation");
              Console.WriteLine("-----FEED-----");
              Console.WriteLine("Feed Cu Grade: " + FlotationCell.FeedCuGrade);
              Console.WriteLine("Feed As Grade: " + FlotationCell.FeedAsGrade);
              Console.WriteLine("Feed Solids: " + FlotationCell.FeedFlowRate);
              Console.WriteLine("-----FLOTATION CELL-----");
              Console.WriteLine("<A> to set air flow");
              Console.WriteLine("<F> to set froth thickness");
              Console.WriteLine("Air flow: " + FlotationCell.AirFlow);
              Console.WriteLine("Froth Thickness: " + FlotationCell.FrothThickness);
              Console.WriteLine("-----CONCENTRATE STREAM-----");
              Console.WriteLine("Concentrate Cu Recovery: " + FlotationCell.ConcentrateCuRecovery());
              Console.WriteLine("Concentrate Cu Grade: " + FlotationCell.ConcentrateCuGrade());
              Console.WriteLine("Concentrate As Grade: " + FlotationCell.ConcentrateAsGrade());
              Console.WriteLine("Concentrate Mass flow : " + FlotationCell.ConcentrateMassFlowInTPH());
              Console.WriteLine("-----TAILINGS STREAM-----");
              Console.WriteLine("Tailings Cu Grade: " + FlotationCell.TailingsCuGrade());
              Console.WriteLine("Tailings As Grade: " + FlotationCell.TailingsAsGrade());
              Console.WriteLine("Tailings Mass: " + FlotationCell.TailingsFlowRate());
              Console.WriteLine("-----FINANCIAL VALUES-----");
              Console.WriteLine("Profit per second: " + FlotationCell.ProfitPerSecond());
          }

        private static void GetAirFlow(){
        if(Console.ReadKey().Key == ConsoleKey.A){
          timer.Stop();
          string Air;
          Console.Clear();
          Console.WriteLine("Set value of air flow followed by <Enter>");
          Air = Console.ReadLine();
          if (Air != ""){
            FlotationCell.AirFlow = float.Parse(Air);
          }
          StartProcess();
          }
        }

        private static void GetFrothThickness(){
        if(Console.ReadKey().Key == ConsoleKey.F){
          timer.Stop();
          string Froth;
          Console.Clear();
          Console.WriteLine("Set value of froth thickness followed by <Enter>");
          Froth = Console.ReadLine();
          if (Froth != ""){
            FlotationCell.FrothThickness = float.Parse(Froth);
          }
          StartProcess();
          }
        }

        private static void StartProcess()  {
          timer.Elapsed += (sender,e) =>
         {
            Sample();
            FlotationParameters.TimeSinceStartUp += MiliSecondsForNextSampling / 1000;
         };
         timer.Interval = MiliSecondsForNextSampling;
         do {
           timer.Start();
           GetAirFlow();
           GetFrothThickness();
         }
         while (Console.ReadKey().Key != ConsoleKey.Escape);

        }

        static void Main(string[] args)
        {
          FlotationCell = new FlotationParameters();
          timer = new Timer();
          StartProcess();
        }
      }
}
