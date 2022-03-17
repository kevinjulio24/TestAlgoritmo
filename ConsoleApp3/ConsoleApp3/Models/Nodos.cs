namespace ConsoleApp3.Models
{
    public class Nodos
    {
        public Nodos(int id, Puntos loc, int nodeV, int parentId, int level)
        {
            Id = id;
            Loc = loc;
            NodeV = nodeV;
            ParentId = parentId;
            Level = level;
        }

        public int Id { get; set; }
        public Puntos Loc { get; set; }
        public int NodeV { get; set; }
        public int ParentId { get; set; }
        public int Level { get; set; }
    }

}
