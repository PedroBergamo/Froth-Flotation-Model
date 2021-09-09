using System;

public class FlotationParameters
    {
        public float FeedFlowRate = 100;
        public float SolidsPercentage = 0.3f;
        public float AirFlow;
        public float FrothThickness;
        public float CollectorDosage;
        public float FrotherDosage;
        public float CumulativeProfit;
        public float FeedCuGrade;
        public float FeedAsGrade;
        private float CuKinetics = 1;
        private float AsKinetics = 1;
        public int FeedStep;
        public float[] CuGrades = new float[] { 1.5f, 11.2f, 13f };
        public float[] AsGrades = new float[] { 0.1f, 1.1f, 1.2f };
        public float NoiseSizePercentage = 50;
        public static float TimeSinceStartUp = 0;

        public FlotationParameters()
        {
            SetInitialVariables();
        }

        public void SetInitialVariables()
        {
            AirFlow = 13;
            FrothThickness = 30;
            CollectorDosage = 25;
            FrotherDosage = 8;
            CumulativeProfit = 0;
            FeedCuGrade = CuGrades[0];
            FeedAsGrade = AsGrades[0];
            FeedStep = 0;
            TimeSinceStartUp = 0;
        }

        /// <summary>
        /// Weights obtained from a Machine learning algorythm
        /// </summary>

        private float[] CuRecoveryWeights() {
            float Intercept = 25.41f;
            float AirWeight = 1.02f;
            float ThicknessWeight = -0.31f;
            float[] Weights = { Intercept, AirWeight, ThicknessWeight };
            return Weights;
        }

        private float[] AsRecoveryWeights()
        {
            float Intercept = 7.48f;
            float AirWeight = 0.53f;
            float ThicknessWeight = -0.11f;
            float[] Weights = { Intercept, AirWeight, ThicknessWeight };
            return Weights;
        }

        private float[] SolidsFlowWeights() {
            float Intercept = 0.04f;
            float AsRecoveryWeight = 0.07f;
            float CuRecoveryWeight = 0.08f;
            float[] Weights = { Intercept, AsRecoveryWeight, CuRecoveryWeight };
            return Weights;
        }

        public float ConcentrateCuRecovery() {
           float CuRec = CuRecoveryWeights()[0] +
                (CuRecoveryWeights()[1] * AirFlow) +
                (CuRecoveryWeights()[2] * FrothThickness);
            float DecayFactor = 0.047f;
            CuRec = CuRec - ((float)Math.Exp(DecayFactor * FeedCuGrade));
            float Noise = (new Random().Next(0,100) / NoiseSizePercentage) / 100;
            CuRec += CuRec + (CuRec * Noise);
            return CuRec;
        }

        private float AsRecovery()
        {
            float AsRec = AsRecoveryWeights()[0] + (AsRecoveryWeights()[1] * AirFlow) + (AsRecoveryWeights()[2] * FrothThickness);
            return AsRec;
        }

        public float ConcentrateCuGrade()
        {
            float ConcCuInfGrade = (FeedCuGrade * ConcentrationRatio() * ConcentrateCuRecovery() / 100);
            double ConcCuGrade = ConcCuInfGrade * (1 - ((float)Math.Exp(-CuKinetics * TimeSinceStartUp)));
            float Noise = (new Random().Next(0,100) / NoiseSizePercentage) / 100;
            double NoisyConcCuGrade = ConcCuGrade + (ConcCuGrade * Noise);
            return (float)Math.Round(NoisyConcCuGrade, 1);
        }

        public float ConcentrateAsGrade()
        {
            float AsInfGrade = FeedAsGrade * ConcentrationRatio() * AsRecovery() / 100;
            double ConcAsGrade = AsInfGrade * (1 - (Math.Exp(-AsKinetics * TimeSinceStartUp)));
            float Noise = (new Random().Next(0,100) / NoiseSizePercentage) / 100;
            double NoisyConcAsGrade = ConcAsGrade + (ConcAsGrade * Noise);
            return (float)Math.Round(NoisyConcAsGrade, 1);
        }

        public float TailingsCuGrade() {
            float MetalLoss = 100 - ConcentrateCuRecovery();
            float TailingsCuGrade = MetalLoss * FeedCuGrade * ReverseConcentrationRatio() / 100;

            float Noise = (new Random().Next(0,100) / NoiseSizePercentage) / 100;
            float NoisyTailingsCuGrade = TailingsCuGrade + (TailingsCuGrade * Noise);
            return NoisyTailingsCuGrade;
        }

        public float TailingsAsGrade()
        {
            float MetalLoss = 100 - AsRecovery();
            float TailingsAsGrade = MetalLoss * FeedAsGrade * ReverseConcentrationRatio() / 100;
            float Noise = (new Random().Next(0,100) / NoiseSizePercentage) / 100;
            float NoisyTailingsAsGrade = TailingsAsGrade + (TailingsAsGrade * Noise);
            return NoisyTailingsAsGrade;
        }

        private float ReverseConcentrationRatio() {
            float TailingsSolidsFlow = FeedSolidsFlow() - ConcentrateSolidsFlow();
            return FeedSolidsFlow() / TailingsSolidsFlow;
        }

        public float TailingsFlowRate() {
            float Tailing = FeedFlowRate - ConcentrateMassFlowInTPH();
            float Noise = (new Random().Next(0,100) / NoiseSizePercentage) / 100;
            Tailing += Tailing * Noise;
            return (float)Math.Round(Tailing, 1);
        }

        private float FeedSolidsFlow() {
            float Result =FeedFlowRate * SolidsPercentage;
            float Noise = (new Random().Next(0,100) / NoiseSizePercentage) / 100;
            Result = Result + (Result * Noise);
            return Result;
        }

        public float ConcentrateSolidsFlow() {
            float SolidsFlow = SolidsFlowWeights()[0] + (SolidsFlowWeights()[1] * AsRecovery()) + (SolidsFlowWeights()[2] * ConcentrateCuRecovery());
            return SolidsFlow;
        }

        public float ConcentrationRatio() {
            float Result = FeedSolidsFlow() / ConcentrateSolidsFlow();
            return Result;
        }

        public float ConcentrateMassFlowInTPH()
        {
            float Result = ConcentrateSolidsFlow() / SolidsPercentage;
            float RandomNumber = new Random().Next(0,100);
            Result += Result + (Result * (RandomNumber / NoiseSizePercentage));
            return Result;
        }

        public float ProfitPerSecond()
        {
            float ProfitPerSecond = RevenuePerSecond() - MiningCostPerSecond();
            return (float)Math.Round(ProfitPerSecond, 1);
        }

        private float MiningCostPerSecond()
        {
            float MiningCost = 140f;
            float CollectorPrice = 0.002012f;
            float FrotherPrice = 0.002840f;
            float MiningCostPerSecond = (FeedFlowRate * 0.3f * MiningCost * 1.3f / 3600) +
              FeedFlowRate * (CollectorPrice * CollectorDosage + FrotherPrice * CollectorDosage) / 3600;
            return MiningCostPerSecond;
        }

        private float RevenuePerSecond() {
            float CopperPrice = 6000f;
             return (CopperPrice * ((ConcentrateCuGrade() / 100) *
              ConcentrateSolidsFlow() / 3600)) - Penalty();
        }

        private float Penalty()
        {
            float PenaltyCost = 10000;
            if (ConcentrateAsGrade() > 1f)
            {
                return (ConcentrateAsGrade() - 1) * (ConcentrateSolidsFlow() /
                  3600) * PenaltyCost;
            }
            else
            {
                return 0;
            }
        }
}
