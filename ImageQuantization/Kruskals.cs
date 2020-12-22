using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class Kruskals
    {
        Dictionary<string, string> parent;
        Dictionary<string, int> Rank;
        Dictionary<string, Dictionary<string, double>> graph;
        string findparent(string city)
        {
            if (parent[city] == city)
                return city;
            return parent[city] = findparent(parent[city]);
        }
        void join(string a, string b)
        {
            if (Rank[a] > Rank[b])
            {
                string tmp = a;
                a = b;
                b = tmp;
                parent[a] = b;
            }
            if (Rank[a] == Rank[b])
                Rank[b]++;
        }
        public static Dictionary<string, Dictionary<string, double>> initial(Dictionary<RGBPixelD, int> colors)
        {
            Dictionary<string, Dictionary<string, double>> graph = construct_graph(colors);

           foreach (var a in graph.Keys)
            parent[a] = a;
            parent[b] = b;
            Rank[a] = 1;
            Rank[b] = 1;
            kruskalQ.push({ -1 * c,{ a, b } });


            return graph;

        }

    }
}
