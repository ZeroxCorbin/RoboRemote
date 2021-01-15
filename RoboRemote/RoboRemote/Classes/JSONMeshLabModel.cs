namespace RoboRemote.Classes
{
    public class JSONMeshLabModel
    {
        public class Rootobject
        {
            public string version { get; set; }
            public string comment { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public Vertex[] vertices { get; set; }
            public Connectivity[] connectivity { get; set; }
            public Mapping[] mapping { get; set; }
            public object custom { get; set; }
        }

        public class Vertex
        {
            public string name { get; set; }
            public int size { get; set; }
            public string type { get; set; }
            public bool normalized { get; set; }
            public float[] values { get; set; }
        }

        public class Connectivity
        {
            public string name { get; set; }
            public string mode { get; set; }
            public bool indexed { get; set; }
            public string indexType { get; set; }
            public int[] indices { get; set; }
        }

        public class Mapping
        {
            public string name { get; set; }
            public string primitives { get; set; }
            public Attribute[] attributes { get; set; }
        }

        public class Attribute
        {
            public string source { get; set; }
            public string semantic { get; set; }
            public int set { get; set; }
        }
    }
}