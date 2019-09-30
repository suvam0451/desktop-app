using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing.Libraries
{
    public static void WriteLinesToFile(string filepath, List<string> InList)
    {

    }
    class clsGraph
    {
        private int Vertices;
        private List<Int32>[] adj;

        public clsGraph(int v) {
            Vertices = v;
            adj = new List<Int32>[v];
            // Instantiate adjacency matrix
            for (int i = 0; i < v; i++) {
                adj[i] = new List<int>();
            }
        }

        public void AddEdge(int v, int w) {
            adj[v].Add(w);
        }

        public void BFS(int s) {
            bool[] visited = new bool[Vertices];

            // Queue for BFS
            Queue<int> queue = new Queue<int>();
            visited[s] = true;
            queue.Enqueue(s);

            // loop through queue
            while (queue.Count != 0) {
                s = queue.Dequeue();
                Console.WriteLine("next->" + s);

                // Get all adjacent vertices of s
                foreach(Int32 next in adj[s])
                {
                    if(!visited[next])
                    {
                        visited[next] = true;
                        queue.Enqueue(next);
                    }
                }
            }
        }

        public void DFS(int s)
        {
            bool[] visited = new bool[Vertices];

            // For DFS, use stack
            Stack<int> stack = new Stack<int>();
            visited[s] = true;
            stack.Push(s);

            while (stack.Count != 0) {
                s = stack.Pop();
                Console.WriteLine("next->" + s);
                foreach (int i in adj[s])
                {
                    if (!visited[i]) {
                        visited[i] = true;
                        stack.Push(i);
                    }
                }
            }
        }

        public void PrintAdjacencyMatrix()
        {
            for (int i = 0; i < Vertices; i++) {
                Console.Write(i + ":[");
                string s = "";
                foreach (var k in adj[i])
                {
                    s = s + (k + ",");
                }
                s = s.Substring(0, s.Length - 1);
                s = s + "]";
                Console.Write(s);
                Console.WriteLine();
            }
        }
    }

    class MST
    {
        private int Vertices;
        private int[,] cost;
        private int[,] connectivity;

        public MST(int v)
        {
            Vertices = v;
            cost = new int[Vertices, Vertices];
            connectivity = new int[Vertices, Vertices];
        }

        private int minKey(int[] key, bool[] visited)
        {
            int min = int.MaxValue;
            int min_index = 1;
            for (int i = 0; i < Vertices; i++)
            {
                if (visited[i] == false && key[i] < min)
                {
                    min = key[i];
                    min_index = i;
                }
            }
            return min_index;
        }

        public void UpdateNode(int a, int b, int value) {
            connectivity[a, b] = 1;
            connectivity[b, a] = 1;
            cost[a, b] = value;
            cost[b, a] = value;
        }

        // Print the MST
        public void printMST(int[] parent, int[,] graph) {
            List<String> lines = new List<String>();
            lines.Add("digraph G {");

            Console.WriteLine("Edge \tWeight");
            for (int i = 1; i < Vertices; i++) {
                String line = parent[i] + " -> " + i + " [label=\"" + graph[i, parent[i]] + "\"];";
                Console.WriteLine(line);
                lines.Add(line);
                Console.WriteLine(parent[i] + " - " + i + "\t" + graph[i, parent[i]]);
            }
            lines.Add("}");

            string DocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(DocPath, "Exmple.dot")))
            {
                foreach (string line in lines) {
                    outputFile.WriteLine(line);
                }
            }
        }

        public void primMST(int[,] graph)
        {
            int[] parent = new int[Vertices];
            int[] key = new int[Vertices];
            bool[] mstSet = new bool[Vertices];

            for (int i = 0; i < Vertices; i++)
            {
                key[i] = int.MaxValue;
                mstSet[i] = false;
            }

            key[0] = 0;
            parent[0] = -1;

            for (int count = 0; count < Vertices - 1; count++) {
                int u = minKey(key, mstSet);
                mstSet[u] = true;

                for (int v = 0; v < Vertices; v++) {
                    if (graph[u, v] != 0 && 
                        mstSet[v] == false && 
                        graph[u,v]<key[v])
                    {
                        parent[v] = u;
                        key[v] = graph[u, v];
                    }
                }
            }

            printMST(parent, graph);
        }

        public void PrimsMST(int a) {
            bool[] visited = new bool[Vertices];
            visited[a] |= true;
            int mincost = int.MaxValue;
            int minindex = a;
            Queue<int> toprocess;

            for (int i = 0; i < Vertices; i++) {

                // Okay fixed!! 
                _ = (connectivity[a, i] == 1) ? (mincost = Math.Min(cost[a, i], mincost)) : 1;

                // Oh, yes. This is more like it :heh:
                _ = (connectivity[a, i] == 1) ? 
                    (((cost[a, i] < mincost) ? (minindex = i) : 1), 
                    (mincost = Math.Min(cost[a, i], mincost))) : (1, 1);

                // Code it represents
                if (connectivity[a, i] == 1) {

                    if (cost[a, i] < mincost) {
                        minindex = i;
                    }
                    mincost = Math.Min(cost[a, i], mincost);
                }
            }
        }
    }
}
