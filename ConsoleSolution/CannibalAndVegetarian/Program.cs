using System;
using System.Collections.Generic;
using System.Linq;

namespace CannibalAndVegetarian
{
    class Program
    {
        public static Dictionary<string, State> OpenStates = new Dictionary<string, State>();
        public static Dictionary<string, State> ClosedStates = new Dictionary<string, State>();
        public static List<State> Path = new List<State>();
        static void Main(string[] args)
        {
            bool isWantToExit = false;
            do
            {
                int inputV, inputC;
                bool isNumericV, isNumericC;
                do
                {

                    Console.Write("Kérem a vegetáriánusok számát: ");
                    isNumericV = int.TryParse(Console.ReadLine(), out inputV);
                    Console.Write("Kérem a kannibálok számát: ");
                    isNumericC = int.TryParse(Console.ReadLine(), out inputC);
                    Console.Clear();
                }
                while (((inputV <= 0 && inputC <= 0) || inputC < 0 || inputV < 0) || (!isNumericV || !isNumericC));

                string id = String.Format("{1}{0}{2}{4}{3}", inputC, inputV, false, 0, 0);
                State currentState = new State(inputC, inputV, false, 0, 0, 0, id, null, 0, 0);
                OpenStates.Add(id, currentState);


                if ((inputC == 1 && inputV == 0) || (inputV == 1 && inputC == 0))
                {
                    Console.WriteLine("{0}-->", inputC == 0 ? "V" : "C");                   
                }
                else
                {
                    if ((inputC == inputV && inputC < 4 && inputV < 4) || inputV > inputC)
                    {
                        Console.WriteLine("\tInitial state: <{0},{1},{2},{4},{5}> Level: {3}, Id: {6} ParentId: {7}\n", currentState.V, currentState.C, currentState.B, currentState.Level, currentState.Vr, currentState.Cr, currentState.Id, currentState.ParentId);
                        do
                        {
                            FindRoutes(currentState);
                            ShowOpenStates();
                            ShowClosedStates();
                            Console.WriteLine("\n");
                            if (OpenStates.Count > 0)
                            {
                                currentState = OpenStates.FirstOrDefault().Value;
                            }
                        } while (!(currentState.C == 0 && currentState.V == 0 && currentState.B));
                        Console.WriteLine("\tLast state: <{0},{1},{2},{4},{5}> Level: {3}, Id: {6} ParentId: {7}\n", currentState.V, currentState.C, currentState.B, currentState.Level, currentState.Vr, currentState.Cr, currentState.Id, currentState.ParentId);
                        Path.Add(currentState);
                        PathFinder(currentState.ParentId);
                        /*    foreach (State item in Path)
                            {
                                 Console.WriteLine("\t Real path: <{0},{1},{2},{4},{5}> Level: {3}, Id: {6} ParentId: {7}  Vb{8} Cb{9}",item.V, item.C, item.B, item.Level, item.Vr, item.Cr, item.Id, item.ParentId, item.Vb, item.Cb);
                            } */
                    }
                    else
                    {
                        Console.WriteLine("Nincs megoldás");
                    }
                }



                Console.WriteLine("\nKilépés: ESC --- Reset: Bármi más");
                ConsoleKey x = Console.ReadKey().Key;
                if (x == ConsoleKey.Escape)
                {
                    isWantToExit = true;
                }
                Console.Clear();
                ClosedStates.Clear();
                OpenStates.Clear();
                Path.Clear();
            } while (!isWantToExit);

        }

        private static void PathFinder(string parentId)
        {
            string id = parentId;
            int moveIndex = 0;
            do
            {
                Path.Add(ClosedStates[id]);
                id = ClosedStates[id].ParentId;
            } while (id != null);
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("Moves required: ");
            for (int i = Path.Count - 2; i >= 0; i--)
            {
                moveIndex++;
                if (!Path[i].B)
                {
                    if (Path[i].Vb == 1 && Path[i].Cb == 0)
                    {
                        Console.WriteLine("\t{0}.  <--V", moveIndex);
                    }
                    else if (Path[i].Cb == 1 && Path[i].Vb == 0)
                    {
                        Console.WriteLine("\t{0}.  <--C", moveIndex);
                    }
                    else
                    {
                        Console.WriteLine("\t{0}. <--VC", moveIndex);
                    }
                }
                else
                {
                    if (Path[i].Vb == 2 && Path[i].Cb == 0)
                    {
                        Console.WriteLine("\t{0}. VV-->", moveIndex);
                    }
                    else if (Path[i].Cb == 2 && Path[i].Vb == 0)
                    {
                        Console.WriteLine("\t{0}. CC-->", moveIndex);
                    }
                    else
                    {
                        Console.WriteLine("\t{0}. VC-->", moveIndex);
                    }
                }
            }
            Console.WriteLine("--------------------------------------------------------------------------------");
        }

