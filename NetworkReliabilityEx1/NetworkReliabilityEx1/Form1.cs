using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkReliabilityEx1
{
    public partial class Form1 : Form
    {
        Random rnd = new Random();
        const int edgeNumber = 48;// 12*4 - number of edges
        const int probNumbers = 11; //network reliability
        double[] prob = new double[edgeNumber];//state vector
        int[] edgeUpDown = new int[edgeNumber];
        int[,] edgeMat = new int[2, edgeNumber];
        int[] dss = new int[edgeNumber];
        double[] res1 = new double[probNumbers];
        double[] res2 = new double[probNumbers];
        double[] p = new double[probNumbers];
        
           
        public Form1()
        {
            for (int i = 1; i <= 9; i++) p[i - 1] = i / 10.0;
            p[9] = 0.95; p[10] = 0.99;

            InitializeComponent();

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
            edgeMat[0, 16] = 1; edgeMat[1, 16] = 5;     edgeMat[0, 20] = 1; edgeMat[1, 20] = 9;
            edgeMat[0, 17] = 2; edgeMat[1, 17] = 6;     edgeMat[0, 21] = 2; edgeMat[1, 21] = 10;
            edgeMat[0, 18] = 4; edgeMat[1, 18] = 7;     edgeMat[0, 22] = 3; edgeMat[1, 22] = 12;
            edgeMat[0, 19] = 3; edgeMat[1, 19] = 8;     edgeMat[0, 23] = 4; edgeMat[1, 23] = 11;

            //cube 2,1 connection                       //cube 2,4 connection
            edgeMat[0, 24] = 7; edgeMat[1, 24] = 4;     edgeMat[0, 28] = 7; edgeMat[1, 28] = 15;
            edgeMat[0, 25] = 8; edgeMat[1, 25] = 3;     edgeMat[0, 29] = 8; edgeMat[1, 29] = 16;
            edgeMat[0, 26] = 6; edgeMat[1, 26] = 2;     edgeMat[0, 30] = 5; edgeMat[1, 30] = 13;
            edgeMat[0, 27] = 5; edgeMat[1, 27] = 1;     edgeMat[0, 31] = 6; edgeMat[1, 31] = 14;

            //cube 3,1 connection                       //cube 2,4 connection
            edgeMat[0, 32] = 9; edgeMat[1, 32] = 1;     edgeMat[0, 36] = 9; edgeMat[1, 36] = 13;
            edgeMat[0, 33] = 10; edgeMat[1, 33] = 2;     edgeMat[0, 37] = 10; edgeMat[1, 37] = 14;
            edgeMat[0, 34] = 12; edgeMat[1, 34] = 3;     edgeMat[0, 38] = 12; edgeMat[1, 38] = 16;
            edgeMat[0, 35] = 11; edgeMat[1, 35] = 4;     edgeMat[0, 39] = 11; edgeMat[1, 39] = 15;

            //cube 4,2 connection                       //cube 4,3 connection
            edgeMat[0, 40] = 13; edgeMat[1, 40] = 5;    edgeMat[0, 44] = 13; edgeMat[1, 44] = 9;
            edgeMat[0, 41] = 14; edgeMat[1, 41] = 6;    edgeMat[0, 45] = 14; edgeMat[1, 45] = 10;
            edgeMat[0, 42] = 16; edgeMat[1, 42] = 8;    edgeMat[0, 46] = 16; edgeMat[1, 46] = 12;
            edgeMat[0, 43] = 15; edgeMat[1, 43] = 7;    edgeMat[0, 47] = 15; edgeMat[1, 47] = 11;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int terminal1, terminal2, terminal3;

            terminal1 = int.Parse(textBox1.Text);
            terminal2 = int.Parse(textBox2.Text);
            terminal3 = int.Parse(textBox3.Text);

            double r = 0, rr = 0, m = 1000, mm = 10000;


            r = CalcProb(terminal1, terminal2, terminal3, r, m);

            for (int i = 1; i <= 11; i++)
            {
                this.Controls["Label" + i].Text = res1[i-1].ToString();
                this.Controls["Label" + i].Visible = true;
            }

            rr = CalcProb(terminal1, terminal2, terminal3, rr, mm);

            for (int i = 12; i <= 22; i++)
            {
                this.Controls["Label" + i].Text = res1[i - 12].ToString();
                this.Controls["Label" + i].Visible = true;
            }
        }

        private double CalcProb(int terminal1, int terminal2, int terminal3, double r, double m)
        {
            //give all edge probability 0.1, 0.2,...0.99 as instructed
            for (int z = 0; z < probNumbers; z++)
            {
                for (int i = 0; i < edgeNumber; i++)
                {
                    prob[i] = p[z];
                }

                for (int k = 0; k < m; k++)
                {
                    for (int i = 0; i < edgeNumber; i++)
                    {
                        if (rnd.NextDouble() > prob[i])
                        {
                            edgeUpDown[i] = 0;
                        }
                        else
                        {
                            edgeUpDown[i] = 1;
                        }
                    }

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
                        r++;
                    }
                }

                res1[z] = (r / m);
                r = 0;
            }

            return r;
        }
    }
}
