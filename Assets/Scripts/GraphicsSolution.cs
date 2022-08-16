using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSolution : MonoBehaviour
{
    public class State
    {
        private int c;
        private int cr;
        private int v;
        private int vr;
        private int vb;
        private int cb;
        private bool b;
        private int level;
        private string id;
        private string parentId;

        public State(int Cannibal, int Vegetarian, bool Boat, int lvl, int cr, int vr, string id, string parentId, int vb, int cb)
        {
            this.C = Cannibal;
            this.V = Vegetarian;
            this.B = Boat;
            this.Level = lvl;
            this.Cr = cr;
            this.Vr = vr;
            this.Id = id;
            this.Cb = cb;
            this.Vb = vb;
            this.ParentId = parentId;
        }

        public int C { get => c; set => c = value; }
        public int V { get => v; set => v = value; }
        public bool B { get => b; set => b = value; }
        public int Level { get => level; set => level = value; }
        public int Cr { get => cr; set => cr = value; }
        public int Vr { get => vr; set => vr = value; }
        public string Id { get => id; set => id = value; }
        public string ParentId { get => parentId; set => parentId = value; }
        public int Vb { get => vb; set => vb = value; }
        public int Cb { get => cb; set => cb = value; }
    }


    public Animator boatAnimator;

    public Text LeftVegNumText;
    public Text LeftCanNumText;
    public Text RightVegNumText;
    public Text RightCanNumText;

    public GameObject panel;

    public GameObject VegaTemplate;
    public GameObject CannibalTemplate;

    public List<GameObject> leftSpaces;
    public List<GameObject> rightSpaces;
    public GameObject boatOne;
    public GameObject boatTwo;

    public Dictionary<string, State> OpenStates;
    public Dictionary<string, State> ClosedStates;
    public List<State> Path;
    public List<int> Moves; //11 = CV , 1 = C, 2 = V , 3 = CC , 4 = VV

    public List<GameObject> LeftSideVegetarians;
    public List<GameObject> LeftSideCannibals;

    public List<GameObject> OnTheBoat;

    public List<GameObject> RightSideVegetarians;
    public List<GameObject> RightSideCannibals;

    private int V, C;
    private int moveIndex = 0;

    private const string VEG_tag = "VEG";
    private const string CAN_tag = "CAN";
    void Start()
    {
        V = InputPeople.inputV;
        C = InputPeople.inputC;
        OpenStates = new Dictionary<string, State>();
        ClosedStates = new Dictionary<string, State>();
        Path = new List<State>();
        LeftSideVegetarians = new List<GameObject>();
        LeftSideCannibals = new List<GameObject>();
        RightSideVegetarians = new List<GameObject>();
        RightSideCannibals = new List<GameObject>();

        LeftVegNumText.text = V.ToString();
        LeftCanNumText.text = C.ToString();
        RightCanNumText.text = "0";
        RightVegNumText.text = "0";

        PlacePeople();
        DetermineSolution();
        panel.SetActive(false);

        boatAnimator.Play("BoatToRight");
    }

    private void DetermineSolution()
    {
        string id = String.Format("{1}{0}{2}{4}{3}", InputPeople.inputC, InputPeople.inputV, false, 0, 0);
        State currentState = new State(InputPeople.inputC, InputPeople.inputV, false, 0, 0, 0, id, null, 0, 0);
        OpenStates.Add(id, currentState);
        do
        {
            FindRoutes(currentState);
            if (OpenStates.Count > 0)
            {
                currentState = OpenStates.FirstOrDefault().Value;
            }
        } while (!(currentState.C == 0 && currentState.V == 0 && currentState.B));
        Path.Add(currentState);
        PathFinder(currentState.ParentId);
    }

    private void PlacePeople()
    {
        for (int i = 0; i < InputPeople.inputV + InputPeople.inputC; i++)
        {

            if (((i % 2 == 0) && (V > 0)) || C == 0)
            {
                GameObject temp = VegaTemplate;
                temp.name = i.ToString();
                temp.tag = VEG_tag;

                GameObject newObj = Instantiate(temp, leftSpaces[i].transform.position, VegaTemplate.transform.rotation);
                newObj.transform.SetParent(leftSpaces[i].transform);
                LeftSideVegetarians.Add(newObj);
                --V;
            }
            else
            {
                GameObject temp = CannibalTemplate;
                temp.name = i.ToString();
                temp.tag = CAN_tag;

                GameObject newObj = Instantiate(temp, leftSpaces[i].transform.position, CannibalTemplate.transform.rotation);
                newObj.transform.SetParent(leftSpaces[i].transform);
                LeftSideCannibals.Add(newObj);
                --C;
            }
        }
    }
    private void PathFinder(string parentId)
    {
        string id = parentId;
        do
        {
            Path.Add(ClosedStates[id]);
            id = ClosedStates[id].ParentId;
        } while (id != null);
        //11 = CV , 1 = C, 2 = V , 3 = CC , 4 = VV
        for (int i = Path.Count - 2; i >= 0; i--)
        {
            if (Path[i].Vb == 1 && Path[i].Cb == 0)
                Moves.Add(2);
            else if (Path[i].Vb == 2)
                Moves.Add(4);
            else if (Path[i].Cb == 2)
                Moves.Add(3);
            else if (Path[i].Cb == 1 && Path[i].Vb == 0)
                Moves.Add(1);
            else
                Moves.Add(11);

        }
    }
    private void FindRoutes(State state)
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
    //11 = CV , 1 = C, 2 = V , 3 = CC , 4 = VV
    public void FromLeftCruise()
    {
        if (moveIndex > Moves.Count - 1)
        {
            boatAnimator.enabled = false;
        }
        else
        {
            if (Moves[moveIndex] == 11)
            {
                LeftSideCannibals[0].transform.position = boatOne.transform.position;
                LeftSideCannibals[0].transform.SetParent(boatOne.transform);
                OnTheBoat.Add(LeftSideCannibals[0]);
                LeftSideCannibals.Remove(LeftSideCannibals[0]);

                LeftSideVegetarians[0].transform.position = boatTwo.transform.position;
                LeftSideVegetarians[0].transform.SetParent(boatTwo.transform);
                OnTheBoat.Add(LeftSideVegetarians[0]);
                LeftSideVegetarians.Remove(LeftSideVegetarians[0]);
            }
            else if (Moves[moveIndex] == 3)
            {
                LeftSideCannibals[0].transform.position = boatOne.transform.position;
                LeftSideCannibals[0].transform.SetParent(boatOne.transform);
                OnTheBoat.Add(LeftSideCannibals[0]);
                LeftSideCannibals.Remove(LeftSideCannibals[0]);

                LeftSideCannibals[0].transform.position = boatTwo.transform.position;
                LeftSideCannibals[0].transform.SetParent(boatTwo.transform);
                OnTheBoat.Add(LeftSideCannibals[0]);
                LeftSideCannibals.Remove(LeftSideCannibals[0]);
            }
            else if (Moves[moveIndex] == 4)
            {
                LeftSideVegetarians[0].transform.position = boatOne.transform.position;
                LeftSideVegetarians[0].transform.SetParent(boatOne.transform);
                OnTheBoat.Add(LeftSideVegetarians[0]);
                LeftSideVegetarians.Remove(LeftSideVegetarians[0]);

                LeftSideVegetarians[0].transform.position = boatTwo.transform.position;
                LeftSideVegetarians[0].transform.SetParent(boatTwo.transform);
                OnTheBoat.Add(LeftSideVegetarians[0]);
                LeftSideVegetarians.Remove(LeftSideVegetarians[0]);
            }
            LeftCanNumText.text = LeftSideCannibals.Count.ToString();
            LeftVegNumText.text = LeftSideVegetarians.Count.ToString();
            ++moveIndex;
        }
    }
    public void ExitRight()
    {
        for (int i = 0; i < rightSpaces.Count; i++)
        {
            if (OnTheBoat.Count > 0)
            {
                if (rightSpaces[i].transform.childCount == 0)
                {
                    OnTheBoat[0].transform.position = rightSpaces[i].transform.position;
                    OnTheBoat[0].transform.SetParent(rightSpaces[i].transform);
                    if (OnTheBoat[0].gameObject.tag == "CAN")
                        RightSideCannibals.Add(OnTheBoat[0]);
                    else
                        RightSideVegetarians.Add(OnTheBoat[0]);
                    OnTheBoat.Remove(OnTheBoat[0]);
                }
            }
            RightCanNumText.text = RightSideCannibals.Count.ToString();
            RightVegNumText.text = RightSideVegetarians.Count.ToString();
        }
        boatAnimator.Play("BoatToLeft");
    }

    //11 = CV , 1 = C, 2 = V , 3 = CC , 4 = VV
    public void FromRightCruise()
    {
        if(moveIndex > Moves.Count -1)
        {
            boatAnimator.enabled = false;
        }
        else
        {
            if (Moves[moveIndex] == 11)
            {
                RightSideCannibals[0].transform.position = boatOne.transform.position;
                RightSideCannibals[0].transform.SetParent(boatOne.transform);
                OnTheBoat.Add(RightSideCannibals[0]);
                RightSideCannibals.Remove(RightSideCannibals[0]);

                RightSideVegetarians[0].transform.position = boatTwo.transform.position;
                RightSideVegetarians[0].transform.SetParent(boatTwo.transform);
                OnTheBoat.Add(RightSideVegetarians[0]);
                RightSideVegetarians.Remove(RightSideVegetarians[0]);
            }
            else if (Moves[moveIndex] == 1)
            {
                RightSideCannibals[0].transform.position = boatOne.transform.position;
                RightSideCannibals[0].transform.SetParent(boatOne.transform);
                OnTheBoat.Add(RightSideCannibals[0]);
                RightSideCannibals.Remove(RightSideCannibals[0]);
            }
            else if (Moves[moveIndex] == 2)
            {
                RightSideVegetarians[0].transform.position = boatOne.transform.position;
                RightSideVegetarians[0].transform.SetParent(boatOne.transform);
                OnTheBoat.Add(RightSideVegetarians[0]);
                RightSideVegetarians.Remove(RightSideVegetarians[0]);
            }
            RightCanNumText.text = RightSideCannibals.Count.ToString();
            RightVegNumText.text = RightSideVegetarians.Count.ToString();
            ++moveIndex;
        }
       
    }
    public void ExitLeft()
    {
        for (int i = 0; i < leftSpaces.Count; i++)
        {
            if (OnTheBoat.Count > 0)
            {
                if (leftSpaces[i].transform.childCount == 0)
                {
                    OnTheBoat[0].transform.position = leftSpaces[i].transform.position;
                    OnTheBoat[0].transform.SetParent(leftSpaces[i].transform);
                    if (OnTheBoat[0].gameObject.tag == "CAN")
                        LeftSideCannibals.Add(OnTheBoat[0]);
                    else
                        LeftSideVegetarians.Add(OnTheBoat[0]);
                    OnTheBoat.Remove(OnTheBoat[0]);
                }
            }
            LeftCanNumText.text = LeftSideCannibals.Count.ToString();
            LeftVegNumText.text = LeftSideVegetarians.Count.ToString();
        }
        boatAnimator.Play("BoatToRight");
    }
}
