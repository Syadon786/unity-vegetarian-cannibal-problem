namespace CannibalAndVegetarian
{
    class State
    {
        private int c;
        private int cr;
        private int v;
        private int vr;
        private int vb;
        private int cb;
        private bool b;
        private  int level;
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
}
