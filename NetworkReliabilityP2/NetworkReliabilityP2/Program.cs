using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkReliabilityP2
{
    class Program
    {      
        const int edgeNumber = 48;
        const int numOfSumples = 10000;        
        static Random rnd;
        static double[] TTL;
        static int[] edgeUpDown;
        static int[,] edgeMat;
        static int[] dss;
        static double[] systemTTLs;    
        static double[] times = { 0.0, 0.05, 0.1, 0.15, 0.2, 0.25, 0.3, 0.35, 0.4, 0.45, 0.5, 0.55, 0.6, 0.65, 0.7, 0.75, 0.8, 0.85, 0.9, 0.95, 1.0 };

        static void Main(string[] args)
        {
            rnd = new Random();
            TTL = new double[edgeNumber];//edge lifesycle per index 
            edgeUpDown = new int[edgeNumber];
            edgeMat = new int[2, edgeNumber];
            dss = new int[edgeNumber];
            systemTTLs = new double[numOfSumples];


            //cube 1 2 3 4 = we name "Cube1"
            edgeMat[0, 0] = 1; edgeMat[1, 0] = 2;
            edgeMat[0, 1] = 2; edgeMat[1, 1] = 3;
            edgeMat[0, 2] = 3; edgeMat[1, 2] = 4;
            edgeMat[0, 3] = 4; edgeMat[1, 3] = 1;

            //cube 5 6 7 8 = we name "Cube2"
            edgeMat[0, 4] = 5; edgeMat[1, 4] = 6;
            edgeMat[0, 5] = 6; edgeMat[1, 5] = 8;
            edgeMat[0, 6] = 8; edgeMat[1, 6] = 7;
            edgeMat[0, 7] = 7; edgeMat[1, 7] = 5;

            //cube 9 10 11 12 = we name "Cube3"
            edgeMat[0, 8] = 9; edgeMat[1, 8] = 10;
            edgeMat[0, 9] = 10; edgeMat[1, 9] = 12;
            edgeMat[0, 10] = 12; edgeMat[1, 10] = 11;
            edgeMat[0, 11] = 11; edgeMat[1, 11] = 9;

            //cube 13 14 15 16 = we name "Cube4"
            edgeMat[0, 12] = 13; edgeMat[1, 12] = 14;
            edgeMat[0, 13] = 14; edgeMat[1, 13] = 16;
            edgeMat[0, 14] = 16; edgeMat[1, 14] = 15;
            edgeMat[0, 15] = 15; edgeMat[1, 15] = 13;

            //cube 1,2 connection                       //cube 1,3 connection
            edgeMat[0, 16] = 1; edgeMat[1, 16] = 5; edgeMat[0, 20] = 1; edgeMat[1, 20] = 9;
            edgeMat[0, 17] = 2; edgeMat[1, 17] = 6; edgeMat[0, 21] = 2; edgeMat[1, 21] = 10;
            edgeMat[0, 18] = 4; edgeMat[1, 18] = 7; edgeMat[0, 22] = 3; edgeMat[1, 22] = 12;
            edgeMat[0, 19] = 3; edgeMat[1, 19] = 8; edgeMat[0, 23] = 4; edgeMat[1, 23] = 11;

            //cube 2,1 connection                       //cube 2,4 connection
            edgeMat[0, 24] = 7; edgeMat[1, 24] = 4; edgeMat[0, 28] = 7; edgeMat[1, 28] = 15;
            edgeMat[0, 25] = 8; edgeMat[1, 25] = 3; edgeMat[0, 29] = 8; edgeMat[1, 29] = 16;
            edgeMat[0, 26] = 6; edgeMat[1, 26] = 2; edgeMat[0, 30] = 5; edgeMat[1, 30] = 13;
            edgeMat[0, 27] = 5; edgeMat[1, 27] = 1; edgeMat[0, 31] = 6; edgeMat[1, 31] = 14;

            //cube 3,1 connection                       //cube 2,4 connection
            edgeMat[0, 32] = 9; edgeMat[1, 32] = 1; edgeMat[0, 36] = 9; edgeMat[1, 36] = 13;
            edgeMat[0, 33] = 10; edgeMat[1, 33] = 2; edgeMat[0, 37] = 10; edgeMat[1, 37] = 14;
            edgeMat[0, 34] = 12; edgeMat[1, 34] = 3; edgeMat[0, 38] = 12; edgeMat[1, 38] = 16;
            edgeMat[0, 35] = 11; edgeMat[1, 35] = 4; edgeMat[0, 39] = 11; edgeMat[1, 39] = 15;

            //cube 4,2 connection                       //cube 4,3 connection
            edgeMat[0, 40] = 13; edgeMat[1, 40] = 5; edgeMat[0, 44] = 13; edgeMat[1, 44] = 9;
            edgeMat[0, 41] = 14; edgeMat[1, 41] = 6; edgeMat[0, 45] = 14; edgeMat[1, 45] = 10;
            edgeMat[0, 42] = 16; edgeMat[1, 42] = 8; edgeMat[0, 46] = 16; edgeMat[1, 46] = 12;
            edgeMat[0, 43] = 15; edgeMat[1, 43] = 7; edgeMat[0, 47] = 15; edgeMat[1, 47] = 11;


            int terminal1 = 2;
            int terminal2 = 5;
            int terminal3 = 15;

            CalcProb(terminal1, terminal2, terminal3, numOfSumples);

            Console.Write("Press any key to continue...");
            Console.ReadKey();

        }


        private static void CalcProb(int terminal1, int terminal2, int terminal3, double m)
        {
            // requested iterations loop
            for (int k = 0; k < m; k++)
            {               
                // random edge ttl 
                for (int i = 0; i < edgeNumber; i++)
                {
                    // generate a rantom time for current edge to live
                    //TTL[i] = rnd.NextDouble();
                    TTL[i] = 1 - Math.Pow(Math.E, times[rnd.Next(0, times.Length - 1)] * -1);
                }

                double nTimeOfStop = 1.0;
                bool isUp = true;

                // times loop
                for (int i = 0; i < times.Length && isUp == true; i++)
                {
                    // random edge ttl 
                    for (int j = 0; j < edgeNumber; j++)
                    {
                        // determinate based on the time if the edge is up or down
                        edgeUpDown[j] = times[i] <= TTL[j] ? 1 : 0;
                    }
                    // run monte carlo / dss algorithm to tell if the system is up at current time
                    isUp = MonteCarlo(terminal1, terminal2, terminal3);
                    if (!isUp)
                    {
                        nTimeOfStop = times[i];
                        Console.WriteLine("Stopped after: " + nTimeOfStop);
                        systemTTLs[k] = nTimeOfStop;
                    }
                }
            }           
        }

        private static bool MonteCarlo(int terminal1, int terminal2, int terminal3)
        {
            int u, v, min, max;

            for (int i = 0; i < edgeNumber; i++)
            {
                dss[i] = i;
            }
            //we go over all edges
            for (int i = 0; i < edgeNumber; i++)
            {
                //check if edge up
                if (edgeUpDown[i] == 1)
                {
                    //i+1 represent the edge
                    u = edgeMat[0, i]; //from what vertical we start
                    v = edgeMat[1, i]; //to what vertical we go

                    //because we started from 0 in dss
                    u--;
                    v--;

                    //check what groups the verticles are in
                    if (dss[u] > dss[v])
                    {
                        max = dss[u];
                        min = dss[v];
                    }
                    else
                    {
                        max = dss[v];
                        min = dss[u];
                    }
                    //we connect verticle which have edge in up
                    for (int j = 0; j < edgeNumber; j++)
                    {
                        if (dss[j] == max)
                        {
                            dss[j] = min;
                        }
                    }
                }

            }

            //check if our terminals in same ground therefore up 
            if (dss[terminal1 - 1] == dss[terminal2 - 1] && dss[terminal2 - 1] == dss[terminal3 - 1])
            {                
                return true;
            }
            return false;            
        }
    }
}
