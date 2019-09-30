using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing.Libraries
{
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

        private void UpdateNode(int a, int b, int value) {
            connectivity[a, b] = 1;
            connectivity[b, a] = 1;
            cost[a, b] = value;
            cost[b, a] = value;
        }

        private void printMST(int[] parent, int[,] graph) {
            Console.WriteLine("Edge \tWeight");
            for (int i = 1; i < Vertices; i++) {
                Console.WriteLine(parent[i] + " - " + i + "\t" + graph[i, parent[i]]);
            }
        }

        private void PrimsMST(int a) {
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



                int x = 20, y = 10;

                _ = (x > y) ? (x = 1, x = 2) : (1, 10);
            }
        }
    }
}