        public static void FindRoutes(State state)
        {
            State temp = null;
            int lvl = state.Level + 1;
            if (!ClosedStates.ContainsKey(state.Id))
                ClosedStates.Add(state.Id, state);
            OpenStates.Remove(state.Id);
            if (state.B)
            {
                if (((state.Cr - 1 <= state.Vr) || (state.Vr == 0)) && (state.Cr - 1 >= 0) && ((state.C + 1 <= state.V) || (state.V == 0)))
                {
                    string id = "";
                    id = String.Format("{1}{0}{2}{4}{3}", state.C + 1, state.V, false, state.Cr - 1, state.Vr);

                    temp = new State(state.C + 1, state.V, false, lvl, state.Cr - 1, state.Vr, id, state.Id, 0, 1);
                    if (!ClosedStates.ContainsKey(id) && !OpenStates.ContainsKey(id))
                        OpenStates.Add(id, temp);
                }
                if ((state.Vr - 1 >= state.Cr) && (state.Vr - 1 >= 0) && (state.V + 1 >= state.C))
                {
                    string id = "";
                    id = String.Format("{1}{0}{2}{4}{3}", state.C, state.V + 1, false, state.Cr, state.Vr - 1);
                    temp = new State(state.C, state.V + 1, false, lvl, state.Cr, state.Vr - 1, id, state.Id, 1, 0);
                    if (!ClosedStates.ContainsKey(id) && !OpenStates.ContainsKey(id))
                        OpenStates.Add(id, temp);
                }
                if ((state.Vr - 1 >= state.Cr - 1) && (state.Vr - 1 >= 0) && (state.Cr - 1 >= 0) && (state.C + 1 <= state.V + 1))
                {
                    string id = "";
                    id = String.Format("{1}{0}{2}{4}{3}", state.C + 1, state.V + 1, false, state.Cr - 1, state.Vr - 1);
                    temp = new State(state.C + 1, state.V + 1, false, lvl, state.Cr - 1, state.Vr - 1, id, state.Id, 1, 1);
                    if (!ClosedStates.ContainsKey(id) && !OpenStates.ContainsKey(id))
                        OpenStates.Add(id, temp);
                }


            }
            else
            {

                if (((state.C - 1 <= state.V - 1) && (state.Cr + 1 <= state.Vr + 1) || (state.Cr == 0 && state.Vr == 0)) && (state.C - 1 >= 0) && (state.V - 1 >= 0))
                {
                    string id = "";
                    id = String.Format("{1}{0}{2}{4}{3}", state.C - 1, state.V - 1, true, state.Cr + 1, state.Vr + 1);
                    temp = new State(state.C - 1, state.V - 1, true, lvl, state.Cr + 1, state.Vr + 1, id, state.Id, 1, 1);
                    if (!ClosedStates.ContainsKey(id) && !OpenStates.ContainsKey(id))
                        OpenStates.Add(id, temp);
                }
                if (((state.C - 2 <= state.V) || (state.V == 0)) && (state.C - 2 >= 0) && ((state.Cr + 2 <= state.Vr) || (state.Vr == 0)))
                {
                    string id = "";
                    id = String.Format("{1}{0}{2}{4}{3}", state.C - 2, state.V, true, state.Cr + 2, state.Vr);
                    temp = new State(state.C - 2, state.V, true, lvl, state.Cr + 2, state.Vr, id, state.Id, 0, 2);
                    if (!ClosedStates.ContainsKey(id) && !OpenStates.ContainsKey(id))
                        OpenStates.Add(id, temp);

                }
                if (((state.V - 2 >= state.C) && ((state.V - 2 > 0)) || (state.V - 2 == 0)) && (state.Vr + 2 >= state.Cr))
                {
                    string id = "";
                    id = String.Format("{1}{0}{2}{4}{3}", state.C, state.V - 2, true, state.Cr, state.Vr + 2);
                    temp = new State(state.C, state.V - 2, true, lvl, state.Cr, state.Vr + 2, id, state.Id, 2, 0);
                    if (!ClosedStates.ContainsKey(id) && !OpenStates.ContainsKey(id))
                        OpenStates.Add(id, temp);
                }
            }

        }
        public static void ShowOpenStates()
        {
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("Open States:");
            foreach (KeyValuePair<string, State> item in OpenStates)
            {
                Console.WriteLine("\t<{0},{1},{2},{4},{5}> Level: {3}, Id: {6} ParentId: {7}", item.Value.V, item.Value.C, item.Value.B, item.Value.Level, item.Value.Vr, item.Value.Cr, item.Value.Id, item.Value.ParentId);
            }
        }
        public static void ShowClosedStates()
        {
            Console.WriteLine("Closed States:");
            foreach (KeyValuePair<string, State> item in ClosedStates)
            {
                Console.WriteLine("\t<{0},{1},{2},{4},{5}> Level: {3}, Id: {6} ParentId: {7}", item.Value.V, item.Value.C, item.Value.B, item.Value.Level, item.Value.Vr, item.Value.Cr, item.Value.Id, item.Value.ParentId);
            }
            Console.WriteLine("--------------------------------------------------------------------------------");
        }
    }

}

