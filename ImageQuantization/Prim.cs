using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class Prim
    {
        public static int size;
        public static double mstCost;
        public static Dictionary<int, Dictionary<int, double>> MST = new Dictionary<int, Dictionary<int, double>>();
        public static List<RGBPixel> col_distinct = new List<RGBPixel>();
        public static bool[] visited;
        public static double[] distance;
        public static int[] parent;
        public static minHeap edge;
        public Prim()
        {
            size = 0;
            mstCost = 0;
            MST.Clear();
            col_distinct = ImageOperations.distinct_colour();
            size = col_distinct.Count;
            visited = new bool[size];
            parent = new int[size];
            distance = new double[size];
            edge = new minHeap();
        }


        public static void prim()
        {
            int currentNode = size - 1;
            int next = 0;
            RGBPixel RGB1, RGB2;
            double minD = 1e9;
            for (int i = 0; i < size; i++)
            {
                distance[i] = 1e9;
                visited[i] = false;
            }

            for (int i = 0; i < size - 1; i++)
            {
                if (visited[currentNode]) continue;
                visited[currentNode] = true;
                for (int j = 0; j < size; j++)
                {
                    if (currentNode != j)
                    {
                        if (!visited[j])
                        {
                            RGB1 = col_distinct[currentNode];
                            RGB2 = col_distinct[j];
                            double distanceD = Math.Sqrt(((RGB1.red - RGB2.red) * (RGB1.red - RGB2.red)) + (RGB1.green - RGB2.green) * (RGB1.green - RGB2.green) + (RGB1.blue - RGB2.blue) * (RGB1.blue - RGB2.blue));
                            if (distance[j] > distanceD)
                            {
                                distance[j] = distanceD;
                                parent[j] = currentNode;
                            }
                            if (minD > distance[j])
                            {
                                minD = distance[j];
                                next = j;
                            }
                        }
                    }
                }
                if (minD != 1e9)
                {
                    mstCost += distance[next];
                    //add to heap
                    colorProb c = new colorProb();
                    c.distance = -1 * distance[next];
                    c.colour1 = parent[next];
                    c.colour2 = next;
                    edge.push(c);
                    //add to graph adjlist
                    if (!MST.ContainsKey(parent[next]))
                        MST.Add(parent[next], new Dictionary<int, double>());
                    MST[parent[next]].Add(next, distance[next]);
                    currentNode = next; minD = 1e9;
                }

            }
        }
    }
}